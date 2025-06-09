using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Base;

namespace DeisExam.Data.Interfaces.User
{
    public interface IUserRepository : IBaseRepository<DaisExam.Models.User, UserFilter, UserUpdate>
    {
    }
}
