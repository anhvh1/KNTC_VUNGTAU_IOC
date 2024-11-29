using Com.Gosol.KNTC.Ultilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace Com.Gosol.KNTC.Ultilities
{
    public class Format
    {
        public static string FormatDateTime(DateTime dt)
        {
            if (dt == DateTime.MinValue)
            {
                return string.Empty;
            }

            return dt.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static object GetValueParm(object value)
        {
            if (value is int)
            {
                return (int)value == 0 ? DBNull.Value : value;
            }
            else if (value is DateTime)
            {
                return (DateTime)value == DateTime.MinValue ? DBNull.Value : value;
            }
            else
                return value;
        }
        public static String FormatDate(DateTime dt)
        {
            if (dt == DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToString("dd/MM/yyyy");
        }
        public static String FormatDate_Hour(DateTime dt)
        {
            if (dt == DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToString("dd/MM/yyyy hh:mm:ss");
        }
        public static string FormatDateGoTime(DateTime dt)
        {
            if (dt == DateTime.MinValue)
            {
                return string.Empty;
            }
            return dt.ToString("dd/MM/yyy hh:mm:ss tt");
        }
        public static String FormatDate(string dt)
        {
            return Convert.ToDateTime(dt).ToString("dd/MM/yyyy");
        }

        public static String FormatNumber(string value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ",";
            numFormat.NumberDecimalDigits = 0;
            numFormat.NumberNegativePattern = 0;

            return Convert.ToInt64(value).ToString("N", numFormat);
        }

        public static String FormatNumber_Dot(string value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ".";
            numFormat.NumberDecimalDigits = 0;
            numFormat.NumberNegativePattern = 0;

            return Convert.ToInt64(value).ToString("N", numFormat);
        }

        public static String FormatNumber(Int32 value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ",";
            numFormat.NumberDecimalDigits = 0;
            numFormat.NumberNegativePattern = 0;

            return value.ToString("N", numFormat);
        }

        public static String FormatNumber(Int64 value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ",";
            numFormat.NumberDecimalDigits = 0;
            numFormat.NumberNegativePattern = 0;

            return value.ToString("N", numFormat);
        }

        public static String FormatNumber(double value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ",";
            numFormat.NumberDecimalDigits = 0;
            numFormat.NumberNegativePattern = 0;

            return value.ToString("N", numFormat);
        }

        public static String FormatNumber(decimal value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ",";
            numFormat.NumberDecimalDigits = 0;
            numFormat.NumberNegativePattern = 0;

            return value.ToString("N", numFormat);
        }
        public static String FormatNumberDecimal2(decimal value)
        {
            System.Globalization.NumberFormatInfo numFormat = new System.Globalization.NumberFormatInfo();
            numFormat.NumberGroupSeparator = ",";
            numFormat.NumberDecimalDigits = 2;
            numFormat.NumberNegativePattern = 0;

            return value.ToString("N", numFormat);
        }

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
    }

    public class Season
    {
        public static int GetSeasonByDateTime(DateTime dt)
        {
            if (dt.Month >= 1 && dt.Month <= 3)
            {
                return 1;
            }
            else if (dt.Month >= 4 && dt.Month <= 6)
            {
                return 2;
            }
            else if (dt.Month >= 7 && dt.Month <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        public static DateTime GetFirstDayOfSeason(int year, int season)
        {
            if (season == 1)
            {
                return new DateTime(year, 1, 1);
            }
            else if (season == 2)
            {
                return new DateTime(year, 4, 1);
            }
            else if (season == 3)
            {
                return new DateTime(year, 7, 1);
            }
            else
            {
                return new DateTime(year, 10, 1);
            }
        }

        public static DateTime GetLastDayOfSeason(int year, int season)
        {
            if (season == 1)
            {
                return new DateTime(year, 3, 31);
            }
            else if (season == 2)
            {
                return new DateTime(year, 6, 30);
            }
            else if (season == 3)
            {
                return new DateTime(year, 9, 30);
            }
            else
            {
                return new DateTime(year, 12, 31);
            }
        }

        public static DateTime GetFirstDayOfYear(int year)
        {
            return new DateTime(year, 1, 1);
        }

        public static DateTime GetFirstDayOfSeason(DateTime dt)
        {
            if (dt.Month >= 1 && dt.Month <= 3)
            {
                return new DateTime(dt.Year, 1, 1);
            }
            else if (dt.Month >= 4 && dt.Month <= 6)
            {
                return new DateTime(dt.Year, 4, 1);
            }
            else if (dt.Month >= 7 && dt.Month <= 9)
            {
                return new DateTime(dt.Year, 7, 1);
            }
            else
            {
                return new DateTime(dt.Year, 10, 1);
            }
        }

        public static DateTime GetLastDayOfSeason(DateTime dt)
        {
            if (dt.Month >= 1 && dt.Month <= 3)
            {
                return new DateTime(dt.Year, 3, 31);
            }
            else if (dt.Month >= 4 && dt.Month <= 6)
            {
                return new DateTime(dt.Year, 6, 30);
            }
            else if (dt.Month >= 7 && dt.Month <= 9)
            {
                return new DateTime(dt.Year, 9, 30);
            }
            else
            {
                return new DateTime(dt.Year, 12, 31);
            }
        }

        public static DateTime GetFirstDayOfYear(DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime GetFirstDayOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime GetLastDayOfMonth(int month, int year)
        {
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                return new DateTime(year, month, 31);
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                return new DateTime(year, month, 30);
            }
            else if (month == 2)
            {
                if (CheckLeapYear(year))
                {
                    return new DateTime(year, month, 29);
                }
                else
                {
                    return new DateTime(year, month, 28);
                }
            }
            return Constant.DEFAULT_DATE;
        }

        public static DateTime GetFirstDayOfWeek(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday)
            {
                return dt;
            }
            else if (dt.DayOfWeek == DayOfWeek.Tuesday)
            {
                return dt.AddDays(-1);
            }
            else if (dt.DayOfWeek == DayOfWeek.Wednesday)
            {
                return dt.AddDays(-2);
            }
            else if (dt.DayOfWeek == DayOfWeek.Thursday)
            {
                return dt.AddDays(-3);
            }
            else if (dt.DayOfWeek == DayOfWeek.Friday)
            {
                return dt.AddDays(-4);
            }
            else if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                return dt.AddDays(-5);
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return dt.AddDays(-6);
            }
            else
            {
                return Constant.DEFAULT_DATE;
            }
        }

        public static DateTime GetLastDayOfWeek(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday)
            {
                return dt.AddDays(6);
            }
            else if (dt.DayOfWeek == DayOfWeek.Tuesday)
            {
                return dt.AddDays(5);
            }
            else if (dt.DayOfWeek == DayOfWeek.Wednesday)
            {
                return dt.AddDays(4);
            }
            else if (dt.DayOfWeek == DayOfWeek.Thursday)
            {
                return dt.AddDays(3);
            }
            else if (dt.DayOfWeek == DayOfWeek.Friday)
            {
                return dt.AddDays(2);
            }
            else if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                return dt.AddDays(1);
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return dt;
            }
            else
            {
                return Constant.DEFAULT_DATE;
            }
        }

        private static bool CheckLeapYear(int year)
        {
            if (year % 4 != 0)
            {
                return false;
            }
            else
            {
                if (year % 100 != 0)
                {
                    return true;
                }
                else
                {
                    if (year % 400 == 0)
                    {
                        return true;
                    }
                    else return false;
                }
            }
        }
    };

    public class ConvertFilePattern
    {
        private string _path;
        private int _oldid;
        private int _newid;

        public ConvertFilePattern()
        {
            _path = "";
            _oldid = -1;
            _newid = -1;
        }

        public ConvertFilePattern(string path, int oldid, int newid)
        {
            _path = path;
            _oldid = oldid;
            _newid = newid;
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public int Oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        public int Newid
        {
            get { return _newid; }
            set { _newid = value; }
        }
    }


    public static class Utils
    {
        public static string AddCommas(double? number)
        {
            string result = "";
            if (number != null)
            {
                try
                {
                    string num = number.Value.ToString();
                    if (num.IndexOf(".") > 0)
                    {
                        string withoutDecimals = ((int)number.Value).ToString("N0");
                        withoutDecimals = withoutDecimals.Replace(',', '.');
                        string withDecimals = "," + num.Substring(num.IndexOf(".") + 1);
                        result = withoutDecimals + withDecimals;
                    }
                    else
                    {
                        result = number.Value.ToString("N0");
                        result = result.Replace(',', '.');
                    }
                   
                }
                catch (Exception)
                {
                    result = number.Value.ToString();
                }
            }

            return result;
        }

        public static string AddCommasDouble(string numberstr)
        {
            string result = "";
            if (numberstr != null)
            {
                try
                {
                    var number = Convert.ToDouble(numberstr.ToString().Trim());
                    string num = number.ToString();
                    if (num.IndexOf(".") > 0)
                    {
                        string withoutDecimals = ((int)number).ToString("N0");
                        withoutDecimals = withoutDecimals.Replace(',', '.');
                        string withDecimals = "," + num.Substring(num.IndexOf(".") + 1);
                        result = withoutDecimals + withDecimals;
                    }
                    else
                    {
                        result = number.ToString("N0");
                        result = result.Replace(',', '.');
                    }

                }
                catch (Exception)
                {
                    result = numberstr;
                }
            }

            return result;
        }

        public static string AddCommasDouble_Tien(string numberstr)
        {
            string result = "";
            if (numberstr != null)
            {
                try
                {
                    var tmp = Convert.ToDouble(numberstr.ToString().Trim());
                    var number = Math.Round(tmp / 1000000, 3);
                    string num = number.ToString();
                    if (num.IndexOf(".") > 0)
                    {
                        string withoutDecimals = ((int)number).ToString("N0");
                        withoutDecimals = withoutDecimals.Replace(',', '.');
                        string withDecimals = "," + num.Substring(num.IndexOf(".") + 1);
                        result = withoutDecimals + withDecimals;
                    }
                    else
                    {
                        result = number.ToString("N0");
                        result = result.Replace(',', '.');
                    }

                }
                catch (Exception)
                {
                    result = numberstr;
                }
            }

            return result;
        }

        public static string AddCommas(int? number)
        {
            string result = "";
            if (number != null)
            {
                try
                {
                    result = number.Value.ToString("N0");
                }
                catch (Exception)
                {  
                    if(number != null) result = number.Value.ToString();
                }
            }
        
            return result;
        }

        public static string AddCommas(string number)
        {
            string result = "";
            if (number != null)
            {
                try
                {
                    int tmp = ConvertToInt32(number, 0);
                    result = string.Format("{0:n0}", tmp);
                }
                catch (Exception)
                {
                    result = number;
                }
            }

            return result;
        }

        public static T DeepCopy<T>(this T self)
        {
            var serialized = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static string GetTenCap(int CapID)
        {
            if (CapID == CapQuanLy.CapUBNDTinh.GetHashCode())
            {
                return "UBND Cấp Tỉnh";
            }
            else if (CapID == CapQuanLy.CapSoNganh.GetHashCode())
            {
                return "Cấp Sở, Ngành";
            }
            else if (CapID == CapQuanLy.CapUBNDHuyen.GetHashCode())
            {
                return "UBND Cấp Huyện";
            }
            else if (CapID == CapQuanLy.CapUBNDXa.GetHashCode())
            {
                return "UBND Cấp Xã";
            }
            else return "";
        }
        public static int GetThuTrongTuan(string ThuStr)
        {
            int thu = 0;
            if (ThuStr == "Monday")
            {
                thu = 1;
            }
            else if (ThuStr == "Tuesday")
            {
                thu = 2;
            }
            else if (ThuStr == "Wednesday")
            {
                thu = 3;
            }
            else if (ThuStr == "Thursday")
            {
                thu = 4;
            }
            else if (ThuStr == "Friday")
            {
                thu = 5;
            }
            else if (ThuStr == "Saturday")
            {
                thu = 6;
            }
            else if (ThuStr == "Sunday")
            {
                thu = 0;
            }
            return thu;
        }
        public static DateTime? ConvertLongToDate(long Ticks)
        {
            if (Ticks == 0)
            {
                return null;
            }
            else
            {
                try
                {
                    return new DateTime(Ticks);
                }
                catch (Exception ex)
                {
                    //return DateTime.MinValue;
                    return null;
                }
            }
        }
        public static int? ConvertTimeToInt32(TimeSpan? time)
        {
            try
            {
                return Convert.ToInt32(time.Value.ToString("hhmm"));
            }
            catch
            {
                return null;
            }
        }
        public static int ConvertTimeToInt32(DateTime time)
        {
            try
            {
                return Convert.ToInt32(time.ToString("hhmm"));
            }
            catch
            {
                return 0;
            }
        }

        public static TimeSpan? ConvertInt32ToTime(int? variable)
        {
            if (variable != null)
            {
                try
                {
                    int hour = variable.Value / 100;
                    int minute = variable.Value % 100;
                    return new TimeSpan(hour, minute, 0);
                }
                catch
                {
                    //return new TimeSpan(0, 0, 0);
                    return null;
                }
            }
            else return null;
        }

        public static TimeSpan? ConvertDatetimeToTimeSpan(DateTime variable)
        {
            if (variable != DateTime.MinValue && variable != DateTime.MaxValue)
            {
                try
                {
                    return variable.TimeOfDay;
                }
                catch
                {
                    //return new TimeSpan(0, 0, 0);
                    return null;
                }
            }
            else return null;
        }

        public static double GetGMTInMS()
        {
            var unixTime = DateTime.Now.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (double)unixTime.TotalMilliseconds;
        }
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

        public static string ConvertToString(object value, string defaultValue)
        {
            try
            {
                if (value == null) return defaultValue;
                return Convert.ToString(value.ToString().Trim());
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
                if(value == null) return defaultValue;
                return Convert.ToInt32(value.ToString().Trim());
            }
            catch (Exception ex)
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

        public static double ConvertToIntDouble(object value, double defaultValue)
        {
            try
            {
                if (value is null)
                {
                    return defaultValue;
                }
                return Convert.ToDouble(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double? ConvertToNullAbleDouble(object value, double? defaultValue)
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
                return Convert.ToBoolean(value.ToString());
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
                else
                {
                    //datetime = DateTime.Parse(value.ToString("yyyy/MM/dd hh:mm:ss"));
                }
                return datetime;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ConvertToDateTime_Hour(object value, DateTime defaultValue)
        {
            try
            {
                DateTime datetime = new DateTime();
                bool result = DateTime.TryParseExact(value.ToString(), "dd/MM/yyyy hh:mm:ss", null, DateTimeStyles.None, out datetime);
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

        public static decimal ConvertToDecimal(object value, int defaultValue)
        {
            try
            {
                if (value is null)
                {
                    return defaultValue;
                }
                return Convert.ToDecimal(value.ToString().Trim());
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

        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }


        public static Guid? ConvertToGuid(object value, Guid? defaultValue)
        {
            try
            {
                return Guid.Parse(value.ToString().Trim());
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>
        /// Bacth: Hash file content into string
        /// for example: 1e5e4212f86d8ecbe5aKNTC956c97fa373
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
        /// for example: [1e]-[5e]-[42]-[1e5e4212f86d8ecbe5aKNTC956c97fa373]
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
            return toReturn; // for example: [1e]-[5e]-[42]-[1e5e4212f86d8ecbe5aKNTC956c97fa373]
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
            try
            {
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
            catch
            {

                return valueIfNull;
            }

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

        public static float GetFloat(object value, long valueIfNull)
        {
            try
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
                if (String.IsNullOrEmpty(value.ToString()))
                    return 0;
                return float.Parse(value.ToString());
            }
            catch
            {
                return valueIfNull;
            }

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

        public static double GetDouble(object value, double valueIfNull)
        {
            value = GetObject(value, null);
            if (value == null)
            {
                return valueIfNull;
            }
            if (value is double)
            {
                return (double)value;
            }
            return double.Parse(value.ToString());
        }

        //public static int GetidFromQueryString(string key, int valueIfNull)
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

        public static int idFromString(string base64)
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

        public static string idToString(int id)
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

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
        public static string GetDateTime(DateTime datetime)
        {
            string pDate = String.Empty;
            if (datetime.Month < 10)
            {
                pDate = pDate + "0" + datetime.Month.ToString();
            }
            else
            {
                pDate = pDate + datetime.Month.ToString();
            }
            if (datetime.Day < 10)
            {
                pDate = pDate + "0" + datetime.Day.ToString();
            }
            else
            {
                pDate = pDate + datetime.Day.ToString();
            }
            string year = datetime.Year.ToString().Substring(2, 2);
            pDate = year + pDate;
            return pDate;
        }
        public static string GetMaPhieu(string pMa)
        {
            int pMaBerfor = 0;
            string pMaAfter = String.Empty;

            pMaBerfor = Utils.ConvertToInt32(pMa.Substring(pMa.Length - 6), 0) + 1;
            for (int i = 0; i < 6 - pMaBerfor.ToString().Length; i++)
            {
                pMaAfter = pMaAfter + "0";
            }
            return pMaAfter + pMaBerfor.ToString();
        }
        public static bool CheckSpecialCharacter(string Chuoi)
        {
            var val = Regex.IsMatch(Chuoi, @"[~`!@#$%^&*()-+=|\{}':;.<>/?]");
            if (val == true)
            {
                return false;
            }
            return true;
        }

        public static bool CheckPhoneNumber(string Value)
        {
            if (Regex.IsMatch(Value, @"^[0-9]*$"))
            {
                return false;
            }
            return true;

        }
        public static bool CheckCharacter(string Value)
        {
            if (Regex.IsMatch(Value, @"[A-Za-z]") || Regex.IsMatch(Value, @"[A-Za-zÀ-ȕ ]"))
            {
                return true;
            }
            return false;

        }
        public static bool CheckEmail(string Chuoi)
        {
            Chuoi = Chuoi.ToLower();
            if (Regex.IsMatch(Chuoi, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") && Regex.IsMatch(Chuoi, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z") && Regex.IsMatch(Chuoi, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*"))
            {
                return true;
            }
            return false;
        }
        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }
        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }
        public static string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                        "đ",
                                        "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                        "í","ì","ỉ","ĩ","ị",
                                        "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                        "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                        "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                        "d",
                                        "e","e","e","e","e","e","e","e","e","e","e",
                                        "i","i","i","i","i",
                                        "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                        "u","u","u","u","u","u","u","u","u","u","u",
                                        "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static DateTime? UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = DateTime.MinValue;
            try
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp);
                dateTime = dateTimeOffset.UtcDateTime;
            }
            catch (Exception)
            {
                //throw;
            }

            return dateTime;
        }

        public static String FormatDate(DateTime dt)
        {
            if (dt == DateTime.MinValue)
            {
                return String.Empty;
            }
            return dt.ToString("dd/MM/yyyy");
        }

        #region DataTable
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        #endregion
    }

    /// <summary>
    /// sử dụng để chuyển đổi từ ngày âm lịch sang ngày dương lịch
    /// </summary>
    public static class LunarYearTools
    {

        #region Các hàm tính toán chung
        const double PI = Math.PI;

        /* Discard the fractional part of a number, e.g., INT(3.2) = 3 */
        public static long INT(double d)
        {
            return (long)Math.Floor(d);
        }

        /* Compute the (integral) Julian day number of day dd/mm/yyyy, i.e., the number 
         * of days between 1/1/4713 BC (Julian calendar) and dd/mm/yyyy. 
         * Formula from http://www.tondering.dk/claus/calendar.html
         */
        public static long jdFromDate(int dd, int mm, int yy)
        {
            long a, y, m, jd;
            a = INT((14 - mm) / 12);
            y = (yy + 4800 - a);
            m = (mm + 12 * a - 3);
            jd = dd + INT((153 * m + 2) / 5) + 365 * y + INT(y / 4) - INT(y / 100) + INT(y / 400) - 32045;
            if (jd < 2299161)
            {
                jd = dd + INT((153 * m + 2) / 5) + 365 * y + INT(y / 4) - 32083;
            }
            return jd;
        }

        /* Convert a Julian day number to day/month/year. Parameter jd is an integer */
        public static DateTime jdToDate(long jd)
        {
            long a, b, c, d, e, m, day, month, year;
            if (jd > 2299160)
            { // After 5/10/1582, Gregorian calendar
                a = jd + 32044;
                b = INT((4 * a + 3) / 146097);
                c = a - INT((b * 146097) / 4);
            }
            else
            {
                b = 0;
                c = jd + 32082;
            }
            d = INT((4 * c + 3) / 1461);
            e = c - INT((1461 * d) / 4);
            m = INT((5 * e + 2) / 153);
            day = e - INT((153 * m + 2) / 5) + 1;
            month = m + 3 - 12 * INT(m / 10);
            year = b * 100 + d - 4800 + INT(m / 10);
            return new DateTime((int)year, (int)month, (int)day);
        }

        /* Compute the time of the k-th new moon after the new moon of 1/1/1900 13:52 UCT 
         * (measured as the number of days since 1/1/4713 BC noon UCT, e.g., 2451545.125 is 1/1/2000 15:00 UTC).
         * Returns a floating number, e.g., 2415079.9758617813 for k=2 or 2414961.935157746 for k=-2
         * Algorithm from: "Astronomical Algorithms" by Jean Meeus, 1998
         */
        public static long NewMoon(long k)
        {
            double T, T2, T3, dr, Jd1, M, Mpr, F, C1, deltat, JdNew;
            T = k / 1236.85; // Time in Julian centuries from 1900 January 0.5
            T2 = T * T;
            T3 = T2 * T;
            dr = PI / 180;
            Jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * T2 - 0.000000155 * T3;
            Jd1 = Jd1 + 0.00033 * Math.Sin((166.56 + 132.87 * T - 0.009173 * T2) * dr); // Mean new moon
            M = 359.2242 + 29.10535608 * k - 0.0000333 * T2 - 0.00000347 * T3; // Sun's mean anomaly
            Mpr = 306.0253 + 385.81691806 * k + 0.0107306 * T2 + 0.00001236 * T3; // Moon's mean anomaly
            F = 21.2964 + 390.67050646 * k - 0.0016528 * T2 - 0.00000239 * T3; // Moon's argument of latitude
            C1 = (0.1734 - 0.000393 * T) * Math.Sin(M * dr) + 0.0021 * Math.Sin(2 * dr * M);
            C1 = C1 - 0.4068 * Math.Sin(Mpr * dr) + 0.0161 * Math.Sin(dr * 2 * Mpr);
            C1 = C1 - 0.0004 * Math.Sin(dr * 3 * Mpr);
            C1 = C1 + 0.0104 * Math.Sin(dr * 2 * F) - 0.0051 * Math.Sin(dr * (M + Mpr));
            C1 = C1 - 0.0074 * Math.Sin(dr * (M - Mpr)) + 0.0004 * Math.Sin(dr * (2 * F + M));
            C1 = C1 - 0.0004 * Math.Sin(dr * (2 * F - M)) - 0.0006 * Math.Sin(dr * (2 * F + Mpr));
            C1 = C1 + 0.0010 * Math.Sin(dr * (2 * F - Mpr)) + 0.0005 * Math.Sin(dr * (2 * Mpr + M));
            if (T < -11)
            {
                deltat = 0.001 + 0.000839 * T + 0.0002261 * T2 - 0.00000845 * T3 - 0.000000081 * T * T3;
            }
            else
            {
                deltat = -0.000278 + 0.000265 * T + 0.000262 * T2;
            };
            JdNew = Jd1 + C1 - deltat;
            return (long)Math.Round(JdNew);
        }

        /* Compute the longitude of the sun at any time. 
         * Parameter: floating number jdn, the number of days since 1/1/4713 BC noon
         * Algorithm from: "Astronomical Algorithms" by Jean Meeus, 1998
         */
        public static double SunLongitude(double jdn)
        {
            double T, T2, dr, M, L0, DL, L;
            T = (jdn - 2451545.0) / 36525; // Time in Julian centuries from 2000-01-01 12:00:00 GMT
            T2 = T * T;
            dr = PI / 180; // degree to radian
            M = 357.52910 + 35999.05030 * T - 0.0001559 * T2 - 0.00000048 * T * T2; // mean anomaly, degree
            L0 = 280.46645 + 36000.76983 * T + 0.0003032 * T2; // mean longitude, degree
            DL = (1.914600 - 0.004817 * T - 0.000014 * T2) * Math.Sin(dr * M);
            DL = DL + (0.019993 - 0.000101 * T) * Math.Sin(dr * 2 * M) + 0.000290 * Math.Sin(dr * 3 * M);
            L = L0 + DL; // true longitude, degree
            L = L * dr;
            L = L - PI * 2 * (INT(L / (PI * 2))); // Normalize to (0, 2*PI)
            return L;
        }

        /* Compute sun position at midnight of the day with the given Julian day number. 
         * The time zone if the time difference between local time and UTC: 7.0 for UTC+7:00.
         * The function returns a number between 0 and 11. 
         * From the day after March equinox and the 1st major term after March equinox, 0 is returned. 
         * After that, return 1, 2, 3 ... 
         */
        public static long getSunLongitude(long dayNumber, int timeZone)
        {
            return INT(SunLongitude(dayNumber - 0.5 - timeZone / 24) / PI * 6);
        }

        /* Compute the day of the k-th new moon in the given time zone.
         * The time zone if the time difference between local time and UTC: 7.0 for UTC+7:00
         */
        public static long getNewMoonDay(long k, int timeZone)
        {
            return INT(NewMoon(k) + 0.5 + timeZone / 24);
        }

        /* Find the day that starts the luner month 11 of the given year for the given time zone */
        public static long getLunarMonth11(int yy, int timeZone)
        {
            long k, off, nm, sunLong;
            //off = jdFromDate(31, 12, yy) - 2415021.076998695;
            off = jdFromDate(31, 12, yy) - 2415021;
            k = INT(off / 29.530588853);
            nm = getNewMoonDay(k, timeZone);
            sunLong = getSunLongitude(nm, timeZone); // sun longitude at local midnight
            if (sunLong >= 9)
            {
                nm = getNewMoonDay(k - 1, timeZone);
            }
            return nm;
        }

        /* Find the index of the leap month after the month starting on the day a11. */
        public static long getLeapMonthOffset(long a11, int timeZone)
        {
            long k, last, arc, i;
            k = INT((a11 - 2415021.076998695) / 29.530588853 + 0.5);
            last = 0;
            i = 1; // We start with the month following lunar month 11
            arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
            do
            {
                last = arc;
                i++;
                arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
            } while (arc != last && i < 14);
            return i - 1;
        }
        #endregion

        #region Các hàm chuyển đổi
        /* Convert solar date dd/mm/yyyy to the corresponding lunar date */
        public static LunarDate SolarToLunar(DateTime date)
        {
            return SolarToLunar(date, 7);
        }

        public static LunarDate SolarToLunar(DateTime date, int timeZone)
        {
            long k, dayNumber, monthStart, a11, b11, lunarDay, lunarMonth, lunarYear, diff, leapMonthDiff;
            bool lunarLeap;

            dayNumber = LunarYearTools.jdFromDate(date.Day, date.Month, date.Year);
            k = LunarYearTools.INT((dayNumber - 2415021.076998695) / 29.530588853);
            monthStart = LunarYearTools.getNewMoonDay(k + 1, timeZone);
            if (monthStart > dayNumber)
            {
                monthStart = LunarYearTools.getNewMoonDay(k, timeZone);
            }
            // alert(dayNumber+" -> "+monthStart);
            a11 = LunarYearTools.getLunarMonth11(date.Year, timeZone);
            b11 = a11;
            if (a11 >= monthStart)
            {
                lunarYear = date.Year;
                a11 = LunarYearTools.getLunarMonth11(date.Year - 1, timeZone);
            }
            else
            {
                lunarYear = date.Year + 1;
                b11 = LunarYearTools.getLunarMonth11(date.Year + 1, timeZone);
            }
            lunarDay = dayNumber - monthStart + 1;
            diff = LunarYearTools.INT((monthStart - a11) / 29);
            lunarLeap = false;
            lunarMonth = diff + 11;
            if (b11 - a11 > 365)
            {
                leapMonthDiff = LunarYearTools.getLeapMonthOffset(a11, timeZone);
                if (diff >= leapMonthDiff)
                {
                    lunarMonth = diff + 10;
                    if (diff == leapMonthDiff)
                    {
                        lunarLeap = true;
                    }
                }
            }
            if (lunarMonth > 12)
            {
                lunarMonth = lunarMonth - 12;
            }
            if (lunarMonth >= 11 && diff < 4)
            {
                lunarYear -= 1;
            }
            return new LunarDate((int)lunarDay, (int)lunarMonth, (int)lunarYear, lunarLeap);
        }

        /* Convert a lunar date to the corresponding solar date */
        public static DateTime LunarToSolar(LunarDate ld)
        {
            return LunarToSolar(ld, 7);
        }

        public static DateTime LunarToSolar(LunarDate ld, int timeZone)
        {
            long k, a11, b11, off, leapOff, leapMonth, monthStart;
            if (ld.Month < 11)
            {
                a11 = LunarYearTools.getLunarMonth11(ld.Year - 1, timeZone);
                b11 = LunarYearTools.getLunarMonth11(ld.Year, timeZone);
            }
            else
            {
                a11 = LunarYearTools.getLunarMonth11(ld.Year, timeZone);
                b11 = LunarYearTools.getLunarMonth11(ld.Year + 1, timeZone);
            }
            k = LunarYearTools.INT(0.5 + (a11 - 2415021.076998695) / 29.530588853);
            off = ld.Month - 11;
            if (off < 0)
            {
                off += 12;
            }
            if (b11 - a11 > 365)
            {
                leapOff = LunarYearTools.getLeapMonthOffset(a11, timeZone);
                leapMonth = leapOff - 2;
                if (leapMonth < 0)
                {
                    leapMonth += 12;
                }
                if (ld.IsLeapYear && ld.Month != leapMonth)
                {
                    return DateTime.MinValue;
                }
                else if (ld.IsLeapYear || off >= leapOff)
                {
                    off += 1;
                }
            }
            monthStart = LunarYearTools.getNewMoonDay(k + off, timeZone);
            return LunarYearTools.jdToDate(monthStart + ld.Day - 1);
        }
        #endregion


       
    }

    public class LunarDate
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLeapYear { get; set; }

        public LunarDate() { }

        public LunarDate(int day, int month, int year, bool leap)
        {
            Day = day;
            Month = month;
            Year = year;
            IsLeapYear = leap;
        }

        bool checkYear(int year)
        {
            // Nếu số năm chia hết cho 400,
            // đó là 1 năm nhuận
            if (year % 400 == 0)
                return true;

            // Nếu số năm chia hết cho 4 và không chia hết cho 100,
            // đó không là 1 năm nhuận
            if (year % 4 == 0 && year % 100 != 0)
                return true;

            // trường hợp còn lại 
            // không phải năm nhuận
            return false;
        }

        public LunarDate(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
            IsLeapYear = checkYear(year);
        }

        public override string ToString()
        {
            return Day.ToString() + "/" + Month.ToString() + "/" + Year.ToString() + (IsLeapYear ? "N" : "");
        }

        public DateTime ToSolarDate(int timeZone)
        {
            return LunarYearTools.LunarToSolar(this);
        }

        public DateTime ToSolarDate()
        {
            return ToSolarDate(7);
        }
    }
}
