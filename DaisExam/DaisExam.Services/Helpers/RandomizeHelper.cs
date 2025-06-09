using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.Helpers
{
    public static class RandomizeHelper
    {
        public static string GenerateAccountNumber()
        {
            return $"ACC-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
