using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Application.Models.GuestBook;
using Trisatech.KampDigi.Application.Models.ResidentFamilies;

namespace Trisatech.KampDigi.Application.Models
{
    public class UserDetailModel
    {
        public ResidentDetailModel Residents { get; set; }
        public IEnumerable<GuestBookListModel> Guests { get; set; }
        public GuestBookListModel GuestEdit { get; set; }
        public IEnumerable<ResidentFamilyModel> Family { get; set; }
    
    }
}
