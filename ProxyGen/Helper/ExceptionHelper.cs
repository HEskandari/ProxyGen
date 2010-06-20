using System;
using System.Text;

namespace ProxyGen.Helper
{
    public static class ExceptionHelper
    {
        public static string GetExceptionReport(this Exception exception)
        {
            var sb = new StringBuilder(exception.Message);

            Exception detail = exception.InnerException;
            while (detail != null)
            {
                sb.AppendFormat("Details: {0}", detail.Message);
                detail = detail.InnerException;
            }

            return sb.ToString();
        }
    }
}