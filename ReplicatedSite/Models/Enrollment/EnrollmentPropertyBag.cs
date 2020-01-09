using Common;
using ExigoService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReplicatedSite.Models
{
    public class EnrollmentPropertyBag : BasePropertyBag
    {
        private string version = "1.0.0";
        private int expires = 31;

        #region Constructors
        public EnrollmentPropertyBag()
        {
            this.Expires = expires;

            this.Customer = this.Customer ?? new Customer();
            this.DirectDeposit = this.DirectDeposit ?? new BankAccount();
            this.ShippingAddress = this.ShippingAddress ?? new ShippingAddress();
        }
        #endregion

        #region Properties
        [Required]
        public Customer Customer { get; set; }
        public BankAccount DirectDeposit { get; set; }
        public int EnrollerID { get; set; }
        public EnrollmentType EnrollmentType { get; set; }
        public MarketName SelectedMarket { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public int PaymentTypeID { get; set; }
        public IPaymentMethod PaymentMethod { get; set; }
        [Required]
        public int ShipMethodID { get; set; }
        public IEnumerable<IShipMethod> ShipMethods { get; set; }
        public bool UseSameShippingAddress { get; set; }
        public bool IsSubmitting { get; set; }
        public int NewCustomerID { get; set; }
        public int NewOrderID { get; set; }
        public int NewAutoOrderID { get; set; }        
        public string OrderException { get; set; }
        public string OrderNotes { get; set; }
        #endregion

        #region Methods
        public override T OnBeforeUpdate<T>(T propertyBag)
        {
            propertyBag.Version = version;

            return propertyBag;
        }
        public override bool IsValid()
        {
            return this.Version == version;
        }
        #endregion
    }
}