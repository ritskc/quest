using DAL.IRepository;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IServices;

namespace WebApi.Services
{
    public class PartService : IPartService
    {

        private readonly IPartRepository _partRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICustomerRepository _customerRepository;

        public PartService(IPartRepository partRepository, ISupplierRepository supplierRepository, ICustomerRepository customerRepository)
        {
            _partRepository = partRepository;
            _supplierRepository = supplierRepository;
            _customerRepository = customerRepository;
        }
       
        public async Task<IEnumerable<Part>> GetAllPartsAsync(int companyId,int warehouseId,int userId)
        {
            var parts = await this._partRepository.GetAllPartsAsync(companyId,warehouseId, userId);            

            return parts;
        }        

        public async Task<Part> GetPartAsync(long id, int warehouseId)
        { 
            return await this._partRepository.GetPartAsync(id, warehouseId);
        }       

        public async Task AddPartAsync(Part part)
        {            
            await this._partRepository.AddPartAsync(part);
        }        

        public async Task UpdatePartAsync(Part part)
        {
            await this._partRepository.UpdatePartAsync(part);
        }       
        
        public async Task DeletePartAsync(long id)
        {            
            await Task.Run(() => this._partRepository.DeletePartAsync(id));
        }        

        public async Task UpdateQtyInHandByPartIdAsync(int companyId, int warehouseId, int partId, int QtyInHand, string direction, string note)
        {
            await Task.Run(() => this._partRepository.UpdateQtyInHandByPartIdAsync(companyId, warehouseId, partId, QtyInHand,direction,note));
        }       
    }
}
