using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TuulariApi.Models;

namespace TuulariApi.Controllers
{
    public class AuthTokensController : ApiController
    {
        private tuulariEntities db = new tuulariEntities();

        // GET: api/AuthTokens
        public IQueryable<AuthToken> GetAuthTokens()
        {
            return db.AuthTokens;
        }

        // GET: api/AuthTokens/5
        [ResponseType(typeof(AuthToken))]
        public async Task<IHttpActionResult> GetAuthToken(int id)
        {
            AuthToken authToken = await db.AuthTokens.FindAsync(id);
            if (authToken == null)
            {
                return NotFound();
            }

            return Ok(authToken);
        }

        // PUT: api/AuthTokens/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAuthToken(int id, AuthToken authToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != authToken.Id)
            {
                return BadRequest();
            }

            db.Entry(authToken).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthTokenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AuthTokens
        [ResponseType(typeof(AuthToken))]
        public async Task<IHttpActionResult> PostAuthToken(AuthToken authToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AuthTokens.Add(authToken);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = authToken.Id }, authToken);
        }

        // DELETE: api/AuthTokens/5
        [ResponseType(typeof(AuthToken))]
        public async Task<IHttpActionResult> DeleteAuthToken(int id)
        {
            AuthToken authToken = await db.AuthTokens.FindAsync(id);
            if (authToken == null)
            {
                return NotFound();
            }

            db.AuthTokens.Remove(authToken);
            await db.SaveChangesAsync();

            return Ok(authToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthTokenExists(int id)
        {
            return db.AuthTokens.Count(e => e.Id == id) > 0;
        }
    }
}