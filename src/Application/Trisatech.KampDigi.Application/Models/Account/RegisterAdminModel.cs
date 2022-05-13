using System.ComponentModel.DataAnnotations;

namespace Trisatech.KampDigi.Application.Models.Account
{
    public class RegisterAdminModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}
