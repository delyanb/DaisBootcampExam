using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeisExam.Data.Interfaces.UserAccount
{
    public class UserAccountFilter
    {
        public SqlInt32? UserId { get; set; }
        public SqlInt32? AccountId { get; set; }
    }
}
