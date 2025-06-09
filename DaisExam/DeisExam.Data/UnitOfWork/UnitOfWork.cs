using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Interfaces.Account;
using DeisExam.Data.Interfaces.Payment;
using DeisExam.Data.Interfaces.User;
using DeisExam.Data.Interfaces.UserAccount;
using Microsoft.Data.SqlClient;

namespace DeisExam.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        public IUserRepository Users { get; }
        public IAccountRepository Accounts { get; }
        public IUserAccountRepository UserAccounts { get; }
        public IPaymentRepository Payments { get; }

        public UnitOfWork(
            IUserRepository users,
            IAccountRepository accounts,
            IUserAccountRepository userAccounts,
            IPaymentRepository payments)
        {
            
     Users = users;
            Accounts = accounts;
            UserAccounts = userAccounts;
            Payments = payments;
        }
    }
}
