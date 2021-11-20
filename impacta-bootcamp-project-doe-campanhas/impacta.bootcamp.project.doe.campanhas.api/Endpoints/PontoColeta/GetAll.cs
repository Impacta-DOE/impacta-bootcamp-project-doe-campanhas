using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models.Errors;
using impacta.bootcamp.project.doe.campanhas.api.Models.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.PontoColeta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.PontoColeta
{
    public class GetAll: BaseEndPoint<PontoDeColetaGetResponse>
    {
        private readonly IPontoColetaGetAllUseCase pontoColetaGetAllUseCase;
        public GetAll(IPontoColetaGetAllUseCase useCase)
        {
            pontoColetaGetAllUseCase = useCase;
        }
        /// <summary>
        /// Cria um novo ponto de coleta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("pontocoleta")]
        public async override Task<ActionResult<PontoDeColetaGetResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            PontoDeColetaGetResponse response = new PontoDeColetaGetResponse();



            try
            {
         
               
               var user = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await pontoColetaGetAllUseCase.getAll(user);
                var createResponse = dtoToResponse(create);



                if (createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao criar ponto de coleta";

                var resp = new PontoDeColetaGetResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }

        private PontoDeColetaGetResponse dtoToResponse(OperationPontoColetaGetAllDTO operationCreateDTO)
        {
            var response = new PontoDeColetaGetResponse();

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
                response.pontosColeta = operationCreateDTO.pontosColeta;
            }

            return response;
        }
    }
}
