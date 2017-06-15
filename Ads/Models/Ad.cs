using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ads.Models
{
    /// <summary>
    /// Ad model class.
    /// </summary>
    public class Ad
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int StatId { get; set; }

        [ForeignKey("StatId")]
        public Stats Stats { get; set; }
    }
}