﻿using System.Data.Entity;

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
        /// Ads DbSet.
        /// </summary>
        public virtual DbSet<Ad> Ads { get; set; }

        /// <summary>
        /// Stats DbSet.
        /// </summary>
        public virtual DbSet<Stats> Stats { get; set; }

        /// <summary>
        /// Auths DbSet.
        /// </summary>
        public virtual DbSet<Auth> Auths { get; set; }

        /// <summary>
        /// Method to set EntityState in Ad
        /// </summary>
        /// <param name="ad"></param>
        public virtual void SetEntityStateModified(Ad ad)
        {
            Entry(ad).State = EntityState.Modified;
        }

        /// <summary>
        /// Method to set EntityState in Stats
        /// </summary>
        /// <param name="stats"></param>
        public virtual void SetEntityStateModified(Stats stats)
        {
            Entry(stats).State = EntityState.Modified;
        }

        /// <summary>
        /// Method to set EntityState in Auth
        /// </summary>
        /// <param name="auth"></param>
        public virtual void SetEntityStateModified(Auth auth)
        {
            Entry(auth).State = EntityState.Modified;
        }
    }
}

