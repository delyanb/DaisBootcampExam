using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs
{
    public abstract class Response
    {
        public bool Success { get; set; } = false;  
        public string? ErrorMessage { get; set; }
    }
}
