using System;
using System.Threading.Tasks;
using Dapper;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context;

namespace impacta.bootcamp.project_doe.campanhas.infra.data.Data.Repositories.PontoDeColeta
{
    public class CreatePontoColetaRepository : ICreatePontoColetaRepository
    {

        private readonly SqlContext context;
        public CreatePontoColetaRepository(SqlContext sqlContext)
        {
            context = sqlContext;
        }
        public async Task<OperationCreatePontoColetaDTO> createPontoColeta(PontoDeColetaDTO dto)
        {

            try
            {

                var sqlCon = await context.GetConnection();

                string insert = $"declare @campanhaId int " +


$" declare @userId int " +
$" select @userId = us.id from users us  where us.user_name =@user " +

$" insert into  PontoColeta(" +
$"   userId ," +
$"     cep ," +
$"   logradouro ," +
$"   numero      ," +
$"  bairro        ," +
$"   cidade        , " +
$"     uf        ," +
$"    complemento ," +
$"    responsavel)" +
$" select " +
$"    @userId    ," +
$"    @cep  ," +
$"    @logradouro ," +
$"    @numero ," +
$"    @bairro," +
$"    @cidade ," +
$"    @uf ," +
$"    @complemento ," +
$"    @responsavel ";




                using (var transaction = sqlCon.BeginTransaction())
                {

                    try
                    {
                      sqlCon.Execute(insert, dto, transaction);

                 

                        transaction.Commit();

                        return new OperationCreatePontoColetaDTO() { sucesso = true };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
