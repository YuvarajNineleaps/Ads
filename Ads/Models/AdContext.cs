using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MySql.Data.Entity;

namespace Ads.Models
{
    /// <summary>
    /// Ad context class.
    /// </summary>
    public class AdContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        /// <summary>
        /// Ad context class constructor.        
        /// </summary>
        public AdContext() : base("name=AdContext")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual  System.Data.Entity.DbSet<Ads.Models.Ad> Ads { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual  System.Data.Entity.DbSet<Ads.Models.Stats> Stats { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual  System.Data.Entity.DbSet<Ads.Models.Auth> Auths { get; set; }
    }
}
