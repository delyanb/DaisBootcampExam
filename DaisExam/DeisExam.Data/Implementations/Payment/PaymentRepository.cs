using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Models;
using DeisExam.Data.Base;
using DeisExam.Data.Helpers;
using DeisExam.Data.Interfaces.Payment;
using Microsoft.Data.SqlClient;

namespace DeisExam.Data.Implementations.Payment
{
        public class PaymentRepository : BaseRepository<DaisExam.Models.Payment>, IPaymentRepository
        {
            private const string IdDbFieldEnumeratorName = "PaymentId";

            protected override string GetTableName() => "Payments";

            protected override string[] GetColumns() => new[]
            {
                IdDbFieldEnumeratorName,
                "FromAccountId",
        "ToAccountId",
                "Amount",
        "Reason",
        "DateTimeMade",
        "UserId",
        "Status",
        "DateApproved"
    };

            protected override DaisExam.Models.Payment MapEntity(SqlDataReader reader)
            {


            return new DaisExam.Models.Payment
            {
                PaymentId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                FromAccountId = Convert.ToInt32(reader["FromAccountId"]),
                ToAccountId = Convert.ToInt32(reader["ToAccountId"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                Reason = Convert.ToString(reader["Reason"]),
                DateTimeMade = Convert.ToDateTime(reader["DateTimeMade"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                Status = Convert.ToString(reader["Status"]),
                DateApproved = reader["DateApproved"] == DBNull.Value
    ? (DateTime?)null
    : Convert.ToDateTime(reader["DateApproved"])
            };
            }

            public Task<int> CreateAsync(DaisExam.Models.Payment entity)
            {
                    return base.CreateAsync(entity, IdDbFieldEnumeratorName);
            }

            public Task<DaisExam.Models.Payment> RetrieveAsync(int objectId)
            {
                return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
            }

            public IAsyncEnumerable<DaisExam.Models.Payment> RetrieveCollectionAsync(PaymentFilter filter)
            {
                Filter commandFilter = new Filter();

                if (filter.PaymentId != null)
                    commandFilter.AddCondition("PaymentId", filter.PaymentId);

                if (filter.FromAccountId != null)
                    commandFilter.AddCondition("FromAccountId", filter.FromAccountId);

                if (filter.ToAccountId != null)
                    commandFilter.AddCondition("ToAccountId", filter.ToAccountId);

                if (filter.Amount != null)
                    commandFilter.AddCondition("Amount", filter.Amount);

                if (filter.DateTimeMade != null)
                    commandFilter.AddCondition("DateTimeMade", filter.DateTimeMade);

                if (filter.UserId != null)
                    commandFilter.AddCondition("UserId", filter.UserId);

                return base.RetrieveCollectionAsync(commandFilter);
            }

        public async Task<bool> UpdateAsync(int objectId, PaymentUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            var updateCommand = new UpdateCommand(
                connection,
                "Payments",
                "PaymentId", objectId);


            if (update.Status != null)
                updateCommand.AddSetClause("Status", update.Status);
            if (update.DateApproved != null)
                updateCommand.AddSetClause("DateApproved", update.DateApproved);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }


        public Task<bool> DeleteAsync(int objectId)
            {
                throw new NotImplementedException();
            }
        }
    }

