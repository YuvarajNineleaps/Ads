﻿using System;
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
    public class StatsController : ApiController
    {
        private AdContext db = new AdContext();

        // GET: api/Stats
        public IQueryable<Stats> GetStats()
        {
            return db.Stats;
        }

        // GET: api/Stats/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatsExists(int id)
        {
            return db.Stats.Count(e => e.Id == id) > 0;
        }
    }
}