using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public enum Role
    {
        /// <summary>
        /// User Administrator
        /// </summary>
        Administrator,
        /// <summary>
        /// Ketua, Sektretaris, Bendahara
        /// </summary>
        HeadOfResidence,
        /// <summary>
        /// Kepala Bidang
        /// </summary>
        HeadOfProgram,
        /// <summary>
        /// Warga
        /// </summary>
        Resident
    }
    public class User : BaseEntity
    {
        [StringLength(StringLengthConstant.StringNameLength)]
        public string Name { get; set; }
        [StringLength(StringLengthConstant.StringUserName)]
        public string Username { get; set; }
        [StringLength(128)]
        public string Password { get; set; }
        public Role Role { get; set; }
        public Guid? ResidentId { get; set; }
    }
}
