using Cassandra;

namespace CassandraAPI.Services
{
    public class CassandraService
    {
        private readonly Cassandra.ISession _session;

        public CassandraService()
        {
            var cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .WithPort(9042)
                .Build();

            _session = cluster.Connect("studyhub");
        }

        public Cassandra.ISession GetSession()
        {
            return _session;
        }
    }
}