using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using Ads.Controllers;

namespace Ads
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    // Gets header parameters  
                    string authenticationString = actionContext.Request.Headers.Authorization.Parameter;
                    string originalString = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationString));

                    // Gets username and password  
                    string usrename = originalString.Split(':')[0];
                    string password = originalString.Split(':')[1];

                    AuthsController auth = new AuthsController();
                    // Validate username and password  
                    if (!auth.VaidateUser(usrename, password))
                    {
                        // returns unauthorized error  
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }

                base.OnAuthorization(actionContext);
            }
            catch(Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}