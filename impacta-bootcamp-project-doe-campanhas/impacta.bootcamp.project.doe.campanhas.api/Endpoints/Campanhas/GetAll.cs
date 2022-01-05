using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.Create;
using impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.GetAll;
using impacta.bootcamp.project_doe.campanhas.core.Entities.Settings;
using impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.Campanhas
{
    [ApiController]
    public class GetAll : ControllerBase
    {
        private readonly SqlContext context;
        public GetAll(SqlContext sqlContext)
        {
            context = sqlContext;
        }
        private readonly ConnectionStringsSetings _connectionStringsSetings;
        [HttpGet("campanhas/getall")]
        public async Task<ActionResult<List<CampanhaGetAllResponse>>> HandleAsync([FromQuery]string nome,[FromQuery] string uf, [FromQuery] string cidade, [FromQuery] int? tipoArrecadacaoId,[FromQuery] string documento)
        {
            try
            {

                var resp = await getAllResponse(nome, uf, cidade, tipoArrecadacaoId, documento);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao  campanha" + ex.ToString();

                var resp = new CreateResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }
        [HttpGet("organizacoes/getall")]
        public async Task<ActionResult<List<OrganizacaoGetAllResponse>>> HandleAsyncOrganizacao([FromQuery] string nome, [FromQuery] string uf, [FromQuery] string cidade, [FromQuery] int? tipoArrecadacaoId, [FromQuery] string documento)
        {
            try
            {

                var resp = await getAllResponseOrganizacoes( documento);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao  buscar organizacoes" + ex.ToString();

                var resp = new CreateResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }
        private async Task<List<OrganizacaoGetAllResponse>> getAllResponseOrganizacoes(string documento)
        {
            StringBuilder resp = new StringBuilder();
            List<OrganizacaoGetAllResponse> organizacaoGetAllResponse = new List<OrganizacaoGetAllResponse>();
            using (SqlConnection conn = await context.GetConnection())
            {
                using (SqlCommand sqlCommand = new SqlCommand("pr_organizacao_sel", conn))
                {

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
              
                    sqlCommand.Parameters.Add("@documento", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@documento"].Value = documento;

                    if (sqlCommand.Connection.State.ToString() == "Closed")
                    {
                        conn.Open();
                    }
                    var sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (sqlDataReader.Read())
                    {
                        resp.Append(sqlDataReader.GetString(0));
                    }
                    Console.WriteLine(resp.ToString());


                    organizacaoGetAllResponse = JsonConvert.DeserializeObject<List<OrganizacaoGetAllResponse>>(resp.ToString());
                }


                return organizacaoGetAllResponse;
            }
        }


                private async Task<List<CampanhaGetAllResponse>> getAllResponse( string nome,  string uf, string cidade, int? tipoArrecadacaoId, string documento)
        {
            StringBuilder resp = new StringBuilder();
            List < CampanhaGetAllResponse> campanhaGetAllResponse = new List<CampanhaGetAllResponse>();
            using (SqlConnection conn = await context.GetConnection())
            {
                using (SqlCommand sqlCommand = new SqlCommand("pr_campanha_sel", conn))
                {
                   
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@nome", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@nome"].Value = nome;

                    sqlCommand.Parameters.Add("@uf", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@uf"].Value = uf;
                    sqlCommand.Parameters.Add("@cidade", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@cidade"].Value = cidade;
                    sqlCommand.Parameters.Add("@tipoArrecadacaoId", System.Data.SqlDbType.TinyInt);
                    sqlCommand.Parameters["@tipoArrecadacaoId"].Value = tipoArrecadacaoId;
                    sqlCommand.Parameters.Add("@documento", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@documento"].Value = documento;

                  if(sqlCommand.Connection.State.ToString() == "Closed")
                    {
                        conn.Open();
                    }
                    var sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while(sqlDataReader.Read())
                    {
                        resp.Append(sqlDataReader.GetString(0));
                    }
                    Console.WriteLine(resp.ToString());
                   
                 
                    campanhaGetAllResponse = JsonConvert.DeserializeObject<List<CampanhaGetAllResponse>>(resp.ToString());
                }


                return campanhaGetAllResponse;
            }
               }

            }

        }
