using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Base;
using DeisExam.Data.Interfaces.User;

namespace DeisExam.Data.Interfaces.Account
{
    public interface IAccountRepository : IBaseRepository<DaisExam.Models.Account, AccountFilter, AccountUpdate>
    {
    }
}
