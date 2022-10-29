using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public class SlidersDTO
    {
        public int id { get; set; }

        public string? heading { get; set; }

        public string? description { get; set; }

        public string? linke { get; set; }

        public string? linkeName { get; set; }

        public string? image { get; set; }

        public bool? status { get; set; }

        public DateTimeOffset? Create_AT { get; set; }

        public DateTimeOffset? Update_AT { get; set; }
    }
}
