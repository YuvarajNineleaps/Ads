﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ads.Models
{
    /// <summary>
    /// Stats model class.
    /// </summary>
    public class Stats
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public double Price { get; set; }

    }
}