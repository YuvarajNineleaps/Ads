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
using System.Data.Entity.Infrastructure;

namespace AdsApp.Test
{
    /// <summary>
    /// TestStatsController class.
    /// </summary>
    [TestClass]
    public class TestStatsController
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
            var fakeDbSet = Aef.FakeDbSet<Stats>(GetTestStats()); //55 Model fakes created by FakeItEasy
            A.CallTo(() => context.Stats).Returns(fakeDbSet);

        }

        /// <summary>
        /// Test Get stats to return all stats.
        /// </summary>
        [TestMethod]
        public void GetStats_ShouldReturnAllStats()
        {
            StatsController controller = new StatsController(context);

            var stats = controller.GetStats().ToList();

            Assert.AreEqual(4, stats.Count);

        }

        /// <summary>
        /// Test Get stat to return stat by id.
        /// </summary>
        [TestMethod]
        public async Task GetStats_ShouldReturnStatsByIdAsync()
        {
            // Data found
            int mock_id = 1;

            //Stub FindAsync method
            var return_data = new Stats { Id = mock_id, Price = 10.0 } ;
            A.CallTo(() => context.Stats.FindAsync(mock_id)).Returns(return_data);

            StatsController controller = new StatsController(context);


            var stats = await controller.GetStats(mock_id);
            var response = stats as OkNegotiatedContentResult<Stats>;

            Assert.IsNotNull(response);
            Assert.AreEqual(mock_id, response.Content.Id);


        }

        /// <summary>
        /// Test Get stat to return stat by id.
        /// </summary>
        [TestMethod]
        public async Task GetStats_ShouldReturnNotFound()
        {

            var mock_id = 100;

            //Stub FindAsync method
            Stats return_data = null;

            A.CallTo(() => context.Stats.FindAsync(mock_id)).Returns(return_data);

            StatsController controller = new StatsController(context);


            var stats = await controller.GetStats(mock_id);

            Assert.AreEqual(typeof(NotFoundResult), stats.GetType());

        }

        /// <summary>
        /// Test Put Stat.
        /// </summary>
        [TestMethod]
        public async Task PutStats_ShouldReturnInvalidModelState()
        {
            int mock_id = 15;

            //Stub FindAsync method
            var mock_stats = new Stats { Id = 2, Price = 1.0 };

            StatsController controller = new StatsController(context);

            //Faking ModelState.IsValid = false , Source : https://forums.asp.net/t/1328199.aspx?Substituting+for+ModelState+when+unit+testing
            controller.ModelState.Add("Modelstate", new ModelState());
            controller.ModelState.AddModelError("Modelstate", "test");

            var stats = await controller.PutStats(mock_id, mock_stats);

            Assert.AreEqual(typeof(InvalidModelStateResult), stats.GetType());

        }

        /// <summary>
        /// Test Put Stat.
        /// </summary>
        [TestMethod]
        public async Task PutStats_ShouldReturnBadRequest()
        {
            int mock_id = 15;

            //Stub FindAsync method
            var mock_stats = new Stats { Id = 2, Price = 1.0 } ;

            StatsController controller = new StatsController(context);

            var stats = await controller.PutStats(mock_id, mock_stats);

            Assert.AreEqual(typeof(BadRequestResult), stats.GetType());

        }

        /// <summary>
        /// Test Put Stat.
        /// </summary>
        [TestMethod]
        public async Task PutStats_ShouldReturnStatusCode()
        {
            var mock_stats = new Stats { Id = 2, Price = 1.0 } ;

            A.CallTo(() => context.SetEntityStateModified(mock_stats));

            StatsController controller = new StatsController(context);
            var stats = await controller.PutStats(mock_stats.Id, mock_stats);

            var statusCode = stats as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusCode.StatusCode);


        }

        /// <summary>
        /// Test Put Stat.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public async Task PutAuth_ShouldRaiseException()
        {

            var mock_stats = new Stats { Id = 2, Price = 1.0 };

            //Stub FindAsync method
            A.CallTo(() => context.SaveChangesAsync()).Throws<DbUpdateConcurrencyException>();

            StatsController controller = new StatsController(context);
            await controller.PutStats(mock_stats.Id, mock_stats);
        }

        /// <summary>
        /// Test Post Stat.
        /// </summary>
        [TestMethod]
        public async Task PostStats_ShouldPostStats()
        {
            // Data found
            int mock_id = 10;

            //Stub FindAsync method
            var mock_stats = new Stats { Id = mock_id, Price = 1.0 };

            StatsController controller = new StatsController(context);

            var stats = await controller.PostStats(mock_stats);
            Assert.AreEqual(typeof(CreatedAtRouteNegotiatedContentResult<Stats>), stats.GetType());

            var response = stats as CreatedAtRouteNegotiatedContentResult<Stats>;
            Assert.AreEqual(mock_id, response.Content.Id);
            Assert.AreEqual(5, context.Stats.Count());


        }

        /// <summary>
        /// Test Post Stat.
        /// </summary>
        [TestMethod]
        public async Task PostStats_ShouldReturnInvalidModelState()
        {

            //Stub FindAsync method
            var mock_stats = new Stats { Id = 2, Price = 1.0 } ;

            StatsController controller = new StatsController(context);

            //Faking ModelState.IsValid = false , Source : https://forums.asp.net/t/1328199.aspx?Substituting+for+ModelState+when+unit+testing
            controller.ModelState.Add("Modelstate", new ModelState());
            controller.ModelState.AddModelError("Modelstate", "test");

            var stats = await controller.PostStats(mock_stats);
            Assert.AreEqual(typeof(InvalidModelStateResult), stats.GetType());
        }

        /// <summary>
        /// Test Delete Stat by Id.
        /// </summary>
        [TestMethod]
        public async Task DeleteStats_ShouldDeleteStatsByIdAsync()
        {
            // Data found
            int mock_id = 2;

            //Stub FindAsync method
            var return_data = new Stats { Id = mock_id, Price = 1.0 };
            A.CallTo(() => context.Stats.FindAsync(mock_id)).Returns(return_data);

            StatsController controller = new StatsController(context);

            var stats = await controller.DeleteStats(mock_id);
            Assert.AreEqual(typeof(OkNegotiatedContentResult<Stats>), stats.GetType());

            var response = stats as OkNegotiatedContentResult<Stats>;
            Assert.AreEqual(mock_id, response.Content.Id);

        }

        /// <summary>
        /// Test Delete Stat by Id.
        /// </summary>
        [TestMethod]
        public async Task DeleteStats_ShouldNotFound()
        {
            var mock_id = 100;

            //Stub FindAsync method
            Stats return_data = null;
            A.CallTo(() => context.Stats.FindAsync(mock_id)).Returns(return_data);

            StatsController controller = new StatsController(context);
            var stats = await controller.DeleteStats(mock_id);
            var response = stats as OkNegotiatedContentResult<Stats>;

            Assert.IsNull(response);
            Assert.AreEqual(typeof(NotFoundResult), stats.GetType());

        }

        /// <summary>
        /// Get Test stats data.
        /// </summary>
        /// <returns>List of Stats</returns>
        public List<Stats> GetTestStats()
        {

            var testStats = new List<Stats>();
            testStats.Add(new Stats { Id = 1, Price = 1.0 } );
            testStats.Add(new Stats { Id = 2, Price = 1.0 } );
            testStats.Add(new Stats { Id = 3, Price = 1.0 } );
            testStats.Add(new Stats { Id = 4, Price = 1.0 } );

            return testStats;
        }
    }
}
