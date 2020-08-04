using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;

namespace PunasMarketing.Helpers.Utilities
{
    public static class Notification
    {
        private static string stringValueOf(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
        private static string booToLowerString(this bool value)
        {
            return value.ToString().ToLower();
        }



        public static string Show(string message, string title = "",
            ToastType type = ToastType.Info,
            ToastPosition position = ToastPosition.TopRight,
            int timeOut = 6000,
            bool closeButton = true,
            bool progressBar = true,
            bool newestOnTop = true,
            string onclick = null)
        {
            var scriptOption = "<script>";
            scriptOption += "toastr.options = {";

            scriptOption += "'closeButton': '" + closeButton.booToLowerString() + "','debug': false,'newestOnTop': " + newestOnTop.booToLowerString() + ",'progressBar': " + progressBar.booToLowerString() + ",'positionClass': '" + stringValueOf(position) + "','preventDuplicates': false,'onclick': " + (onclick ?? "null") + ",'showDuration': '300','hideDuration': '1000','timeOut': '" + timeOut + "','extendedTimeOut': '1000','showEasing': 'swing','hideEasing': 'linear','showMethod': 'fadeIn','hideMethod': 'fadeOut'";
            scriptOption += "};";
            scriptOption += "toastr['" + stringValueOf(type) + "']('" + message + "', '" + title + "');</script>";

            return scriptOption;
        }


        public static string BriefShow(string title, ToastType type = ToastType.Info)
        {
            var scriptOption = title + "," + stringValueOf(type);
            return scriptOption;
        }
        public static JavaScriptResult jsonShow(string message, string title = "",
         ToastType type = ToastType.Info,
         ToastPosition position = ToastPosition.TopRight,
         int timeOut = 6000,
         bool closeButton = true,
         bool progressBar = true,
         bool newestOnTop = true,
         string onclick = null)
        {

            var scriptOption = "toastr.options = {";

            scriptOption += "'closeButton': '" + closeButton.booToLowerString() + "','debug': false,'newestOnTop': " + newestOnTop.booToLowerString() + ",'progressBar': " + progressBar.booToLowerString() + ",'positionClass': '" + stringValueOf(position) + "','preventDuplicates': false,'onclick': " + (onclick ?? "null") + ",'showDuration': '300','hideDuration': '1000','timeOut': '" + timeOut + "','extendedTimeOut': '1000','showEasing': 'swing','hideEasing': 'linear','showMethod': 'fadeIn','hideMethod': 'fadeOut'";
            scriptOption += "};";
            scriptOption += "toastr['" + stringValueOf(type) + "']('" + message + "', '" + title + "');";

            return new JavaScriptResult() { Script = scriptOption };
        }
    }

    public enum ToastPosition
    {
        [Description("toast-top-right")]
        TopRight,

        [Description("toast-bottom-righ")]
        BottomRight,

        [Description("toast-bottom-left")]
        BottomLeft,

        [Description("toast-top-left")]
        TopLeft,

        [Description("toast-top-full-width")]
        TopFullWidth,

        [Description("toast-bottom-full-width")]
        BottomFullWidth,

        [Description("toast-top-center")]
        TopCenter,

        [Description("toast-bottom-center")]
        BottomCenter
    }

    public enum ToastType
    {

        [Description("success")]
        Success,

        [Description("info")]
        Info,

        [Description("warning")]
        Warning,

        [Description("error")]
        Error,

    }

    public class JsonData
    {
        public string Script { get; set; }
        public string Html { get; set; }
        public bool Success { get; set; }
        public string Option { get; set; }
        public string Url { get; set; }
        public bool IsUsed { get; set; }
    }
}