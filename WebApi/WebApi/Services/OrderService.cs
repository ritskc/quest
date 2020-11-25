using DAL.IRepository;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IServices;

namespace WebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPartRepository _partRepository;
        private readonly IPackingSlipService _packingSlipService;
        private readonly IEntityTrackerRepository entityTrackerRepository;

        public OrderService(IOrderRepository orderRepository, IPartRepository partRepository, IPackingSlipService packingSlipService,
            IEntityTrackerRepository entityTrackerRepository)
        {
            _orderRepository = orderRepository;
            _partRepository = partRepository;
            _packingSlipService = packingSlipService;
            this.entityTrackerRepository = entityTrackerRepository;
        }
        public async Task<long> AddOrderMasterAsync(OrderMaster order)
        {
            var entity = await this.entityTrackerRepository.GetEntityAsync(order.CompanyId, order.PoDate, BusinessConstants.ENTITY_TRACKER_ORDER);
            order.PONo = entity.EntityNo;
            return await this._orderRepository.AddOrderMasterAsync(order);
        }

        public async Task DeleteOrderMasterAsync(long orderId)
        {
            await this._orderRepository.DeleteOrderMasterAsync(orderId);
        }

        public async Task<IEnumerable<OrderMaster>> GetAllOrderMastersAsync(int companyId, int userId)
        {
            //return await this.orderRepository.GetAllOrderMastersAsync(companyId);

            var result = await this._orderRepository.GetAllOrderMastersAsync(companyId, userId);
            var packingSlips = await this._packingSlipService.GetAllPackingSlipsAsync(companyId, result.FirstOrDefault().WarehouseId, userId);
            foreach (OrderMaster pos in result)
            {
                foreach (OrderDetail poDetail in pos.OrderDetails)
                {
                    var partDetail = await this._partRepository.GetPartAsync(poDetail.PartId, pos.WarehouseId);
                    poDetail.part = partDetail;
                    poDetail.PackingSlipNo = "";
                    if (poDetail.ShippedQty > 0)
                    {
                        foreach (PackingSlip packingSlip in packingSlips)
                        {
                            foreach (PackingSlipDetails packingSlipDetails in packingSlip.PackingSlipDetails)
                            {
                                if (packingSlipDetails.OrderDetailId == poDetail.Id)
                                {
                                    poDetail.PackingSlipNo = packingSlip.PackingSlipNo;
                                    poDetail.ShippingDate = packingSlip.ShippingDate;
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            return result;
        }

        public async Task<OrderMaster> GetOrderMasterAsync(long orderId)
        {
            //return await this._orderRepository.GetOrderMasterAsync(orderId);

            var result = await this._orderRepository.GetOrderMasterAsync(orderId);
            foreach (OrderDetail poDetail in result.OrderDetails)
            {
                var partDetail = await this._partRepository.GetPartAsync(poDetail.PartId, result.WarehouseId);
                poDetail.part = partDetail;
            }
            return result;
        }

        public async Task UpdateOrderAsync(int id, string path)
        {
            await this._orderRepository.UpdateOrderAsync(id, path);
        }

        public async Task UpdateOrderMasterAsync(OrderMaster order)
        {
            await this._orderRepository.UpdateOrderMasterAsync(order);
        }

        public async Task CloseOrderMasterAsync(OrderMaster order)
        {
            await this._orderRepository.CloseOrderMasterAsync(order);
        }
    }
}
