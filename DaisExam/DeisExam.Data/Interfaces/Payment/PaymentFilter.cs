using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeisExam.Data.Interfaces.Payment
{
    public class PaymentFilter
    {
        public SqlInt32? PaymentId { get; set; }
        public SqlInt32? FromAccountId { get; set; }
        public SqlInt32? ToAccountId { get; set; }
        public SqlDecimal? Amount { get; set; }
        public SqlDateTime? DateTimeMade { get; set; }
        public SqlInt32? UserId { get; set; }

    }
}
