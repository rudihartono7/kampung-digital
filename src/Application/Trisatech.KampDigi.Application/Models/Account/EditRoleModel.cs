using Trisatech.KampDigi.Domain.Entities;


namespace Trisatech.KampDigi.Application.Models.Account
{
    public class EditRoleModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}
