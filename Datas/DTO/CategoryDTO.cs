using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class CategoryDTO
    {
        public int id { get; set; }

        public string categoryName { get; set; }
        
        public string picture { get; set; }
        
        public DateTimeOffset Create_AT { get; set; }
        
        public DateTimeOffset Update_AT { get; set; }


    }
}
