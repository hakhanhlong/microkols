using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class Extensions
    {
        public static string ToSlug(this string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return string.Empty;
            string str = Helpers.StringHelper.UnicodeUnSign(phrase).RemoveDiacritics().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 500 ? str.Length : 500).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }
        public static string RemoveDiacritics(this string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        }

        public static string ToTimeAgo(this DateTime timeInput)
        {
            string result = "";

            DateTime startDate = DateTime.Now;
            TimeSpan deltaMinutes = startDate.Subtract(timeInput);

            double minutes = deltaMinutes.TotalMinutes;
            //int mi = (minutes);
            if (minutes < 1)
            {
                result = "vài giây trước";
            }
            else if (minutes < 50)
            {
                result = Math.Round(new decimal(minutes)) + " phút trước";
            }
            //else if (minutes < 90)
            //{
            //    result = "một giờ trước";
            //}
            else if (minutes < 1080)
            {
                result = Math.Round(new decimal((minutes / 60))) + " giờ trước";
            }
            //else if (minutes < 1440)
            //{
            //    result = "một ngày trước";
            //}
            //else if (minutes < 2880)
            //{
            //    result = "about one day";
            //}
            else
            {
                result = Math.Round(new decimal((minutes / 1440))) + " ngày trước";
            }

            return result;

        }

       
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }



        public static List<int> ToListInt(this string str, char c = '|')
        {

            var result = new List<int>();

            foreach (var item in str.Split(c))
            {
                if (!string.IsNullOrEmpty(item))
                {

                    var tmp = 0;
                    if (int.TryParse(item, out tmp))
                    {
                        result.Add(tmp);
                    }
                }
            }

            return result;
        }
        public static string ToListInt(this List<int> list, char c = '|')
        {
            var result = string.Empty;

            if (list != null && list.Count > 0)
            {
                list = list.Distinct().ToList();
                foreach (int s in list)
                {
                    result += c + s.ToString();
                }
            }
            return result.Trim(c);
        }

        public static string ToListString(this List<string> list, char c = '|')
        {
            var result = string.Empty;

            if (list != null && list.Count > 0)
            {
                list = list.Distinct().ToList();
                foreach (string s in list)
                {
                    result += c + s.Trim();
                }

            }



            return result.Trim(c);
        }


        public static List<string> ToListString(this string input, char c = '|')
        {
            var result = new List<string>();
            foreach (var item in input.Split(c))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
