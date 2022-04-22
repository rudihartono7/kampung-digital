using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IHouseService : ICrudService<House>
{
   Task<HouseDetailModel> DetailHouse(Guid Id);
}
