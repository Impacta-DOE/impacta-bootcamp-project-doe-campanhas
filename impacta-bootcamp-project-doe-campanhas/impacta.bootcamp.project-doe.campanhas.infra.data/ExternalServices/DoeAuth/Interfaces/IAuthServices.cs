using System;
using System.Threading.Tasks;

namespace impacta.bootcamp.project_doe.campanhas.infra.data.ExternalServices.DoeAuth.Interfaces
{
    public interface IAuthServices
    {

        public Task<impacta.bootcamp.project_doe.campanhas.core.Entities.DoeAuth.UserAuth> validate(string Token);
    }
}
