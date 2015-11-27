using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace TuulariApi.Controllers
{
    public class AboutController : ApiController
    {

        public partial class Author
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public partial class Endpoint
        {
            public string URI { get; set; }
            public HttpMethod Method { get; set; }
            public List<EndpointParam> Parameters { get; set; }
        }

        public partial class EndpointParam
        {
            public string Name { get; set; }
            public ApiParameterSource Source { get; set; }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Mvc.ActionName("Authors")]
        [ResponseType(typeof(Author))]
        public IEnumerable<Author> Authors()
        {

            Author[] data = { new Author { Name="Niko Jokipalo", Email="h3136@student.labranet.jamk.fi" }, new Author { Name="Jussi Heikkinen", Email="h....@student.labranet.jamk.fi" } };
            return data;

        }

        [System.Web.Http.HttpGet]
        [System.Web.Mvc.ActionName("Endpoints")]
        [ResponseType(typeof(Endpoint))]
        public IEnumerable<Endpoint> Endpoints()
        {

            var config = ControllerContext.Configuration;// new HttpConfiguration();//
            IApiExplorer apiExplorer = new ApiExplorer(config);//config.Services.GetApiExplorer();

            Endpoint[] data = { };
            int i = 0;

            foreach (ApiDescription api in apiExplorer.ApiDescriptions)
            {
                Endpoint e = new Endpoint { Method = api.HttpMethod, URI = api.RelativePath, Parameters = new List<EndpointParam>() };

                foreach (ApiParameterDescription parameter in api.ParameterDescriptions)
                {
                    if (parameter != null)
                    {
                        e.Parameters.Add(new EndpointParam { Name = parameter.Name, Source = parameter.Source });
                    }
                }
                
                data[i] = e;
                i++;
            }

            return data;

        }

     }
}
