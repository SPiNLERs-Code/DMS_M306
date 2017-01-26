using System;

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