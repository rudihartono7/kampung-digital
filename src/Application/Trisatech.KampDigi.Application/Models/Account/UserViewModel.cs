using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models.Account
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public Guid? ResidentId { get; set; }
    }
}
