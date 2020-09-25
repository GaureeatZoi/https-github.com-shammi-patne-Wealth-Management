using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace ZOI.APP
{
    public static class CommonFunctions
    {
       
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetBoolean(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static bool? GetBoolean(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToBoolean(data, 0);
        }

        public static void SetDouble(this ISession session, string key, double value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static double? GetDouble(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToDouble(data, 0);
        }

        public static string GetAPIPath()
        {
            return "https://ms.zoifintech.com/";
            //return "http://localhost:65481/";
        }

        //public static void SetCookies(string key,string value)
        //{
        //    CookieOptions cookieOptions = new CookieOptions();
        //  //  Response.Cookies.Append(key, value);
        //    HttpContext.Response.Cookies.Append("ReminderDate","");

        //}
    }
}
