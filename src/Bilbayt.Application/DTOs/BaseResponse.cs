using System;
using System.Collections.Generic;
using System.Text;

namespace Bilbayt.Application.DTOs
{
    public struct BaseResponse<T> where T : class
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public T ResultData { get; set; }
    }

}
