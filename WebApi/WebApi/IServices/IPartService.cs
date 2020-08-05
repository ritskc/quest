using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IServices
{
    public interface IPartService
    {
        Task<IEnumerable<Part>> GetAllPartsAsync(int companyId, int warehouseId, int userId);

        Task<Part> GetPartAsync(long id, int warehouseId);

        Task AddPartAsync(Part part);

        Task UpdatePartAsync(Part part);

        Task DeletePartAsync(long id);

        Task UpdateQtyInHandByPartIdAsync(int companyId, int warehouseId, int partId, int QtyInHand, string direction, string note);
    }
}
