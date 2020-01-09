
namespace Resources
{
    using System.Threading;
    using ExigoResourceSet.Providers.Resource;
    using System.Globalization;
    using System.Web;
    using System.Collections.Generic;
        
    public static class Common
    {
       ///<summary>
       ///
       ///</summary>
        public static string _ToViewAllOfYourOrders => GetObject("_ToViewAllOfYourOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string A => GetObject("A", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string About => GetObject("About", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AboutUs => GetObject("AboutUs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AboutUsandWhoWeAre => GetObject("AboutUsandWhoWeAre", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccessPersonal => GetObject("AccessPersonal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Account => GetObject("Account", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountEndingIn => GetObject("AccountEndingIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountInfo => GetObject("AccountInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountNumber => GetObject("AccountNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountNumberError => GetObject("AccountNumberError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountNumberRules => GetObject("AccountNumberRules", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountSettings => GetObject("AccountSettings", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AccountStatus => GetObject("AccountStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Action => GetObject("Action", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Actions => GetObject("Actions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Active => GetObject("Active", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Activity => GetObject("Activity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Add => GetObject("Add", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddAlert => GetObject("AddAlert", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddANewAddress => GetObject("AddANewAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddBankAccount => GetObject("AddBankAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddCard => GetObject("AddCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddCollector => GetObject("AddCollector", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddExistingCustomer => GetObject("AddExistingCustomer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddExistingCustomerParty => GetObject("AddExistingCustomerParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddGuest => GetObject("AddGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddInvoice => GetObject("AddInvoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddItems => GetObject("AddItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddItemsText => GetObject("AddItemsText", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddItemToCart => GetObject("AddItemToCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddItemUppercase => GetObject("AddItemUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AdditionalDetails => GetObject("AdditionalDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AdditionalInformation => GetObject("AdditionalInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddMoreInformation => GetObject("AddMoreInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddMoreItems => GetObject("AddMoreItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewAddress => GetObject("AddNewAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewBankAccount => GetObject("AddNewBankAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewCard => GetObject("AddNewCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewCreditCard => GetObject("AddNewCreditCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewGuest => GetObject("AddNewGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewInvoice => GetObject("AddNewInvoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewSubscription => GetObject("AddNewSubscription", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddNewWebAccount => GetObject("AddNewWebAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Address => GetObject("Address", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Address2Placeholder => GetObject("Address2Placeholder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Addresses => GetObject("Addresses", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddShippingInstructions => GetObject("AddShippingInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddToAutoOrder => GetObject("AddToAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddToAutoShip => GetObject("AddToAutoShip", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddToCart => GetObject("AddToCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AddToOrder => GetObject("AddToOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string After => GetObject("After", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Agenda => GetObject("Agenda", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AgreeToPrivacyPolicy => GetObject("AgreeToPrivacyPolicy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AgreeToTerms => GetObject("AgreeToTerms", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AgreeToTermsShipping => GetObject("AgreeToTermsShipping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AlertList => GetObject("AlertList", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string All => GetObject("All", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AllDay => GetObject("AllDay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AllOrders => GetObject("AllOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AllProducts => GetObject("AllProducts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AllRanks => GetObject("AllRanks", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AllRightsReserved => GetObject("AllRightsReserved", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AlsoIfYouNeed => GetObject("AlsoIfYouNeed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AltProductInfo => GetObject("AltProductInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AmountPaid => GetObject("AmountPaid", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Answer => GetObject("Answer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AnUnexpectedErrorHasOccurred => GetObject("AnUnexpectedErrorHasOccurred", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AnyQuestionsCallCustomerService => GetObject("AnyQuestionsCallCustomerService", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Applicant => GetObject("Applicant", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ApplyFilter => GetObject("ApplyFilter", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AreYouSureCancelOrder => GetObject("AreYouSureCancelOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AreYouSureDeleteGuest => GetObject("AreYouSureDeleteGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  number of items
       ///</summary>
        public static string artQuantity => GetObject("artQuantity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Attended => GetObject("Attended", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Audio => GetObject("Audio", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrder => GetObject("AutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderAnswer1 => GetObject("AutoOrderAnswer1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderAnswer2 => GetObject("AutoOrderAnswer2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderBankAccount => GetObject("AutoOrderBankAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderBankAccountCannotBeDeleted => GetObject("AutoOrderBankAccountCannotBeDeleted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderCannotProcess_MissingPaymentMethod => GetObject("AutoOrderCannotProcess_MissingPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderCard => GetObject("AutoOrderCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderChanges => GetObject("AutoOrderChanges", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderCreditCard => GetObject("AutoOrderCreditCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderCreditCardCannotBeDeleted => GetObject("AutoOrderCreditCardCannotBeDeleted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderDate => GetObject("AutoOrderDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderDay => GetObject("AutoOrderDay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderItems => GetObject("AutoOrderItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderItemsUpdated => GetObject("AutoOrderItemsUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderNotScheduled => GetObject("AutoOrderNotScheduled", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderPreference => GetObject("AutoOrderPreference", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderQuestion1 => GetObject("AutoOrderQuestion1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderQuestion2 => GetObject("AutoOrderQuestion2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrders => GetObject("AutoOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderSaved => GetObject("AutoOrderSaved", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderSetting => GetObject("AutoOrderSetting", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderStartDate => GetObject("AutoOrderStartDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoOrderTotals => GetObject("AutoOrderTotals", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoShip => GetObject("AutoShip", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AutoshipDetails => GetObject("AutoshipDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  auto ship ID
       ///</summary>
        public static string AutoShipIDUpdated => GetObject("AutoShipIDUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Availability => GetObject("Availability", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Avatar => GetObject("Avatar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string AvatarUpdateMessage => GetObject("AvatarUpdateMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string B => GetObject("B", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Cart_RemoveThisItem => GetObject("BackOffice_Cart_RemoveThisItem", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Dashboard_CurrentMonthlyProjected => GetObject("BackOffice_Dashboard_CurrentMonthlyProjected", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Dashboard_CustomerWall => GetObject("BackOffice_Dashboard_CustomerWall", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Dashboard_LastMonthlyAccepted => GetObject("BackOffice_Dashboard_LastMonthlyAccepted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Dashboard_NoContentAvailable => GetObject("BackOffice_Dashboard_NoContentAvailable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Dashboard_ViewAllCustomers => GetObject("BackOffice_Dashboard_ViewAllCustomers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Error_NotFound => GetObject("BackOffice_Error_NotFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Error_TreePlacementRequired => GetObject("BackOffice_Error_TreePlacementRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Error_UnexpectedError => GetObject("BackOffice_Error_UnexpectedError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Login_Background => GetObject("BackOffice_Login_Background", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_CancelHostess => GetObject("BackOffice_Review_CancelHostess", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_FreeProduct => GetObject("BackOffice_Review_FreeProduct", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_HereAreRewards => GetObject("BackOffice_Review_HereAreRewards", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_PlacingOrderParty => GetObject("BackOffice_Review_PlacingOrderParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_ReturnPartyManagement => GetObject("BackOffice_Review_ReturnPartyManagement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_Warning => GetObject("BackOffice_Review_Warning", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Review_YouAreCreatingHostessOrder => GetObject("BackOffice_Review_YouAreCreatingHostessOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackOffice_Shopping_OrderComplete => GetObject("BackOffice_Shopping_OrderComplete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToCalendar => GetObject("BackToCalendar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToMyTree => GetObject("BackToMyTree", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToOpenParties => GetObject("BackToOpenParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToPacks => GetObject("BackToPacks", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToPartySummary => GetObject("BackToPartySummary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToProducts => GetObject("BackToProducts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToSignIn => GetObject("BackToSignIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BackToTop => GetObject("BackToTop", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BalanceForwarded => GetObject("BalanceForwarded", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  autoorder count
       ///</summary>
        public static string BankAccountUsedAsAutoOrderPaymentMethod => GetObject("BankAccountUsedAsAutoOrderPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BankName => GetObject("BankName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BasicInfo => GetObject("BasicInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BasicInformation => GetObject("BasicInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BecauseWeAreReliable => GetObject("BecauseWeAreReliable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BecomeAMember => GetObject("BecomeAMember", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BeginProcessInvitingAllGuestMesage => GetObject("BeginProcessInvitingAllGuestMesage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BilledTo => GetObject("BilledTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BillingAddress => GetObject("BillingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BillingAddressSameAsShipping => GetObject("BillingAddressSameAsShipping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BillingPayment => GetObject("BillingPayment", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BillTo => GetObject("BillTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BillToThisAddress => GetObject("BillToThisAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BinaryTree => GetObject("BinaryTree", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard Header
       ///</summary>
        public static string BinaryTreeViewer => GetObject("BinaryTreeViewer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Birthday => GetObject("Birthday", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Block => GetObject("Block", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Blog => GetObject("Blog", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Bonus => GetObject("Bonus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Bonus_1_001 => GetObject("Bonus_1_001", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BonusDescription => GetObject("BonusDescription", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BonusID => GetObject("BonusID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BookAnotherParty => GetObject("BookAnotherParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Bootstrap_Carousel_Grass => GetObject("Bootstrap_Carousel_Grass", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Bootstrap_Carousel_Grass_Mobile => GetObject("Bootstrap_Carousel_Grass_Mobile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Bootstrap_Carousel_Ships => GetObject("Bootstrap_Carousel_Ships", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Bootstrap_Carousel_Ships_Mobile => GetObject("Bootstrap_Carousel_Ships_Mobile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BottomLeft => GetObject("BottomLeft", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BottomRight => GetObject("BottomRight", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Browse => GetObject("Browse", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BusinessEntityDistributor => GetObject("BusinessEntityDistributor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Buy => GetObject("Buy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string BV => GetObject("BV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string By => GetObject("By", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ByPlacingYourOrder_Formatted => GetObject("ByPlacingYourOrder_Formatted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string C => GetObject("C", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CalculateAutoOrderFailed => GetObject("CalculateAutoOrderFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CalculateEstimatedShipping => GetObject("CalculateEstimatedShipping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Calculating => GetObject("Calculating", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CalculatingPleaseWait => GetObject("CalculatingPleaseWait", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CalculationTakingLonger => GetObject("CalculationTakingLonger", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Calendar => GetObject("Calendar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CalendarSubscriptions => GetObject("CalendarSubscriptions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Cancel => GetObject("Cancel", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CancelAutoOrder => GetObject("CancelAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CancelledOrders => GetObject("CancelledOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CancelOrder => GetObject("CancelOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CancelParty => GetObject("CancelParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CancelUppercase => GetObject("CancelUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Card => GetObject("Card", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CardCantBeDeleted_AutoOrder => GetObject("CardCantBeDeleted_AutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CardEndingIn => GetObject("CardEndingIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CardFailedToLoad => GetObject("CardFailedToLoad", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CardIsInvalid => GetObject("CardIsInvalid", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CardNumber => GetObject("CardNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Carousel_Paragraph1 => GetObject("Carousel_Paragraph1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Carousel_Paragraph2 => GetObject("Carousel_Paragraph2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Carousel_Paragraph3 => GetObject("Carousel_Paragraph3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Cart => GetObject("Cart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Cart_ItemName1 => GetObject("Cart_ItemName1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Cart_ItemName2 => GetObject("Cart_ItemName2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Cart_ItemName3 => GetObject("Cart_ItemName3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CartContainsNoItems => GetObject("CartContainsNoItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string CartItem => GetObject("CartItem", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CartQuantity => GetObject("CartQuantity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CartSubtotal => GetObject("CartSubtotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CartTotals => GetObject("CartTotals", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CashPayments => GetObject("CashPayments", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Categories => GetObject("Categories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Category => GetObject("Category", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Change => GetObject("Change", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChangeSettings => GetObject("ChangeSettings", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  autoorder count
       ///</summary>
        public static string ChangesToAutoOrderBankAccounts => GetObject("ChangesToAutoOrderBankAccounts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  autoorder count
       ///</summary>
        public static string ChangesToAutoOrderCreditCards => GetObject("ChangesToAutoOrderCreditCards", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CheckAvailability => GetObject("CheckAvailability", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CheckForDifferentBilling => GetObject("CheckForDifferentBilling", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CheckIfIsVirtualParty => GetObject("CheckIfIsVirtualParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Checking => GetObject("Checking", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CheckingAccount => GetObject("CheckingAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Checkout => GetObject("Checkout", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CheckOutTheProducts => GetObject("CheckOutTheProducts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Choose => GetObject("Choose", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChooseCountry => GetObject("ChooseCountry", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChooseEnroller => GetObject("ChooseEnroller", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChooseEnrollerSearchInstructions => GetObject("ChooseEnrollerSearchInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChooseLang => GetObject("ChooseLang", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChoosePassword => GetObject("ChoosePassword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChoosePaymentMethod => GetObject("ChoosePaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChoosePersonNewMember => GetObject("ChoosePersonNewMember", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChooseShippingMethod => GetObject("ChooseShippingMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChooseWhichCardToOverwrite => GetObject("ChooseWhichCardToOverwrite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ChoseNotToPlace => GetObject("ChoseNotToPlace", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string City => GetObject("City", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CityStateZip => GetObject("CityStateZip", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Clear => GetObject("Clear", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClearFilter => GetObject("ClearFilter", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Click => GetObject("Click", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickCancelPartyOrder => GetObject("ClickCancelPartyOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickCreateOrderBehalfOfCurrentGuest => GetObject("ClickCreateOrderBehalfOfCurrentGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickHere => GetObject("ClickHere", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickHereToLogin => GetObject("ClickHereToLogin", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickingOnThe => GetObject("ClickingOnThe", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickNewMemberWishPlace => GetObject("ClickNewMemberWishPlace", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickNewPlacementSponsor => GetObject("ClickNewPlacementSponsor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickToAdd => GetObject("ClickToAdd", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickToManagePayments => GetObject("ClickToManagePayments", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickToReviewYourInvoice => GetObject("ClickToReviewYourInvoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClickToViewAllOrders => GetObject("ClickToViewAllOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Close => GetObject("Close", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CloseParty => GetObject("CloseParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ClosePartyModalFailed => GetObject("ClosePartyModalFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CloseSubmitParty => GetObject("CloseSubmitParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CoApplicant => GetObject("CoApplicant", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CoApplicantFirstName => GetObject("CoApplicantFirstName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CoApplicantLastName => GetObject("CoApplicantLastName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CoApplicantMiddle => GetObject("CoApplicantMiddle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Collector => GetObject("Collector", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ComeAndExperience => GetObject("ComeAndExperience", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ComingSoon => GetObject("ComingSoon", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CommissionableVolume => GetObject("CommissionableVolume", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CommissionPayout => GetObject("CommissionPayout", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Commissions => GetObject("Commissions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CommissionSetting => GetObject("CommissionSetting", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CommissionsPayableToText => GetObject("CommissionsPayableToText", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Company => GetObject("Company", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyEmail => GetObject("CompanyEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyLegalName => GetObject("CompanyLegalName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyName => GetObject("CompanyName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyNews => GetObject("CompanyNews", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyNewsDetail => GetObject("CompanyNewsDetail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyPhone => GetObject("CompanyPhone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyRegisteredForVat => GetObject("CompanyRegisteredForVat", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompanyRegistrationNumber => GetObject("CompanyRegistrationNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Complete => GetObject("Complete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CompleteEnrollment => GetObject("CompleteEnrollment", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConferenceCallNumber => GetObject("ConferenceCallNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConferenceNumber => GetObject("ConferenceNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConferencePIN => GetObject("ConferencePIN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConfigureAutoOrder => GetObject("ConfigureAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConfirmCancelAutoOrder => GetObject("ConfirmCancelAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConfirmCancelOrder => GetObject("ConfirmCancelOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConfirmCustomerPlacement => GetObject("ConfirmCustomerPlacement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConfirmPassword => GetObject("ConfirmPassword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConfirmUseItemsInEnrollment => GetObject("ConfirmUseItemsInEnrollment", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Connect => GetObject("Connect", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConnectionError => GetObject("ConnectionError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConnectWithAParty => GetObject("ConnectWithAParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ConnectWithUs => GetObject("ConnectWithUs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///ie contact info
       ///</summary>
        public static string Contact => GetObject("Contact", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContactFirstName => GetObject("ContactFirstName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContactInfo => GetObject("ContactInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContactInformation => GetObject("ContactInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContactLastName => GetObject("ContactLastName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContactMiddle => GetObject("ContactMiddle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContactUs => GetObject("ContactUs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Content => GetObject("Content", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ContentDescription => GetObject("ContentDescription", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Continue => GetObject("Continue", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart HeaderOption
       ///</summary>
        public static string ContinueShopping => GetObject("ContinueShopping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Controller => GetObject("Controller", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Copyright => GetObject("Copyright", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CopyToCalendar => GetObject("CopyToCalendar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CorporateCalendar => GetObject("CorporateCalendar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Cost => GetObject("Cost", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CouldNotFetchData => GetObject("CouldNotFetchData", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CouldntFindAddress => GetObject("CouldntFindAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country => GetObject("Country", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AD => GetObject("Country_AD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AE => GetObject("Country_AE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AF => GetObject("Country_AF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AG => GetObject("Country_AG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AI => GetObject("Country_AI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AL => GetObject("Country_AL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AM => GetObject("Country_AM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AN => GetObject("Country_AN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AO => GetObject("Country_AO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AQ => GetObject("Country_AQ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AR => GetObject("Country_AR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AS => GetObject("Country_AS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AT => GetObject("Country_AT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AU => GetObject("Country_AU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AW => GetObject("Country_AW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AY => GetObject("Country_AY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_AZ => GetObject("Country_AZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BA => GetObject("Country_BA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BB => GetObject("Country_BB", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BD => GetObject("Country_BD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BE => GetObject("Country_BE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BF => GetObject("Country_BF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BG => GetObject("Country_BG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BH => GetObject("Country_BH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BI => GetObject("Country_BI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BJ => GetObject("Country_BJ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BM => GetObject("Country_BM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BN => GetObject("Country_BN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BO => GetObject("Country_BO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BR => GetObject("Country_BR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BS => GetObject("Country_BS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BT => GetObject("Country_BT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BV => GetObject("Country_BV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BW => GetObject("Country_BW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BY => GetObject("Country_BY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_BZ => GetObject("Country_BZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CA => GetObject("Country_CA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CC => GetObject("Country_CC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CD => GetObject("Country_CD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CF => GetObject("Country_CF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CG => GetObject("Country_CG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CH => GetObject("Country_CH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CI => GetObject("Country_CI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CK => GetObject("Country_CK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CL => GetObject("Country_CL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CM => GetObject("Country_CM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CN => GetObject("Country_CN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CO => GetObject("Country_CO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CR => GetObject("Country_CR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CS => GetObject("Country_CS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CU => GetObject("Country_CU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CV => GetObject("Country_CV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CX => GetObject("Country_CX", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CY => GetObject("Country_CY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_CZ => GetObject("Country_CZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_DE => GetObject("Country_DE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_DJ => GetObject("Country_DJ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_DK => GetObject("Country_DK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_DM => GetObject("Country_DM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_DO => GetObject("Country_DO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_DZ => GetObject("Country_DZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_EC => GetObject("Country_EC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_EE => GetObject("Country_EE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_EG => GetObject("Country_EG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_EH => GetObject("Country_EH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ER => GetObject("Country_ER", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ES => GetObject("Country_ES", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ET => GetObject("Country_ET", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FI => GetObject("Country_FI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FJ => GetObject("Country_FJ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FK => GetObject("Country_FK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FM => GetObject("Country_FM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FO => GetObject("Country_FO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FR => GetObject("Country_FR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_FX => GetObject("Country_FX", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GA => GetObject("Country_GA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GB => GetObject("Country_GB", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GD => GetObject("Country_GD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GE => GetObject("Country_GE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GF => GetObject("Country_GF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GG => GetObject("Country_GG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GH => GetObject("Country_GH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GI => GetObject("Country_GI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GL => GetObject("Country_GL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GM => GetObject("Country_GM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GN => GetObject("Country_GN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GP => GetObject("Country_GP", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GQ => GetObject("Country_GQ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GR => GetObject("Country_GR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GS => GetObject("Country_GS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GT => GetObject("Country_GT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GU => GetObject("Country_GU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GW => GetObject("Country_GW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_GY => GetObject("Country_GY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HE => GetObject("Country_HE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HK => GetObject("Country_HK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HM => GetObject("Country_HM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HN => GetObject("Country_HN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HR => GetObject("Country_HR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HT => GetObject("Country_HT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_HU => GetObject("Country_HU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ID => GetObject("Country_ID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IE => GetObject("Country_IE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IL => GetObject("Country_IL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IM => GetObject("Country_IM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IN => GetObject("Country_IN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IO => GetObject("Country_IO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IQ => GetObject("Country_IQ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IR => GetObject("Country_IR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IS => GetObject("Country_IS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_IT => GetObject("Country_IT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_JE => GetObject("Country_JE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_JM => GetObject("Country_JM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_JO => GetObject("Country_JO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_JP => GetObject("Country_JP", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KE => GetObject("Country_KE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KG => GetObject("Country_KG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KH => GetObject("Country_KH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KI => GetObject("Country_KI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KM => GetObject("Country_KM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KN => GetObject("Country_KN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KP => GetObject("Country_KP", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KR => GetObject("Country_KR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KW => GetObject("Country_KW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KY => GetObject("Country_KY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_KZ => GetObject("Country_KZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LA => GetObject("Country_LA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LB => GetObject("Country_LB", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LC => GetObject("Country_LC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LI => GetObject("Country_LI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LK => GetObject("Country_LK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LR => GetObject("Country_LR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LS => GetObject("Country_LS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LT => GetObject("Country_LT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LU => GetObject("Country_LU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LV => GetObject("Country_LV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_LY => GetObject("Country_LY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MA => GetObject("Country_MA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MC => GetObject("Country_MC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MD => GetObject("Country_MD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ME => GetObject("Country_ME", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MF => GetObject("Country_MF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MG => GetObject("Country_MG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MH => GetObject("Country_MH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MK => GetObject("Country_MK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ML => GetObject("Country_ML", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MM => GetObject("Country_MM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MN => GetObject("Country_MN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MO => GetObject("Country_MO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MP => GetObject("Country_MP", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MQ => GetObject("Country_MQ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MR => GetObject("Country_MR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MS => GetObject("Country_MS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MT => GetObject("Country_MT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MU => GetObject("Country_MU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MV => GetObject("Country_MV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MW => GetObject("Country_MW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MX => GetObject("Country_MX", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MY => GetObject("Country_MY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_MZ => GetObject("Country_MZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NA => GetObject("Country_NA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NC => GetObject("Country_NC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NE => GetObject("Country_NE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NF => GetObject("Country_NF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NG => GetObject("Country_NG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NI => GetObject("Country_NI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NL => GetObject("Country_NL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NO => GetObject("Country_NO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NP => GetObject("Country_NP", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NR => GetObject("Country_NR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NU => GetObject("Country_NU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_NZ => GetObject("Country_NZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_OM => GetObject("Country_OM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_OO => GetObject("Country_OO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PA => GetObject("Country_PA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PE => GetObject("Country_PE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PF => GetObject("Country_PF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PG => GetObject("Country_PG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PH => GetObject("Country_PH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PK => GetObject("Country_PK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PL => GetObject("Country_PL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PM => GetObject("Country_PM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PN => GetObject("Country_PN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PR => GetObject("Country_PR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PS => GetObject("Country_PS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PT => GetObject("Country_PT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PW => GetObject("Country_PW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_PY => GetObject("Country_PY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_QA => GetObject("Country_QA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_RE => GetObject("Country_RE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_RO => GetObject("Country_RO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_RS => GetObject("Country_RS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_RU => GetObject("Country_RU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_RW => GetObject("Country_RW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SA => GetObject("Country_SA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SB => GetObject("Country_SB", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SC => GetObject("Country_SC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SD => GetObject("Country_SD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SE => GetObject("Country_SE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SG => GetObject("Country_SG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SH => GetObject("Country_SH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SI => GetObject("Country_SI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SJ => GetObject("Country_SJ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SK => GetObject("Country_SK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SL => GetObject("Country_SL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SM => GetObject("Country_SM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SN => GetObject("Country_SN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SO => GetObject("Country_SO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SQ => GetObject("Country_SQ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SR => GetObject("Country_SR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SS => GetObject("Country_SS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ST => GetObject("Country_ST", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SU => GetObject("Country_SU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SV => GetObject("Country_SV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SX => GetObject("Country_SX", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SY => GetObject("Country_SY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_SZ => GetObject("Country_SZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TC => GetObject("Country_TC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TD => GetObject("Country_TD", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TF => GetObject("Country_TF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TG => GetObject("Country_TG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TH => GetObject("Country_TH", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TJ => GetObject("Country_TJ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TK => GetObject("Country_TK", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TL => GetObject("Country_TL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TM => GetObject("Country_TM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TN => GetObject("Country_TN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TO => GetObject("Country_TO", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TP => GetObject("Country_TP", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TR => GetObject("Country_TR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TT => GetObject("Country_TT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TV => GetObject("Country_TV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TW => GetObject("Country_TW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_TZ => GetObject("Country_TZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_UA => GetObject("Country_UA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_UG => GetObject("Country_UG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_UM => GetObject("Country_UM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_US => GetObject("Country_US", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_UY => GetObject("Country_UY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_UZ => GetObject("Country_UZ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VA => GetObject("Country_VA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VC => GetObject("Country_VC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VE => GetObject("Country_VE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VG => GetObject("Country_VG", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VI => GetObject("Country_VI", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VN => GetObject("Country_VN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_VU => GetObject("Country_VU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_WF => GetObject("Country_WF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_WS => GetObject("Country_WS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_YE => GetObject("Country_YE", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_YT => GetObject("Country_YT", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_YU => GetObject("Country_YU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ZA => GetObject("Country_ZA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ZM => GetObject("Country_ZM", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ZR => GetObject("Country_ZR", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Country_ZW => GetObject("Country_ZW", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CountryOfResidence => GetObject("CountryOfResidence", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Create => GetObject("Create", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateAnAccount => GetObject("CreateAnAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateAutoOrder => GetObject("CreateAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateEvent => GetObject("CreateEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateHostersOrder => GetObject("CreateHostersOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewAutoOrder => GetObject("CreateNewAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewCategory => GetObject("CreateNewCategory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewEvent => GetObject("CreateNewEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewGuest => GetObject("CreateNewGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewHostess => GetObject("CreateNewHostess", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewParty => GetObject("CreateNewParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateNewResource => GetObject("CreateNewResource", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateOrder => GetObject("CreateOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateOrderCloseParty => GetObject("CreateOrderCloseParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateOrderClosePartyTitle => GetObject("CreateOrderClosePartyTitle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateOrderCurrentParty => GetObject("CreateOrderCurrentParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreateYourAutoOrder => GetObject("CreateYourAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreatingPartyFromParty => GetObject("CreatingPartyFromParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreatingViewingEvent => GetObject("CreatingViewingEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardEnding => GetObject("CreditCardEnding", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardEndingIn => GetObject("CreditCardEndingIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType => GetObject("CreditCardType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_01 => GetObject("CreditCardType_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_02 => GetObject("CreditCardType_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_03 => GetObject("CreditCardType_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_04 => GetObject("CreditCardType_04", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_05 => GetObject("CreditCardType_05", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_06 => GetObject("CreditCardType_06", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_07 => GetObject("CreditCardType_07", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_08 => GetObject("CreditCardType_08", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_09 => GetObject("CreditCardType_09", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_10 => GetObject("CreditCardType_10", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_11 => GetObject("CreditCardType_11", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_12 => GetObject("CreditCardType_12", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_13 => GetObject("CreditCardType_13", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_14 => GetObject("CreditCardType_14", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_15 => GetObject("CreditCardType_15", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_16 => GetObject("CreditCardType_16", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_17 => GetObject("CreditCardType_17", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_18 => GetObject("CreditCardType_18", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_19 => GetObject("CreditCardType_19", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_20 => GetObject("CreditCardType_20", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_21 => GetObject("CreditCardType_21", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_22 => GetObject("CreditCardType_22", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_23 => GetObject("CreditCardType_23", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_24 => GetObject("CreditCardType_24", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_25 => GetObject("CreditCardType_25", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_26 => GetObject("CreditCardType_26", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_27 => GetObject("CreditCardType_27", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_28 => GetObject("CreditCardType_28", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_29 => GetObject("CreditCardType_29", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_30 => GetObject("CreditCardType_30", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_31 => GetObject("CreditCardType_31", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_32 => GetObject("CreditCardType_32", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditCardType_33 => GetObject("CreditCardType_33", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  autoorder count
       ///</summary>
        public static string CreditCardUsedAsAutoOrderPaymentMethod => GetObject("CreditCardUsedAsAutoOrderPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditDebitCardEndingIn => GetObject("CreditDebitCardEndingIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CreditDebitCards => GetObject("CreditDebitCards", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string CreditEarned => GetObject("CreditEarned", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Current => GetObject("Current", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CurrentCommissions => GetObject("CurrentCommissions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CurrentlyReceivingNotificationsAt => GetObject("CurrentlyReceivingNotificationsAt", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CurrentRank => GetObject("CurrentRank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Customer => GetObject("Customer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerCouldNotBePlaced => GetObject("CustomerCouldNotBePlaced", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerDetails => GetObject("CustomerDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_00 => GetObject("CustomerStatus_00", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_01 => GetObject("CustomerStatus_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_02 => GetObject("CustomerStatus_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_03 => GetObject("CustomerStatus_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_04 => GetObject("CustomerStatus_04", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_05 => GetObject("CustomerStatus_05", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_06 => GetObject("CustomerStatus_06", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_07 => GetObject("CustomerStatus_07", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerStatus_08 => GetObject("CustomerStatus_08", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType => GetObject("CustomerType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType_01 => GetObject("CustomerType_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType_01_Short => GetObject("CustomerType_01_Short", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType_02 => GetObject("CustomerType_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType_02_Short => GetObject("CustomerType_02_Short", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType_03 => GetObject("CustomerType_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CustomerType_03_Short => GetObject("CustomerType_03_Short", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Customize => GetObject("Customize", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CV => GetObject("CV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string CVV => GetObject("CVV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string D => GetObject("D", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Daily => GetObject("Daily", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Danish => GetObject("Danish", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Dashboard => GetObject("Dashboard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Data => GetObject("Data", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Date => GetObject("Date", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Day => GetObject("Day", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DayOfMonth => GetObject("DayOfMonth", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DayOfWeek => GetObject("DayOfWeek", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Days => GetObject("Days", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DeclinedOrders => GetObject("DeclinedOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Delete => GetObject("Delete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DeleteAlert => GetObject("DeleteAlert", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DeleteAlertQuestion => GetObject("DeleteAlertQuestion", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DeleteResource => GetObject("DeleteResource", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DeleteResourceQuestion => GetObject("DeleteResourceQuestion", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Description => GetObject("Description", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Details => GetObject("Details", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DirectDeposit => GetObject("DirectDeposit", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DirectionsMakingPlacement => GetObject("DirectionsMakingPlacement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DistributorSince => GetObject("DistributorSince", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DoNotRepeatEvent => GetObject("DoNotRepeatEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DoNotUseYourHostessEmail => GetObject("DoNotUseYourHostessEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DontHaveAccount => GetObject("DontHaveAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DontHaveDDText => GetObject("DontHaveDDText", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DownlineAutoOrders => GetObject("DownlineAutoOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DownlineOrders => GetObject("DownlineOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DownlineRanks => GetObject("DownlineRanks", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Auto order popup modal
       ///</summary>
        public static string DoYouWishToUpdateAutoOrder => GetObject("DoYouWishToUpdateAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DragAndDropResources => GetObject("DragAndDropResources", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Dress => GetObject("Dress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Dutch => GetObject("Dutch", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string DynamicItemPrice => GetObject("DynamicItemPrice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string E => GetObject("E", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Earned => GetObject("Earned", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Earnings => GetObject("Earnings", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Edit => GetObject("Edit", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditAddToOrder => GetObject("EditAddToOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditAvatar => GetObject("EditAvatar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditContent => GetObject("EditContent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditEvent => GetObject("EditEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditGuest => GetObject("EditGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditingAutoOrderBankAccounts => GetObject("EditingAutoOrderBankAccounts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditingAutoOrderCreditCards => GetObject("EditingAutoOrderCreditCards", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditItems => GetObject("EditItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditParty => GetObject("EditParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditPaymentMethod => GetObject("EditPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditProcessDate => GetObject("EditProcessDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditResource => GetObject("EditResource", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditSeries => GetObject("EditSeries", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditShipMethod => GetObject("EditShipMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EditShippingAddress => GetObject("EditShippingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Email => GetObject("Email", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EmailAlreadyInUse => GetObject("EmailAlreadyInUse", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EmailHasBeenSent => GetObject("EmailHasBeenSent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EmailNotFound => GetObject("EmailNotFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EmailVerified => GetObject("EmailVerified", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string End => GetObject("End", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EndDate => GetObject("EndDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Ends => GetObject("Ends", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EndTimezone => GetObject("EndTimezone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string English => GetObject("English", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Enroller => GetObject("Enroller", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollerChosenMessage => GetObject("EnrollerChosenMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollerSearchHeader => GetObject("EnrollerSearchHeader", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollerSponsor => GetObject("EnrollerSponsor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollerTree => GetObject("EnrollerTree", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentCommissionSettingsHint => GetObject("EnrollmentCommissionSettingsHint", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentComplete => GetObject("EnrollmentComplete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentConfiguration => GetObject("EnrollmentConfiguration", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentOptionOneHeader => GetObject("EnrollmentOptionOneHeader", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentOptionThreeHeader => GetObject("EnrollmentOptionThreeHeader", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentOptionTwoHeader => GetObject("EnrollmentOptionTwoHeader", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentPack => GetObject("EnrollmentPack", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentShopping => GetObject("EnrollmentShopping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollmentSubmittingMessage => GetObject("EnrollmentSubmittingMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnrollNew => GetObject("EnrollNew", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterAPersonalMessage => GetObject("EnterAPersonalMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///LoginAlerts
       ///</summary>
        public static string EnterCategoryInAllLanguages => GetObject("EnterCategoryInAllLanguages", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterLocation => GetObject("EnterLocation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterLoginNameBeforeProceeding => GetObject("EnterLoginNameBeforeProceeding", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterPassword => GetObject("EnterPassword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterPasswordBeforeProceeding => GetObject("EnterPasswordBeforeProceeding", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterYourCreditCardInformation => GetObject("EnterYourCreditCardInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterYourEmail => GetObject("EnterYourEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EnterYourEmailAddress => GetObject("EnterYourEmailAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Error => GetObject("Error", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ErrorLoadingItemList => GetObject("ErrorLoadingItemList", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///DashboardHeader
       ///</summary>
        public static string EssentialReports => GetObject("EssentialReports", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Estimated => GetObject("Estimated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EstimatedCost => GetObject("EstimatedCost", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EstimatedTotal => GetObject("EstimatedTotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Event => GetObject("Event", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EventDate => GetObject("EventDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EventDetails => GetObject("EventDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Events => GetObject("Events", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EventTime => GetObject("EventTime", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EventTitleRequired => GetObject("EventTitleRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EventType => GetObject("EventType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string EventTypes => GetObject("EventTypes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExceptionCheckingElem => GetObject("ExceptionCheckingElem", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExceptionDetails => GetObject("ExceptionDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExceptionDetailsError => GetObject("ExceptionDetailsError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string ExigoCredit => GetObject("ExigoCredit", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExpirationDate => GetObject("ExpirationDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExpirationMonth => GetObject("ExpirationMonth", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Expired => GetObject("Expired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExpiredResetMessage => GetObject("ExpiredResetMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Expires => GetObject("Expires", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ExploreOurWorld => GetObject("ExploreOurWorld", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string F => GetObject("F", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Facebook => GetObject("Facebook", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Failed => GetObject("Failed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FAQ => GetObject("FAQ", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Fax => GetObject("Fax", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Faxes => GetObject("Faxes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FeaturedProducts => GetObject("FeaturedProducts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Fee => GetObject("Fee", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Female => GetObject("Female", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FIENumber => GetObject("FIENumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FileType => GetObject("FileType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Filters => GetObject("Filters", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FindByKeyword => GetObject("FindByKeyword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FindByMedia => GetObject("FindByMedia", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FindRepresentative => GetObject("FindRepresentative", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FindSponsorByNameOrID => GetObject("FindSponsorByNameOrID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FindYourReferrer => GetObject("FindYourReferrer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Finnish => GetObject("Finnish", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string First => GetObject("First", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FirstName => GetObject("FirstName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Flyer => GetObject("Flyer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ForgotPassword => GetObject("ForgotPassword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ForgotPasswordEmailBody => GetObject("ForgotPasswordEmailBody", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ForgotPasswordEmailSubject => GetObject("ForgotPasswordEmailSubject", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ForgotPasswordInstructions => GetObject("ForgotPasswordInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FreeDelivery => GetObject("FreeDelivery", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FreeShipping => GetObject("FreeShipping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string French => GetObject("French", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Frequency => GetObject("Frequency", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyChoice => GetObject("FrequencyChoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType => GetObject("FrequencyType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_01 => GetObject("FrequencyType_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_02 => GetObject("FrequencyType_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_03 => GetObject("FrequencyType_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_04 => GetObject("FrequencyType_04", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_05 => GetObject("FrequencyType_05", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_06 => GetObject("FrequencyType_06", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_07 => GetObject("FrequencyType_07", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_08 => GetObject("FrequencyType_08", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_09 => GetObject("FrequencyType_09", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_10 => GetObject("FrequencyType_10", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyType_11 => GetObject("FrequencyType_11", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FrequencyUppercase => GetObject("FrequencyUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string From => GetObject("From", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FromCustomerID => GetObject("FromCustomerID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FromCustomerName => GetObject("FromCustomerName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string FullName => GetObject("FullName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GenealogyType => GetObject("GenealogyType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string General => GetObject("General", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GeneralError => GetObject("GeneralError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GenericSomethingWentWrong => GetObject("GenericSomethingWentWrong", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string German => GetObject("German", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetCompanyNewsFailed => GetObject("GetCompanyNewsFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetCurrentEarningsFailed => GetObject("GetCurrentEarningsFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetInTouch => GetObject("GetInTouch", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetNewestDistributorsFailed => GetObject("GetNewestDistributorsFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetRecentActivityFailed => GetObject("GetRecentActivityFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetRecentOrdersFailed => GetObject("GetRecentOrdersFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GetVolumesFailed => GetObject("GetVolumesFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_AddNewCard => GetObject("Global_AddNewCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_Banner_Image1 => GetObject("Global_Banner_Image1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_Banner_Image3 => GetObject("Global_Banner_Image3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_Boy_Laptop => GetObject("Global_Boy_Laptop", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_CreateAutoOrder => GetObject("Global_CreateAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_EditMailingAddress => GetObject("Global_EditMailingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_EditMainAddress => GetObject("Global_EditMainAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_EditOtherAddress => GetObject("Global_EditOtherAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_EditPrimaryCard => GetObject("Global_EditPrimaryCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_EditSecondaryCard => GetObject("Global_EditSecondaryCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_Exigo_Logo_Base => GetObject("Global_Exigo_Logo_Base", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_LoginToYourAccount => GetObject("Global_LoginToYourAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_MailingAddress => GetObject("Global_MailingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_MainAddress => GetObject("Global_MainAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_ManageAutoOrder => GetObject("Global_ManageAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_OtherAddress => GetObject("Global_OtherAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_Pattern2 => GetObject("Global_Pattern2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Global_video_play => GetObject("Global_video_play", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Go => GetObject("Go", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GoBack => GetObject("GoBack", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GooglePlus => GetObject("GooglePlus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GrandTotal => GetObject("GrandTotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Gravatar => GetObject("Gravatar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GuestAttendanceUpdated => GetObject("GuestAttendanceUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GuestCreatedSuccessfully => GetObject("GuestCreatedSuccessfully", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GuestRemovedFromList => GetObject("GuestRemovedFromList", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GuestUpdated => GetObject("GuestUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string GuestUpdatedSuccessfully => GetObject("GuestUpdatedSuccessfully", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HalfPriceItem => GetObject("HalfPriceItem", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HasAttended => GetObject("HasAttended", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HasAutoOrder => GetObject("HasAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HasBeenAddedToAutoOrder => GetObject("HasBeenAddedToAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HasBeenAddedToOrder => GetObject("HasBeenAddedToOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///used in javascript for toastr
       ///</summary>
        public static string HasBeenAddedToYour => GetObject("HasBeenAddedToYour", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HasRewardsForHostingParty => GetObject("HasRewardsForHostingParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HaveNotInvited => GetObject("HaveNotInvited", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Heading => GetObject("Heading", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Hello => GetObject("Hello", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Hey => GetObject("Hey", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HighestRank => GetObject("HighestRank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HighToLow => GetObject("HighToLow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  accoung holder's name
       ///</summary>
        public static string HoldersAccountEndingIn => GetObject("HoldersAccountEndingIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///shopping breadcrumbs
       ///</summary>
        public static string Home => GetObject("Home", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Home_2_Paragraph2 => GetObject("Home_2_Paragraph2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HomePhone => GetObject("HomePhone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Host => GetObject("Host", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostAddress => GetObject("HostAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostedBy => GetObject("HostedBy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Hostess => GetObject("Hostess", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostessCredits => GetObject("HostessCredits", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostessInfo => GetObject("HostessInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostessInvite => GetObject("HostessInvite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostessRewardsMessage => GetObject("HostessRewardsMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HostessShopportunity => GetObject("HostessShopportunity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Hours => GetObject("Hours", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HowOftenDoYouWantToRecieve => GetObject("HowOftenDoYouWantToRecieve", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HowToGuide => GetObject("HowToGuide", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HowWouldYouLikeToPay => GetObject("HowWouldYouLikeToPay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HowWouldYouLikeToPayAutoOrder => GetObject("HowWouldYouLikeToPayAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string HowWouldYouLikeToReceiveCommissions => GetObject("HowWouldYouLikeToReceiveCommissions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ID => GetObject("ID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IfAResource => GetObject("IfAResource", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IfYouAreUnable => GetObject("IfYouAreUnable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IfYouDelete => GetObject("IfYouDelete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Image => GetObject("Image", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InactiveAutoOrder => GetObject("InactiveAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IncludingTaxAbbreviated => GetObject("IncludingTaxAbbreviated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IncompleteAccount => GetObject("IncompleteAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Index => GetObject("Index", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Index_ItemName1 => GetObject("Index_ItemName1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Index_ItemName2 => GetObject("Index_ItemName2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Index_ItemName3 => GetObject("Index_ItemName3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Index_ItemName4 => GetObject("Index_ItemName4", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Index_ItemName5 => GetObject("Index_ItemName5", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IndividualDistributor => GetObject("IndividualDistributor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Info => GetObject("Info", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InfoWhyAutoOrder => GetObject("InfoWhyAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InnerException => GetObject("InnerException", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InOrderToCreate => GetObject("InOrderToCreate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InOrderToSearch => GetObject("InOrderToSearch", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Instagram => GetObject("Instagram", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InsufficientPrivileges => GetObject("InsufficientPrivileges", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InThePast_Days => GetObject("InThePast_Days", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InvalidPaymentType => GetObject("InvalidPaymentType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InvalidPaymentTypeAdded => GetObject("InvalidPaymentTypeAdded", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InvalidWebAlias => GetObject("InvalidWebAlias", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Invite => GetObject("Invite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Invoice => GetObject("Invoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string InvoiceFor => GetObject("InvoiceFor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IsAllDay => GetObject("IsAllDay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IsAvailable => GetObject("IsAvailable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IssuePlacingOrder => GetObject("IssuePlacingOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IssueValidatingLoginName => GetObject("IssueValidatingLoginName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Item => GetObject("Item", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemAdded => GetObject("ItemAdded", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemC => GetObject("ItemC", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemDeleted => GetObject("ItemDeleted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemNotFound => GetObject("ItemNotFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string ItemQty => GetObject("ItemQty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Items => GetObject("Items", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemsInCart => GetObject("ItemsInCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemsToBuyNow => GetObject("ItemsToBuyNow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemsUppercase => GetObject("ItemsUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  item quantity
       ///</summary>
        public static string ItemsWillProcessOn => GetObject("ItemsWillProcessOn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ItemUpdated => GetObject("ItemUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string IWouldLikeToRecieveEmails => GetObject("IWouldLikeToRecieveEmails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Join => GetObject("Join", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string JoinAndSave => GetObject("JoinAndSave", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string JoinDate => GetObject("JoinDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Joined => GetObject("Joined", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string JoinNow => GetObject("JoinNow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string JoinTheAutoOrder => GetObject("JoinTheAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string JoinTheFamily => GetObject("JoinTheFamily", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_And => GetObject("Kendo_And", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Clear => GetObject("Kendo_Clear", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Contains => GetObject("Kendo_Contains", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_DoesNotContain => GetObject("Kendo_DoesNotContain", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_DragColumnHeader => GetObject("Kendo_DragColumnHeader", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_EndsWith => GetObject("Kendo_EndsWith", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Filter => GetObject("Kendo_Filter", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_GoToFirstPage => GetObject("Kendo_GoToFirstPage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_GoToLastPage => GetObject("Kendo_GoToLastPage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_GoToNextPage => GetObject("Kendo_GoToNextPage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_GoToPreviousPage => GetObject("Kendo_GoToPreviousPage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsAfter => GetObject("Kendo_IsAfter", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsAfterOrEqualTo => GetObject("Kendo_IsAfterOrEqualTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsBefore => GetObject("Kendo_IsBefore", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsBeforeOrEqualTo => GetObject("Kendo_IsBeforeOrEqualTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsEqualTo => GetObject("Kendo_IsEqualTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsFalse => GetObject("Kendo_IsFalse", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsGreaterThan => GetObject("Kendo_IsGreaterThan", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsGreaterThanOrEqualTo => GetObject("Kendo_IsGreaterThanOrEqualTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsLessThan => GetObject("Kendo_IsLessThan", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsLessThanOrEqualTo => GetObject("Kendo_IsLessThanOrEqualTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsNotEqualTo => GetObject("Kendo_IsNotEqualTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_IsTrue => GetObject("Kendo_IsTrue", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_ItemsOf => GetObject("Kendo_ItemsOf", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_ItemsPerPage => GetObject("Kendo_ItemsPerPage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_MorePages => GetObject("Kendo_MorePages", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_NoItemsToDisplay => GetObject("Kendo_NoItemsToDisplay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Of => GetObject("Kendo_Of", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Of_Formatted => GetObject("Kendo_Of_Formatted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Page => GetObject("Kendo_Page", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_Refresh => GetObject("Kendo_Refresh", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_ShowItemsWithValueThat => GetObject("Kendo_ShowItemsWithValueThat", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Kendo_StartsWith => GetObject("Kendo_StartsWith", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Key => GetObject("Key", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Keywords => GetObject("Keywords", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language => GetObject("Language", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_000 => GetObject("Language_000", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_001 => GetObject("Language_001", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_002 => GetObject("Language_002", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_003 => GetObject("Language_003", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_004 => GetObject("Language_004", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_005 => GetObject("Language_005", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_006 => GetObject("Language_006", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_007 => GetObject("Language_007", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_008 => GetObject("Language_008", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_009 => GetObject("Language_009", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_010 => GetObject("Language_010", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_011 => GetObject("Language_011", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_012 => GetObject("Language_012", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_013 => GetObject("Language_013", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_014 => GetObject("Language_014", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_015 => GetObject("Language_015", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_016 => GetObject("Language_016", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_017 => GetObject("Language_017", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_018 => GetObject("Language_018", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_019 => GetObject("Language_019", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_020 => GetObject("Language_020", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_021 => GetObject("Language_021", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_022 => GetObject("Language_022", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Language_203 => GetObject("Language_203", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Last => GetObject("Last", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LastName => GetObject("LastName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LastProcessDate => GetObject("LastProcessDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LeadSource => GetObject("LeadSource", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Level => GetObject("Level", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LifetimeRank => GetObject("LifetimeRank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LinkedIn => GetObject("LinkedIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Loading => GetObject("Loading", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoadingFailed => GetObject("LoadingFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoadingSummary => GetObject("LoadingSummary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Location => GetObject("Location", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Login => GetObject("Login", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoginAlerts => GetObject("LoginAlerts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoginFailed => GetObject("LoginFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoginNameInvalid => GetObject("LoginNameInvalid", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoginNameNotAvailable => GetObject("LoginNameNotAvailable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoginNameRules => GetObject("LoginNameRules", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LoginNameValid => GetObject("LoginNameValid", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_AmericanExpress => GetObject("Logo_AmericanExpress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_Diners => GetObject("Logo_Diners", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_Discover => GetObject("Logo_Discover", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_JCB => GetObject("Logo_JCB", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_Mastercard => GetObject("Logo_Mastercard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_UnionPay => GetObject("Logo_UnionPay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Logo_VISA => GetObject("Logo_VISA", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string LowToHigh => GetObject("LowToHigh", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Lvl => GetObject("Lvl", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MakeAPlacement => GetObject("MakeAPlacement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MakeOurCustomersHappy => GetObject("MakeOurCustomersHappy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Male => GetObject("Male", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Manage => GetObject("Manage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ManageCategories => GetObject("ManageCategories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ManageParty => GetObject("ManageParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ManagePaymentMethods => GetObject("ManagePaymentMethods", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ManageResourceCategories => GetObject("ManageResourceCategories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard  Navigation Header
       ///</summary>
        public static string ManageResources => GetObject("ManageResources", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Map => GetObject("Map", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Market => GetObject("Market", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MarketLanguage_1 => GetObject("MarketLanguage_1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MarketLanguage_2 => GetObject("MarketLanguage_2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MarketLanguage_3 => GetObject("MarketLanguage_3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MarketLanguage_4 => GetObject("MarketLanguage_4", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Me => GetObject("Me", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MemberInfo => GetObject("MemberInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Message => GetObject("Message", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Metrics => GetObject("Metrics", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MiddleName => GetObject("MiddleName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Minutes => GetObject("Minutes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Month => GetObject("Month", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Monthly => GetObject("Monthly", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Months => GetObject("Months", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MoreInfo => GetObject("MoreInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MorePaymentOptions => GetObject("MorePaymentOptions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Resources Manager
       ///</summary>
        public static string MoveDown => GetObject("MoveDown", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Resources Manager
       ///</summary>
        public static string MoveUp => GetObject("MoveUp", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Waiting Room
       ///</summary>
        public static string MustBePlacedBy => GetObject("MustBePlacedBy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MustBeReff => GetObject("MustBeReff", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MustSaveCardMessage => GetObject("MustSaveCardMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MyAccount => GetObject("MyAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MyCart => GetObject("MyCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MyCustomers => GetObject("MyCustomers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MyOpenParties => GetObject("MyOpenParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MyPartySales => GetObject("MyPartySales", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string MySpace => GetObject("MySpace", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Name => GetObject("Name", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NameOfBank => GetObject("NameOfBank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NameOnAccount => GetObject("NameOnAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NameOnCard => GetObject("NameOnCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NameOnYourAccount => GetObject("NameOnYourAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NeedHelp => GetObject("NeedHelp", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NetAxept => GetObject("NetAxept", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Never => GetObject("Never", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewCategory => GetObject("NewCategory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewCopiedEvent => GetObject("NewCopiedEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewDistributorsList => GetObject("NewDistributorsList", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Newer => GetObject("Newer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewEvent => GetObject("NewEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewGuestCreated => GetObject("NewGuestCreated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewGuestParty => GetObject("NewGuestParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string News => GetObject("News", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NewTeamMembers => GetObject("NewTeamMembers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Next => GetObject("Next", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NextAutoOrderDay => GetObject("NextAutoOrderDay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NextDate => GetObject("NextDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NextProcessDate => GetObject("NextProcessDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NextRank => GetObject("NextRank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NextRunDate => GetObject("NextRunDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string No => GetObject("No", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoAcceptedCommissions => GetObject("NoAcceptedCommissions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoActivity => GetObject("NoActivity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoAddressesOnFile => GetObject("NoAddressesOnFile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Conditional statement if page is empty
       ///</summary>
        public static string NoAutoOrderFound => GetObject("NoAutoOrderFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Conditional statement if page is empty
       ///</summary>
        public static string NoAutoShipsFound => GetObject("NoAutoShipsFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoCurrentNewsDisplay => GetObject("NoCurrentNewsDisplay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoCurrentOrdersParty => GetObject("NoCurrentOrdersParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoDistributorMessage => GetObject("NoDistributorMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoItemsInCart => GetObject("NoItemsInCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoItemsPurchased => GetObject("NoItemsPurchased", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoItemsPurchasedInOrder => GetObject("NoItemsPurchasedInOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoMembersToPlace => GetObject("NoMembersToPlace", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string None => GetObject("None", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoNews => GetObject("NoNews", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoOrdersFound => GetObject("NoOrdersFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoOrdersParty => GetObject("NoOrdersParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoPaymentMethod => GetObject("NoPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Conditional statement if page is empty
       ///</summary>
        public static string NoRecentActivity => GetObject("NoRecentActivity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoRecentEnrollments => GetObject("NoRecentEnrollments", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoResourcesSaved => GetObject("NoResourcesSaved", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Norsk => GetObject("Norsk", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Notes => GetObject("Notes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Notifications => GetObject("Notifications", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NotificationStatus => GetObject("NotificationStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///S
       ///</summary>
        public static string NotificationSubscriptionInstructions => GetObject("NotificationSubscriptionInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoTitle => GetObject("NoTitle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NoTitleSpecified => GetObject("NoTitleSpecified", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NotReferred => GetObject("NotReferred", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NotReferredShopNow => GetObject("NotReferredShopNow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string NotUseHostessEmail => GetObject("NotUseHostessEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Occurrences => GetObject("Occurrences", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OfEachMonth => GetObject("OfEachMonth", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OkEmail => GetObject("OkEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OkToEmail => GetObject("OkToEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Older => GetObject("Older", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string On => GetObject("On", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OnceCreatedHostessOrderMessage => GetObject("OnceCreatedHostessOrderMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Online => GetObject("Online", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Oops => GetObject("Oops", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OopsSomethingWentWrong => GetObject("OopsSomethingWentWrong", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OpenOrders => GetObject("OpenOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OpenParties => GetObject("OpenParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OpenPartiesFound => GetObject("OpenPartiesFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OpenPosition => GetObject("OpenPosition", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OptedInStatus => GetObject("OptedInStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OptedOutStatus => GetObject("OptedOutStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OptedOutStatusNote => GetObject("OptedOutStatusNote", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string or => GetObject("or", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Order => GetObject("Order", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderComplete => GetObject("OrderComplete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderDate => GetObject("OrderDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderDetails => GetObject("OrderDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Ordered => GetObject("Ordered", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderHistory => GetObject("OrderHistory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderID => GetObject("OrderID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderItems => GetObject("OrderItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderNumber => GetObject("OrderNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Orders => GetObject("Orders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrdersFound => GetObject("OrdersFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_00 => GetObject("OrderStatus_00", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_01 => GetObject("OrderStatus_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_02 => GetObject("OrderStatus_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_03 => GetObject("OrderStatus_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_04 => GetObject("OrderStatus_04", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_05 => GetObject("OrderStatus_05", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_06 => GetObject("OrderStatus_06", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_07 => GetObject("OrderStatus_07", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_08 => GetObject("OrderStatus_08", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_09 => GetObject("OrderStatus_09", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderStatus_10 => GetObject("OrderStatus_10", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderSubmittingMessage => GetObject("OrderSubmittingMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string OrderSubtotal => GetObject("OrderSubtotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderSummary => GetObject("OrderSummary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderTotal => GetObject("OrderTotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrderUppercase => GetObject("OrderUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OrEnterNewAddress => GetObject("OrEnterNewAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Organization => GetObject("Organization", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Original => GetObject("Original", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OtherGuestHaveAlreadyInvited => GetObject("OtherGuestHaveAlreadyInvited", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string OurBestProducts => GetObject("OurBestProducts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PageNotFound => GetObject("PageNotFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PageNotFoundInstructions => GetObject("PageNotFoundInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaidLevel => GetObject("PaidLevel", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaidRank => GetObject("PaidRank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaidWith => GetObject("PaidWith", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ParentCategory => GetObject("ParentCategory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Parties => GetObject("Parties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Party => GetObject("Party", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyCreated => GetObject("PartyCreated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyCreationFailed => GetObject("PartyCreationFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyDashboard => GetObject("PartyDashboard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyDate => GetObject("PartyDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyDetails => GetObject("PartyDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyGuests => GetObject("PartyGuests", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyHostedByYourFriend => GetObject("PartyHostedByYourFriend", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyInvalidMessage => GetObject("PartyInvalidMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyIsScheduled => GetObject("PartyIsScheduled", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyLocation => GetObject("PartyLocation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyManager => GetObject("PartyManager", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyOrders => GetObject("PartyOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyReadyClosed => GetObject("PartyReadyClosed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyStartDate => GetObject("PartyStartDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyTime => GetObject("PartyTime", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyWebsite => GetObject("PartyWebsite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PartyWillCloseCompleteHostessOrder => GetObject("PartyWillCloseCompleteHostessOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Password => GetObject("Password", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  attempted password
       ///</summary>
        public static string PasswordNotAvailable => GetObject("PasswordNotAvailable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PasswordsDontMatch => GetObject("PasswordsDontMatch", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PasswordUpdated => GetObject("PasswordUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PasteLink => GetObject("PasteLink", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Path => GetObject("Path", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Rank Advancement
       ///</summary>
        public static string PathFullfullment => GetObject("PathFullfullment", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableToPlaceholder => GetObject("PayableToPlaceholder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_00 => GetObject("PayableType_00", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_01 => GetObject("PayableType_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_02 => GetObject("PayableType_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_03 => GetObject("PayableType_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_04 => GetObject("PayableType_04", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_05 => GetObject("PayableType_05", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_06 => GetObject("PayableType_06", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_07 => GetObject("PayableType_07", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_08 => GetObject("PayableType_08", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_09 => GetObject("PayableType_09", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_10 => GetObject("PayableType_10", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_11 => GetObject("PayableType_11", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_12 => GetObject("PayableType_12", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_13 => GetObject("PayableType_13", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_14 => GetObject("PayableType_14", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_15 => GetObject("PayableType_15", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_16 => GetObject("PayableType_16", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayableType_17 => GetObject("PayableType_17", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayAutoOrderHow => GetObject("PayAutoOrderHow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayLater => GetObject("PayLater", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Payment => GetObject("Payment", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentInfo => GetObject("PaymentInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentMethod => GetObject("PaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentMethods => GetObject("PaymentMethods", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentMethodUpdated => GetObject("PaymentMethodUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentOptions => GetObject("PaymentOptions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_00 => GetObject("PaymentType_00", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_01 => GetObject("PaymentType_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_02 => GetObject("PaymentType_02", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_03 => GetObject("PaymentType_03", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_04 => GetObject("PaymentType_04", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_05 => GetObject("PaymentType_05", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_06 => GetObject("PaymentType_06", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_07 => GetObject("PaymentType_07", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_08 => GetObject("PaymentType_08", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_09 => GetObject("PaymentType_09", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_10 => GetObject("PaymentType_10", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_11 => GetObject("PaymentType_11", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_12 => GetObject("PaymentType_12", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_13 => GetObject("PaymentType_13", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_14 => GetObject("PaymentType_14", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_15 => GetObject("PaymentType_15", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_16 => GetObject("PaymentType_16", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PaymentType_17 => GetObject("PaymentType_17", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayWithThisAccount => GetObject("PayWithThisAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PayWithThisCard => GetObject("PayWithThisCard", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PDF => GetObject("PDF", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PendingOptedInStatus => GetObject("PendingOptedInStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Period => GetObject("Period", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Period1 => GetObject("Period1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PeriodType_001 => GetObject("PeriodType_001", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string PersonalDiscount => GetObject("PersonalDiscount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PersonalIdentityNumber => GetObject("PersonalIdentityNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PersonalInformation => GetObject("PersonalInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PersonalizedWebsite => GetObject("PersonalizedWebsite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PersonallyEnrolled => GetObject("PersonallyEnrolled", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PersonallyEnrolledTeam => GetObject("PersonallyEnrolledTeam", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string PersonalQty => GetObject("PersonalQty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string PersonalTotal => GetObject("PersonalTotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PersonalWebLogin => GetObject("PersonalWebLogin", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Phone => GetObject("Phone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PhoneNumbers => GetObject("PhoneNumbers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PIN => GetObject("PIN", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Pinterest => GetObject("Pinterest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Waiting Room
       ///</summary>
        public static string Place => GetObject("Place", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Placement => GetObject("Placement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PlaceOrder => GetObject("PlaceOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PlacingOrder => GetObject("PlacingOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PleaseChooseCategory => GetObject("PleaseChooseCategory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PleaseEnterACompleteAddress => GetObject("PleaseEnterACompleteAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PleaseRegisterButtonInstruction => GetObject("PleaseRegisterButtonInstruction", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PointAccount_01 => GetObject("PointAccount_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PopUpAppearConfPlacement => GetObject("PopUpAppearConfPlacement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PowerPoint => GetObject("PowerPoint", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PreferNotToSay => GetObject("PreferNotToSay", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PreferredCustomer => GetObject("PreferredCustomer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PreferredCustomers => GetObject("PreferredCustomers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PreferredLanguage => GetObject("PreferredLanguage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PreferredName => GetObject("PreferredName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PremiumCustomer => GetObject("PremiumCustomer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Preview => GetObject("Preview", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PreviousBalance => GetObject("PreviousBalance", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Price => GetObject("Price", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PricePerUnit => GetObject("PricePerUnit", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PricingInformation => GetObject("PricingInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Primary => GetObject("Primary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PrimaryApplicantFirstName => GetObject("PrimaryApplicantFirstName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PrimaryApplicantLastName => GetObject("PrimaryApplicantLastName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PrimaryApplicantMiddle => GetObject("PrimaryApplicantMiddle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PrimaryCardOnFile => GetObject("PrimaryCardOnFile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Print => GetObject("Print", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Printed => GetObject("Printed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PrintInvoice => GetObject("PrintInvoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Privacy => GetObject("Privacy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PrivacyPolicy => GetObject("PrivacyPolicy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Private => GetObject("Private", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProblemLoadingSubcategories => GetObject("ProblemLoadingSubcategories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProblemWithInformationText => GetObject("ProblemWithInformationText", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Procede => GetObject("Procede", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Proceed => GetObject("Proceed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProceedToCheckout => GetObject("ProceedToCheckout", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProcessDateUpdate => GetObject("ProcessDateUpdate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Processing => GetObject("Processing", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProcessingOn => GetObject("ProcessingOn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProcessSendingElectronicInvitesDescription => GetObject("ProcessSendingElectronicInvitesDescription", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Product => GetObject("Product", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProductDescription => GetObject("ProductDescription", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProductDescription2 => GetObject("ProductDescription2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProductDescription3 => GetObject("ProductDescription3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProductDescription4 => GetObject("ProductDescription4", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Products => GetObject("Products", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProductUppercase => GetObject("ProductUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Profile => GetObject("Profile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PromotionsOrDiscountsMessage => GetObject("PromotionsOrDiscountsMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PromotionStatus => GetObject("PromotionStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ProvideTaxID => GetObject("ProvideTaxID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Public => GetObject("Public", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PublishDate => GetObject("PublishDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PublishOnDate => GetObject("PublishOnDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string PV => GetObject("PV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string QTY => GetObject("QTY", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Qualified => GetObject("Qualified", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string QualifiedAs => GetObject("QualifiedAs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string QualifyingAs => GetObject("QualifyingAs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Quantity => GetObject("Quantity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Question => GetObject("Question", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string QuickShop => GetObject("QuickShop", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string QuickView => GetObject("QuickView", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string QV => GetObject("QV", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Rank => GetObject("Rank", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Rank_00 => GetObject("Rank_00", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RankAdvancement => GetObject("RankAdvancement", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RankAdvancementFailed => GetObject("RankAdvancementFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RankRequirements => GetObject("RankRequirements", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RankVolumeItem => GetObject("RankVolumeItem", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReadandAgree => GetObject("ReadandAgree", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RecentActivityList => GetObject("RecentActivityList", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RecentlyAddedItems => GetObject("RecentlyAddedItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RecentlyAddedItSearchForYourReferrer => GetObject("RecentlyAddedItSearchForYourReferrer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RecognitionName => GetObject("RecognitionName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RecurrenceRule => GetObject("RecurrenceRule", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReEnterAccountNumber => GetObject("ReEnterAccountNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReEnterCardNumber => GetObject("ReEnterCardNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string RemainingPersonalBalance => GetObject("RemainingPersonalBalance", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Remove => GetObject("Remove", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RemoveSubBeforeModifying => GetObject("RemoveSubBeforeModifying", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Repeat => GetObject("Repeat", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RepeatBy => GetObject("RepeatBy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RepeatEvery => GetObject("RepeatEvery", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RepeatOn => GetObject("RepeatOn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Repeats => GetObject("Repeats", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RepeatSettings => GetObject("RepeatSettings", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_AboutUs_Content => GetObject("Replicated_AboutUs_Content", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_AutoOrder_NoAutoOrderFound => GetObject("Replicated_AutoOrder_NoAutoOrderFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_EnrolleeInfo_EnrollmentInformation => GetObject("Replicated_EnrolleeInfo_EnrollmentInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Enrollment_BannerImage1 => GetObject("Replicated_Enrollment_BannerImage1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Enrollment_JoinSection => GetObject("Replicated_Enrollment_JoinSection", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Enrollment_QuantityAbbreviation => GetObject("Replicated_Enrollment_QuantityAbbreviation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_EnrollmentConfiguration_ChooseEnroller => GetObject("Replicated_EnrollmentConfiguration_ChooseEnroller", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_EnrollmentConfiguration_KeepItemsModal => GetObject("Replicated_EnrollmentConfiguration_KeepItemsModal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Error_InvalidWebAlias => GetObject("Replicated_Error_InvalidWebAlias", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Error_NotFound => GetObject("Replicated_Error_NotFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Error_UnexpectedError => GetObject("Replicated_Error_UnexpectedError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Error_WebAliasRequired => GetObject("Replicated_Error_WebAliasRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Global_Footer => GetObject("Replicated_Global_Footer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Global_Menu => GetObject("Replicated_Global_Menu", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_AboutUs => GetObject("Replicated_Home_AboutUs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_BannerImage1 => GetObject("Replicated_Home_BannerImage1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_BannerImage2 => GetObject("Replicated_Home_BannerImage2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_BannerImage3 => GetObject("Replicated_Home_BannerImage3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_BannerSection1 => GetObject("Replicated_Home_BannerSection1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_ContentImage1 => GetObject("Replicated_Home_ContentImage1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_ContentImage2 => GetObject("Replicated_Home_ContentImage2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_ContentImage3 => GetObject("Replicated_Home_ContentImage3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_Section1 => GetObject("Replicated_Home_Section1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Top Teaser
       ///</summary>
        public static string Replicated_Home_Section2 => GetObject("Replicated_Home_Section2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///About Section
       ///</summary>
        public static string Replicated_Home_Section3 => GetObject("Replicated_Home_Section3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_Section4 => GetObject("Replicated_Home_Section4", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_Section5 => GetObject("Replicated_Home_Section5", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Home_TopCarousel (  ) { 
                return GetCarouselObject("Replicated_Home_TopCarousel", Thread.CurrentThread.CurrentUICulture, 
                     new Dictionary<string,dynamic>() {}); 
        } 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Login_NewAccount => GetObject("Replicated_Login_NewAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_OrderInvoice_InvoiceForOrder => GetObject("Replicated_OrderInvoice_InvoiceForOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_OrderInvoice_OrderInvoice => GetObject("Replicated_OrderInvoice_OrderInvoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_PrivacyPolicy_Content => GetObject("Replicated_PrivacyPolicy_Content", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_Shopping_OrderComplete => GetObject("Replicated_Shopping_OrderComplete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Replicated_ShoppingController_PartyLinkText => GetObject("Replicated_ShoppingController_PartyLinkText", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReplicatedSite_Shipping_HostessGetCredit => GetObject("ReplicatedSite_Shipping_HostessGetCredit", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReplicatedSite_Shipping_PartyNoLongerValid => GetObject("ReplicatedSite_Shipping_PartyNoLongerValid", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Reports => GetObject("Reports", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Required => GetObject("Required", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Requirements => GetObject("Requirements", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ResetPassword => GetObject("ResetPassword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Resource => GetObject("Resource", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ResourceCategories => GetObject("ResourceCategories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ResourceCategory => GetObject("ResourceCategory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ResourceItems => GetObject("ResourceItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ResourceLibrary => GetObject("ResourceLibrary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Resources => GetObject("Resources", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard  Navigation Header
       ///</summary>
        public static string ResourcesLibrary => GetObject("ResourcesLibrary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RetailCustomer => GetObject("RetailCustomer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RetailCustomerPricingInfo => GetObject("RetailCustomerPricingInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RetailCustomers => GetObject("RetailCustomers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string RetailQty => GetObject("RetailQty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string RetailTotal => GetObject("RetailTotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReturnedOrders => GetObject("ReturnedOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReturningCustomer => GetObject("ReturningCustomer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Review => GetObject("Review", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReviewYourAutoOrder => GetObject("ReviewYourAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReviewYourInformation => GetObject("ReviewYourInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ReviewYourOrder => GetObject("ReviewYourOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RewardsToDate => GetObject("RewardsToDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RoutingNumber => GetObject("RoutingNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string RoutingNumberError => GetObject("RoutingNumberError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Runs => GetObject("Runs", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Sale => GetObject("Sale", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SalesToDate => GetObject("SalesToDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SandboxMode => GetObject("SandboxMode", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Save => GetObject("Save", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SaveAsPrimaryAddress => GetObject("SaveAsPrimaryAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SaveChanges => GetObject("SaveChanges", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SavedPaymentMethods => GetObject("SavedPaymentMethods", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SavePrimaryAddress => GetObject("SavePrimaryAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SaveSortOrder => GetObject("SaveSortOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SaveTheDate => GetObject("SaveTheDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SaveUppercase => GetObject("SaveUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Saving => GetObject("Saving", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Schedule => GetObject("Schedule", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Search => GetObject("Search", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchByNameOrID => GetObject("SearchByNameOrID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchForYourDistributor => GetObject("SearchForYourDistributor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchForYourReferrer => GetObject("SearchForYourReferrer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchingForEvent => GetObject("SearchingForEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchNoResults => GetObject("SearchNoResults", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchNotFound => GetObject("SearchNotFound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SearchResults => GetObject("SearchResults", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Secondary => GetObject("Secondary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SecondaryCardOnFile => GetObject("SecondaryCardOnFile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SecurityReEnterAccountNumber => GetObject("SecurityReEnterAccountNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectAddressError => GetObject("SelectAddressError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectAKeyword => GetObject("SelectAKeyword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectBillingAddress => GetObject("SelectBillingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectCategory => GetObject("SelectCategory", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectCountLang => GetObject("SelectCountLang", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectCountry => GetObject("SelectCountry", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectItemsBeforeAutoOrder => GetObject("SelectItemsBeforeAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectLanguage => GetObject("SelectLanguage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectPaymentMethod => GetObject("SelectPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectShipMethBeforeSaving => GetObject("SelectShipMethBeforeSaving", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectShippingAddress => GetObject("SelectShippingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectTags => GetObject("SelectTags", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SelectYourMarket => GetObject("SelectYourMarket", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Send => GetObject("Send", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SendFaxesTo => GetObject("SendFaxesTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SendInvitations => GetObject("SendInvitations", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SendInvites => GetObject("SendInvites", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SendTextsTo => GetObject("SendTextsTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SendVerificationEmailFailure => GetObject("SendVerificationEmailFailure", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SendVerificationEmailSuccess => GetObject("SendVerificationEmailSuccess", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SetAsPrimary => GetObject("SetAsPrimary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SetPrimary => GetObject("SetPrimary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Share => GetObject("Share", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShipMethod => GetObject("ShipMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShipMethodUpdated => GetObject("ShipMethodUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Shipped => GetObject("Shipped", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippedOrders => GetObject("ShippedOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippedTo => GetObject("ShippedTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Shipping => GetObject("Shipping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingAddress => GetObject("ShippingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingAddressMessage => GetObject("ShippingAddressMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingAddressUpdated => GetObject("ShippingAddressUpdated", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingAndBilingAddressAreTheSame => GetObject("ShippingAndBilingAddressAreTheSame", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingAndHandling => GetObject("ShippingAndHandling", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingInformation => GetObject("ShippingInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShippingMethod => GetObject("ShippingMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShipTo => GetObject("ShipTo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShipToThisAddress => GetObject("ShipToThisAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///shopping breadcrumbs
       ///</summary>
        public static string Shop => GetObject("Shop", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopNow => GetObject("ShopNow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopOnlineHere => GetObject("ShopOnlineHere", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Shopping => GetObject("Shopping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string ShoppingCart => GetObject("ShoppingCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopSingle => GetObject("ShopSingle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopSingle_30DaysContent => GetObject("ShopSingle_30DaysContent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopSingle_FreeDeliveryContent => GetObject("ShopSingle_FreeDeliveryContent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopSingle_OriginalContent => GetObject("ShopSingle_OriginalContent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShopSingle_PaymentOptionsContent => GetObject("ShopSingle_PaymentOptionsContent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Short => GetObject("Short", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShouldCreditOrder => GetObject("ShouldCreditOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShowAll => GetObject("ShowAll", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ShowMyPassword => GetObject("ShowMyPassword", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SignIn => GetObject("SignIn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SignMeUp => GetObject("SignMeUp", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SignOut => GetObject("SignOut", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string since => GetObject("since", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SiteCurrentlyUsingSandbox => GetObject("SiteCurrentlyUsingSandbox", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SiteOwnerInformation => GetObject("SiteOwnerInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SKU => GetObject("SKU", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SocialMedia => GetObject("SocialMedia", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SomethingWentWrong => GetObject("SomethingWentWrong", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Sorry => GetObject("Sorry", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SorryErrorText => GetObject("SorryErrorText", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SortBy => GetObject("SortBy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Source => GetObject("Source", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SourceThumbnail => GetObject("SourceThumbnail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Spanish => GetObject("Spanish", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Speakers => GetObject("Speakers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SpeakersName => GetObject("SpeakersName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Sponsor => GetObject("Sponsor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Src => GetObject("Src", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Start => GetObject("Start", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string StartDate => GetObject("StartDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string StartingOn => GetObject("StartingOn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string StartNewAutoOrder => GetObject("StartNewAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string StartsOn => GetObject("StartsOn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string StartTimezone => GetObject("StartTimezone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string State => GetObject("State", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Status => GetObject("Status", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Step1 => GetObject("Step1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Step2 => GetObject("Step2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Step3 => GetObject("Step3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Step4 => GetObject("Step4", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Step5 => GetObject("Step5", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Store => GetObject("Store", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string StreetAddress => GetObject("StreetAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Subcategories => GetObject("Subcategories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SubcategoriesFor => GetObject("SubcategoriesFor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Submit => GetObject("Submit", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SubmitRegistration => GetObject("SubmitRegistration", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SubmitSomething => GetObject("SubmitSomething", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Subscribe => GetObject("Subscribe", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Subscription_01 => GetObject("Subscription_01", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Subscriptions => GetObject("Subscriptions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Subtotal => GetObject("Subtotal", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Success => GetObject("Success", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Suggestions => GetObject("Suggestions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Summary => GetObject("Summary", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Support => GetObject("Support", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Swedish => GetObject("Swedish", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string SyncWithGoogle => GetObject("SyncWithGoogle", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Tags => GetObject("Tags", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Tax => GetObject("Tax", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shopping Cart Header
       ///</summary>
        public static string TaxableAmount => GetObject("TaxableAmount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TermsAndConditions => GetObject("TermsAndConditions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TermsAndConditions_Checkbox => GetObject("TermsAndConditions_Checkbox", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Content for Terms and Conditions in Enrollment flow
       ///</summary>
        public static string TermsAndConditions_Content => GetObject("TermsAndConditions_Content", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string testssss => GetObject("testssss", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TextMessages => GetObject("TextMessages", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Thanks => GetObject("Thanks", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThankYou => GetObject("ThankYou", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThankYouForJoining => GetObject("ThankYouForJoining", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThankYouPage => GetObject("ThankYouPage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TheFollowingError => GetObject("TheFollowingError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TheseItemsWillBeShipped => GetObject("TheseItemsWillBeShipped", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThirtyDayReturnPolicy => GetObject("ThirtyDayReturnPolicy", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThisCardFailed => GetObject("ThisCardFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThisFieldIsRequired => GetObject("ThisFieldIsRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThisFieldRequired => GetObject("ThisFieldRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThisGuestNotAllowEmailInvitations => GetObject("ThisGuestNotAllowEmailInvitations", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThisGuestNotInvitedYet => GetObject("ThisGuestNotInvitedYet", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ThisPartyIsNotAvailable => GetObject("ThisPartyIsNotAvailable", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Thumbnail => GetObject("Thumbnail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Time => GetObject("Time", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TimeZone => GetObject("TimeZone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Title => GetObject("Title", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToBegin => GetObject("ToBegin", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Today => GetObject("Today", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TodaysOrder => GetObject("TodaysOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TodaysOrderTotals => GetObject("TodaysOrderTotals", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToggleNavigation => GetObject("ToggleNavigation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToManageYourAutoOrderPaymentMethods => GetObject("ToManageYourAutoOrderPaymentMethods", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Tomorrow => GetObject("Tomorrow", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToolTip1 => GetObject("ToolTip1", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToolTip2 => GetObject("ToolTip2", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToolTip3 => GetObject("ToolTip3", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToolTips => GetObject("ToolTips", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TooManyEmails => GetObject("TooManyEmails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToRedefineYourBrand => GetObject("ToRedefineYourBrand", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Total => GetObject("Total", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TotalBeforeTax => GetObject("TotalBeforeTax", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Shoppign Cart Header
       ///</summary>
        public static string TotalPrice => GetObject("TotalPrice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ToUpdateYourStatus => GetObject("ToUpdateYourStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Tracking => GetObject("Tracking", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TrackPackage => GetObject("TrackPackage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TrackPackages => GetObject("TrackPackages", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TrackYourOrder => GetObject("TrackYourOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TreePlacementRequired => GetObject("TreePlacementRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TreePlacementRequiredInstructions => GetObject("TreePlacementRequiredInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string TreeViewer => GetObject("TreeViewer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Twitter => GetObject("Twitter", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Type => GetObject("Type", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnableAddPack => GetObject("UnableAddPack", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnableCompleteOrder => GetObject("UnableCompleteOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnableLoadHosts => GetObject("UnableLoadHosts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnableRemoveItem => GetObject("UnableRemoveItem", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnableToDelete => GetObject("UnableToDelete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnableUpdateQuantity => GetObject("UnableUpdateQuantity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnassignedResources => GetObject("UnassignedResources", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnexpectedError => GetObject("UnexpectedError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnexpectedErrorInstructions => GetObject("UnexpectedErrorInstructions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnitPrice => GetObject("UnitPrice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnitPriceUppercase => GetObject("UnitPriceUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnknownError => GetObject("UnknownError", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnrecognizedType => GetObject("UnrecognizedType", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Unsubscribe => GetObject("Unsubscribe", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UnsupportedLang => GetObject("UnsupportedLang", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UntitledEvent => GetObject("UntitledEvent", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UPCNumber => GetObject("UPCNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpcomingPromotions => GetObject("UpcomingPromotions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UPCPound => GetObject("UPCPound", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Update => GetObject("Update", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpdateCart => GetObject("UpdateCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpdatedDDInfo => GetObject("UpdatedDDInfo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpdateGuest => GetObject("UpdateGuest", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpdateParty => GetObject("UpdateParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpdateTotals => GetObject("UpdateTotals", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UplineViewer => GetObject("UplineViewer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Upload => GetObject("Upload", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UploadFromURL => GetObject("UploadFromURL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UploadImageFailed => GetObject("UploadImageFailed", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UploadImageFromComputer => GetObject("UploadImageFromComputer", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UpOneLevel => GetObject("UpOneLevel", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string URL => GetObject("URL", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string USCurrency => GetObject("USCurrency", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UseCollector => GetObject("UseCollector", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UseFacebookImage => GetObject("UseFacebookImage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UseGravatarImage => GetObject("UseGravatarImage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UseMainAddressAsShipping => GetObject("UseMainAddressAsShipping", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UserLogin => GetObject("UserLogin", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Username => GetObject("Username", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UseSeparateStartEndTimeZones => GetObject("UseSeparateStartEndTimeZones", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UseShippingAddress => GetObject("UseShippingAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string UsingNewPaymentMethod => GetObject("UsingNewPaymentMethod", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string VATNumber => GetObject("VATNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string VerifyAddress => GetObject("VerifyAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Video => GetObject("Video", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewAlertsList => GetObject("ViewAlertsList", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///0  number of items
       ///</summary>
        public static string ViewAllItems => GetObject("ViewAllItems", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewAllMetrics => GetObject("ViewAllMetrics", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewAllNews => GetObject("ViewAllNews", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewAllOpenParties => GetObject("ViewAllOpenParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewAllOrders => GetObject("ViewAllOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewAllRanks => GetObject("ViewAllRanks", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewAllRecentActivity => GetObject("ViewAllRecentActivity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewAllTeamMembers => GetObject("ViewAllTeamMembers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewAllUpcomingPromotions => GetObject("ViewAllUpcomingPromotions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewAllVolumes => GetObject("ViewAllVolumes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewCart => GetObject("ViewCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewClosedParties => GetObject("ViewClosedParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewCurrentEarnings => GetObject("ViewCurrentEarnings", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewDetails => GetObject("ViewDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewDownline => GetObject("ViewDownline", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewFullProductDetails => GetObject("ViewFullProductDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewGraphicalDownline => GetObject("ViewGraphicalDownline", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewInvoice => GetObject("ViewInvoice", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewNewestDistributors => GetObject("ViewNewestDistributors", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewOnMap => GetObject("ViewOnMap", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewOpenParties => GetObject("ViewOpenParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewProductDetails => GetObject("ViewProductDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewProfile => GetObject("ViewProfile", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard
       ///</summary>
        public static string ViewPromotionDetails => GetObject("ViewPromotionDetails", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewResourcesFor => GetObject("ViewResourcesFor", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string ViewSubcategories => GetObject("ViewSubcategories", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string VisitWebsite => GetObject("VisitWebsite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volume => GetObject("Volume", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volume_001 => GetObject("Volume_001", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volume_002 => GetObject("Volume_002", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volume_003 => GetObject("Volume_003", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volume_004 => GetObject("Volume_004", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volume_005 => GetObject("Volume_005", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Volumes => GetObject("Volumes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WaitingRoom => GetObject("WaitingRoom", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WaitingRoomEmpty => GetObject("WaitingRoomEmpty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WaitingRoomPlacementSuccessful => GetObject("WaitingRoomPlacementSuccessful", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WantMoreInformation => GetObject("WantMoreInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WasNotCalled => GetObject("WasNotCalled", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Rank Advancement0qualification leg count 1rank description
       ///</summary>
        public static string WaysToQualify => GetObject("WaysToQualify", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Web => GetObject("Web", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WebAliasRequired => GetObject("WebAliasRequired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WebAliasRules => GetObject("WebAliasRules", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Website => GetObject("Website", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WebsiteInformation => GetObject("WebsiteInformation", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WeCouldntFind => GetObject("WeCouldntFind", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WeCouldntFindWebalias => GetObject("WeCouldntFindWebalias", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Week => GetObject("Week", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Weekly => GetObject("Weekly", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Weeks => GetObject("Weeks", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WeHaveRecievedYourOrder => GetObject("WeHaveRecievedYourOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WelcomeToCompany => GetObject("WelcomeToCompany", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WelcomeToExigo => GetObject("WelcomeToExigo", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WellcomeToMySiteMessage => GetObject("WellcomeToMySiteMessage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WereNotSure => GetObject("WereNotSure", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WereSorry => GetObject("WereSorry", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WeWereUnableToComplete => GetObject("WeWereUnableToComplete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WeWereUnableToRemove => GetObject("WeWereUnableToRemove", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WeWereUnableToUpdate => GetObject("WeWereUnableToUpdate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string What => GetObject("What", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhatIsGravatar => GetObject("WhatIsGravatar", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhatIsYourEmail => GetObject("WhatIsYourEmail", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhatIsYourFacebook => GetObject("WhatIsYourFacebook", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhatsYourEmailAddress => GetObject("WhatsYourEmailAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhatsYourFacebookUsername => GetObject("WhatsYourFacebookUsername", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhatURLIsYourImage => GetObject("WhatURLIsYourImage", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string When => GetObject("When", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhenAutoOrderProcess => GetObject("WhenAutoOrderProcess", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhenCreatingEditing => GetObject("WhenCreatingEditing", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhenNextAutoOrderProcess => GetObject("WhenNextAutoOrderProcess", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Where => GetObject("Where", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhoWillBeHostingYourParty => GetObject("WhoWillBeHostingYourParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WhyDoWeNeedThis => GetObject("WhyDoWeNeedThis", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WillPerDelete => GetObject("WillPerDelete", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Word => GetObject("Word", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WorkPhone => GetObject("WorkPhone", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string WouldYouLikeSendPartyInvitations => GetObject("WouldYouLikeSendPartyInvitations", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Year => GetObject("Year", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Yearly => GetObject("Yearly", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Years => GetObject("Years", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Yes => GetObject("Yes", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouAreOn => GetObject("YouAreOn", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouArePurchasing => GetObject("YouArePurchasing", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouDoNotHaveAnyAutoOrder => GetObject("YouDoNotHaveAnyAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouHaveNoParties => GetObject("YouHaveNoParties", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouHaveNoSubscriptions => GetObject("YouHaveNoSubscriptions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouHaveNotEarnedCommissions => GetObject("YouHaveNotEarnedCommissions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouMayAlsoLike => GetObject("YouMayAlsoLike", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouMayNotUnsubscribe => GetObject("YouMayNotUnsubscribe", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouMustAcceptTermsAndConditions => GetObject("YouMustAcceptTermsAndConditions", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourAccount => GetObject("YourAccount", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourAccountNumber => GetObject("YourAccountNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourAddress => GetObject("YourAddress", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourAOPS => GetObject("YourAOPS", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourAutoOrder => GetObject("YourAutoOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourAutoOrders => GetObject("YourAutoOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourBankAccounts => GetObject("YourBankAccounts", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourCart => GetObject("YourCart", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourCartIsEmpty => GetObject("YourCartIsEmpty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourCartUppercase => GetObject("YourCartUppercase", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourCreditCards => GetObject("YourCreditCards", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard Header
       ///</summary>
        public static string YourCurrentEarnings => GetObject("YourCurrentEarnings", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourCustomerID => GetObject("YourCustomerID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourCustomers => GetObject("YourCustomers", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourEnroller => GetObject("YourEnroller", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourInvitedOurParty => GetObject("YourInvitedOurParty", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourLeads => GetObject("YourLeads", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard Header
       ///</summary>
        public static string YourMetrics => GetObject("YourMetrics", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourName => GetObject("YourName", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard Header
       ///</summary>
        public static string YourNewestDistributors => GetObject("YourNewestDistributors", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourOrder => GetObject("YourOrder", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourOrderID => GetObject("YourOrderID", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourOrderIsSubmitting => GetObject("YourOrderIsSubmitting", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourOrders => GetObject("YourOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourOrganization => GetObject("YourOrganization", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourPartyDate => GetObject("YourPartyDate", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourPartyIsScheduledFabulousExperience => GetObject("YourPartyIsScheduledFabulousExperience", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourPaymentMethods => GetObject("YourPaymentMethods", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourPersonalWebsiteAndLogin => GetObject("YourPersonalWebsiteAndLogin", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourPlacementCompleted => GetObject("YourPlacementCompleted", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourPrimaryPhoneNumber => GetObject("YourPrimaryPhoneNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourProfilePicture => GetObject("YourProfilePicture", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard Header
       ///</summary>
        public static string YourPromotionStatus => GetObject("YourPromotionStatus", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///DashboardHeader
       ///</summary>
        public static string YourRecentOrders => GetObject("YourRecentOrders", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourRoutingNumber => GetObject("YourRoutingNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourSavedAddresses => GetObject("YourSavedAddresses", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourSecondaryPhoneNumber => GetObject("YourSecondaryPhoneNumber", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourSessionHasExpired => GetObject("YourSessionHasExpired", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourTeam => GetObject("YourTeam", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///Dashboard Header
       ///</summary>
        public static string YourTeamsRecentActivity => GetObject("YourTeamsRecentActivity", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YourWebsite => GetObject("YourWebsite", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string YouTube => GetObject("YouTube", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Zip => GetObject("Zip", Thread.CurrentThread.CurrentUICulture); 
       ///<summary>
       ///
       ///</summary>
        public static string Zipcode => GetObject("Zipcode", Thread.CurrentThread.CurrentUICulture); 

        public static string GetObject(string resourceName, CultureInfo culture)
        {
            return DbResourceProvider.GetResourceValue("Common", HttpContext.Current.Server.MapPath("~/App_Data/"), culture.Name, resourceName);
        } 

        public static string GetObject(string resourceName)
        {
            return DbResourceProvider.GetResourceValue("Common", HttpContext.Current.Server.MapPath("~/App_Data/"), Thread.CurrentThread.CurrentUICulture.Name, resourceName);
        } 

        private static string GetCarouselObject(string resourceName, CultureInfo culture, Dictionary<string,dynamic> filters)
        {
            return DbResourceProvider.GetCarouselObject("Common", HttpContext.Current.Server.MapPath("~/App_Data/"), culture.Name, resourceName, filters);
        } 

        public static bool IsValueInList(string resourceName, string valueToCompare)
        {
            return GetCustom(Thread.CurrentThread.CurrentUICulture, resourceName, valueToCompare);
        }

        private static bool GetCustom(CultureInfo culture, string resourceName, string valueToCompare)
        {
            return DbResourceProvider.ValueInList("Common", HttpContext.Current.Server.MapPath("~/App_Data/"), culture.Name, resourceName, valueToCompare);
        }
    }
}