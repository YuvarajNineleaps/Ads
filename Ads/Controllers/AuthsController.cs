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

namespace Ads.Controllers
{
    /// <summary>
    /// Auths controller class.
    /// </summary>
    public class AuthsController : ApiController
    {
        private AdContext db = new AdContext();

        // GET: api/Auths
        /// <summary>
        /// Get all auth data.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Auth> GetAuths()
        {
            return db.Auths;
        }

        // GET: api/Auths/5
        /// <summary>
        /// Get auth by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Auth))]
        public async Task<IHttpActionResult> GetAuth(int id)
        {
            Auth auth = await db.Auths.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            return Ok(auth);
        }

        // PUT: api/Auths/5
        /// <summary>
        /// Put auth by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAuth(int id, Auth auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != auth.Id)
            {
                return BadRequest();
            }

            db.Entry(auth).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthExists(id))
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

        // POST: api/Auths
        /// <summary>
        /// Post auth.
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        [ResponseType(typeof(Auth))]
        public async Task<IHttpActionResult> PostAuth(Auth auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Auths.Add(auth);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = auth.Id }, auth);
        }

        // DELETE: api/Auths/5
        /// <summary>
        /// Delete auth by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Auth))]
        public async Task<IHttpActionResult> DeleteAuth(int id)
        {
            Auth auth = await db.Auths.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            db.Auths.Remove(auth);
            await db.SaveChangesAsync();

            return Ok(auth);
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

        private bool AuthExists(int id)
        {
            return db.Auths.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Boolean</returns>
        [NonAction]
        public bool VaidateUser(string userName, string password)
        {
            // Check if it is valid credential  
            var queryable = db.Auths
                            .Where(x => x.Name == userName)
                            .Where(x => x.Password == password);
            if (queryable != null)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    
}