using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models.Resident
{
    public class ResidentAddModel
    {
        public string Name { get; set; }
        public string IdentityNumber { get; set; }
        public string ContactNumber { get; set; }
        public OccupantType OccupantType { get; set; }
        public int TotalOccupant { get; set; }
        #nullable enable
        public string? IdentityPhoto { get; set; }
        public string? IdentityFamilyPhoto { get; set; }
        #nullable disable
        public Gender Gender { get; set; }
        public string EmergencyCallName { get; set; }
        public string EmergencyCallNumber { get; set; }
        public Relationship EmergencyCallRelation { get; set; }

        #nullable enable
        public IFormFile? KTP { get; set; }
        public IFormFile? KK { get; set; }
        #nullable disable


        public Guid HouseId { get; set; }
        public bool IsOccupant { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
