using Common.Api.ExigoWebService;
using System;

namespace ExigoService
{
    public enum CreditCardType
    {
        New       = 0,
        Primary   = 1,
        Secondary = 2
    }
}

public static class ExigoWebService_AccountCreditCardType_Extensions
{
    public static AccountCreditCardType ToAccountCreditCardType(this ExigoService.CreditCardType type)
    {
        switch (type)
        {
            case ExigoService.CreditCardType.Primary: return AccountCreditCardType.Primary;
            case ExigoService.CreditCardType.Secondary: return AccountCreditCardType.Secondary;
            default: throw new Exception("Unable to convert CreditCardType '{0}' to AccountCreditCardType.".FormatWith(type.ToString()));
        }
    }
}