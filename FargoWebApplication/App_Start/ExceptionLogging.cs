using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using context = System.Web.HttpContext;

namespace Fargo_Application.App_Start
{
    public static class ExceptionLogging
    {
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static string SendErrorToText(Exception ex)
        {
            string ErrorMessage = "";
            var line = Environment.NewLine + Environment.NewLine;
            
            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = context.Current.Request.Url.ToString();
            ErrorLocation = ex.Message.ToString();
            try
            {
                string filepath = context.Current.Server.MapPath("~/LogFiles/");  //Text File Path
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Today.ToString("ddMMyyyymmss") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter streamWriter = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    streamWriter.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    streamWriter.WriteLine("-------------------------------------------------------------------------------------");
                    streamWriter.WriteLine(line);
                    streamWriter.WriteLine(error);
                    streamWriter.WriteLine("--------------------------------*End*------------------------------------------");
                    streamWriter.WriteLine(line);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception exception)
            {
               ErrorMessage=  exception.ToString();
            }
            return ErrorMessage;
        }  
    }
}