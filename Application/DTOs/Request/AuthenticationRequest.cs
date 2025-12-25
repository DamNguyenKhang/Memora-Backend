using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class AuthenticationRequest
    {
        public required string Identifier { get; set; }
        public required string Password { get; set; }
    }
}