using Trisatech.KampDigi.Domain.Entities;
namespace Trisatech.KampDigi.Application.Models;
public class ResidentFamilyModel {

    public string Name { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public Relationship Relationship { get; set; }
    public Guid HeadOfFamilyId { get; set; }
}
