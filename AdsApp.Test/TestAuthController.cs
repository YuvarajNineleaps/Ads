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
    [TestClass]
    public class TestAuthsController
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
            var fakeDbSet = Aef.FakeDbSet<Auth>(GetTestAuths()); //55 Model fakes created by FakeItEasy
            A.CallTo(() => context.Auths).Returns(fakeDbSet);

        }

        /// <summary>
        /// Test Get auths to return all auths.
        /// </summary>
        [TestMethod]
        public void GetAuths_ShouldReturnAllAuths()
        {
            AuthsController controller = new AuthsController(context);

            var auths = controller.GetAuths().ToList();

            Assert.AreEqual(2, auths.Count);

        }

        /// <summary>
        /// Test Get auth to return auth by id.
        /// </summary>
        [TestMethod]
        public async Task GetAuths_ShouldReturnAuthsByIdAsync()
        {
            // Data found
            int mock_id = 1;

            //Stub FindAsync method
            var return_data = new Auth { Id = 1, Name = "123", Password = "123" };
            A.CallTo(() => context.Auths.FindAsync(mock_id)).Returns(return_data);

            AuthsController controller = new AuthsController(context);


            var auths = await controller.GetAuth(mock_id);
            var response = auths as OkNegotiatedContentResult<Auth>;

            Assert.IsNotNull(response);
            Assert.AreEqual(mock_id, response.Content.Id);


        }

        /// <summary>
        /// Test Get auth to return auth by id.
        /// </summary>
        [TestMethod]
        public async Task GetAuths_ShouldReturnNotFound()
        {

            var mock_id = 100;

            //Stub FindAsync method
            Auth return_data = null;

            A.CallTo(() => context.Auths.FindAsync(mock_id)).Returns(return_data);

            AuthsController controller = new AuthsController(context);


            var auths = await controller.GetAuth(mock_id);

            Assert.AreEqual(typeof(NotFoundResult), auths.GetType());

        }

        /// <summary>
        /// Test Put Auth.
        /// </summary>
        [TestMethod]
        public async Task PutAuths_ShouldReturnInvalidModelState()
        {
            //TODO: Should Work
            int mock_id = 15;

            //Stub FindAsync method
            var mock_auths = new Auth { Id = 1, Name = "123", Password = "123" };

            AuthsController controller = new AuthsController(context);

            //Faking ModelState.IsValid = false , Source : https://forums.asp.net/t/1328199.aspx?SubstitutingforModelStatewhenunittesting
            controller.ModelState.Add("Modelstate", new ModelState());
            controller.ModelState.AddModelError("Modelstate", "test");

            var auths = await controller.PutAuth(mock_id, mock_auths);

            Assert.AreEqual(typeof(InvalidModelStateResult), auths.GetType());

        }

        /// <summary>
        /// Test Put Auth.
        /// </summary>
        [TestMethod]
        public async Task PutAuths_ShouldReturnBadRequest()
        {
            //TODO: Should Work
            int mock_id = 15;

            //Stub FindAsync method
            var mock_auths = new Auth { Id = 1, Name = "123", Password = "123" };

            AuthsController controller = new AuthsController(context);

            var auths = await controller.PutAuth(mock_id, mock_auths);

            Assert.AreEqual(typeof(BadRequestResult), auths.GetType());

        }

        /// <summary>
        /// Test Put Auth.
        /// </summary>
        [TestMethod]
        public async Task PutAuths_ShouldReturnStatusCode()
        {
            var mock_auths = new Auth { Id = 1, Name = "123", Password = "123" };

            A.CallTo(() => context.SetEntityStateModified(mock_auths));

            AuthsController controller = new AuthsController(context);
            var auths = await controller.PutAuth(mock_auths.Id, mock_auths);

            var statusCode = auths as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusCode.StatusCode);


        }

        /// <summary>
        /// Test Put Auth.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public async Task PutAuth_ShouldRaiseException()
        {

            var mock_auths = new Auth { Id = 1, Name = "123", Password = "123" };

            //Stub FindAsync method
            A.CallTo(() => context.SaveChangesAsync()).Throws<DbUpdateConcurrencyException>(); ;

            AuthsController controller = new AuthsController(context);
            await controller.PutAuth(mock_auths.Id, mock_auths);
        }


        /// <summary>
        /// Test Post Auth.
        /// </summary>
        [TestMethod]
        public async Task PostAuths_ShouldPostAuths()
        {
            // Data found
            int mock_id = 10;

            //Stub FindAsync method
            var mock_auths = new Auth { Id = mock_id, Name = "123", Password = "123" };

            AuthsController controller = new AuthsController(context);

            var auths = await controller.PostAuth(mock_auths);
            Assert.AreEqual(typeof(CreatedAtRouteNegotiatedContentResult<Auth>), auths.GetType());

            var response = auths as CreatedAtRouteNegotiatedContentResult<Auth>;
            Assert.AreEqual(mock_id, response.Content.Id);
            Assert.AreEqual(3, context.Auths.Count());


        }

        /// <summary>
        /// Test Post Auth.
        /// </summary>
        [TestMethod]
        public async Task PostAuths_ShouldReturnInvalidModelState()
        {

            //TODO: Should Work
            int mock_id = 15;

            //Stub FindAsync method
            var mock_auths = new Auth { Id = mock_id, Name = "123", Password = "123" };

            AuthsController controller = new AuthsController(context);

            //Faking ModelState.IsValid = false , Source : https://forums.asp.net/t/1328199.aspx?SubstitutingforModelStatewhenunittesting
            controller.ModelState.Add("Modelstate", new ModelState());
            controller.ModelState.AddModelError("Modelstate", "test");

            var auths = await controller.PostAuth(mock_auths);
            Assert.AreEqual(typeof(InvalidModelStateResult), auths.GetType());
        }

        /// <summary>
        /// Test Delete Auth by Id.
        /// </summary>
        [TestMethod]
        public async Task DeleteAuths_ShouldDeleteAuthsByIdAsync()
        {
            // Data found
            int mock_id = 2;

            //Stub FindAsync method
            var return_data = new Auth { Id = mock_id, Name = "123", Password = "123" };
            A.CallTo(() => context.Auths.FindAsync(mock_id)).Returns(return_data);

            AuthsController controller = new AuthsController(context);

            var auths = await controller.DeleteAuth(mock_id);
            Assert.AreEqual(typeof(OkNegotiatedContentResult<Auth>), auths.GetType());

            var response = auths as OkNegotiatedContentResult<Auth>;
            Assert.AreEqual(mock_id, response.Content.Id);

        }

        /// <summary>
        /// Test Delete Auth by Id.
        /// </summary>
        [TestMethod]
        public async Task DeleteAuths_ShouldNotFound()
        {
            var mock_id = 100;

            //Stub FindAsync method
            Auth return_data = null;
            A.CallTo(() => context.Auths.FindAsync(mock_id)).Returns(return_data);

            AuthsController controller = new AuthsController(context);
            var auths = await controller.DeleteAuth(mock_id);
            var response = auths as OkNegotiatedContentResult<Auth>;

            Assert.IsNull(response);
            Assert.AreEqual(typeof(NotFoundResult), auths.GetType());

        }


        public List<Auth> GetTestAuths()
        {

            var testAuth = new List<Auth>();
            testAuth.Add(new Auth { Id = 1, Name = "123" , Password = "123"});
            testAuth.Add(new Auth { Id = 2, Name = "456", Password = "456" });
            return testAuth;
        }
    }
}