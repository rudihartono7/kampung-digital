using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidenceService
{
   Task<Residence> modify(ResidenceModel req, Guid? userId);
   Task<Residence> initialCreate(Residence req, Guid? userId);
   Task<ResidenceModel> getData ();
}