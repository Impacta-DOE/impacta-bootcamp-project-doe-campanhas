using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models;
using impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas;
using impacta.bootcamp.project.doe.campanhas.api.Models.Errors;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.Campanhas
{
    public class Comment : BaseEndPoint<CommentRequest, BaseResponse>
    {

        private readonly ICreateUseCase useCase;
        public Comment(ICreateUseCase createUseCase)

        {
            useCase = createUseCase;
        }

        /// <summary>
        /// Cria uma comentario para  campanha
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("campanhas/comment")]
        public async override Task<ActionResult<BaseResponse>> HandleAsync(CommentRequest request, CancellationToken cancellationToken = default)
        {
            BaseResponse response = new BaseResponse();



            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    string msg = $"Erro ao criar campanha, request invalido" + modelErrors.ElementAt(0).ErrorMessage;

                    var responseError = new BaseResponse()
                    {
                        status = "400",
                        error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                    };
                    return BadRequest(responseError);

                }
                var model = ModelToDTO(request);

                // model.user = "damian_lindgren@yahoo.com";
                model.userName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await useCase.comment(model);
                var createResponse = dtoToResponse(create);



                if (createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao criar campanha" + ex.ToString();

                var resp = new BaseResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }

        private CommentDTO ModelToDTO(CommentRequest request)
        {

            CommentDTO dTO = new CommentDTO()
            {
                comentario = request.comentario,
                campanhaId = request.campanhaId
            };
            return dTO;
        }
        private BaseResponse dtoToResponse(OperationCreateDTO operationCreateDTO)
        {
            var response = new BaseResponse();

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
