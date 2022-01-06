using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;

namespace impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create
{
    public interface ICreateUseCase
    {

        public Task<OperationCreateDTO> create(CreateDTO request);
        public Task<OperationCreateDTO> comment(CommentDTO request);
    }
}
