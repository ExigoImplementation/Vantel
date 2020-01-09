using Backoffice.Models;
using Common;
using Common.Api.ExigoWebService;
using Common.Providers;
using Dapper;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Backoffice.Providers
{
    public class GatekeeperLogicProvider : BaseLogicProvider
    {
        #region Constructors
        public GatekeeperLogicProvider() : base() { }
        public GatekeeperLogicProvider(Controller controller, GateKeeperPropertyBag propertyBag)
        {
            Controller = controller;
            PropertyBag = propertyBag;
        }
        #endregion

        #region Properties
        public GateKeeperPropertyBag PropertyBag { get; set; }
        #endregion

        #region Logic
        public override CheckLogicResult CheckLogic()
        {     
            if (AccontStatusNotAllowed())
            {
                return CheckLogicResult.Failure(RedirectToAction("AccountStatus"));
            }            

            return CheckLogicResult.Success(RedirectToAction("Index", "Dashboard"));
        }

       

        public bool AccontStatusNotAllowed()
        {
            // testing reset
            //if (false)
            //{
            //    ExigoDAL.UpdateCustomer(new UpdateCustomerRequest()
            //    {
            //        CustomerID = PropertyBag.CustomerID,
            //        CustomerStatus = CustomerStatuses.Deleted
            //    });
            //}

            var unacceptedCustomerStatuses = new List<int>() { CustomerStatuses.Deleted };
            return unacceptedCustomerStatuses.Contains(Identity.Current.CustomerStatusID);              
        }         
        #endregion
    }
}