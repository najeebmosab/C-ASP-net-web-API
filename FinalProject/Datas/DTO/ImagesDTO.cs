using FinalProject.Datas.Edintites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class ImagesDTO
    {
        public int id { get; set; }
        public int productId { get; set; }
        public virtual Products product { get; set; }
        public string Path { get; set; }
        public DateTimeOffset Create_AT { get; set; }
        public DateTimeOffset Update_AT { get; set; }



    }
}
