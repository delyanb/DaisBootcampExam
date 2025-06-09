using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeisExam.Data.Interfaces.Payment
{
    public class PaymentUpdate
    {
        public SqlString? Status { get; set; }
        public SqlDateTime? DateApproved { get; set; }
    }
}
