using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Datas.DTO
{
    public enum StatusCode
    {
        Success = 200,
        Error = 404,
        Warning
    }
    public class ResponseDTO
    {
        public StatusCode Status { get; set; }
        public string StatusText { get; set; }
    }
}
