using Common;
using Common.Api.ExigoWebService;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Runtime.Caching;
using System;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static IEnumerable<Address> GetCustomerAddresses(int customerID, bool getRealtime = false)
        {
            if (!MemoryCache.Default.Contains("Customer_" + customerID))
            {
                if (!getRealtime)
                {
                    using (var ctx = Sql())
                    {
                        var addresses = ctx.Query(@"
                                SELECT 
                                    c.FirstName,
                                    c.LastName,
                                    c.Email,
                                    c.Phone,

                                    c.MainAddress1,
                                    c.MainAddress2,
                                    c.MainAddress3,
                                    c.MainCity,
                                    c.MainState,
                                    c.MainZip,
                                    c.MainCountry,
                                    c.MainVerified,

                                    c.MailAddress1,
                                    c.MailAddress2,
                                    c.MailAddress3,
                                    c.MailCity,
                                    c.MailState,
                                    c.MailZip,
                                    c.MailCountry,
                                    c.MailVerified,

                                    c.OtherAddress1,
                                    c.OtherAddress2,
                                    c.OtherAddress3,
                                    c.OtherCity,
                                    c.OtherState,
                                    c.OtherZip,
                                    c.OtherCountry,
                                    c.OtherVerified

                                FROM Customers c
                                WHERE c.CustomerID = @customerID
                                ", new { customerID }).FirstOrDefault();

                        yield return new ShippingAddress()
                        {
                            AddressType = AddressType.Main,
                            FirstName = addresses.FirstName,
                            LastName = addresses.LastName,
                            Email = addresses.Email,
                            Phone = addresses.Phone,
                            Address1 = addresses.MainAddress1,
                            Address2 = addresses.MainAddress2,
                            Address3 = addresses.MainAddress3,
                            City = addresses.MainCity,
                            State = addresses.MainState,
                            Zip = addresses.MainZip,
                            Country = addresses.MainCountry,
                            //isVerified = addresses.MainVerified
                        };

                        yield return new ShippingAddress()
                        {
                            AddressType = AddressType.Mailing,
                            FirstName = addresses.FirstName,
                            LastName = addresses.LastName,
                            Email = addresses.Email,
                            Phone = addresses.Phone,
                            Address1 = addresses.MailAddress1,
                            Address2 = addresses.MailAddress2,
                            Address3 = addresses.MailAddress3,
                            City = addresses.MailCity,
                            State = addresses.MailState,
                            Zip = addresses.MailZip,
                            Country = addresses.MailCountry,
                            //isVerified = addresses.MailVerified
                        };

                        yield return new ShippingAddress()
                        {
                            AddressType = AddressType.Other,
                            FirstName = addresses.FirstName,
                            LastName = addresses.LastName,
                            Email = addresses.Email,
                            Phone = addresses.Phone,
                            Address1 = addresses.OtherAddress1,
                            Address2 = addresses.OtherAddress2,
                            Address3 = addresses.OtherAddress3,
                            City = addresses.OtherCity,
                            State = addresses.OtherState,
                            Zip = addresses.OtherZip,
                            Country = addresses.OtherCountry,
                            //isVerified = addresses.OtherVerified
                        };
                    }
                }
                else
                {
                    var customer = GetCustomer(customerID, true);
                    var addresses = new List<ShippingAddress>();

                    if (customer.MainAddress.Address1.IsNotNullOrEmpty())
                    {
                        yield return new ShippingAddress()
                        {
                            AddressType = AddressType.Main,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Email = customer.Email,
                            Phone = customer.PrimaryPhone,
                            Address1 = customer.MainAddress.Address1,
                            Address2 = customer.MainAddress.Address2,
                            Address3 = customer.MainAddress.Address3,
                            City = customer.MainAddress.City,
                            State = customer.MainAddress.State,
                            Zip = customer.MainAddress.Zip,
                            Country = customer.MainAddress.Country,
                            //isVerified = customer.MainAddress.isVerified
                        };
                    }
                    if (customer.MailingAddress.Address1.IsNotNullOrEmpty())
                    {
                        yield return new ShippingAddress()
                        {
                            AddressType = AddressType.Mailing,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Email = customer.Email,
                            Phone = customer.PrimaryPhone,
                            Address1 = customer.MailingAddress.Address1,
                            Address2 = customer.MailingAddress.Address2,
                            Address3 = customer.MailingAddress.Address3,
                            City = customer.MailingAddress.City,
                            State = customer.MailingAddress.State,
                            Zip = customer.MailingAddress.Zip,
                            Country = customer.MailingAddress.Country,
                            //isVerified = customer.MailingAddress.isVerified
                        };
                    }
                    if (customer.OtherAddress.Address1.IsNotNullOrEmpty())
                    {
                        yield return new ShippingAddress()
                        {
                            AddressType = AddressType.Other,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Email = customer.Email,
                            Phone = customer.PrimaryPhone,
                            Address1 = customer.OtherAddress.Address1,
                            Address2 = customer.OtherAddress.Address2,
                            Address3 = customer.OtherAddress.Address3,
                            City = customer.OtherAddress.City,
                            State = customer.OtherAddress.State,
                            Zip = customer.OtherAddress.Zip,
                            Country = customer.OtherAddress.Country,
                            //isVerified = customer.OtherAddress.isVerified
                        };
                    }
                    MemoryCache.Default.Add("Customer_" + customerID, customer, DateTime.Now.AddMinutes(15)); ;
                }
            }
            else
            {
                var customer = MemoryCache.Default.Get("Customer_" + customerID) as Customer;
                var addresses = new List<ShippingAddress>();

                if (customer.MainAddress.Address1.IsNotNullOrEmpty())
                {
                    yield return new ShippingAddress()
                    {
                        AddressType = AddressType.Main,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.PrimaryPhone,
                        Address1 = customer.MainAddress.Address1,
                        Address2 = customer.MainAddress.Address2,
                        Address3 = customer.MainAddress.Address3,
                        City = customer.MainAddress.City,
                        State = customer.MainAddress.State,
                        Zip = customer.MainAddress.Zip,
                        Country = customer.MainAddress.Country,
                        //isVerified = customer.MainAddress.isVerified
                    };
                }
                if (customer.MailingAddress.Address1.IsNotNullOrEmpty())
                {
                    yield return new ShippingAddress()
                    {
                        AddressType = AddressType.Mailing,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.PrimaryPhone,
                        Address1 = customer.MailingAddress.Address1,
                        Address2 = customer.MailingAddress.Address2,
                        Address3 = customer.MailingAddress.Address3,
                        City = customer.MailingAddress.City,
                        State = customer.MailingAddress.State,
                        Zip = customer.MailingAddress.Zip,
                        Country = customer.MailingAddress.Country,
                        //isVerified = customer.MailingAddress.isVerified
                    };
                }
                if (customer.OtherAddress.Address1.IsNotNullOrEmpty())
                {
                    yield return new ShippingAddress()
                    {
                        AddressType = AddressType.Other,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.PrimaryPhone,
                        Address1 = customer.OtherAddress.Address1,
                        Address2 = customer.OtherAddress.Address2,
                        Address3 = customer.OtherAddress.Address3,
                        City = customer.OtherAddress.City,
                        State = customer.OtherAddress.State,
                        Zip = customer.OtherAddress.Zip,
                        Country = customer.OtherAddress.Country,
                        //isVerified = customer.OtherAddress.isVerified
                    };
                }
            }
        }

        public static Address SaveNewCustomerAddress(int customerID, Address address)
        {
            var addressesOnFile = ExigoDAL.GetCustomerAddresses(customerID).Where(c => c.IsComplete);

            // Do any of the addresses on file match the one we are using?
            // If not, save this address to the next available slot
            if (!addressesOnFile.Any(c => c.Equals(address)))
            {
                var saveAddress = false;
                var request = new UpdateCustomerRequest();
                request.CustomerID = customerID;

                // Main address
                if (!addressesOnFile.Any(c => c.AddressType == AddressType.Main))
                {
                    saveAddress = true;
                    address.AddressType = AddressType.Main;
                    request.MainAddress1 = address.Address1;
                    request.MainAddress2 = address.Address2;
                    request.MainCity = address.City;
                    request.MainState = address.State;
                    request.MainZip = address.Zip;
                    request.MainCountry = address.Country;
                }

                // Mailing address
                else if (!addressesOnFile.Any(c => c.AddressType == AddressType.Mailing))
                {
                    saveAddress = true;
                    address.AddressType = AddressType.Mailing;
                    request.MailAddress1 = address.Address1;
                    request.MailAddress2 = address.Address2;
                    request.MailCity = address.City;
                    request.MailState = address.State;
                    request.MailZip = address.Zip;
                    request.MailCountry = address.Country;
                }

                // Other address
                else
                {
                    saveAddress = true;
                    address.AddressType = AddressType.Other;
                    request.OtherAddress1 = address.Address1;
                    request.OtherAddress2 = address.Address2;
                    request.OtherCity = address.City;
                    request.OtherState = address.State;
                    request.OtherZip = address.Zip;
                    request.OtherCountry = address.Country;
                }

                if (saveAddress)
                {
                    ExigoDAL.WebService().UpdateCustomer(request);
                    ExigoDAL.PurgeCustomer(customerID);
                }
            }

            return address;
        }
        public static Address SetCustomerAddressOnFile(int customerID, Address address)
        {
            return SetCustomerAddressOnFile(customerID, address, address.AddressType);
        }
        public static Address SetCustomerAddressOnFile(int customerID, Address address, AddressType type)
        {
            var saveAddress = false;
            var request = new UpdateCustomerRequest();
            request.CustomerID = customerID;

            // Attempt to validate the user's entered address if US address
            address = GlobalUtilities.ValidateAddress(address) as Address;

            // New Addresses
            if (type == AddressType.New)
            {
                return ExigoDAL.SaveNewCustomerAddress(customerID, address);
            }

            // Main address
            if (type == AddressType.Main)
            {
                saveAddress = true;
                request.MainAddress1 = address.Address1;
                request.MainAddress2 = address.Address2 ?? string.Empty;
                request.MainCity = address.City;
                request.MainState = address.State;
                request.MainZip = address.Zip;
                request.MainCountry = address.Country;
            }

            // Mailing address
            if (type == AddressType.Mailing)
            {
                saveAddress = true;
                request.MailAddress1 = address.Address1;
                request.MailAddress2 = address.Address2 ?? string.Empty;
                request.MailCity = address.City;
                request.MailState = address.State;
                request.MailZip = address.Zip;
                request.MailCountry = address.Country;
            }

            // Other address
            if (type == AddressType.Other)
            {
                saveAddress = true;
                request.OtherAddress1 = address.Address1;
                request.OtherAddress2 = address.Address2 ?? string.Empty;
                request.OtherCity = address.City;
                request.OtherState = address.State;
                request.OtherZip = address.Zip;
                request.OtherCountry = address.Country;
            }

            if (saveAddress)
            {
                ExigoDAL.WebService().UpdateCustomer(request);
                ExigoDAL.PurgeCustomer(customerID);
            }

            return address;
        }
        public static void SetCustomerPrimaryAddress(int customerID, AddressType type)
        {
            if (type == AddressType.Main || type == AddressType.New) return;

            var addressesOnFile = ExigoDAL.GetCustomerAddresses(customerID)
                .Where(c => c.IsComplete);

            var oldPrimaryAddress = addressesOnFile
                .Where(c => c.AddressType == AddressType.Main)
                .FirstOrDefault();

            var newPrimaryAddress = addressesOnFile
                .Where(c => c.AddressType == type)
                .FirstOrDefault();

            if (oldPrimaryAddress == null || newPrimaryAddress == null) return;

            // Swap the addresses
            ExigoDAL.SetCustomerAddressOnFile(customerID, (Address)newPrimaryAddress, AddressType.Main);
            ExigoDAL.SetCustomerAddressOnFile(customerID, (Address)oldPrimaryAddress, type);
        }

        public static void DeleteCustomerAddress(int customerID, AddressType type)
        {
            var deleteAddress = false;
            var request = new UpdateCustomerRequest();
            request.CustomerID = customerID;

            // Main address
            if (type == AddressType.Main)
            {
                deleteAddress = true;
                request.MainAddress1 = string.Empty;
                request.MainAddress2 = string.Empty;
                request.MainCity = string.Empty;
                request.MainState = string.Empty;
                request.MainZip = string.Empty;
                request.MainCountry = string.Empty;
            }

            // Mailing address
            else if (type == AddressType.Mailing)
            {
                deleteAddress = true;
                request.MailAddress1 = string.Empty;
                request.MailAddress2 = string.Empty;
                request.MailCity = string.Empty;
                request.MailState = string.Empty;
                request.MailZip = string.Empty;
                request.MailCountry = string.Empty;
            }

            // Other address
            else if (type == AddressType.Other)
            {
                deleteAddress = true;
                request.OtherAddress1 = string.Empty;
                request.OtherAddress2 = string.Empty;
                request.OtherCity = string.Empty;
                request.OtherState = string.Empty;
                request.OtherZip = string.Empty;
                request.OtherCountry = string.Empty;
            }

            if (deleteAddress)
            {
                ExigoDAL.WebService().UpdateCustomer(request);
            }
        }

        public static VerifyAddressResponse VerifyAddress(Address address)
        {
            var result = new VerifyAddressResponse();
            result.OriginalAddress = address;
            result.IsValid = false;

            try
            {
                if (address.Country.ToUpper() == "US" && address.IsComplete)
                {
                    var verifiedAddress = ExigoDAL.WebService().VerifyAddress(new VerifyAddressRequest
                    {
                        Address = address.AddressDisplay,
                        City = address.City,
                        State = address.State,
                        Zip = address.Zip,
                        Country = address.Country
                    });

                    result.VerifiedAddress = new Address()
                    {
                        AddressType = address.AddressType,
                        Address1 = verifiedAddress.Address,
                        Address2 = string.Empty,
                        City = verifiedAddress.City,
                        State = verifiedAddress.State,
                        Zip = verifiedAddress.Zip,
                        Country = verifiedAddress.Country
                    };

                    result.IsValid = true;
                }
            }
            catch
            {
                return result;
            }

            return result;
        }
    }
}