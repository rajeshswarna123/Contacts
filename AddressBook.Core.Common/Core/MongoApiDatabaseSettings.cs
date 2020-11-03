using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Common.Core
{
    public class MongoApiDatabaseSettings : IMongoApiDatabaseSettings
    {
        public string UsersCollectionShardKey { get; set; }

        public string ContactsCollectionSharedKey { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
    public interface IMongoApiDatabaseSettings
    {
        string UsersCollectionShardKey { get; set; }

        string ContactsCollectionSharedKey { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
