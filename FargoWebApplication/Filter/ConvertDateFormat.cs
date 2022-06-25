using Fargo_Application.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FargoWebApplication.Filter
{
    public class ConvertDateFormat
    {
        public static string ConvertMMDDYYYY(string Date)
        {
            try
            {
                if (!string.IsNullOrEmpty(Date))
                {
                    if (Date.Contains('-'))
                    {
                        string[] _DATE = Date.Split('-');
                        Date = _DATE[1].ToString() + "-" + _DATE[0].ToString() + "-" + _DATE[2].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return Date;
        }
    }
}