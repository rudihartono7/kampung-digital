using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models.Resident
{
    public class ResidentDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IdentityNumber { get; set; }
        public string ContactNumber { get; set; }
        public OccupantType OccupantType { get; set; }
        public int TotalOccupant { get; set; }
        public string IdentityPhoto { get; set; }
        public string IdentityFamilyPhoto { get; set; }
        public Gender Gender { get; set; }
        public string EmergencyCallName { get; set; }
        public string EmergencyCallNumber { get; set; }
        public Relationship EmergencyCallRelation { get; set; }

        public Guid HouseId { get; set; }
        public string HouseNumber { get; set; }
        public bool IsOccupant { get; set; }

        public string Username { get; set; }
        public Role Role { get; set; }
        public DateTime Join { get; set; }

        public IFormFile? KK { get; set; }
        public IFormFile? KTP { get; set; }

    }
}
