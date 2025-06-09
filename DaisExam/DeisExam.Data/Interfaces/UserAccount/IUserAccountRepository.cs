using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Base;

namespace DeisExam.Data.Interfaces.UserAccount
{
    public interface IUserAccountRepository : IBaseRepository<DaisExam.Models.UserAccount, UserAccountFilter, UserAccountUpdate>
    {
        Task<bool> DeleteComposite(int userId, int accountId);
    }
}
