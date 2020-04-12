using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaytmTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePayment(Models.RequestData data)
        {
            String merchantKey = Key.merchantKey;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID",Key.merchantId);
            parameters.Add("CHANNEL_ID", "channel id value");
            parameters.Add("INDUSTRY_TYPE_ID", "industry value");
            parameters.Add("WEBSITE", "website value");
            parameters.Add("EMAIL",data.email);
            parameters.Add("MOBILE_NO",data.mobileNumber);
            parameters.Add("CUST_ID", "1");
            parameters.Add("ORDER_ID", "ajsldfsafd4a56s4fd8sa7df5sd4f54a6sf4sda10");
            parameters.Add("TXN_AMOUNT",data.amount);
            parameters.Add("CALLBACK_URL", "url"); //This parameter is not mandatory. Use this to pass the callback url dynamically.

            string checksum = paytm.CheckSum.generateCheckSum(merchantKey, parameters);

            string paytmURL = "https://securegw-stage.paytm.in/theia/processTransaction?orderid=" + parameters.FirstOrDefault(x=>x.Key == "ORDER_ID").Value;

            string outputHTML = "<html>";
            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";
            foreach (string key in parameters.Keys)
            {
                outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";

            ViewBag.htmlData = outputHTML;

            return View("PaymentPage");
        }

        [HttpPost]
        public ActionResult PaytmResponse(Models.PaytmResponse data)
        {
            return View("PaytmResponse", data);
        }
    }
}