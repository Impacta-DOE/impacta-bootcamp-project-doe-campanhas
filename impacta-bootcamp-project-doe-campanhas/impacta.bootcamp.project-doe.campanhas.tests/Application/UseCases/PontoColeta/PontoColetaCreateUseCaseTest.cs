using System;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.application.UseCases.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta;
using Moq;
using NUnit.Framework;

namespace impacta.bootcamp.project_doe.campanhas.tests.Application.UseCases.PontoColeta
{
    public class PontoColetaCreateUseCaseTest
    {
          [Test]
         public async Task cadastrar_Ponto_Coleta_Sem_CEP()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {
           logradouro ="Rua Lorena",
         numero=30,
         bairro="Engenho Novo",
         cidade="Barueri",
         uf= "SP",
         
         responsavel="Mateus" 
    };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "cep e obrigatorio");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [Test]
        public async Task cadastrar_Ponto_Coleta_CEP_nao_numerico()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {   cep = "cep",
                    logradouro = "Rua Lorena",
                    numero = 30,
                    bairro = "Engenho Novo",
                    cidade = "Barueri",
                    uf = "SP",

                    responsavel = "Mateus"
                };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "cep deve ser numerico");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        [Test]
        public async Task cadastrar_Ponto_Coleta_Sem_Logradouro()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {
                    cep = "06416230",
                  
                    numero = 30,
                    bairro = "Engenho Novo",
                    cidade = "Barueri",
                    uf = "SP",

                    responsavel = "Mateus"
                };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "logradouro e obrigatorio");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [Test]
        public async Task cadastrar_Ponto_Coleta_Sem_Bairro()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {
                    cep = "06416230",
                    logradouro = "Rua Lorena",
                    numero = 30,
                   
                    cidade = "Barueri",
                    uf = "SP",

                    responsavel = "Mateus"
                };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "bairro e obrigatorio");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [Test]
        public async Task cadastrar_Ponto_Coleta_Sem_Cidade()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {
                    cep = "06416230",
                    logradouro = "Rua Lorena",
                    numero = 30,

                 bairro = "Engenho Novo",
                    uf = "SP",

                    responsavel = "Mateus"
                };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "cidade e obrigatoria");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [Test]
        public async Task cadastrar_Ponto_Coleta_Sem_UF()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {
                    cep = "06416230",
                    logradouro = "Rua Lorena",
                    numero = 30,

                    bairro = "Engenho Novo",
                    cidade = "Barueri",
                    responsavel = "Mateus"
                };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "uf e obrigatorio");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        [Test]
        public async Task cadastrar_Ponto_Coleta_UF_invalida()
        {
            try
            {
                PontoDeColetaDTO dto = new PontoDeColetaDTO()
                {
                    cep = "06416230",
                    logradouro = "Rua Lorena",
                    numero = 30,

                    bairro = "Engenho Novo",
                    cidade = "Barueri",
                    uf = "uf",
                    responsavel = "Mateus"
                };

                var mock = new Mock<ICreatePontoColetaRepository>();
                mock.Setup(m => m.createPontoColeta(dto)).Returns(Task.FromResult(new OperationCreatePontoColetaDTO() { sucesso = true }));
                var usecase = new PontoColetaCreateUseCase(mock.Object);
                var response = await usecase.create(dto);

                Assert.IsFalse(response.sucesso);
                Assert.AreEqual(response.error.errorMessage, "uf invalida");
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
