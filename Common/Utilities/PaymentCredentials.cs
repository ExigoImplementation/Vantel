using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace PaymentConfiguration
{
    public static class  PaymentCredentials
    {

        public static string Create()
        {
            string ApiLoginName = Common.GlobalSettings.Exigo.PaymentApi.LoginName;
            string ApiTransactionKey = Common.GlobalSettings.Exigo.PaymentApi.Password;

            if (ApiLoginName==null || ApiTransactionKey==null) throw new Exception("Configuration missing");

            var time = DateTime.UtcNow.Ticks;
            using (var md5Hash = MD5.Create())
            {
                return Convert.ToBase64String( 
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}:{2}",
                            Convert.ToBase64String(
                                md5Hash.ComputeHash(Encoding.ASCII.GetBytes(time + ApiTransactionKey))),
                            ApiLoginName,
                            time
                    )));
            }
        }
    }
}
