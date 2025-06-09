using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeisExam.Data.Interfaces.User
{
    public class UserUpdate
    {
        public SqlString? PasswordHash { get; set; }
        public SqlString? FullName { get; set; }
        public SqlString? Username { get; set; }
    }
}
