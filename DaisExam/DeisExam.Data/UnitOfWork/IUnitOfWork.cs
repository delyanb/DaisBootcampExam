using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Interfaces.Account;
using DeisExam.Data.Interfaces.Payment;
using DeisExam.Data.Interfaces.User;
using DeisExam.Data.Interfaces.UserAccount;

namespace DeisExam.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        IUserAccountRepository UserAccounts { get; }
        IPaymentRepository Payments { get; }
    }
}
