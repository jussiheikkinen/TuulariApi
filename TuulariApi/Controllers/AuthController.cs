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

        // POST: Auth/Login
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> Login()
        {

            db.Configuration.ProxyCreationEnabled = false;

            var auth = Request.Headers.Authorization ?? null;
            String[] data = Encoding.UTF8.GetString(Convert.FromBase64String(auth.Parameter)).Split(':');
            string email = data[0].Trim() ?? "", password = data[1].Trim() ?? "";
            //System.Diagnostics.Debug.WriteLine("mail=" + email + ", password=" + password);

            if (email.Length == 0 || password.Length == 0)
            {
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }

            var query = db.Users.Where(u => u.Email == email);
            User user = await query.SingleOrDefaultAsync();
            //var user = from c in db.Users where (c.Email = email) select (c.Id, c.Nickname, c.Email, c.UserDatas, c.AuthTokens);
            //System.Diagnostics.Debug.WriteLine("query=" + query.Count());
            //System.Diagnostics.Debug.WriteLine("user=" + user);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    if (!DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(password, user.Password))
                    {
                        return StatusCode(System.Net.HttpStatusCode.Unauthorized);
                    }
                } catch (Exception e)
                {
                    //System.Diagnostics.Debug.WriteLine(e.Message);
                    return StatusCode(System.Net.HttpStatusCode.NotFound);
                }
            }

            AuthToken token = await UpdateToken(user.Id);
            user.Password = null;

            return Ok(user);

        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Logout()
        {

            try {
                var header = Request.Headers.GetValues("Bearer");

                if (header != null)
                {
                    string value = header.First();
                    AuthToken token = await db.AuthTokens.Where(t => t.Token == value).FirstOrDefaultAsync();

                    db.AuthTokens.Remove(token);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return StatusCode(System.Net.HttpStatusCode.NoContent);

        }

        private async Task<AuthToken> UpdateToken(int userId)
        {

            if (userId == 0)
            {
                return null;
            }

            AuthToken token = await db.AuthTokens.Where(t => t.UserId == userId).FirstOrDefaultAsync();

            if (token == null)
            {
                String guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                token = new AuthToken { UserId = userId, GenerationTime = DateTime.Now, Token = guid };
                db.AuthTokens.Add(token);
            }
            
            await db.SaveChangesAsync();

            return token;

        }

    }
}
