using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ads;
using System.Web.Http.Controllers;
using FakeItEasy;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Net.Http.Headers;
using System.Net;
using Ads.Controllers;

namespace AdsApp.Test
{
    [TestClass]
    public class TestBasicAuthenticationAttribute
    {
        private HttpActionContext actionContext;

        [TestMethod]
        public void TestOnAuthorization()
        {
            BasicAuthenticationAttribute basicAuthenticationAttribute = new BasicAuthenticationAttribute();
            //// HttpActionContext actionContext = new HttpActionContext();
            //actionContext = A.Fake<HttpActionContext>();
            ////A.CallTo(() => actionContext.Request.Headers.Authorization).Returns(return_data);
            //actionContext.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Abc");
            ////basicAuthenticationAttribute.OnAuthorization(actionContext);

            HttpRequestMessage request = new HttpRequestMessage();
             
            HttpControllerContext controllerContext = new HttpControllerContext();
            controllerContext.Request = request;

           
            HttpRequestHeaders headers = request.Headers;
            AuthenticationHeaderValue authorization = new AuthenticationHeaderValue("YTpi");

            actionContext = new HttpActionContext();
            actionContext.ControllerContext = controllerContext;

            headers.Authorization = authorization;
            AuthorizationFilterAttribute n = new AuthorizationFilterAttribute;
            AuthsController auth = A.Fake<AuthsController>();
            A.CallTo(() => auth.VaidateUser("a", "b")).Returns(true);
            A.CallTo(() => n.OnAuthorization(actionContext));
            basicAuthenticationAttribute.OnAuthorization(actionContext);

            Assert.AreEqual(actionContext.Response, actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
        }
    }
}
