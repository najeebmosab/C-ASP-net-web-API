using AutoMapper;
using FinalProject.Datas.DTO;
using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Mapping
{
    public class FromCategoryToDTO : Profile
    {
        public FromCategoryToDTO()
        {
            //CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
