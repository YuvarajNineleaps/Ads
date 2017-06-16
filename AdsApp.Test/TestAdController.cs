using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ads.Models;
using System.Linq;
using System.Collections.Generic;
using Ads.Controllers;
using System.Data.Entity;
using AdsApp.Tests;
using FakeItEasy;
using System.Data.Entity.Infrastructure;
using EntityFramework.FakeItEasy;
using System.Web.Http.Results;
using System.Threading.Tasks;

namespace AdsApp.Test
{
    [TestClass]
    public class TestAdController
    {
        private AdContext context;

        [TestInitialize]
        public void testInit()
        {
            //Arrange

            //Mock Context
            context = A.Fake<AdContext>();

            //Mock DbSet
            var fakeDbSet = Aef.FakeDbSet<Ad>(GetTestAds()); //55 Model fakes created by FakeItEasy
            A.CallTo(() => context.Ads).Returns(fakeDbSet);
        }
        [TestMethod]
        public void GetAds_ShouldReturnAllAds()
        {
            //var mockSet = new Mock<DbSet<Ad>>();

            //var data = GetTestAdsQuerayable();

            //mockSet.As<IQueryable<Ad>>().Setup(m => m.Provider).Returns(data.Provider);
            //mockSet.As<IQueryable<Ad>>().Setup(m => m.Expression).Returns(data.Expression);
            //mockSet.As<IQueryable<Ad>>().Setup(m => m.ElementType).Returns(data.ElementType);
            //mockSet.As<IQueryable<Ad>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //var mockContext = new Mock<AdContext>();
            //mockContext.Setup(c => c.Ads).Returns(mockSet.Object);

            //AdController controller = new AdController(mockContext.Object);


            //////////var mock = new Mock<AdContext>();
            ////////var context = A.Fake<AdContext>();
            ////////var fakeDbSet = Aef.FakeDbSet<Ad>(GetTestAds()); //55 Model fakes created by FakeItEasy
            ////////A.CallTo(() => context.Ads).Returns(fakeDbSet);

            //mock.Setup(x => x.Set<Ad>())
            //    .Returns(A.Fake<DbSet<Ad>>(builder =>
            //             builder.Implements(typeof(IQueryable<Ad>))));

            ////mock.Setup(x => x.Set<Ad>())
            ////    .Returns(new TestDbSet<Ad>
            ////    {
            ////        new Ad { Id = 1, Name = "Demo1", StatId = 1, Stats = new Stats {Id = 1, Price =10.0 } }
            ////    });

            AdController controller = new AdController(context);

            var ads = controller.GetAds().ToList();

            Assert.AreEqual(4, ads.Count);

        }

        [TestMethod]
        public async Task GetAd_ShouldReturnAdsByIdAsync()
        {
            // Data found
            int mock_id = 11;

            //Stub FindAsync method
            var return_data = Task.FromResult<Ad>(new Ad { Id = mock_id, Name = "Demo1", StatId = 1, Stats = new Stats { Id = 1, Price = 10.0 } });
            A.CallTo(() => context.Ads.FindAsync(mock_id)).Returns(return_data);

            AdController controller = new AdController(context);

            
            var ads = await controller.GetAd(mock_id);
            var response = ads as OkNegotiatedContentResult<Ad>;

            Assert.IsNotNull(response);
            Assert.AreEqual(mock_id, response.Content.Id);


            // No data Found
            mock_id = 100;

            //Stub FindAsync method
            return_data = Task.FromResult<Ad>(null);

            A.CallTo(() =>  context.Ads.FindAsync(mock_id)).Returns(return_data);

            controller = new AdController(context);


            var ad = await controller.GetAd(mock_id);

            Assert.AreEqual(typeof(NotFoundResult), ad.GetType());

        }

        [TestMethod]
        public async Task DeleteAd_ShouldDeleteAdsByIdAsync()
        {
            // Data found
            int mock_id = 11;

            //Stub FindAsync method
            var return_data = new Ad { Id = mock_id, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };
            A.CallTo(() => context.Ads.FindAsync(mock_id)).Returns(return_data);

            AdController controller = new AdController(context);

            var ads = await controller.DeleteAd(mock_id);
            Assert.AreEqual(typeof(OkNegotiatedContentResult<Ad>), ads.GetType());

            var response = ads as OkNegotiatedContentResult<Ad>;
            Assert.AreEqual(mock_id, response.Content.Id);


            //// No data Found
            //mock_id = 100;

            ////Stub FindAsync method
            //return_data = Task.FromResult<Ad>(null);
            //A.CallTo(() => context.Ads.FindAsync(mock_id)).Returns(return_data);

            //controller = new AdController(context);
            //var ad = await controller.GetAd(mock_id);
            //var response = ad as OkNegotiatedContentResult<Ad>;

            //Assert.IsNull(response);
            //Assert.AreEqual(typeof(NotFoundResult), ad.GetType());

        }


        public IQueryable<Ad> GetTestAdsQuerayable()
        {

            var testAds = new List<Ad>{
                new Ad { Id = 11, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } },
                new Ad { Id = 12, Name = "Demo2", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } },
                new Ad { Id = 13, Name = "Demo3", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } },
                new Ad { Id = 14, Name = "Demo4", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } }
            }.AsQueryable();
            return testAds;
        }

        public List<Ad> GetTestAds()
        {

            var testAds = new List<Ad>();
            testAds.Add(new Ad { Id = 11, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } });
            testAds.Add(new Ad { Id = 12, Name = "Demo2", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } });
            testAds.Add(new Ad { Id = 13, Name = "Demo3", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } });
            testAds.Add(new Ad { Id = 14, Name = "Demo4", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } });

            return testAds;
        }
    }
}
