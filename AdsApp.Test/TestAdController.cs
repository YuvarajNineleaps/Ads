using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdsApp.Tests;
using Ads.Controllers;
using Ads.Models;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Threading;
using Ninject.Activation;
using FakeItEasy;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Ads.Migrations;
using System.Web.Http;
using System.Threading.Tasks;

namespace AdsApp.Test
{
    [TestClass]
    public class TestAdController
    {
        //Mock<AdContext> mock;

        //[TestInitialize]
        //public void testInit()
        //{
        //    //Arrange

        //    mock = new Mock<AdContext>();
        //    mock.Setup(x => x.Set<Ad>())
        //        .Returns(A.Fake<DbSet<Ad>>(o => o.Implements(typeof(IQueryable<Ad>)).Implements(typeof(IDbAsyncEnumerable<Ad>)))
        //                .SetupData(GetTestAds()));
        //}

        //[TestMethod]
        //public void GetAds_ShouldReturnAllAdsAsync()
        //{
        //    //Arrange
        //    var mock = new Mock<AdContext>();
        //    mock.Setup(x => x.Set<Ad>())
        //        .Returns(new TestDbSet<Ad>
        //        {
        //            new Ad { Id = 1, Name = "Demo1", StatId = 1, Stats = new Stats {Id = 1, Price =10.0 } },
        //            new Ad { Id = 2, Name = "Demo1", StatId = 1, Stats = new Stats { } }
        //        });

        //    var return_data = Task.FromResult<Ad>(new Ad { Id = 1, Name = "Demo1", StatId = 1, Stats = new Stats { Id = 1, Price = 10.0 } });
        //    mock.Setup(x => x.Ads.FindAsync(1)).Returns(return_data);

        //    //Arrange
        //    //var mock = A.Fake<AdContext>();
        //    // mock.Ads.Add(new Ad { Id = 1, Name = "Demo1", StatId = 1, Stats = new Stats { Id = 1, Price = 10.0 } });
        //    // mock.Stats.Add(new Stats { Id = 1, Price = 10.0 });
        //    // mock.SaveChangesAsync();

        //    var data = GetTestAds();
        //   // var fakeDbSet = A.Fake<DbSet<Ad>>(o => o.Implements(typeof(IQueryable<Ad>)).Implements(typeof(IDbAsyncEnumerable<Ad>)))
        //   //             .SetupData(data);

        //   //// A.CallTo(() => ((IQueryable<Ad>)fakeDbSet).Provider).Returns(data.Provider);
        //   ////A.CallTo(() => ((IQueryable<Ad>)fakeDbSet).Expression).Returns(data.Expression);
        //   //// A.CallTo(() => ((IQueryable<Ad>)fakeDbSet).ElementType).Returns(data.ElementType);
        //   // A.CallTo(() => ((IQueryable<Ad>)fakeDbSet).GetEnumerator()).Returns(data.GetEnumerator());

        //   // var fakeContext = A.Fake<AdContext>();
        //   // A.CallTo(() => fakeContext.Ads).Returns(fakeDbSet);

        //    //AdController controller = new AdController(fakeContext);

        //    // Act
        //    //var ads = controller.GetAds();

        //    // Assert
        //    //Assert.AreEqual(ads.Count(), 2);
        //    //Assert.AreEqual(14, ads.Last().Id, "Should be 14");

        //}


        [TestMethod]
        public void GetAd_ShouldReturnAdByIdAsync()
        {
            //TODO : Add False Condition

            // Arrange

            var mock = new Mock<AdContext>();
            mock.Setup(x => x.Set<Ad>())
                .Returns(A.Fake<DbSet<Ad>>(o => o.Implements(typeof(IQueryable<Ad>)).Implements(typeof(IDbAsyncEnumerable<Ad>)))
                        .SetupData(GetTestAds()));

            var return_data = Task.FromResult<Ad>(new Ad { Id = 2, Name = "Demo1", StatId = 3, Stats = new Stats { Id = 2, Price = 10.0 } });
            // mock.Setup(x => x.Ads.FindAsync(1)).Returns(return_data);

            AdController controller = new AdController(mock.Object);

            // Act

            int actual_id = 100;

            var ads = controller.GetAd(actual_id);


            //var response = ads as OkNegotiatedContentResult<Enumerable<Ad>>;

            //Assert.IsInstanceOfType(ads, typeof(NotFoundResult));
            //Assert.AreEqual(2, response.Content);
            //var message = ads.ExecuteAsync();
            //Assert.AreEqual(response.TryGetContentValue<Ad>(out ad));

            //var getRespone = ads.ExecuteAsync(CancellationToken.None);
            Assert.IsInstanceOfType(ads, typeof(OkResult));
            //Assert.IsNotNull(response);

        }

        //[TestMethod]
        //public async System.Threading.Tasks.Task DeleteAd_ShouldReturnOKAsync()
        //{
        //    // Arrange
        //    var mock = new Mock<AdContext>();
        //    mock.Setup(x => x.Set<Ad>())
        //        .Returns(A.Fake<DbSet<Ad>>(o => o.Implements(typeof(IQueryable<Ad>)).Implements(typeof(IDbAsyncEnumerable<Ad>)))
        //                .SetupData(GetTestAds()));
        //    var data = GetTestAds();


        //    AdController controller = new AdController(mock.Object);
        //    int actual_id = 11;


        //    // Act
        //    var ads = await controller.DeleteAd(actual_id);


        //    var result = ads as OkResult;

        //    //var blogs = (List<Ad>)Ad.Models;
        //    Assert.IsNotNull(ads);
        //    Assert.AreEqual(result, typeof(OkResult));
        //}




        public List<Ad> GetTestAds()
        {

            var testAds = new List<Ad>();
            testAds.Add(new Ad { Id = 11, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } });
            testAds.Add(new Ad { Id = 12, Name = "Demo2", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } });
            testAds.Add(new Ad { Id = 13, Name = "Demo3", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 }  });
            testAds.Add(new Ad { Id = 14, Name = "Demo4", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 }  });

            return testAds;
        }
       
    }
}
