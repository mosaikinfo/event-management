using System;

namespace EventManagement
{
    public static class UriHelper
    {
        /// <summary>
        /// Combine a base uri and a relative uri.
        /// </summary>
        /// <param name="baseUri">a complete uri</param>
        /// <param name="relativeUri">a relative uri that must begin with '~/'</param>
        /// <returns>combined absolute uri</returns>
        public static string MakeAbsoluteUri(string baseUri, string relativeUri)
        {
            if (relativeUri.StartsWith("~/"))
            {
                relativeUri = relativeUri.Substring(2, relativeUri.Length - 2);
                if (!baseUri.EndsWith('/'))
                {
                    baseUri += '/';
                }
                return new Uri(new Uri(baseUri), relativeUri).AbsoluteUri;
            }
            return relativeUri;
        }
    }
}