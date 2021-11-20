using System;
namespace impacta.bootcamp.project.doe.campanhas.api.Models.Errors
{
    public class Error
    {
        public Error()
        {
        }

        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}
