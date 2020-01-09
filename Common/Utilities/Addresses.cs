using Common.Api.ExigoWebService;
using ExigoService;
using System;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Validates US Addresses
        /// </summary>
        /// <param name="address">The Address To Validate</param>
        /// <returns>address, validated and optimized if possible</returns>
        public static IAddress ValidateAddress(IAddress address)
        {
            var validateAddresses = true;
            var isShippingAddress = address.CanBeParsedAs<ShippingAddress>();
            var shippingAddress = new ShippingAddress();
            var normalAddress = new Address();

            // Turn the address passed in into its correct type
            if (isShippingAddress)
            {
                shippingAddress = address as ShippingAddress;
            }
            else
            {
                normalAddress = address as Address;
            }

            // Ensure that only US addresses are attempted to be validated
            if (validateAddresses && address.Country == "US")
            {
                // Handle shipping addresses and regular addresses differently
                if (isShippingAddress)
                {
                    // Convert the shipping address to a normal address
                    var convertedAddress = new Address(shippingAddress);

                    // Verify that the newly converted address is valid
                    var verifyAddressResponse = ExigoDAL.VerifyAddress(convertedAddress);

                    // if the address can be validated, update and return the shipping address, otherwise return the original shipping address
                    if (verifyAddressResponse.IsValid)
                    {
                        var verifiedAddress = verifyAddressResponse.VerifiedAddress;
                        shippingAddress.AddressType = verifiedAddress.AddressType;
                        shippingAddress.Address1 = verifiedAddress.Address1;
                        shippingAddress.Address2 = verifiedAddress.Address2;
                        shippingAddress.City = verifiedAddress.City;
                        shippingAddress.State = verifiedAddress.State;
                        shippingAddress.Zip = verifiedAddress.Zip;
                        shippingAddress.Country = verifiedAddress.Country;

                        return shippingAddress;
                    }
                    else
                    {
                        return address;
                    }
                }
                else
                {
                    // Verify that the address is valid
                    var verifyAddressResponse = ExigoDAL.VerifyAddress(normalAddress);

                    // if the address can be validated, return the validated address, otherwise return the original address
                    if (verifyAddressResponse.IsValid)
                    {
                        return verifyAddressResponse.VerifiedAddress;
                    }
                    else
                    {
                        return address;
                    }
                }
            }
            else
            {
                return address;
            }
        }
    }
}