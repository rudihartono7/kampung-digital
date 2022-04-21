using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Application.Models.GuestBook
{
    public class GuestBookAddModel
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Necessity { get; set; }
        public string? ImageUrl { get; set; }
        public Guid GuestToId { get; set; }
        public IFormFile PhotoTamu { get; set; }
    }
}
