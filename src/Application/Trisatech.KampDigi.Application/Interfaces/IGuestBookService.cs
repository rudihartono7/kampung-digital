using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Application.Models.GuestBook;

namespace Trisatech.KampDigi.Application.Interfaces
{
    public interface IGuestBookService
    {
        Task<List<GuestBookListModel>> GetDashboard();
        Task<GuestBookAddModel> GuestAdd(GuestBookAddModel model, Guid idCurrentUser);
        Task<List<GuestBookListModel>> GuestResidentList(Guid id);
        Task<List<GuestBookListModel>> GuestBookList();
        Task<GuestBookEditModel> GuestBookEdit(GuestBookEditModel model, Guid idCurrentUser);
        Task<bool> DeleteUser(Guid id);
    }
}
