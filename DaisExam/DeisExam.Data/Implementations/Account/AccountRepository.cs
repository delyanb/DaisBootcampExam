using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data;
using DeisExam.Data.Base;
using DeisExam.Data.Helpers;
using DeisExam.Data.Interfaces.Account;
using Microsoft.Data.SqlClient;

namespace DaisExam.Data.Implementations.Account
{
   public class AccountRepository : BaseRepository<Models.Account>, IAccountRepository
    {
        private const string IdDbFieldEnumeratorName = "AccountId";

        protected override string GetTableName() => "Accounts";

        protected override string[] GetColumns() => new[]
        {
        IdDbFieldEnumeratorName,
        "AccountNumber",
        "AvailableAmount"
    };

        protected override Models.Account MapEntity(SqlDataReader reader)
        {
            return new Models.Account
            {
                AccountId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                AccountNumber = Convert.ToString(reader["AccountNumber"]),
                AvailableAmount = Convert.ToDecimal(reader["AvailableAmount"])
            };
        }

        public Task<int> CreateAsync(Models.Account entity)
        {
            return base.CreateAsync(entity, IdDbFieldEnumeratorName);
        }

        public Task<Models.Account> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.Account> RetrieveCollectionAsync(AccountFilter filter)
        {
            Filter commandFilter = new Filter();

            if (filter.AccountNumber is not null)
                commandFilter.AddCondition("AccountNumber", filter.AccountNumber);

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, AccountUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            var updateCommand = new UpdateCommand(
                connection,
                "Accounts",
               IdDbFieldEnumeratorName, objectId);

            if (update.AvailableAmount.HasValue)
                updateCommand.AddSetClause("AvailableAmount", update.AvailableAmount.Value);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            return base.DeleteAsync(IdDbFieldEnumeratorName, objectId);
        }
    }
}
