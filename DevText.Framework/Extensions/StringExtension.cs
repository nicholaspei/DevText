using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DevText.Framework.Extensions
{
    public static class StringExtension
    {
        private static readonly Regex emailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex webUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex stripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static string FormatWith(this string instance,params object[] args)
        {
            return string.Format(instance,args);
        }

        private static string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(input);
                byte[] hash = md5.ComputeHash(data);
                return Convert.ToBase64String(data);
            }
        }

      public static string GetSHA256Hash(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(sha.Hash);
            }
        }

        public static bool IsEmail(string input)
      {
          return !string.IsNullOrWhiteSpace(input) && emailExpression.IsMatch(input);
      }

        public static bool IsWebUrl(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && webUrlExpression.IsMatch(input);
        }

        public static bool IsIPAddress(string input)
        {
            IPAddress ip;
            return !string.IsNullOrWhiteSpace(input) && IPAddress.TryParse(input, out ip);
        }

        public static string StripHtml(string input)
        {
            return stripHTMLExpression.Replace(input, string.Empty);
        }

      public static string RemoveIllegalCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            text = text.Replace(":", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace(".", string.Empty);
            text = text.Replace(",", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace(" ", "-");
            text = RemoveDiacritics(text);
            text = RemoveExtraHyphen(text);

            return HttpUtility.UrlEncode(text).Replace("%", string.Empty);
        }

      private static string RemoveExtraHyphen(string text)
      {
          if (text.Contains("--"))
          {
              text = text.Replace("--", "-");
              return RemoveExtraHyphen(text);
          }

          return text;
      }

      private static String RemoveDiacritics(string text)
      {
          String normalized = text.Normalize(NormalizationForm.FormD);
          StringBuilder sb = new StringBuilder();

          for (int i = 0; i < normalized.Length; i++)
          {
              Char c = normalized[i];
              if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                  sb.Append(c);
          }

          return sb.ToString();
      }
    }
}
