using System;
using System.Data.Entity;
using Ads.Models;

namespace AdsApp.Tests
{
    public class TestAdsAppContext : AdContext
    {
        public TestAdsAppContext()
        {
            this.Ads = new TestAdDbSet();
        }

        public DbSet<Ad> Ads { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Ad item) { }
        public void Dispose() { }
    }
}