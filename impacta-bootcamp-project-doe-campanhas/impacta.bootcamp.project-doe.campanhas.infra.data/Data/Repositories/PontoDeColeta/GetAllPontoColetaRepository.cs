using System;

using System.Threading.Tasks;
using Dapper;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context;

namespace impacta.bootcamp.project_doe.campanhas.infra.data.Data.Repositories.PontoDeColeta
{
    public class GetAllPontoColetaRepository: IGetAllPontoColetaRepository
    {

        private readonly SqlContext context;
        public GetAllPontoColetaRepository(SqlContext sqlContext)
        {
            context = sqlContext;
        }

        public async Task<OperationPontoColetaGetAllDTO> getAll(string user)
        {
            try
            {

                var sqlCon = await context.GetConnection();

                DynamicParameters parameters = new DynamicParameters(new
                {
                    user = user,
                  
                });

                string select = $" declare @userId int " +
$" select @userId = us.id from users us  where us.user_name =@user " +

$" select * from  PontoColeta where userId = @userId";




                        var pontosDeColeta =  sqlCon.Query<PontoDeColetaDTO>(select, parameters);



                      

                        return new OperationPontoColetaGetAllDTO() { sucesso = true , pontosColeta = pontosDeColeta.AsList()};
                    
                  

                

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
