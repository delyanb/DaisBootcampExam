using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DeisExam.Data.Base;
using DeisExam.Data.Helpers;
using DeisExam.Data.Interfaces.User;
using Microsoft.Data.SqlClient;

namespace DeisExam.Data.Implementations.User
{
    public class UserRepository : BaseRepository<DaisExam.Models.User>, IUserRepository
    {
        private const string IdDbFieldEnumeratorName = "UserId";

        protected override string GetTableName() => "Users";

        protected override string[] GetColumns() => new[]
        {
        IdDbFieldEnumeratorName,
            "Username",
        "PasswordHash",
        "FullName"
    };

        protected override DaisExam.Models.User MapEntity(SqlDataReader reader)
        {
            return new DaisExam.Models.User
            {
                UserId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                Username = Convert.ToString(reader["Username"]),
                PasswordHash = Convert.ToString(reader["PasswordHash"]),
                FullName = Convert.ToString(reader["FullName"])
            };
        }

        public Task<int> CreateAsync(DaisExam.Models.User entity)
        {
            throw new NotImplementedException();
        }

        public Task<DaisExam.Models.User> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<DaisExam.Models.User> RetrieveCollectionAsync(UserFilter filter)
        {
            Filter commandFilter = new Filter();

            if (filter.Username is not null)
                commandFilter.AddCondition("Username", filter.Username);

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public Task<bool> UpdateAsync(int objectId, UserUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }
    }
}
