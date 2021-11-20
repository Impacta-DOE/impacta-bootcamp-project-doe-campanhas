using System;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.application.UseCases.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta;
using Moq;
using NUnit.Framework;

namespace impacta.bootcamp.project_doe.campanhas.tests.Application.UseCases.PontoColeta
{
    public class PontoColetaGetAllUseCaseTest
    {
        [Test]
        public async Task buscarPontoColetaSemUSer()
        {
            try
            {
                
              

                var mock = new Mock<IGetAllPontoColetaRepository>();
                mock.Setup(m => m.getAll("")).Returns(Task.FromResult(new OperationPontoColetaGetAllDTO() { sucesso = true }));
                var usecase = new PontoColetaGetAllUseCase(mock.Object);
                var response = await usecase.getAll("");

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "U");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
