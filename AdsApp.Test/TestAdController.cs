using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ads.Models;
using System.Linq;
using System.Collections.Generic;
using Ads.Controllers;
using FakeItEasy;
using EntityFramework.FakeItEasy;
using System.Web.Http.Results;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using System.Net;
using System;
using System.Data.Entity.Infrastructure;

namespace AdsApp.Test
{
    [TestClass]
    public class TestAdController
    {
        private AdContext context;



        [TestInitialize]
        public void TestInit()
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
            var return_data = new Ad { Id = mock_id, Name = "Demo1", StatId = 1, Stats = new Stats { Id = 1, Price = 10.0 } };
            A.CallTo(() => context.Ads.FindAsync(mock_id)).Returns(return_data);

            AdController controller = new AdController(context);


            var ads = await controller.GetAd(mock_id);
            var response = ads as OkNegotiatedContentResult<Ad>;

            Assert.IsNotNull(response);
            Assert.AreEqual(mock_id, response.Content.Id);


        }

        [TestMethod]
        public async Task GetAd_ShouldReturnNotFound()
        {
           
            var mock_id = 100;

            //Stub FindAsync method
            Ad return_data = null;

            A.CallTo(() => context.Ads.FindAsync(mock_id)).Returns(return_data);

            AdController controller = new AdController(context);

            var ad = await controller.GetAd(mock_id);
            Assert.AreEqual(typeof(NotFoundResult), ad.GetType());

        }


        [TestMethod]
        public async Task PutAd_ShouldReturnInvalidModelState()
        {
            //TODO: Should Work
            int mock_id = 15;

            //Stub FindAsync method
            var mock_ad = new Ad { Id = mock_id, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };

            AdController controller = new AdController(context);

            //Faking ModelState.IsValid = false , Source : https://forums.asp.net/t/1328199.aspx?Substituting+for+ModelState+when+unit+testing
            controller.ModelState.Add("Modelstate", new ModelState());
            controller.ModelState.AddModelError("Modelstate", "test");

            var ads = await controller.PutAd(mock_id, mock_ad);

            Assert.AreEqual(typeof(InvalidModelStateResult), ads.GetType());

        }

        [TestMethod]
        public async Task PutAd_ShouldReturnBadRequest()
        {
            //TODO: Should Work
            int mock_id = 15;

            //Stub FindAsync method
            var mock_ad = new Ad { Id = 16, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };

            AdController controller = new AdController(context);

            var ads = await controller.PutAd(mock_id, mock_ad);

            Assert.AreEqual(typeof(BadRequestResult), ads.GetType());

        }

        [TestMethod]
        public async Task PutAd_ShouldReturnStatusCode()
        {
            int mock_id = 11;
            //Stub FindAsync method

            var mock_ad = new Ad { Id = mock_id, Name = "Demo2", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };

            A.CallTo(() => context.SetEntityStateModified(mock_ad));

            AdController controller = new AdController(context);
            var ads = await controller.PutAd(mock_ad.Id, mock_ad);

            var statusCode = ads as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusCode.StatusCode);

        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public async Task PutAd_ShouldRaiseException()
        {
            int mock_id = 11;

            var mock_ad = new Ad { Id = mock_id, Name = "Demo2", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };

            //Stub FindAsync method
            A.CallTo(() => context.SaveChangesAsync()).Throws<DbUpdateConcurrencyException>(); ;

            AdController controller = new AdController(context);
            await controller.PutAd(mock_ad.Id, mock_ad);
        }

        [TestMethod]
        public async Task PostAd_ShouldPostAd()
        {
            // Data found
            int mock_id = 15;

            //Stub FindAsync method
            var mock_ad = new Ad { Id = mock_id, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };

            AdController controller = new AdController(context);

            var ads = await controller.PostAd(mock_ad);
            Assert.AreEqual(typeof(CreatedAtRouteNegotiatedContentResult<Ad>), ads.GetType());

            var response = ads as CreatedAtRouteNegotiatedContentResult<Ad>;
            Assert.AreEqual(mock_id, response.Content.Id);
            Assert.AreEqual(5, context.Ads.Count());


        }
        [TestMethod]
        public async Task PostAd_ShouldReturnInvalidModelState()
        {

            //TODO: Should Work
            int mock_id = 15;

            //Stub FindAsync method
            var mock_ad = new Ad { Id = mock_id, Name = "Demo1", StatId = 2, Stats = new Stats { Id = 2, Price = 1.0 } };
           
            AdController controller = new AdController(context);

            //Faking ModelState.IsValid = false , Source : https://forums.asp.net/t/1328199.aspx?Substituting+for+ModelState+when+unit+testing
            controller.ModelState.Add("Modelstate", new ModelState());
            controller.ModelState.AddModelError("Modelstate", "test");

            var ads = await controller.PostAd(mock_ad);
            Assert.AreEqual(typeof(InvalidModelStateResult), ads.GetType());
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

        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        public async Task DeleteAd_ShouldNotFound()
        {
             var mock_id = 100;

            //Stub FindAsync method
            Ad return_data = null;
            A.CallTo(() => context.Ads.FindAsync(mock_id)).Returns(return_data);

            AdController controller = new AdController(context);
            var ad = await controller.DeleteAd(mock_id);
            var response = ad as OkNegotiatedContentResult<Ad>;

            Assert.IsNull(response);
            Assert.AreEqual(typeof(NotFoundResult), ad.GetType());

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
