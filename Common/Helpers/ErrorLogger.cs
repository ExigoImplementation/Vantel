using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using ExigoService;

namespace Common.Helpers
{
    public static class ErrorLogger
    {
        public static void SendAlert(string subject, string message)
        {
            // Run this in the background
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (GlobalSettings.ErrorLogging.EmailRecipients.Length > 0)
                    {
                        new Email()
                            .To(GlobalSettings.ErrorLogging.EmailRecipients)
                            .From("noreply@exigo.com")
                            .IsHtml(true)
                            .UseExigoApi(true)
                            .Priority(MailPriority.High)
                            .Subject(subject) 
                            .Body(message)
                            .Send();
                    }
                }
                catch { }
            });
        }

        public static void LogException(Exception error, string request = "")
        {
            try
            {
                string assemblyName, controllerName, actionName;
                
                assemblyName = controllerName = actionName = "Unknown";

                if (error.TargetSite != null)
                {
                    assemblyName = error.TargetSite.Module.Assembly.GetName().Name;
                    actionName = error.TargetSite.Name;
                    controllerName = error.TargetSite.DeclaringType.Name;
                }

                string fileName = GetFileName(error);
                int lineNumber = GetLineNumber(error);
                string exceptionName = error.GetType().ToString();
                string exceptionMessage = error.Message;

                var body = @"
                        <style>
                                 body {{font-family:""Verdana"";font-weight:normal;font-size: .7em;color:black;}} 
                                 p {{font-family:""Verdana"";font-weight:normal;color:black;margin-top: -5px}}
                                 b {{font-family:""Verdana"";font-weight:bold;color:black;margin-top: -5px}}
                                 H1 {{ font-family:""Verdana"";font-weight:normal;font-size:18pt;color:red }}
                                 H2 {{ font-family:""Verdana"";font-weight:normal;font-size:14pt;color:maroon }}
                                 pre {{font-family:""Consolas"",""Lucida Console"",Monospace;font-size:11pt;margin:0;padding:0.5em;line-height:14pt}}
                                 .marker {{font-weight: bold; color: black;text-decoration: none;}}
                                 .version {{color: gray;}}
                                 .error {{margin-bottom: 10px;}}
                                 .expandable {{ text-decoration:underline; font-weight:bold; color:navy; cursor:hand; }}
                                 @@media screen and (max-width: 639px) {{
                                  pre {{ width: 440px; overflow: auto; white-space: pre-wrap; word-wrap: break-word; }}
                                 }}
                                 @@media screen and (max-width: 479px) {{
                                  pre {{ width: 280px; }}
                                 }}
                        </style>

                        <span>
                            <H1>Server Error in '{5}' Application.<hr width=100% size=1 color=silver></H1>
                            <h2> <i>{0}</i> </h2>
                        </span>


                        <font face=""Arial, Helvetica, Geneva, SunSans-Regular, sans-serif "">

                        <b> Description: </b>An unhandled exception occurred during the execution of the current web request. Please review the stack trace for more information about the error and where it originated in the code.

                        <br><br>

                        <b> Exception Details: </b>{4}: {0}<br><br>

                        <b> Raw URL: </b> {5}<br />
                        <b> Assembly: </b> {6}<br />
                        <b> Controller: </b> {7}<br />
                        <b> Action: </b> {8}<br />
                        <b> Source File: </b> {3}<b> &nbsp;&nbsp; Line: </b> {2}

                        <br><br>

                        <b>Stack Trace:</b> <br><br>

                        <table width=100% bgcolor=""#ffffcc"">
                           <tr>
                              <td>
                                  <code><pre>
{1}            
                                </pre></code>
                              </td>
                           </tr>
                        </table>
                    ".FormatWith(exceptionMessage, error.ToString(), lineNumber, fileName, exceptionName, request, assemblyName, controllerName, actionName);

                // Email the error
                SendAlert(
                    "{0} Website Error ({1}) - ({2}){3}".FormatWith(GlobalSettings.Company.Name, request, exceptionName, exceptionMessage, exceptionName),
                    body
                );
            }
            catch
            {
                //we can't let an exception of logging of an exception bubble up
            }
        }

        static string GetFileName(Exception error)
        {
            if (error.StackTrace == null)
            {
                return "Unavailable";
            }
            int originalLineIndex = error.StackTrace.IndexOf(":line");
            if (originalLineIndex == -1)
            {
                return "Unavailable";
            }
            string originalLine = error.StackTrace.Substring(0, originalLineIndex);
            string[] sections = originalLine.Split('\\');
            return sections[sections.Length - 1];
        }

        static int GetLineNumber(Exception error)
        {
            if (error.StackTrace == null)
            {
                return 0;
            }

            string[] sections = error.StackTrace.Split(' ');
            int index = 0;
            foreach (string section in sections)
            {
                if (section.EndsWith(":line"))
                {
                    break;
                }
                index++;
            }

            Debug.Assert(index != 0);
            if (index == sections.Length)
            {
                return 0;
            }
            string lineNumber = sections[index + 1];
            int number = -1;
            try//Strip the /r/n if present
            {
                number = Convert.ToInt32(lineNumber.Substring(0, lineNumber.Length - 2));
            }
            catch (FormatException)
            {
                number = Convert.ToInt32(lineNumber);
            }

            return number;
        }
    }
}