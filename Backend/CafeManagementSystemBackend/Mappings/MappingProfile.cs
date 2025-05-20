using AutoMapper;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.DTOs;

namespace CafeManagementSystemBackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
        
            CreateMap<MenuItem,MenuItemDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Cart, CartDTO>()
             .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartItem, CartItemDTO>()
    .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.MenuItem.Price));

            CreateMap<AddCartItemDTO, CartItem>();

 
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderItem, OrderItemDTO>()
.ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name))
               .ForMember(dest => dest.ItemPrice, opt => opt.MapFrom(src => src.ItemPrice));

            CreateMap<Reservation, ReservationDTO>()
.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));

            CreateMap<AddReservationDTO, Reservation>();
            CreateMap<RegisterDTO, User>();
            CreateMap<CartCreationDTO, Cart>();
            

        }

    }
    
}
