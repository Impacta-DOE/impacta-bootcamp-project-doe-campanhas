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


$" declare @pessoaId int " +
$" select @pessoaId = p.pessoaId from users us join Pessoa p on p.userId = us.id where us.user_name =@user " +

$" insert into  PontoColeta(" +
$"   pessoaId ," +
$"     cep ," +
$"   logradouro ," +
$"   numero      ," +
$"  bairro        ," +
$"   cidade        , " +
$"     uf        ," +
$"    complemento ," +
$"    responsavel)" +
$" select " +
$"    @pessoaId    ," +
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
