using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity; // tää piti includettaa että *async-kutsut tulee kaikille entityille
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using TuulariApi.Models;
using System.Web.Http.Description;
using System.Text;

namespace TuulariApi.Controllers
{
    public class AuthController : ApiController
    {

        private tuulariEntities db = new tuulariEntities();

        /*public async Task<IHttpActionResult> GetToken([FromBody]FormDataCollection collection)
        {

            String email = @collection["email"].Trim() ?? "";

            if (email.Length == 0)
            {
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }

            String guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            User user = await db.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            AuthToken token = new AuthToken { UserId = user.Id, GenerationTime = new DateTime(), Token = guid };

            db.AuthTokens.Add(token);
            await db.SaveChangesAsync();

            return Ok(new { token = guid });

        }*/

        // POST: Auth/Login
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostLogin() //[FromBody]FormDataCollection collection)
        {

            //String email = @collection["email"].Trim() ?? "",
            //        password = @collection["password"].Trim() ?? "";

            var auth = Request.Headers.Authorization;
            string scheme = auth.Scheme;
            String[] data = Encoding.UTF8.GetString(Convert.FromBase64String(auth.Parameter)).Split(':');
            string email = data[0].Trim() ?? "", password = data[1].Trim() ?? "";

            System.Diagnostics.Debug.WriteLine("mail=" + email + ", password=" + password);

            if (email.Length == 0 || password.Length == 0)
            {
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }

            var query = db.Users.Where(u => u.Email == email);
            User user = await query.SingleOrDefaultAsync();
            System.Diagnostics.Debug.WriteLine("query=" + query.Count());
            System.Diagnostics.Debug.WriteLine("user=" + user);

            if (user == null)
            {
                return NotFound();
            }
            //else if (user.Password != password)
            else if (DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(password, user.Password))
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }

            return Ok(user); // TODO: vai 302?

        }

        public async Task<IHttpActionResult> GetLogout()
        {

            return StatusCode(System.Net.HttpStatusCode.NoContent);

        }

    }
}
