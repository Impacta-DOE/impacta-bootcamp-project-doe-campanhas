using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.Entities.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
namespace impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context
{
    public class SqlContext:IDisposable
    {
        private SqlConnection _sqlConnection;
        private readonly ConnectionStringsSetings _connectionStringsSetings;

        public SqlContext(IOptions<ConnectionStringsSetings> options)
        {
            _connectionStringsSetings = options.Value;
            _sqlConnection = new SqlConnection(_connectionStringsSetings.connectionStringBD);
        }

        public async Task<SqlConnection> GetConnection()
        {
            if(_sqlConnection.State == System.Data.ConnectionState.Closed || _sqlConnection.State == System.Data.ConnectionState.Broken)
            {
                await _sqlConnection.OpenAsync();
            }
            return _sqlConnection;
        }

        public void Dispose()
        {
            Task.Run(() => _sqlConnection.CloseAsync());
        }
    }
}
