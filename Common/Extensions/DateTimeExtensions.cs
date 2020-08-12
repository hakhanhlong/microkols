using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime date)
        {

            var timeSpan = (date - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        //public static string ToTimeAgo(this DateTime date)
        //{
        //    return StringHelpers.TimeAgo(date);
        //}


       

        public static string ToDateRange(this DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {
                return $"{start.Value.ToViDateTimeRange()} - {end.Value.ToViDateTimeRange()}";
            }
            return "";
        }
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                0,
                dateTime.Kind);
        }

        public static DateTime? ToViDate(this string input)
        {
            DateTime dt;
            if (DateTime.TryParseExact(input, "dd/MM/yyyy", null,
                                   DateTimeStyles.None,
                out dt))
            {
                //valid date
                return dt;
            }
            else
            {

                if (DateTime.TryParseExact(input, "d/M/yyyy", null,
                                   DateTimeStyles.None,
                out dt))
                {
                    //valid date
                    return dt;
                }
                //invalid date
            }
            return null;
        }
        public static string ToViDate(this DateTime date)
        {

            return string.Format("{0: dd/MM/yyyy}", date);
            //return string.Format("{0} giờ {1} phút ngày {2} tháng {3} năm {4}", date.Hour, date.Minute, date.Day, date.Month, date.Year);
        }

        public static string ToViDateTime(this DateTime date)
        {

            return string.Format("{0: HH:mm dd/MM/yyyy}", date);
            //return string.Format("{0} giờ {1} phút ngày {2} tháng {3} năm {4}", date.Hour, date.Minute, date.Day, date.Month, date.Year);
        }

        public static string ToViDateTimeRange(this DateTime date)
        {

            return string.Format("{0:hh:mm tt dd/MM/yyyy}", date);
            //return string.Format("{0} giờ {1} phút ngày {2} tháng {3} năm {4}", date.Hour, date.Minute, date.Day, date.Month, date.Year);
        }
        public static DateTime? ToViDateTime(this string input)
        {
            return DateTime.ParseExact(input, "hh:mm tt dd/MM/yyyy", CultureInfo.InvariantCulture);
          
        }

        public static DateTime? ToViDateTime2(this string input)
        {
            DateTime dt;
            DateTime.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out dt);
            return dt;

        }

    }
}
