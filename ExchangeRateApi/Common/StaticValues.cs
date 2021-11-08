using System;
namespace ExchangeRate.API.Common
{
    public class StaticValues
    {
        public static class Help
        {
            private static string _helpUrlTemplate = "swagger/{0}/swagger.json";
            public static string v1 = "v1";
            public static string Title = "ExchangeRate.API Help";
            public static string GetCurrentUrl()
            {
                return String.Format(_helpUrlTemplate, v1);
            }
        };
    }
}
