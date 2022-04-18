using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Application.Models.Account;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces
{
    public interface IAccountService
    {
        Task<List<UserViewModel>> GetAllUser();
        Task<RegisterAdminModel> Register(RegisterAdminModel model);
        Task<User> Login(string userName, string password);
        Task<UpdatePasswordModel> UpdatePassword(UpdatePasswordModel dataPassword);
        Task<ResetPasswordModel> ResetPassword(ResetPasswordModel dataPassword, Guid currentUserId);
        Task<bool> DeleteUser(User dataUser);
        Task<EditRoleModel> EditRole(EditRoleModel dataRole, Guid currentUserId);

    }
}
