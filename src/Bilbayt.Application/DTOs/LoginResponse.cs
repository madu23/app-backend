using System;
using System.Collections.Generic;
using System.Text;

namespace Bilbayt.Application.DTOs
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string FullName { get; set; }
    }
}
