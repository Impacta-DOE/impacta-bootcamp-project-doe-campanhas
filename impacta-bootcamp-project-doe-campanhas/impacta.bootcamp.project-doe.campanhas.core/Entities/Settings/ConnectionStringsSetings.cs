using System;
namespace impacta.bootcamp.project_doe.campanhas.core.Entities.Settings
{
    public class ConnectionStringsSetings
    {
        public string connectionStringBD { get; set; }
        public string connectionStringBlob { get; set; }
        public string blobName { get; set; }
        public static ConnectionStringsSetings Properties;


        public ConnectionStringsSetings()
        {
            Properties = this;
        }
    }
}
