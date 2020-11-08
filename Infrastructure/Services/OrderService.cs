using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        // private readonly IGenericRepository<Order> _orderRepo;
        // private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        // private readonly IGenericRepository<Product> _productRepo;
        public readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        // public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> dmRepo, IGenericRepository<Product> productRepo,
        // IBasketRepository basketRepo)
        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // get basket from repo
        // get items from basket repo
        // get delivery method from repp
        // calc subtotal
        // create order 
        // save to db
        // return order
        var basket = await _basketRepo.GetBasketAsync(basketId);

        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            // var productItem = await _productRepo.GetByIdAsync(item.Id);
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,productItem.PictureUrl);
            var OrderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(OrderItem);
        }

        // var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        var subTotal = items.Sum(item => item.Price * item.Qunatity);

        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal);

        // TODO: Save to the database  -- We will use UNIT OF WORK for handling multiple repositories;

        _unitOfWork.Repository<Order>().Add(order);

        var result = await _unitOfWork.Complete();

        if (result <= 0) return null;
        
        // delete basket
        await _basketRepo.DeleteBasketAsync(basketId);
        // return order;
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec); 
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
}
}