using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;

public class ResidenceService : BaseDbService, IResidenceService
{
   public ResidenceService(KampDigiContext dbContext) : base(dbContext)
   {

   }

   public async Task<Residence> Add(Residence req)
   {
      req.Id = Guid.NewGuid();
      await Db.AddAsync(req);
      await Db.SaveChangesAsync();
      return req;
   }

   public async Task<ResidenceModel> getData()
   {
      var residence = await (from a in Db.Residences
                             join b in Db.Residents on a.PersonInCharge equals b.Id
                             select new ResidenceModel
                             {
                                Id = a.Id,
                                Address = a.Address,
                                GMapLink = a.GMapLink,
                                ImageUrl = a.ImageUrl,
                                Latitude = a.Latitude,
                                Longitude = a.Longitude,
                                Name = a.Name,
                                Houses = (from h in Db.Houses
                                          where h.ResidenceId == a.Id
                                          select new House
                                          {
                                             Id = h.Id,
                                             Number = h.Number,
                                             Order = h.Order,
                                             Status = h.Status,
                                             Type = h.Type
                                          }).ToArray(),
                                PersonInCharge = a.PersonInCharge,
                                PersonInChargeName = b.Name
                             }).FirstOrDefaultAsync();
      return residence;
   }

   public async Task<ResidenceModel> getData(Guid Id)
   {
      var residence = await Db.Residences.FirstOrDefaultAsync(x => x.Id == Id);
      if (residence == null)
      {
         throw new InvalidOperationException($"Data Desa tidak ditemukan");
      }
      return new ResidenceModel()
      {
         Id = residence.Id,
         Address = residence.Address,
         GMapLink = residence.GMapLink,
         ImageUrl = residence.ImageUrl,
         Latitude = residence.Latitude,
         Longitude = residence.Longitude,
         Name = residence.Name,
         PersonInCharge = residence.PersonInCharge,
      };
   }

   public async Task<Residence> initialCreate(Residence req, Guid? userId)
   {
      var isExist = await Db.Residences.AnyAsync();
      // cek apakah data residence sudah ada
      if (isExist)
      {
         throw new InvalidOperationException($"Desa sudah dibuat, tidak bisa menambah lebih dari 1 desa");
      }

      req.Id = Guid.NewGuid();
      req.CreatedBy = userId.Value;
      req.CreatedDate = DateTime.Now;
      req.AuditActivty = AuditActivtyType.INSERT;

      await Db.AddAsync(req);
      await Db.SaveChangesAsync();

      return req;
   }

   public async Task<Residence> modify(Residence req, Guid? userId)
   {
      var residence = await Db.Residences.FirstOrDefaultAsync(x => x.Id == req.Id);
      if (residence == null)
      {
         throw new InvalidOperationException($"Desa with {req.Id} doesn't exist in database");
      }
      residence.Address = req.Address;
      residence.AuditActivty = AuditActivtyType.UPDATE;
      residence.GMapLink = req.GMapLink;
      residence.Latitude = req.Latitude;
      residence.Longitude = req.Longitude;
      residence.Name = req.Name;
      residence.PersonInCharge = req.PersonInCharge;
      residence.ImageUrl = req.ImageUrl;
      residence.UpdatedBy = userId.Value;
      residence.UpdatedDate = DateTime.Now;

      Db.Update(residence);
      await Db.SaveChangesAsync();

      return residence;
   }


}