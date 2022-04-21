using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidenceService
{
   Task<Residence> Add (Residence req);
   Task<Residence> modify(Residence req, Guid? userId);
   Task<Residence> initialCreate(Residence req, Guid? userId);
   Task<ResidenceModel> getData ();
   Task<ResidenceModel> getData (Guid Id);
}