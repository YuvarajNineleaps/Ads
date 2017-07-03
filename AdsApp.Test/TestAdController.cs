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

        /// <summary>
        /// Test initialize.
        /// </summary>
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

        /// <summary>
        /// Test Get ads to return all ads.
        /// </summary>
        [TestMethod]
        public void GetAds_ShouldReturnAllAds()
        {
            AdController controller = new AdController(context);

            var ads = controller.GetAds().ToList();

            Assert.AreEqual(4, ads.Count);

        }

        /// <summary>
        /// Test Get ad to return ad by id.
        /// </summary>
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

        /// <summary>
        /// Test Get ad to return ad by id.
        /// </summary>
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

        /// <summary>
        /// Test Put Ad.
        /// </summary>
        [TestMethod]
        public async Task PutAd_ShouldReturnInvalidModelState()
        {
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

        /// <summary>
        /// Test Put Ad.
        /// </summary>
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

        /// <summary>
        /// Test Put Ad.
        /// </summary>
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

        /// <summary>
        /// Test Put Ad.
        /// </summary>
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

        /// <summary>
        /// Test Post Ad.
        /// </summary>
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

        /// <summary>
        /// Test Post Ad.
        /// </summary>
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

        /// <summary>
        /// Test Delete Ad by Id.
        /// </summary>
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

        /// <summary>
        /// Test Delete Ad by Id.
        /// </summary>
        [TestMethod]
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
