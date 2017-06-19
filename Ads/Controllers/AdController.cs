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
using Ads.Models;
using System.Web;
using System.Text;
using System.Diagnostics;

namespace Ads.Controllers
{
    /// <summary>
    /// AdController class.
    /// </summary>
    [BasicAuthentication]
    public class AdController : ApiController
    {
        private readonly AdContext db = new AdContext();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AdController()
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AdController(AdContext context)
        {
            this.db = context;
        }

        // GET: api/Ad
        /// <summary>
        /// Get all Ads.
        /// </summary>
        public IQueryable<Ad> GetAds()
        {
            return db.Ads.Include(b => b.Stats);
        }

        // GET: api/Ad/5
        /// <summary>
        /// Get ad by ID.
        /// </summary>
        /// <param name="id"></param>
        [ResponseType(typeof(Ad))]
        public async Task<IHttpActionResult> GetAd(int id)
        {
            Ad ad = await db.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            return Ok(ad);
        }


        // PUT: api/Ad/5
        /// <summary>
        /// Put ad by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ad"></param>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAd(int id, Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ad.Id)
            {
                return BadRequest();
            }

            db.Entry(ad).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/Ad
        /// <summary>
        /// Post ad.
        /// </summary>
        /// <param name="ad"></param>
        [ResponseType(typeof(Ad))]
        public async Task<IHttpActionResult> PostAd(Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ads.Add(ad);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ad.Id }, ad);
        }

        // DELETE: api/Ad/5
        /// <summary>
        /// Delete ad by ID.
        /// </summary>
        /// <param name="id"></param>
        [ResponseType(typeof(Ad))]
        public async Task<IHttpActionResult> DeleteAd(int id)
        {
            Ad ad = await db.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            db.Ads.Remove(ad);
            await db.SaveChangesAsync();

            return Ok(ad);
        }

        /// <summary>
        /// Disposing of database.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Check for existing of ad by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean.</returns>
        private bool AdExists(int id)
        {
            return db.Ads.Count(e => e.Id == id) > 0;
        }

    }
}