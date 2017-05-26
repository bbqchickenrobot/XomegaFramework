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
    /// Cross-reference table mapping people to their credit card information in the CreditCard table. 
    ///</summary>
    public partial class PersonCreditCardController : ApiController
    {
        private ErrorParser errorParser;
        private IPersonCreditCardService svc;

        public PersonCreditCardController(IServiceProvider serviceProvider)
        {
            errorParser = serviceProvider.GetService<ErrorParser>();
            svc = serviceProvider.GetService<IPersonCreditCardService>();
            if (svc is IPrincipalProvider)
                ((IPrincipalProvider)svc).CurrentPrincipal = RequestContext.Principal;
        }

        ///<summary>
        /// Reads a list of Person Credit Card objects based on the person ID.
        ///</summary>
        [Route("person/{_businessEntityId}/credit-card")]
        [HttpGet]
        public HttpResponseMessage ReadList([FromUri] int _businessEntityId)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                IEnumerable<PersonCreditCard_ReadListOutput> output = svc.ReadList(_businessEntityId);
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