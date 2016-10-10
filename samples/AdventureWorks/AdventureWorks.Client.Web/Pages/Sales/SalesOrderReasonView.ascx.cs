//---------------------------------------------------------------------------------------------
// This file was AUTO-GENERATED by "ASP.NET Details Pages" Xomega.Net generator.
//
// Manual CHANGES to this file WILL BE LOST when the code is regenerated.
//---------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdventureWorks.Client.Objects;
using AdventureWorks.Services;
using Xomega.Framework;
using Xomega.Framework.Web;

namespace AdventureWorks.Client.Web
{
    public partial class SalesOrderReasonView : DetailsView
    {
        #region Initialization/Activation

        protected SalesOrderReasonObject obj;

        protected override void InitObjects(bool createNew)
        {
            dataObj = obj = GetObject<SalesOrderReasonObject>(createNew);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            btnDelete.Enabled = !IsNew;
        }

        public override bool Activate(NameValueCollection query)
        {
            if (!base.Activate(query)) return false;
            obj.SalesOrderIdProperty.Required = !IsNew;
            obj.SalesReasonIdProperty.Required = !IsNew;
            return true;
        }

        #endregion

        #region Data loading

        protected override void LoadData()
        {
            ISalesOrderService svcSalesOrder = DI.Resolve<ISalesOrderService>();
            ErrorList errorList = new ErrorList();
            try
            {
                SalesOrderReason_ReadOutput outReason_Read;
                using (TimeTracker.ServiceCall)
                    outReason_Read = svcSalesOrder.Reason_Read((int)obj.SalesOrderIdProperty.TransportValue, (int)obj.SalesReasonIdProperty.TransportValue);
                obj.FromDataContract(outReason_Read);
            }
            catch(Exception ex)
            {
                errorList.MergeWith(ErrorList.FromException(ex));
            }
            if (svcSalesOrder is IDisposable) ((IDisposable)svcSalesOrder).Dispose();
            errors.List.DataSource = errorList.Errors;
            errors.List.DataBind();
            Page.DataBind();
        }

        #endregion

        #region Event Handlers

        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
            obj.Validate(true);
            ErrorList valErr = obj.GetValidationErrors();
            errors.List.DataSource = valErr.Errors;
            errors.List.DataBind();
            if (valErr.HasErrors()) return;

            ISalesOrderService svcSalesOrder = DI.Resolve<ISalesOrderService>();
            try
            {
                // for new objects create the object and store its key
                if (IsNew)
                {
                    SalesOrderReason_CreateInput inReason_Create = new SalesOrderReason_CreateInput();
                    obj.ToDataContract(inReason_Create);
                    using (TimeTracker.ServiceCall)
                        svcSalesOrder.Reason_Create(inReason_Create);
                    IsNew = false;
                }
                else
                {
                    SalesOrderReason_UpdateInput_Data inReason_Update_Data = new SalesOrderReason_UpdateInput_Data();
                    obj.ToDataContract(inReason_Update_Data);
                    using (TimeTracker.ServiceCall)
                        svcSalesOrder.Reason_Update((int)obj.SalesOrderIdProperty.TransportValue, (int)obj.SalesReasonIdProperty.TransportValue, inReason_Update_Data);
                }
                obj.SetModified(false, true);
                OnSaved(EventArgs.Empty);
            }
            catch(Exception ex)
            {
                errors.List.DataSource = ErrorList.FromException(ex).Errors;
                errors.List.DataBind();
            }
            finally
            {
                if (svcSalesOrder is IDisposable) ((IDisposable)svcSalesOrder).Dispose();
            }
        }

        protected virtual void btnDelete_Click(object sender, EventArgs e)
        {
            ISalesOrderService svcSalesOrder = DI.Resolve<ISalesOrderService>();
            try
            {
                using (TimeTracker.ServiceCall)
                    svcSalesOrder.Reason_Delete((int)obj.SalesOrderIdProperty.TransportValue, (int)obj.SalesReasonIdProperty.TransportValue);
                obj.SetModified(false, true);
                OnDeleted(EventArgs.Empty);
                Hide();
            }
            catch(Exception ex)
            {
                errors.List.DataSource = ErrorList.FromException(ex).Errors;
                errors.List.DataBind();
            }
            if (svcSalesOrder is IDisposable) ((IDisposable)svcSalesOrder).Dispose();
        }

        #endregion
    }
}