using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class BrandsDTO
    {
        public int id { get; set; }

        public string? name { get; set; }

        public string? image { get; set; }

        public DateTimeOffset Create_AT { get; set; }
        
        public DateTimeOffset Update_AT { get; set; }
        public IList<Products> products { get; set; }

    }
}
