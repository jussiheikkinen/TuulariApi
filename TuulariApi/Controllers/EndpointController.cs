using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TuulariApi.Controllers
{
    public class EndpointController : ApiController
    {
        // GET: api/Endpoint
        public String Get()
        {
            var endpoints = new
            {
                url = "plaah.poop.com/api/Users",
                method = "GET",
                parameters = 0
            };

            string json = JsonConvert.SerializeObject(endpoints);
            string jsonFormatted = JValue.Parse(json).ToString(Formatting.None);
            return jsonFormatted;
        }
    }
}
