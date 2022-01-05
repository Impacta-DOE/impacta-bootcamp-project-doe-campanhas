using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Entities.Settings;
using impacta.bootcamp.project_doe.campanhas.core.Enums;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create;
using impacta.bootcamp.project_doe.campanhas.shared.Azure;
using Microsoft.Extensions.Options;

namespace impacta.bootcamp.project.doe.campanhas.application.UseCases.Create
{
    public class CreateUseCase : ICreateUseCase
    {

        private readonly ICreateRepository repo;
        private  ConnectionStringsSetings _connectionStringsSetings;

        public CreateUseCase( ICreateRepository createRepository, IOptions<ConnectionStringsSetings> connectionStringsSetings)
        {
            repo = createRepository;
            _connectionStringsSetings = connectionStringsSetings.Value;
        }
      
      
        public async Task<OperationCreateDTO> create(CreateDTO request)
        {
            OperationCreateDTO operationCreateDTO = new OperationCreateDTO()
            {
                sucesso = true
            };
             operationCreateDTO =  await isValidCreate(request);
            if (operationCreateDTO.sucesso) 
            {
                request.urlImage = await SaveImage(request.imageCapaBase64);
                request.urlImageCard = await SaveImage(request.imageCardBase64);    
                operationCreateDTO = await repo.createCampaign(request);
            }


            return operationCreateDTO;
        }
        private async Task<string> SaveImage(string Base64)
        {
            var arquivo = BlobManager.BuilderArquivoBlob(Base64, fileType.IMAGE);

            BlobManager blob = new BlobManager(_connectionStringsSetings.connectionStringBlob,_connectionStringsSetings.blobName,arquivo.name,true);
            blob.gravaArquivoBlobStorage(arquivo);

            return arquivo.name;
        }

        private async Task<OperationCreateDTO> isValidCreate(CreateDTO request)
        {
            OperationCreateDTO operationCreateDTO = new OperationCreateDTO() {
                sucesso =true
            };
            if(string.IsNullOrWhiteSpace(request.titulo))
            {
                operationCreateDTO.error = new ErrorDTO()
                { errorCode = "VAL01",
                    errorMessage = "titulo e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;

            }
            if (string.IsNullOrWhiteSpace(request.descricao))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL02",
                    errorMessage = "descricao e obrigatoria"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;

            }
            if(request.tipoArrecadacaoId ==  null)
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL03",
                    errorMessage = "tipoArrecadacaoId e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if (request.tipoCampanhaId == null)
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL04",
                    errorMessage = "tipoCampanhaId e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if( !string.IsNullOrWhiteSpace(request.imageCapaBase64) && !IsBase64String(request.imageCapaBase64))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL05",
                    errorMessage = "imageCapaBase64 nao corresponde a uma string base64"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
          if(!Enum.IsDefined<TipoCampanha>((TipoCampanha)request.tipoCampanhaId))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL06",
                    errorMessage = "tipoCampanhaId invalido"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }

            if(request.tipoCampanhaId == (int)TipoCampanha.CAMPANHA && string.IsNullOrWhiteSpace( request.dataLimite))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL07",
                    errorMessage = "dataLimite obrigatoria para o tipoCampanha Campanha"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if(string.IsNullOrWhiteSpace(request.user))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL08",
                    errorMessage = "pessoaId e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }

            return operationCreateDTO;
        }
        private  bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
    }
}
