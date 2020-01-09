using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        public static bool DebugIsEnabled()
        {
            var httpContext = HttpContext.Current;
            if (httpContext != null && httpContext.Request != null && httpContext.Request.Cookies != null)
            {
                if (httpContext.Request.Cookies[GlobalSettings.Debug.DebugCookieName] != null)
                {
                    return ToBoolean(httpContext.Request.Cookies[GlobalSettings.Debug.DebugCookieName].Value);
                }
            }
            return false;
        }

        public static string DebuggerJSON(object ToSerialize, Newtonsoft.Json.Formatting PreferredFormat = Newtonsoft.Json.Formatting.Indented)
        {
            if (DebugIsEnabled())
            {
                Type objectType = ToSerialize.GetType();
                IList<PropertyInfo> objectprops = new List<PropertyInfo>(objectType.GetProperties());
                var serializedSelect = string.Empty;
                var serializedData = string.Empty;
                foreach(var prop in objectprops)
                {
                    var visible = (prop.Name.ToString() == objectprops[0].Name.ToString()) ? "" : "hidden";
                    serializedSelect += "<option value='" + prop.Name.ToString() + "'>" + prop.Name.ToString() + "</option>";
                    serializedData +=  "<pre class='" + visible + "' id='" + prop.Name.ToString() + "'>" + Newtonsoft.Json.JsonConvert.SerializeObject(prop.GetValue(ToSerialize, null), Newtonsoft.Json.Formatting.Indented) + "</pre>";
                }
                return " <div class='debug-serial-cp well block'><span class='debug-serial-cp-open' data-role='open-debug-serial-cpanel'><i class='fa fa-chevron-left fa-2x debug-low-opacity hidden'></i><i class='fa fa-chevron-right fa-2x debug-low-opacity'></i></span><div class='space-15'></div><h3>Debug Serializations</h3><hr/><select class='form-control'>" + serializedSelect + "</select><div class='space-10'></div><button class='btn btn-default' data-role='add-serialized'><i class='fa fa-plus'></i></button><button class='btn btn-default' data-role='go-to-serialized'><i class='fa fa-arrow-right'></i></button><hr /><div class='scroll-container'>"+ serializedData +"</div></div>";
            }

            return string.Empty;
        }
        //public static string DebuggerJSON(object ToSerialize, Newtonsoft.Json.Formatting PreferredFormat = Newtonsoft.Json.Formatting.Indented)
        //{
        //    if (DebugIsEnabled())
        //    {
        //        return "<pre>" + Newtonsoft.Json.JsonConvert.SerializeObject(ToSerialize, Newtonsoft.Json.Formatting.Indented) + "</pre><div class='space-15'></div>";
        //    }

        //    return string.Empty;
        //}

        public static bool ToBoolean(string value)
        {
            switch (value.ToLower())
            {
                case "yes":
                    return true;
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "no":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                case "0":
                    return false;
                default:
                    return false;
            }
        }

        public static void SetDebugCookie()
        {
            try
            {
                var cookie = new HttpCookie(GlobalSettings.Debug.DebugCookieName);
                cookie.Value = "true";
                cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Setting Debug Cookie");
            }
        }

        public static void DeleteDebugCookie()
        {
            try
            {
                var httpContext = HttpContext.Current;
                var cookie = httpContext.Request.Cookies[GlobalSettings.Debug.DebugCookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    httpContext.Response.Cookies.Add(cookie);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Deleting Debug Cookie");
            }
        }

        public static string DisplayDebugWatermark()
        {
            if (DebugIsEnabled())
            {
                return "<a class='debug-watermark' href='../../stopDebug'><img class='debug-watermark' src='../../Content/images/debug-watermark.gif' /></a>";
            }
            return string.Empty;
        }

        public static string DisplayDebugBasicFill(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='button' class='btn " + style + " btn-default' data-role='basic-info-fill'>Fill Basic Info</button>";
            }
            return string.Empty;
        }

        public static string DisplayDebugAddressFill(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='button' class='btn " + style + " btn-default' data-role='valid-address-fill'>Exigo Address</button>";
            }
            return string.Empty;
        }

        public static string DisplayDebugReset(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='reset' class='btn " + style + " btn-default'>Reset</button>";
            }
            return string.Empty;
        }

        public static string DisplayDebugValidate(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='button' class='btn " + style + " btn-default' data-role='validate'>Validate</button>";
            }
            return string.Empty;
        }

        public static string DisplayDebugNameFill(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='button' class='btn " + style + " btn-default' data-role='name-fill'>Test Name</button>";
            }
            return string.Empty;
        }
        public static string DisplayCreditCardFill(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='button' class='btn " + style + " btn-default' data-role='credit-card-fill'>Test Card</button>";
            }
            return string.Empty;
        }
        public static string DisplayCompleteAutoFill(string style = null)
        {
            if (DebugIsEnabled())
            {
                return "<button type='button' class='btn " + style + " btn-default' data-role='complete-auto-fill'>Complete Autofill</button>";
            }
            return string.Empty;
        }

        public static string DisplayDebugControlPanel()
        {
            if (DebugIsEnabled())
            {
                return "<div class='debug-cp well block'><span class='debug-cp-open' data-role='open-debug-cpanel'><i class='fa fa-chevron-left fa-2x debug-low-opacity'></i><i class='fa fa-chevron-right fa-2x debug-low-opacity hidden'></i></span><div class='space-15'></div><h3>Debug Control Panel</h3><hr><button type=button class='btn block btn-default' data-role=basic-info-fill>Fill Basic Info</button><div class=space-10></div><button type=button class='btn block btn-default' data-role=validate>Validate</button><div class=space-10></div><button type=reset class='btn block btn-default'>Reset</button><div class=space-20></div><a class='btn btn-link' data-toggle=collapse href=#moreDebugControls aria-expanded=false aria-controls=moreDebugControls>More <i class='fa fa-chevron-down'></i></a><div class=collapse id=moreDebugControls><hr><label>Note: The following controls may not function correctly</label><div class=space-10></div><button type=button class='btn btn-default' data-role=credit-card-fill>Test Card</button> <i style=margin-left:5px class='fa fa-info-circle' data-toggle=tooltip data-placement=right title='This will attempt to change the Credit Card field(s) to Exigo' s test card'></i><div class=space-10></div><button type=button class='btn btn-default' data-role=name-fill>Test Name</button> <i style=margin-left:5px class='fa fa-info-circle' data-toggle=tooltip data-placement=right title='This will attempt to change the name field(s) to Exigo Test'></i><div class=space-10></div><button type=button class='btn btn-default' data-role=valid-address-fill>Exigo Address</button> <i style=margin-left:5px class='fa fa-info-circle' data-toggle=tooltip data-placement=right title='This will attempt to autofill the address fields to Exigo' s office'></i><div class=space-10></div><button type=button class='btn btn-default' data-role=complete-auto-fill>Complete Autofill</button> <i style=margin-left:5px class='fa fa-info-circle' data-toggle=tooltip data-placement=right title='This will attempt to change the entire form using all available options'></i><div class=space-10></div></div></div>";
            }

            return string.Empty;
        }
    }
}