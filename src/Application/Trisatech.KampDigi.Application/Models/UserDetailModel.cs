using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Application.Models.GuestBook;

namespace Trisatech.KampDigi.Application.Models
{
    public class UserDetailModel
    {
        public ResidentDetailModel Residents { get; set; }
        public IEnumerable<GuestBookListModel> Guests { get; set; }
        public GuestBookListModel GuestEdit { get; set; }
    }
}
