using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.Entities.DoeAuth;
using impacta.bootcamp.project_doe.campanhas.infra.data.ExternalServices.DoeAuth.Interfaces;
using Microsoft.Extensions.Configuration;

namespace impacta.bootcamp.project_doe.campanhas.infra.data.ExternalServices.DoeAuth.Services
{
    public class AuthServices:IAuthServices
    {
        private readonly IConfiguration _configuration;
        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UserAuth> validate(string Token)
        {
            string url = _configuration["DoeAuth:urlBase"];

            try
            {
                var root = new Dictionary<string, string>();
                root.Add("token", Token);

                var json = JsonSerializer.Serialize(root);
                var cts = new CancellationToken();

                using (var content  = new StringContent(json,System.Text.Encoding.UTF8,"application/json"))
                {
                    using (var client = new HttpClient())
                    {
                     


                        HttpResponseMessage result = await client.PostAsync(url,content, cts);
                        int responseCode = (int)result.StatusCode;
                        if (responseCode == 200 || responseCode == 201)
                        {
                            return JsonSerializer.Deserialize<UserAuth>(await result.Content.ReadAsStringAsync());
                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;

        }
    }
}
