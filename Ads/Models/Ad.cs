using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ads.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int StatId { get; set; }
    }
}