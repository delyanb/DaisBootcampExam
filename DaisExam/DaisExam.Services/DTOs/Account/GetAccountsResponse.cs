using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs.Account
{
    public class GetAccountInfosResponse : Response
    {
        public List<AccountInfo> AccountInfos { get; set; }
    }
}
