using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Base;
using DeisExam.Data.Helpers;
using DeisExam.Data.Interfaces.UserAccount;
using Microsoft.Data.SqlClient;

namespace DeisExam.Data.Implementations.UserAccount
{
    public class UserAccountRepository : BaseRepository<DaisExam.Models.UserAccount>, IUserAccountRepository
    {
        protected override string GetTableName() => "UserAccounts";

        protected override string[] GetColumns() => new[]
        {
            "UserId",
        "AccountId"
    };

        protected override DaisExam.Models.UserAccount MapEntity(SqlDataReader reader)
        {
            return new DaisExam.Models.UserAccount
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                AccountId = Convert.ToInt32(reader["AccountId"])
            };
        }

        public async Task<int> CreateAsync(DaisExam.Models.UserAccount entity)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO UserAccounts (UserId, AccountId)
                            VALUES (@UserId, @AccountId)";

            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@AccountId", entity.AccountId);

            await command.ExecuteNonQueryAsync();
            return 0; 
        }

        public Task<DaisExam.Models.UserAccount> RetrieveAsync(int objectId)
        {
            throw new NotSupportedException("UserAccount has a composite key, use a custom method.");
        }

        public IAsyncEnumerable<DaisExam.Models.UserAccount> RetrieveCollectionAsync(UserAccountFilter filter)
        {
            Filter commandFilter = new Filter();

            if (filter.UserId.HasValue)
                commandFilter.AddCondition("UserId", filter.UserId.Value);

            if (filter.AccountId.HasValue)
                commandFilter.AddCondition("AccountId", filter.AccountId.Value);

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public Task<bool> UpdateAsync(int objectId, UserAccountUpdate update)
        {
            throw new NotSupportedException("UserAccount has a composite key and is not meant to be updated.");
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotSupportedException("UserAccount has a composite key. Use a custom delete method.");
        }
        public async Task<bool> DeleteComposite(int userId, int accountId)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            string query = "DELETE FROM UserAccounts WHERE UserId = @UserId AND AccountId = @AccountId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@AccountId", accountId);

            int affectedRows = await command.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }
    }
}
