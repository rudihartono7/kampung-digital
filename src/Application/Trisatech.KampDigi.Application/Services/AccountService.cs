using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.Account;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Trisatech.KampDigi.Application.Services
{
    public class AccountService : BaseDbService, IAccountService
    {
        public AccountService(KampDigiContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<UserViewModel>> GetAllUser()
        {
            var data = await Db.Users.ToListAsync();
            var listUser = new List<UserViewModel>();
            foreach (var user in data)
            {
                listUser.Add(new UserViewModel
                {
                    Id = user.Id,
                    CreatedDate = user.CreatedDate,
                    Name = user.Name,
                    ResidentId = user.ResidentId,
                    Role = user.Role,
                    UpdatedDate = user.UpdatedDate,
                    Username = user.Username,
                });
            }
            return listUser;
        }

        public async Task<Guid> GetUser(Guid id)
        {
            var dataUser = await Db.Users.FindAsync(id);
            return dataUser.Id;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await Db.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (user == null)
            {
                throw new InvalidOperationException("Tidak ada user dengan nama ini");
            }

            var isPasswordMatch = VerifyPassword(password, user.Salt, user.Password);

            if (!isPasswordMatch)
            {
                throw new InvalidOperationException("Password tidak sesuai");
            }

            return user;
        }

        public async Task<RegisterAdminModel> Register(RegisterAdminModel dataRegister)
        {
            // check Username if exist in database
            if (await Db.Users.AnyAsync(x => x.Username == dataRegister.Username))
            {
                throw new InvalidOperationException($"Username {dataRegister.Username} sudah digunakan");
            }

            if (dataRegister.Password != dataRegister.ConfirmPassword)
            {
                throw new InvalidOperationException($"Password tidak sesuai antara Password dengan Konfirmasi Password");
            }

            var hashAndSalt = EncryptPassword(dataRegister.Password);

            User newRegister = new User
            {
                Name = dataRegister.Name,
                Username = dataRegister.Username,
                Password = hashAndSalt.Hash,
                Salt = hashAndSalt.Salt,
                Role = (Role)0,
                CreatedDate = DateTime.Now
            };

            await Db.AddAsync(newRegister);
            await Db.SaveChangesAsync();

            return dataRegister;
        }


        public async Task<UpdatePasswordModel> UpdatePassword(UpdatePasswordModel dataPassword)
        {
            //var userData = await Db.Users.FindAsync(dataPassword.ResidentId);
            var userData = await Db.Users.FirstOrDefaultAsync(x => x.ResidentId == dataPassword.Id);

            if (userData == null)
            {
                throw new InvalidOperationException($"Data dengan id: {dataPassword.Id}, tidak ditemukan");
            }

            if (dataPassword.NewPassword != dataPassword.ConfirmNewPassword)
            {
                throw new InvalidOperationException("Password dan konfirmasi password tidak sesuai");
            }

            var isPasswordMatch = VerifyPassword(dataPassword.OldPassword, userData.Salt, userData.Password);

            if (!isPasswordMatch)
            {
                throw new InvalidOperationException("Password tidak sesuai");
            }

            var hashedPassword = EncryptPassword(dataPassword.NewPassword);

            userData.UpdatedDate = DateTime.Now;
            userData.UpdatedBy = dataPassword.Id;
            userData.AuditActivty = AuditActivtyType.UPDATE;
            userData.Password = hashedPassword.Hash;
            userData.Salt = hashedPassword.Salt;
            await Db.SaveChangesAsync();

            return dataPassword;
        }

        public async Task<ResetPasswordModel> ResetPassword(ResetPasswordModel dataPassword, Guid currentUserId)
        {
            var userData = await Db.Users.FirstOrDefaultAsync(x => x.Id == dataPassword.Id);

            if (userData == null)
            {
                throw new InvalidOperationException($"Data dengan id: {dataPassword.Id}, tidak ditemukan");
            }

            var hashedPassword = EncryptPassword(dataPassword.NewPassword);

            userData.UpdatedDate = DateTime.Now;
            userData.UpdatedBy = currentUserId;
            userData.AuditActivty = AuditActivtyType.UPDATE;
            userData.Password = hashedPassword.Hash;
            userData.Salt = hashedPassword.Salt;
            await Db.SaveChangesAsync();

            return dataPassword;
        }

        public async Task<bool> DeleteUser(User dataUser)
        {
            Db.Remove(dataUser);
            await Db.SaveChangesAsync();

            return true;
        }

        public async Task<EditRoleModel> EditRole(EditRoleModel dataRole, Guid currentUserId)
        {
            var userData = await Db.Users.FirstOrDefaultAsync(x => x.Id == dataRole.Id);
            if (userData == null)
            {
                throw new InvalidOperationException($"User dengan id {dataRole.Id} tidak dapat ditemukan"); 
            }
            userData.Role = dataRole.Role;
            userData.UpdatedDate = DateTime.Now;
            userData.UpdatedBy = currentUserId;
            userData.AuditActivty = AuditActivtyType.UPDATE;

            Db.Update(userData);
            await Db.SaveChangesAsync();

            return dataRole;
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

        public bool VerifyPassword(string inputPassword, byte[] salt, string userPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: inputPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == userPassword;
        }

        
    }
}
