using AutoMapper;
using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Mapping
{
    public class FromOrderDitelsToDTO : Profile
    {
        public FromOrderDitelsToDTO()
        {
            //CreateMap<OrderDetails, OrderDetailsDTO>().ReverseMap();
        }
    }
}
