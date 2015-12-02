using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using TuulariApi.Models;

namespace TuulariApi
{
    public class ApiAuthorizationAttribute : FilterAttribute, System.Web.Http.Filters.IAuthorizationFilter
    {
        public ApiAuthorizationAttribute() {}

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            IEnumerable<String> header = null;
            var token = "";
            try {
                header = actionContext.Request.Headers.GetValues("Bearer");
                token = (header != null ? header.First() : "");
            }
            catch(Exception e) { }
            
            //System.Diagnostics.Debug.WriteLine("token=" + token);
            if (!IsValid(token))
            {
                return Task.FromResult<HttpResponseMessage>(actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized));
            }
            else
            {
                return continuation();
            }
        }

        private bool IsValid(string val)
        {
            tuulariEntities db = new tuulariEntities();
            AuthToken token = db.AuthTokens.Where(t => t.Token == val).SingleOrDefault();
            //System.Diagnostics.Debug.WriteLine("token=" + token);

            return (token != null);
        }
    }
}
