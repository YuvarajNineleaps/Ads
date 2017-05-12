using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ads.Models;

namespace Ads.Controllers
{
    public class AdsController : ApiController
    {
        Ad[] ads = new Ad[]
        {
            new Ad {Id = 1, Name = "Android", Priority=1 },
            new Ad {Id = 2, Name = "Web", Priority=3 },
            new Ad {Id = 3, Name = "iOS", Priority=2 }
        };  
        
        public IEnumerable<Ad> GetAllAds()
        {
            return ads;
        }

        public IHttpActionResult GetAd(int id)
        {
            var advertise = ads.FirstOrDefault((p) => p.Id == id);
            if (advertise == null)
            {
                return NotFound();
            }
            return Ok(advertise);
        }
    }
}
