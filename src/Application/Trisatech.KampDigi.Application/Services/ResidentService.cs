using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Trisatech.KampDigi.Application.Models.ResidentFamilies;

namespace Trisatech.KampDigi.Application.Services
{
    public class ResidentService : BaseDbService, IResidentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResidentService(KampDigiContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ResidentListModel>> GetList()
        {
            var ListResident = await (from a in Db.Residents
                                      join b in Db.Users on a.Id equals b.ResidentId
                                      join c in Db.Houses on a.HouseId equals c.Id
                                      select new ResidentListModel
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          ContactNumber = a.ContactNumber,
                                          IdentityPhoto = a.IdentityPhoto,
                                          IdentityNumber = a.IdentityNumber,
                                          TotalOccupant = a.TotalOccupant,
                                          HouseId = c.Number,
                                          Username = b.Username,
                                      }
                                      ).Take(10).ToListAsync();
            return ListResident;
        }

        public async Task<ResidentAddModel> ResidentAdd(ResidentAddModel model, Guid idCurrentUser)
        {
            if (await Db.Residents.AnyAsync(x => x.IdentityNumber == model.IdentityNumber))
            {
                throw new InvalidOperationException($"Warga dengan NIK {model.IdentityNumber} sudah terdaftar");
            }

            var newResident = new Resident
            {
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                ContactNumber = model.ContactNumber,
                OccupantType = model.OccupantType,
                TotalOccupant = model.TotalOccupant,
                IdentityPhoto = model.IdentityPhoto,
                IdentityFamilyPhoto = model.IdentityFamilyPhoto,
                Gender = model.Gender,
                EmergencyCallName = model.EmergencyCallName,
                EmergencyCallNumber = model.EmergencyCallNumber,
                EmergencyCallRelation = model.EmergencyCallRelation,
                HouseId = model.HouseId,
                IsOccupant = model.IsOccupant,
                CreatedBy = idCurrentUser,
                CreatedDate = DateTime.Now

            };
            await Db.Residents.AddAsync(newResident);
            await Db.SaveChangesAsync();

            var hashUserBaru = EncryptPassword(model.Password);

            var newUser = new User
            {
                Name = model.Name,
                Username = model.Username,
                Role = model.Role,
                Password = hashUserBaru.Hash,
                Salt = hashUserBaru.Salt,
                ResidentId = newResident.Id,
                CreatedBy = idCurrentUser,
                CreatedDate = DateTime.Now
            };

            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();

            return model;
        }

        public async Task<bool> ResidentDelete(Guid idResident)
        {
            var resident = Db.Residents.Find(idResident);
            var account = Db.Users.FirstOrDefault(x => x.ResidentId == idResident);
            if (resident == null || account == null)
            {
                throw new InvalidOperationException($"User/Resident dengan ID {idResident} tidak dapat ditemukan");
            }


            //Db.RemoveRange(resident, account);
            Db.Remove(resident);
            Db.Remove(account);
            await Db.SaveChangesAsync();
            return true;
        }

        public async Task<ResidentDetailModel> ResidentDetail(Guid idResident)
        {
            var dataResident = await (from a in Db.Users
                                      join b in Db.Residents on a.ResidentId equals b.Id
                                      join c in Db.Houses on b.HouseId equals c.Id
                                      where b.Id == idResident
                                      select new ResidentDetailModel
                                      {
                                          Id = idResident,
                                          Name = b.Name,
                                          Username = a.Username,
                                          IdentityNumber = b.IdentityNumber,
                                          ContactNumber = b.ContactNumber,
                                          OccupantType = b.OccupantType,
                                          TotalOccupant = b.TotalOccupant,
                                          IdentityPhoto = b.IdentityPhoto,
                                          IdentityFamilyPhoto = b.IdentityFamilyPhoto,
                                          Gender = b.Gender,
                                          EmergencyCallName = b.EmergencyCallName,
                                          EmergencyCallNumber = b.EmergencyCallNumber,
                                          EmergencyCallRelation = b.EmergencyCallRelation,
                                          HouseNumber = c.Number,
                                          IsOccupant = b.IsOccupant,
                                          Role = a.Role,
                                          Join = b.CreatedDate,
                                      }).FirstOrDefaultAsync();

            return dataResident;

        }

        public async Task<ResidentEditModel> ResidentGetEditModel(Guid id)
        {
            var dataEdited = await Db.Residents.FirstOrDefaultAsync(x => x.Id == id);
            if (dataEdited == null)
            {
                throw new InvalidOperationException($"User dengan ID {id} tidak dapat ditemukan");
            }

            var dataResident = new ResidentEditModel
            {
                Id = dataEdited.Id,
                Name = dataEdited.Name,
                IdentityNumber = dataEdited.IdentityNumber,
                ContactNumber = dataEdited.ContactNumber,
                OccupantType = dataEdited.OccupantType,
                TotalOccupant = dataEdited.TotalOccupant,
                IsOccupant = dataEdited.IsOccupant,
                IdentityPhoto = dataEdited.IdentityPhoto,
                IdentityFamilyPhoto = dataEdited.IdentityFamilyPhoto,
                Gender = dataEdited.Gender,
                EmergencyCallName = dataEdited.EmergencyCallName,
                EmergencyCallNumber = dataEdited.EmergencyCallNumber,
                EmergencyCallRelation = dataEdited.EmergencyCallRelation,
                HouseId = dataEdited.HouseId,
            };

            return dataResident;
        }

        public async Task<ResidentEditModel> ResidentEdit(ResidentEditModel dataResident, Guid idCurrentUser)
        {
            var dataEdited = await Db.Residents.FirstOrDefaultAsync(x => x.Id == dataResident.Id);
            if (dataResident == null)
            {
                throw new InvalidOperationException($"User dengan ID {dataResident.Id} tidak dapat ditemukan");
            }

            dataEdited.Name = dataResident.Name;
            dataEdited.IdentityNumber = dataResident.IdentityNumber;
            dataEdited.ContactNumber = dataResident.ContactNumber;
            dataEdited.OccupantType = dataResident.OccupantType;
            dataEdited.TotalOccupant = dataResident.TotalOccupant;
            dataEdited.IsOccupant = dataResident.IsOccupant;
            dataEdited.IdentityPhoto = dataResident.IdentityPhoto;
            dataEdited.IdentityFamilyPhoto = dataResident.IdentityFamilyPhoto;
            dataEdited.Gender = dataResident.Gender;
            dataEdited.EmergencyCallName = dataResident.EmergencyCallName;
            dataEdited.EmergencyCallNumber = dataResident.EmergencyCallNumber;
            dataEdited.EmergencyCallRelation = dataResident.EmergencyCallRelation;
            dataEdited.HouseId = dataResident.HouseId;
            dataEdited.UpdatedBy = idCurrentUser;
            dataEdited.UpdatedDate = DateTime.Now;
            dataEdited.AuditActivty = AuditActivtyType.UPDATE;

            Db.Update(dataEdited);
            await Db.SaveChangesAsync();


            return dataResident;
        }

        public class HashSalt
        {
            public string Hash { get; set; }
            public byte[] Salt { get; set; }
        }

        public HashSalt EncryptPassword(string inputPassword)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: inputPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = hashedPassword, Salt = salt };
        }
    }
}
