using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.GuestBook;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Services
{
    public class GuestBookService : BaseDbService, IGuestBookService
    {
        public GuestBookService(KampDigiContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<GuestBookListModel>> GetDashboard()
        {
            var listGuest = await (from a in Db.GuestBooks
                                          select new GuestBookListModel
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              ContactNumber = a.ContactNumber,
                                              StartDate = a.StartDate,
                                              EndDate = a.EndDate,
                                              Necessity = a.Necessity,
                                              ImageUrl = a.ImageUrl,
                                              GuestToId = a.GuestToId
                                          }).Take(5).ToListAsync();
            return listGuest;

        }

        public async Task<List<GuestBookListModel>> GuestBookList()
        {
            var listGuest = await (from a in Db.GuestBooks
                                   join b in Db.Residents on a.GuestToId equals b.Id
                                   select new GuestBookListModel
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       ContactNumber = a.ContactNumber,
                                       StartDate = a.StartDate,
                                       EndDate = a.EndDate,
                                       Necessity = a.Necessity,
                                       ImageUrl = a.ImageUrl,
                                       HouseOwner = b.Name,
                                       GuestToId = a.GuestToId
                                   }).ToListAsync();
            return listGuest;
        }

        public async Task<GuestBookAddModel> GuestAdd(GuestBookAddModel model, Guid idCurrentUser)
        {
            if (await Db.GuestBooks.AnyAsync(x => x.ContactNumber == model.ContactNumber))
            {
                throw new InvalidOperationException($"Tamu dengan nomor telephone {model.ContactNumber} sedang bertamu di kampung ini");
            }

            var newGuest = new GuestBook
            {
                ContactNumber = model.ContactNumber,
                Name = model.Name,
                Necessity = model.Necessity,
                ImageUrl = model.ImageUrl,
                GuestToId = model.GuestToId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CreatedBy = idCurrentUser,
                CreatedDate = DateTime.Now,
                AuditActivty = AuditActivtyType.INSERT

            };
            await Db.GuestBooks.AddAsync(newGuest);
            await Db.SaveChangesAsync();
            return model;
        }

        public async Task<GuestBookEditModel> GuestBookEdit(GuestBookEditModel dataGuest, Guid idCurrentUser)
        {
            var dataEdited = await Db.GuestBooks.FirstOrDefaultAsync(x => x.Id == dataGuest.Id);
            if (dataEdited == null)
            {
                throw new InvalidOperationException($"Data tamu ini tidak dapat ditemukan");
            }

            dataEdited.EndDate = dataGuest.EndDate;
            dataEdited.Necessity = dataGuest.Necessity;
            dataEdited.UpdatedBy = idCurrentUser;
            dataEdited.UpdatedDate = DateTime.Now;
            dataEdited.AuditActivty = AuditActivtyType.UPDATE;

            Db.Update(dataEdited);
            await Db.SaveChangesAsync();
            return dataGuest;
        }

        public async Task<List<GuestBookListModel>> GuestResidentList(Guid id)
        {
            var detailGuest = await (from a in Db.GuestBooks
                                   join b in Db.Residents on a.GuestToId equals b.Id
                                   where b.Id == id
                                   select new GuestBookListModel
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       ContactNumber = a.ContactNumber,
                                       StartDate = a.StartDate,
                                       EndDate = a.EndDate,
                                       Necessity = a.Necessity,
                                       ImageUrl = a.ImageUrl,
                                   }).ToListAsync();
            return detailGuest;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var dataGuest = Db.GuestBooks.Find(id);
            if (dataGuest == null)
            {
                throw new InvalidOperationException($"Tamu dengan ID {id} tidak dapat ditemukan");
            }

            Db.Remove(dataGuest);
            await Db.SaveChangesAsync();
            return true;
        }
    }
}
