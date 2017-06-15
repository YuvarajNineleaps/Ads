using System;
using System.Linq;
using Ads.Models;

namespace AdsApp.Tests
{
    class TestAdDbSet : TestDbSet<Ad>
    {
        public override Ad Find(params object[] keyValues)
        {
            return this.SingleOrDefault(ad => ad.Id == (int)keyValues.Single());
        }
    }
}