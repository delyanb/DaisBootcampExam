using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeisExam.Data.Interfaces.Account
{
    public class AccountFilter
    {
        public SqlInt32? AccountId { get; set; }
        public SqlString? AccountNumber { get; set; }
        public SqlDecimal? AvailableAmount { get; set; }
    }
}
