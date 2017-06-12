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

namespace Ads.Controllers
{
    /// <summary>
    /// Stats controller class.
    /// </summary>
    public class StatsController : ApiController
    {
        private AdContext db = new AdContext();

        // GET: api/Stats
        /// <summary>
        /// Get all stats.
        /// </summary>
        public IQueryable<Stats> GetStats()
        {
            return db.Stats;
        }

        // GET: api/Stats/5
        /// <summary>
        /// Get stats by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Stats))]
        public async Task<IHttpActionResult> GetStats(int id)
        {
            Stats stats = await db.Stats.FindAsync(id);
            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats);
        }

        // PUT: api/Stats/5
        /// <summary>
        /// Put stats by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stats"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStats(int id, Stats stats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stats.Id)
            {
                return BadRequest();
            }

            db.Entry(stats).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatsExists(id))
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

        // POST: api/Stats
        /// <summary>
        /// Post stats
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        [ResponseType(typeof(Stats))]
        public async Task<IHttpActionResult> PostStats(Stats stats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stats.Add(stats);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = stats.Id }, stats);
        }

        // DELETE: api/Stats/5
        /// <summary>
        /// Delete stats by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Stats))]
        public async Task<IHttpActionResult> DeleteStats(int id)
        {
            Stats stats = await db.Stats.FindAsync(id);
            if (stats == null)
            {
                return NotFound();
            }

            db.Stats.Remove(stats);
            await db.SaveChangesAsync();

            return Ok(stats);
        }

        /// <summary>
        /// 
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
        /// Check for stats by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean</returns>
        private bool StatsExists(int id)
        {
            return db.Stats.Count(e => e.Id == id) > 0;
        }
    }
}