using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Workflow
{
    public static class Utils
    {
        public static bool IsValidEmail(string input)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regex.IsMatch(input);
        }

        public static bool IsValidDouble(string input)
        {
            Double temp;
            return Double.TryParse(input, out temp);
        }

        public static bool IsValidInt64(string input)
        {
            Int64 temp;
            return Int64.TryParse(input, out temp);
        }

        public static bool IsValidInt32(string input)
        {
            Int32 temp;
            return Int32.TryParse(input, out temp);
        }
        public static double GetGMTInMS()
        {
            var unixTime = DateTime.Now.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (double)unixTime.TotalMilliseconds;
        }
        public static string ConvertToString(object value, string defaultValue)
        {
            try
            {
                if (value is null)
                {
                    return defaultValue;
                }
                return Convert.ToString(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double ConvertToDouble(object value, double defaultValue)
        {
            try
            {
                return Convert.ToDouble(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }
        public static int ConvertToInt32(object value, int defaultValue)
        {
            try
            {
                if (value == null)
                {
                    return defaultValue;
                }
                return Convert.ToInt32(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int? ConvertToNullableInt32(object value, int? defaultValue)
        {
            try
            {
                return Convert.ToInt32(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long ConvertToInt64(object value, long defaultValue)
        {
            try
            {
                return Convert.ToInt64(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long? ConvertToNullableInt64(object value, long? defaultValue)
        {
            try
            {
                return Convert.ToInt64(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool ConvertToBoolean(object value, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ConvertToDateTime(object value, DateTime defaultValue)
        {
            try
            {
                DateTime datetime = new DateTime();
                bool result = DateTime.TryParseExact(value.ToString(), "dd/MM/yyyy", null, DateTimeStyles.None, out datetime);
                if (!result)
                {
                    datetime = DateTime.Parse(value.ToString().Trim());
                }
                return datetime;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime? ConvertToNullableDateTime(object value, DateTime? defaultValue)
        {
            try
            {
                return DateTime.ParseExact(value.ToString(), "d/m/yyyy", null);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ConvertToDateTime_Edit(object value, DateTime defaultValue)
        {
            try
            {
                DateTime datetime = new DateTime();
                bool result = DateTime.TryParse(value.ToString(), null, DateTimeStyles.None, out datetime);
                if (!result)
                {
                    return defaultValue;
                    //datetime = DateTime.Parse(value.ToString().Trim());
                }
                return datetime;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Bacth: Hash file content into string
        /// for example: 1e5e4212f86d8ecbe5acc956c97fa373
        /// </summary>
        /// <param name="file">file content - array of bytes</param>
        /// <returns>string with 32 characters of length</returns>
        public static string HashFile(byte[] file)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();

            byte[] hashed = md5.ComputeHash(file);
            foreach (byte b in hashed)
                // convert to hexa
                sb.Append(b.ToString("x2").ToLower());

            // sb = set of hexa characters
            return sb.ToString();
        }

        /// <summary>
        /// Bacth: detemine path to store file
        /// for example: [1e]-[5e]-[42]-[1e5e4212f86d8ecbe5acc956c97fa373]
        /// </summary>
        /// <param name="file">file content - array of bytes</param>
        /// <returns>hashed path</returns>
        public static List<string> GetPath(byte[] file)
        {
            string hashed = HashFile(file);
            List<string> toReturn = new List<string>(3);
            toReturn.Add(hashed.Substring(0, 2));
            toReturn.Add(hashed.Substring(2, 2));
            toReturn.Add(hashed.Substring(4, 2));
            toReturn.Add(hashed);
            return toReturn; // for example: [1e]-[5e]-[42]-[1e5e4212f86d8ecbe5acc956c97fa373]
        }

        /// <summary>
        /// Gets the object., 
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static object GetObject(object value, object valueIfNull)
        {
            if ((value != null) && (value != DBNull.Value))
            {
                return value;
            }
            return valueIfNull;
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static DateTime GetDateTime(object value, DateTime valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is DateTime)
            {
                return (DateTime)value;
            }
            return DateTime.Parse(value.ToString());
        }

        public static Decimal GetDecimal(object value, Decimal valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is Decimal)
            {
                return (Decimal)value;
            }
            return Decimal.Parse(value.ToString());
        }
        /// <summary>
        /// Gets the byte.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static byte GetByte(object value, byte valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is byte)
            {
                return (byte)value;
            }
            return byte.Parse(value.ToString());
        }
        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueIfNull">if set to <c>true</c> [value if null].</param>
        /// <returns></returns>
        public static bool GetBoolean(object value, bool valueIfNull)
        {
            value = GetObject(value, valueIfNull);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is bool)
            {
                return (bool)value;
            }
            if (!(value is byte))
            {
                return bool.Parse(value.ToString());
            }
            if (((byte)value) == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the string. 
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static string GetString(object value, string valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is string)
            {
                return (string)value;
            }
            return value.ToString();
        }

        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static float GetSingle(object value, float valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is float)
            {
                return (float)value;
            }
            return float.Parse(value.ToString());
        }

        /// <summary>
        /// Gets the int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static long GetInt64(object value, long valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is long)
            {
                return (long)value;
            }
            return long.Parse(value.ToString());
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueIfNull">The value if null.</param>
        /// <returns></returns>
        public static int GetInt32(object value, int valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is int)
            {
                return (int)value;
            }
            return int.Parse(value.ToString());
        }

        //public static int GetIDFromQueryString(string key, int valueIfNull)
        //{
        //    return Utils.ConvertToInt32(HttpContext.Current.Request.QueryString[key], valueIfNull);
        //}

        public static string GetExtension(string fileName)
        {
            int dotIndex = fileName.LastIndexOf(".");
            return dotIndex == -1 ? string.Empty : fileName.Substring(dotIndex + 1);
        }
        public static string GetFileNameWithoutExtention(string fileName)
        {
            int dotIndex = fileName.LastIndexOf(".");
            return dotIndex == -1 ? fileName : fileName.Substring(0, dotIndex);
        }


        //This code EnCode Base64
        private static string[] d2c = new string[] { "V", "_", "C", "M", "S" };

        private static int c2d(string c)
        {
            int d = 0;
            for (int i = 0, n = d2c.Length; i < n; ++i)
            {
                if (c == d2c[i])
                {
                    d = i;
                    break;
                }
            }
            return d;
        }

        public static int IDFromString(string base64)
        {
            int pos = base64.Length / 2;
            int step = c2d(base64.Substring(pos, 1)) + d2c.Length;
            string orginal = String.Format("{0}{1}", base64.Substring(0, pos), base64.Substring(pos + 1));
            for (int i = 0; i < step; ++i)
            {
                orginal = DecodeFrom64(orginal);
            }
            return Convert.ToInt32(orginal);
        }

        public static string IDToString(int id)
        {
            Random random = new Random();
            int step = random.Next(5, 10);
            string base64 = id.ToString();
            for (int i = 0; i < step; ++i)
            {
                base64 = EncodeTo64(base64);
            }
            int pos = base64.Length / 2;
            return String.Format("{0}{1}{2}", base64.Substring(0, pos), d2c[step - d2c.Length], base64.Substring(pos));
        }

        private static string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);

            string returnValue = ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }

        private static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
    }
}
