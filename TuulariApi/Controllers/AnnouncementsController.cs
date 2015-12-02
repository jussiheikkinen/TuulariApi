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
    [ApiAuthorization]
    public class AnnouncementsController : ApiController
    {
        private tuulariEntities db = new tuulariEntities();

        // GET: api/Announcements
        public IQueryable<Announcement> GetAnnouncements()
        {
            return db.Announcements;
        }

        // GET: api/Announcements/5
        [ResponseType(typeof(Announcement))]
        public async Task<IHttpActionResult> GetAnnouncement(int id)
        {
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return Ok(announcement);
        }

        // PUT: api/Announcements/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnnouncement(int id, Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != announcement.Id)
            {
                return BadRequest();
            }

            db.Entry(announcement).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(id))
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

        // POST: api/Announcements
        [ResponseType(typeof(Announcement))]
        public async Task<IHttpActionResult> PostAnnouncement(Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Announcements.Add(announcement);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = announcement.Id }, announcement);
        }

        // DELETE: api/Announcements/5
        [ResponseType(typeof(Announcement))]
        public async Task<IHttpActionResult> DeleteAnnouncement(int id)
        {
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            db.Announcements.Remove(announcement);
            await db.SaveChangesAsync();

            return Ok(announcement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnnouncementExists(int id)
        {
            return db.Announcements.Count(e => e.Id == id) > 0;
        }
    }
}