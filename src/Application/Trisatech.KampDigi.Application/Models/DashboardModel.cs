using Trisatech.KampDigi.Application.Models.GuestBook;

namespace Trisatech.KampDigi.Application.Models
{
    public class DashboardModel
    {
        public IEnumerable<GuestBookListModel> Guests { get; set; }
    }
}
