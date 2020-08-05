using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepository
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>> GetAllPartsAsync(int companyId, int warehouseId, int userId);
        Part GetPart(long partId, int warehouseId);
        Task<Part> GetPartAsync(long partId, int warehouseId, SqlConnection conn, SqlTransaction transaction);
        Task<Part> GetPartAsync(long partId, int warehouseId);
        Task AddPartAsync(Part part);
        Task UpdatePartAsync(Part part);
        Task UpdateQtyInHandByPartIdAsync(int companyId, int warehouseId, int partId, int QtyInHand, string direction, string note);
        Task DeletePartAsync(long id);
    }
}
