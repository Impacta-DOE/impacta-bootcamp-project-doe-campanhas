using System;
using System.Threading.Tasks;
using Dapper;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories;
using impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context;
using System.Linq;
namespace impacta.bootcamp.project_doe.campanhas.infra.data.Data.Repositories
{
    public class CreateRepository : ICreateRepository
    {
        private readonly SqlContext context;
        public CreateRepository(SqlContext sqlContext)
        {
            context = sqlContext;
        }

        public async Task<OperationCreateDTO> commentCampaign(CommentDTO dto)
        {
            string insertComentario = $" declare @userId int " +
        $" select @userId = us.id from users us  where us.user_name =@userName " +
         " insert into campanhaComentario (descricao,campanhaId,userId) select @comentario , @campanhaId , @userId";

            try
            {

                var sqlCon = await context.GetConnection();



                using (var transaction = sqlCon.BeginTransaction())
                {

                    try
                    {
                     

                        if (!string.IsNullOrWhiteSpace(dto.comentario))
                        {
                            sqlCon.Query<int>(insertComentario, dto, transaction);

                        }

                        transaction.Commit();

                        return new OperationCreateDTO() { sucesso = true };
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

        public async Task<OperationCreateDTO> createCampaign(CreateDTO dto)
        {
           string insert  = $"declare @campanhaId int " +


$" declare @userId int " +
$" select @userId = us.id from users us  where us.user_name =@user " +

$" insert into Campanha ("+
$"    titulo    ,"+
$"    descricao  ,"+
$"    tipoCampanhaId ,"+
$"    tipoArrecadacaoId ," +
$"    unidadeMedidaId," +
$"    userId ," +
$"    urlImage ," +
$"    metaArrecadacao ," +
$"    dataLimite ," +
$"    imageCard ," +
$"    indVoluntariado ," +
$"   descricaoVoluntariado," +
$"   estaAtiva," +
$"    dataInclusao)" +
$" select " +
$"    @titulo    ," +
$"    @descricao  ," +
$"    @tipoCampanhaId ," +
$"    @tipoArrecadacaoId ," +
$"    @unidadeMedidaId," +
$"    @userId ," +
$"    @urlImage ,"+
$"    @metaArrecadacao ," +
$"    @dataLimite ," +
$"    @urlImageCard ," +
$"    @indPrecisaVolutario ," +
$"    @descricaoVoluntario ," +
$"   1 ," +
$"    GETDATE()" +
$" SET @campanhaId = SCOPE_IDENTITY()" +
$" SELECT  @campanhaId ";

          


            try 
            {
            
                    var sqlCon = await context.GetConnection();
             


                using (var transaction = sqlCon.BeginTransaction())
                {

                    try
                    {
                        var campanhaId = sqlCon.Query<int>(insert, dto, transaction).Single();

                        if(dto.pontosColeta != null && dto.pontosColeta.Count > 0)
                        {
                        
                            foreach( var item in dto.pontosColeta)
                            {
                                DynamicParameters parameters = new DynamicParameters(new
                                {
                                    pontoColetaId = item.id,
                                    campanhaId = campanhaId
                                });
                                sqlCon.Execute("insert into PontoColetaCampanha (pontoColetaId,campanhaId)  VALUES(@pontoColetaId, @campanhaId) ", parameters, transaction);
                            }

                            
                        }
                       
                        transaction.Commit();

                        return new OperationCreateDTO() { sucesso = true };
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
