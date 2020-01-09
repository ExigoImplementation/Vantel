using System;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;
using Common;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        //public static ExigoPaymentApi Payments()
        //{
        //    return new ExigoPaymentApi();
        //}
    }

    //public sealed class ExigoPaymentApi
    //{
    //    private string LoginName = GlobalSettings.Exigo.PaymentApi.LoginName;
    //    private string Password = GlobalSettings.Exigo.PaymentApi.Password;

    //    /// <summary>
    //    /// Generate and return a new token to be used in an Exigo credit card transaction.
    //    /// </summary>
    //    /// <param name="creditCardNumber">The credit card number you wish to use for this transaction</param>
    //    /// <param name="expirationMonth">The expiration month of the credit card you wish to use for this transaction</param>
    //    /// <param name="expirationYear">The expiration year of the credit card you wish to use for this transaction</param>
    //    /// <returns></returns>
    //    public string FetchCreditCardToken(string creditCardNumber, int expirationMonth, int expirationYear)
    //    {
    //        XNamespace ns = "http://payments.exigo.com";
    //        var xRequest = new XDocument(
    //            new XElement(ns + "CreditCard",
    //                new XElement(ns + "CreditCardNumber", creditCardNumber),
    //                new XElement(ns + "ExpirationMonth", expirationMonth),
    //                new XElement(ns + "ExpirationYear", expirationYear)
    //                ));
    //        var xResponse = PostRest("https://payments.exigo.com/2.0/token/rest/CreateCreditCardToken", LoginName, Password, xRequest);

    //        return xResponse.Root.Element(ns + "CreditCardToken").Value;
    //    }

    //    private XDocument PostRest(string url, string username, string password, XDocument element)
    //    {
    //        string postData = element.ToString();

    //        var request = (HttpWebRequest)WebRequest.Create(url);
    //        request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));
    //        request.Method = "POST";
    //        request.ContentLength = postData.Length;
    //        request.ContentType = "text/xml";

    //        var writer = new StreamWriter(request.GetRequestStream());
    //        writer.Write(postData);
    //        writer.Close();

    //        try
    //        {
    //            var response = (HttpWebResponse)request.GetResponse();
    //            using (var responseStream = new StreamReader(response.GetResponseStream()))
    //            {
    //                return XDocument.Parse(responseStream.ReadToEnd());
    //            }
    //        }
    //        catch (WebException ex)
    //        {
    //            var response = (HttpWebResponse)ex.Response;
    //            if (response.StatusCode == HttpStatusCode.Unauthorized)
    //                throw new Exception("Invalid Credentials");
    //            using (var responseStream = new StreamReader(ex.Response.GetResponseStream()))
    //            {
    //                XNamespace ns = "http://schemas.microsoft.com/ws/2005/05/envelope/none";
    //                XDocument doc = XDocument.Parse(responseStream.ReadToEnd());
    //                throw new Exception(doc.Root.Element(ns + "Reason").Element(ns + "Text").Value);
    //            }
    //        }
    //    }
    //}
}