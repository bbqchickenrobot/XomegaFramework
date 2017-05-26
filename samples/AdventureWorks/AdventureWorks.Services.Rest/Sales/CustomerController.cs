//---------------------------------------------------------------------------------------------
// This file was AUTO-GENERATED by "Web API Controllers" Xomega.Net generator.
//
// Manual CHANGES to this file WILL BE LOST when the code is regenerated.
//---------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Xomega.Framework;

namespace AdventureWorks.Services
{
    ///<summary>
    /// Current customer information. Also see the Person and Store tables.
    ///</summary>
    public partial class CustomerController : ApiController
    {
        private ErrorParser errorParser;
        private ICustomerService svc;

        public CustomerController(IServiceProvider serviceProvider)
        {
            errorParser = serviceProvider.GetService<ErrorParser>();
            svc = serviceProvider.GetService<ICustomerService>();
            if (svc is IPrincipalProvider)
                ((IPrincipalProvider)svc).CurrentPrincipal = RequestContext.Principal;
        }

        ///<summary>
        /// Reads a list of Customer objects based on the specified criteria.
        ///</summary>
        [Route("customer")]
        [HttpGet]
        public HttpResponseMessage ReadList([FromUri] Customer_ReadListInput_Criteria _criteria)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                IEnumerable<Customer_ReadListOutput> output = svc.ReadList(_criteria);
                response = Request.CreateResponse(output);
            }
            catch (Exception ex)
            {
                ErrorList errors = errorParser.FromException(ex);
                response = Request.CreateResponse(errors);
                response.StatusCode = errors.HttpStatus;
            }
            return response;
        }
    }
}