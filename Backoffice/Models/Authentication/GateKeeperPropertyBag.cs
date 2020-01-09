using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using ExigoService;
using Common.Api.ExigoWebService;
using System.ComponentModel.DataAnnotations;
using Common;

namespace Backoffice.Models
{
    public class GateKeeperPropertyBag : BasePropertyBag
    {
        private string version = "1.0.0";
        private int expires = 31;
        private int customerID = Identity.Current.CustomerID;


        #region Constructors
        public GateKeeperPropertyBag()
        {
            this.CustomerID = customerID;
            this.Expires = expires;            
        }
        #endregion

        #region Properties
        public int CustomerID { get; set; }                   
        #endregion

        #region Methods
        public override T OnBeforeUpdate<T>(T propertyBag)
        {
            propertyBag.Version = version;

            return propertyBag;
        }
        public override bool IsValid()
        {
            var currentCustomerID = (Identity.Current != null) ? Identity.Current.CustomerID : 0;
            return this.Version == version && this.CustomerID == currentCustomerID;
        }
        #endregion
    }
}