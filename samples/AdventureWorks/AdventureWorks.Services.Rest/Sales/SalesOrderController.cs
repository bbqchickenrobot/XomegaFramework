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
    /// General sales order information.
    ///</summary>
    public partial class SalesOrderController : ApiController
    {
        private ErrorParser errorParser;
        private ISalesOrderService svc;

        public SalesOrderController(IServiceProvider serviceProvider)
        {
            errorParser = serviceProvider.GetService<ErrorParser>();
            svc = serviceProvider.GetService<ISalesOrderService>();
            if (svc is IPrincipalProvider)
                ((IPrincipalProvider)svc).CurrentPrincipal = RequestContext.Principal;
        }

        ///<summary>
        /// Reads the values of a Sales Order object by its key.
        ///</summary>
        [Route("sales-order/{_salesOrderId}")]
        [HttpGet]
        public HttpResponseMessage Read([FromUri] int _salesOrderId)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                SalesOrder_ReadOutput output = svc.Read(_salesOrderId);
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

        ///<summary>
        /// Creates a new Sales Order object using the specified data.
        ///</summary>
        [Route("sales-order")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] SalesOrder_CreateInput _data)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                SalesOrder_CreateOutput output = svc.Create(_data);
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

        ///<summary>
        /// Updates existing Sales Order object using the specified data.
        ///</summary>
        [Route("sales-order/{_salesOrderId}")]
        [HttpPut]
        public HttpResponseMessage Update([FromUri] int _salesOrderId, [FromBody] SalesOrder_UpdateInput_Data _data)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                SalesOrder_UpdateOutput output = svc.Update(_salesOrderId, _data);
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

        ///<summary>
        /// Deletes the specified Sales Order object.
        ///</summary>
        [Route("sales-order/{_salesOrderId}")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int _salesOrderId)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                svc.Delete(_salesOrderId);
            }
            catch (Exception ex)
            {
                ErrorList errors = errorParser.FromException(ex);
                response = Request.CreateResponse(errors);
                response.StatusCode = errors.HttpStatus;
            }
            return response;
        }

        ///<summary>
        /// Reads a list of Sales Order objects based on the specified criteria.
        ///</summary>
        [Route("sales-order")]
        [HttpGet]
        public HttpResponseMessage ReadList([FromUri] SalesOrder_ReadListInput_Criteria _criteria)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                IEnumerable<SalesOrder_ReadListOutput> output = svc.ReadList(_criteria);
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

        ///<summary>
        /// Reads the values of a Sales Order Detail object by its key.
        ///</summary>
        [Route("sales-order/detail/{_salesOrderDetailId}")]
        [HttpGet]
        public HttpResponseMessage Detail_Read([FromUri] int _salesOrderDetailId)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                SalesOrderDetail_ReadOutput output = svc.Detail_Read(_salesOrderDetailId);
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

        ///<summary>
        /// Creates a new Sales Order Detail object using the specified data.
        ///</summary>
        [Route("sales-order/{_salesOrderId}/detail")]
        [HttpPost]
        public HttpResponseMessage Detail_Create([FromUri] int _salesOrderId, [FromBody] SalesOrderDetail_CreateInput_Data _data)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                SalesOrderDetail_CreateOutput output = svc.Detail_Create(_salesOrderId, _data);
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

        ///<summary>
        /// Updates existing Sales Order Detail object using the specified data.
        ///</summary>
        [Route("sales-order/detail/{_salesOrderDetailId}")]
        [HttpPut]
        public HttpResponseMessage Detail_Update([FromUri] int _salesOrderDetailId, [FromBody] SalesOrderDetail_UpdateInput_Data _data)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                svc.Detail_Update(_salesOrderDetailId, _data);
            }
            catch (Exception ex)
            {
                ErrorList errors = errorParser.FromException(ex);
                response = Request.CreateResponse(errors);
                response.StatusCode = errors.HttpStatus;
            }
            return response;
        }

        ///<summary>
        /// Deletes the specified Sales Order Detail object.
        ///</summary>
        [Route("sales-order/detail/{_salesOrderDetailId}")]
        [HttpDelete]
        public HttpResponseMessage Detail_Delete([FromUri] int _salesOrderDetailId)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                svc.Detail_Delete(_salesOrderDetailId);
            }
            catch (Exception ex)
            {
                ErrorList errors = errorParser.FromException(ex);
                response = Request.CreateResponse(errors);
                response.StatusCode = errors.HttpStatus;
            }
            return response;
        }

        ///<summary>
        /// Reads a list of Sales Order Detail objects based on the specified criteria.
        ///</summary>
        [Route("sales-order/{_salesOrderId}/detail")]
        [HttpGet]
        public HttpResponseMessage Detail_ReadList([FromUri] int _salesOrderId)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                IEnumerable<SalesOrderDetail_ReadListOutput> output = svc.Detail_ReadList(_salesOrderId);
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