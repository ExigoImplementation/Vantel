using Common;
using Common.Providers;
using Common.Services;
using ExigoService;
using ReplicatedSite.Models;
using ReplicatedSite.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    public class DevController : BaseController
    {
        #region Main Post Method
        [HttpPost]
        public ActionResult DevToolsPostMethod(string line)
        {
            try
            {
                // line splitted
                var splittedLine = line.Split(' ');
                // validate the lime method call

                //
                // main method for receiving the line code
                Assembly asm = Assembly.GetExecutingAssembly();
                // load assembly
                var methods = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                    .SelectMany(type => type.GetMethods())
                    .Where(method => method.IsPublic);
                // get all of the methods
                var stringMethod = splittedLine[0];
                // get the method to call
                var methodCall = methods.First(x => x.Name.ToLower() == stringMethod.ToLower());
                // get the method from all of the avaiable methods
                var parameters = methodCall.GetParameters();
                // get parameters
                Type controller = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).First(x => x.Name == methodCall.DeclaringType.Name);
                // get the controller
                object classInstance = Activator.CreateInstance(controller, null);
                // create a remote class instance of that controller
                List<object> parameterList = new List<object>();
                // compare if the parameters sent matches the method line
                var lineCodeLength = splittedLine.Length;
                var parameterLength = parameters.Where(x => x.ParameterType.FullName != typeof(HttpContextBase).FullName).Count();
                if (lineCodeLength - 1 != parameterLength)
                {
                    return Json(new { success = false, message = "Parameters dont match in the method" });
                }
                // generate the parameter list based on the method parameter
                int count = 1;
                foreach (var param in parameters) {
                    if (param.ParameterType == typeof(HttpContextBase))
                    {
                        parameterList.Add(HttpContext);
                    }
                    else
                    {
                        parameterList.Add(Convert.ChangeType(splittedLine[count],param.ParameterType));
                        count++;
                    }
                }

                var result =  methodCall.Invoke(classInstance, parameterList.ToArray());
                var success = true;
                var message = string.Empty;
                if(result != null)
                {
                    
                    if (result.GetType() == typeof(JsonNetResult)) { message = ((JsonNetResult)result).Data.ToString(); }
                    if (result.GetType() == typeof(JsonResult)) { return (JsonResult)result; }
                    if (result.GetType() == typeof(ActionResult)) { return (ActionResult)result; }
                    if (result.GetType() == typeof(RedirectResult)) { return (RedirectResult)result; }
                    if (result.GetType() == typeof(RedirectToRouteResult)) { return (RedirectToRouteResult)result; }
                    if (message.IsNullOrEmpty()) { message = result.ToString(); }
                }
                // invoke the method
                return Json(    
                    new {
                        success,
                        message
                    });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Wrapper Method calls

        [NonAction]
        [DevToolVisible]
        public JsonResult TemplateMethod() {
            try
            {
                //Your wrapper method logic goes here 
                return Json(new { success = false, message = "Custom message goes here" });
            }
            catch (Exception ex)
            {

                return Json(new {  success = false, message = ex.Message });
            }
        }

        [NonAction]
        [DevToolVisible]
        public ActionResult AddItem(HttpContextBase context, string itemCode)
        {
            var controller = DependencyResolver.Current.GetService<ShoppingController>();
            controller.ControllerContext = new ControllerContext(context.Request.RequestContext, controller);
            var result = controller.AddItemToCart(new Item() { ItemCode = itemCode, Quantity = 1.0m });
            return result;
        }

        [NonAction]
        [DevToolVisible]
        public string Help() {
            // main method for receiving the line code
            Assembly asm = Assembly.GetExecutingAssembly();
            // load assembly
            var methods = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && method.CustomAttributes.Any(x=> x.AttributeType == typeof(DevToolVisible)));

            var methodArray =  methods
                    .Select(x => $"* : {x.Name} {string.Join(" ", x.GetParameters().Where(z=> z.ParameterType != typeof(HttpContextBase)).Select(y => $"({y.ParameterType.Name} : {y.Name})"))}").ToArray();
            return "Method list available for dev tools : <br>" + string.Join("<br>", methodArray);
            
        }
        #endregion
    }

    public class DevToolVisible : Attribute { }
}