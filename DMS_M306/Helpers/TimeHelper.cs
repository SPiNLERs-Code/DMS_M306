using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Helpers
{
    public static class TimeHelper
    {
        public static String GetTimestamp(DateTime value)
        {
            return value.ToUniversalTime().ToString("yyyyMMddHHmmssffff");
        }
    }
}