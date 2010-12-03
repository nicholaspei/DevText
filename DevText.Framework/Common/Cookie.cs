using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;


namespace DevText.Framework.Common
{
     public interface ICookie
    {
        T GetValue<T>(string name);

        T GetValue<T>(string name, bool expireOnceRead);

        void SetValue<T>(string name, T value);

        void SetValue<T>(string name, T value, float expireDurationInMinutes);

        void SetValue<T>(string name, T value, bool httpOnly);

        void SetValue<T>(string name, T value, float expireDurationInMinutes, bool httpOnly);
    }

    public class Cookie : ICookie
    {
        private readonly HttpContextBase httpContext;

        private static bool defaultHttpOnly = true;
        private static float defaultExpireDurationInMinutes = 5;

        public Cookie(HttpContextBase httpContext)
        {
            
            this.httpContext = httpContext;
        }

        public static bool DefaultHttpOnly
        {
            [DebuggerStepThrough]
            get
            {
                return defaultHttpOnly;
            }

            [DebuggerStepThrough]
            set
            {
                defaultHttpOnly = value;
            }
        }

        public static float DefaultExpireDurationInMinutes
        {
            [DebuggerStepThrough]
            get
            {
                return defaultExpireDurationInMinutes;
            }

            [DebuggerStepThrough]
            set
            {
                defaultExpireDurationInMinutes = value;
            }
        }

        public T GetValue<T>(string name)
        {
            return GetValue<T>(name, true);
        }

        public T GetValue<T>(string name, bool expireOnceRead)
        {
            
            HttpCookie cookie = httpContext.Request.Cookies[name];
            T value = default(T);

            if (cookie != null)
            {
                if (!string.IsNullOrWhiteSpace(cookie.Value))
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                    try
                    {
                        value = (T)converter.ConvertFromString(cookie.Value);
                    }
                    catch (NotSupportedException)
                    {
                        if (converter.CanConvertFrom(typeof(string)))
                        {
                            value = (T)converter.ConvertFrom(cookie.Value);
                        }
                    }
                }

                if (expireOnceRead)
                {
                    cookie = httpContext.Response.Cookies[name];

                    if (cookie != null)
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(-1);
                    }
                }
            }

            return value;
        }

        public void SetValue<T>(string name, T value)
        {
            SetValue(name, value, DefaultExpireDurationInMinutes, DefaultHttpOnly);
        }

        public void SetValue<T>(string name, T value, float expireDurationInMinutes)
        {
            SetValue(name, value, expireDurationInMinutes, DefaultHttpOnly);
        }

        public void SetValue<T>(string name, T value, bool httpOnly)
        {
            SetValue(name, value, DefaultExpireDurationInMinutes, httpOnly);
        }

        public void SetValue<T>(string name, T value, float expireDurationInMinutes, bool httpOnly)
        {
            
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            string cookieValue = string.Empty;

            try
            {
                cookieValue = converter.ConvertToString(value);
            }
            catch (NotSupportedException)
            {
                if (converter.CanConvertTo(typeof(string)))
                {
                    cookieValue = (string)converter.ConvertTo(value, typeof(string));
                }
            }

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                HttpCookie cookie = new HttpCookie(name)
                                    {
                                        Value = cookieValue,
                                        Expires = DateTime.Now.AddMinutes(expireDurationInMinutes),
                                        HttpOnly = httpOnly
                                    };

                httpContext.Response.Cookies.Add(cookie);
            }
        }
    }
}
