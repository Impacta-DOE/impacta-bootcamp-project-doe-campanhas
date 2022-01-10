using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.Create;
using impacta.bootcamp.project.doe.campanhas.api.Models.Errors;
using impacta.bootcamp.project.doe.campanhas.api.Models.PlanoMensal;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create;
using impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.PlanoMensal
{
    [ApiController]
    [Authorize]
    public class AdesaoPlan : ControllerBase
    {
        private readonly ICreateUseCase useCase;
          private readonly SqlContext context;

       
    
    public AdesaoPlan(ICreateUseCase createUseCase, SqlContext sqlContext)

    {
            useCase = createUseCase;
        context = sqlContext;

    }

        /// <summary>
        /// adere um plano
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("planos/adesao")]

        public async Task<ActionResult<PlanoMensalResponse>> HandleAsync([FromQuery] int planoMensalId, CancellationToken cancellationToken = default)
        {
            PlanoMensalResponse response = new PlanoMensalResponse();



            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    string msg = $"Erro ao criar campanha, request invalido" + modelErrors.ElementAt(0).ErrorMessage;

                    var responseError = new PlanoMensalResponse()
                    {
                        status = "400",
                        error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                    };
                    return BadRequest(responseError);

                }


                // model.user = "damian_lindgren@yahoo.com";
                var userName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await useCase.adPlan(planoMensalId, userName);
                var createResponse = dtoToResponse(create);



                if (createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao aderir plano" + ex.ToString();

                var resp = new PlanoMensalResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }


        /// <summary>
        /// cancela um plano
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("planos/adesao/cancelamento")]
        public async Task<ActionResult<PlanoMensalResponse>> HandleAsyncCancel([FromQuery] int planoMensalId, CancellationToken cancellationToken = default)
        {
            PlanoMensalResponse response = new PlanoMensalResponse();



            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    string msg = $"Erro ao criar campanha, request invalido" + modelErrors.ElementAt(0).ErrorMessage;

                    var responseError = new PlanoMensalResponse()
                    {
                        status = "400",
                        error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                    };
                    return BadRequest(responseError);

                }


                // model.user = "damian_lindgren@yahoo.com";
                var userName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await useCase.cancelPlan(planoMensalId, userName);
                var createResponse = dtoToResponse(create);



                if (createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao aderir plano" + ex.ToString();

                var resp = new PlanoMensalResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }
      
       

        [HttpGet("planos/getall")]
        public async Task<ActionResult<List<PlanoMensalModel>>> HandleAsyncPlanoMensal()
        {
            try
            {

                var resp = await getAllResponseOrganizacoes();
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
        private async Task<List<PlanoMensalModel>> getAllResponseOrganizacoes()
        {
            StringBuilder resp = new StringBuilder();
            List<PlanoMensalModel> organizacaoGetAllResponse = new List<PlanoMensalModel>();
            var userName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
            using (SqlConnection conn = await context.GetConnection())

            {
                using (SqlCommand sqlCommand = new SqlCommand("pr_planoMensal_sel", conn))
                {

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.Add("@userName", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@userName"].Value = userName;

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


                    organizacaoGetAllResponse = JsonConvert.DeserializeObject<List<PlanoMensalModel>>(resp.ToString());
                }


                return organizacaoGetAllResponse;
            }
        }


       
        private PlanoMensalResponse dtoToResponse(OperationCreateDTO operationCreateDTO)
        {
            var response = new PlanoMensalResponse();

            if (!operationCreateDTO.sucesso)

            {

                if (operationCreateDTO.error != null)
                {
                    Error error = new Error() { errorCode = operationCreateDTO.error.errorCode, errorMessage = operationCreateDTO.error.errorMessage };

                    response.error = error;
                }
                response.status = "400";

            }
            else
            {
                response.status = "200";
            }

            return response;
        }
    }
}