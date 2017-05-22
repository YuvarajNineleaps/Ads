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
    public class AdController : ApiController
    {
        private AdContext db = new AdContext();

        public AdController()
        {
            var result = GetValidateCredential();
            Console.Write(result);
           
        }

        // GET: api/Ad
        /// <summary>
        /// Get all Ads.
        /// </summary>
        public IQueryable<Ad> GetAds()
        {
            IQueryable<Ad> result = null;
            Debug.WriteLine("++++++++++++++++++");
            if (!this.GetValidateCredential())
            {
                result = db.Ads.Include(b => b.Stats);
            }
            return result;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdExists(int id)
        {
            return db.Ads.Count(e => e.Id == id) > 0;
        }

        public bool GetValidateCredential()
        {
            HttpContext httpContext = HttpContext.Current;
            try
            {
                string authHeader = httpContext.Request.Headers["Authorization"];

                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    int seperatorIndex = usernamePassword.IndexOf(':');

                    var userName = usernamePassword.Substring(0, seperatorIndex);
                    var password = usernamePassword.Substring(seperatorIndex + 1);

                    var queryable = db.Auths
                                    .Where(x => x.Name == userName)
                                    .Where(x => x.Password == password);

                    if (queryable == null)
                    {
                        return false;

                    }
                }
                else
                {
                    //Handle what happens if that isn't the case
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

             return true;
        }
    }
}