﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.Edintites
{
    [Table("Brands")]
    public class Brands
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("name")]
        public string? name  { get; set; }

        [Column("image")]
        public string? image { get; set; }

        [Column("Create_AT")]
        public DateTimeOffset Create_AT { get; set; }

        [Column("Update_AT")]
        public DateTimeOffset Update_AT { get; set; }

        public IList<Products> products { get; set; }

    }
}
