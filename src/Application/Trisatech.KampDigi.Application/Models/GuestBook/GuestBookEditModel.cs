using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Application.Models.GuestBook
{
    public class GuestBookEditModel
    {
        public Guid Id { get; set; }
        public DateTime? EndDate { get; set; }
        public string Necessity { get; set; }
    }
}
