using Ionic.Zip;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;

public static class Utilities
{

    public static string GeneratePassword(int length) //length of salt    
    {
        const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        var randNum = new Random();
        var chars = new char[length];
        var allowedCharCount = allowedChars.Length;
        int rnd;
        for (var i = 0; i <= length - 1; i++)
        {
            rnd = randNum.Next(0, 59);
            chars[i] = allowedChars[rnd];
        }
        return new string(chars);
    }
    public static string EncodePassword(string pass, string salt) //encrypt password    
    {
        byte[] bytes = Encoding.Unicode.GetBytes(pass);
        byte[] src = Encoding.Unicode.GetBytes(salt);
        byte[] dst = new byte[src.Length + bytes.Length];
        System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
        System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
        HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
        byte[] inArray = algorithm.ComputeHash(dst);
        //return Convert.ToBase64String(inArray);    
        return EncodePasswordMd5(Convert.ToBase64String(inArray));
    }
    public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
    {
        Byte[] originalBytes;
        Byte[] encodedBytes;
        MD5 md5;
        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
        md5 = new MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(pass);
        encodedBytes = md5.ComputeHash(originalBytes);
        //Convert encoded bytes back to a 'readable' string    
        return BitConverter.ToString(encodedBytes);
    }

    public static string EncryptString(this string plainText, string key = "#+Samoosh-!Group-@+Generate$Key+#")
    {
        // Instantiate a new Aes object to perform string symmetric encryption
        Aes encryptor = Aes.Create();

        encryptor.Mode = CipherMode.CBC;

        // Set key and IV
        byte[] aesKey = new byte[32];
        byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        SHA256 mySHA256 = SHA256Managed.Create();

        byte[] passBytes = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(key));
        Array.Copy(passBytes, 0, aesKey, 0, 32);
        encryptor.Key = aesKey;
        encryptor.IV = iv;

        // Instantiate a new MemoryStream object to contain the encrypted bytes
        MemoryStream memoryStream = new MemoryStream();

        // Instantiate a new encryptor from our Aes object
        ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

        // Instantiate a new CryptoStream object to process the data and write it to the 
        // memory stream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

        // Convert the plainText string into a byte array
        byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

        // Encrypt the input plaintext string
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);

        // Complete the encryption process
        cryptoStream.FlushFinalBlock();

        // Convert the encrypted data from a MemoryStream to a byte array
        byte[] cipherBytes = memoryStream.ToArray();

        // Close both the MemoryStream and the CryptoStream
        memoryStream.Close();
        cryptoStream.Close();

        // Convert the encrypted byte array to a base64 encoded string
        string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

        // Return the encrypted data as a string
        return cipherText;
    }

    public static string DecryptString(this string cipherText, string key = "#+Samoosh-!Group-@+Generate$Key+#")
    {
        // Instantiate a new Aes object to perform string symmetric encryption
        Aes encryptor = Aes.Create();

        encryptor.Mode = CipherMode.CBC;
        SHA256 mySHA256 = SHA256Managed.Create();
        // Set key and IV
        byte[] aesKey = new byte[32];
        byte[] passBytes = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(key));
        Array.Copy(passBytes, 0, aesKey, 0, 32);
        byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        encryptor.Key = aesKey;
        encryptor.IV = iv;

        // Instantiate a new MemoryStream object to contain the encrypted bytes
        MemoryStream memoryStream = new MemoryStream();

        // Instantiate a new encryptor from our Aes object
        ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

        // Instantiate a new CryptoStream object to process the data and write it to the 
        // memory stream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

        // Will contain decrypted plaintext
        string plainText = String.Empty;

        try
        {
            // Convert the ciphertext string into a byte array
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Decrypt the input ciphertext string
            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

            // Complete the decryption process
            cryptoStream.FlushFinalBlock();

            // Convert the decrypted data from a MemoryStream to a byte array
            byte[] plainBytes = memoryStream.ToArray();

            // Convert the decrypted byte array to string
            plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
        }
        finally
        {
            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();
        }

        // Return the decrypted data as a string
        return plainText;
    }

    public static int RandomNumber(this int s, int min, int max)
    {

        Random random = new Random();
        return random.Next(min, max);
    }

    public static int ZipFile(this string path, string fileName)
    {
        try
        {
            using (ZipFile zip = new ZipFile())
            {
                string pass = fileName.EncryptString("#+Samoosh-!Group-@+GenerateBpas+#");
                zip.Password = "@#" + pass + "#@";
                string Filepath = path + fileName + ".bak";
                zip.AddFile(Filepath, "BackupFile");
                zip.Save(path + fileName + ".zip");
                if (System.IO.File.Exists(path + fileName + ".bak"))
                {
                    System.IO.File.Delete(path + fileName + ".bak");
                }
            }
            return 1;
        }
        catch
        {
            return 0;
        }

    }
    public static string HesabFormatation(this string str)
    {
        int Code = int.Parse(str);
        return Code.ToString("D10");
    }

    public static string GetErorr(this ModelStateDictionary modelstate)
    {
        return string.Join("<br />", (from item in modelstate
                                      where item.Value.Errors.Any()
                                      select item.Value.Errors[0].ErrorMessage).ToList());
    }

    public static short GetFId()
    {
        return 0;
    }
    public static decimal ConvertToPrice(this string str)
    {
        if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
        {
            string[] part = str.Split(',');
            string price = "";
            for (int i = 0; i < part.Length; i++)
            {
                price += part[i];
            }
            return Convert.ToDecimal(price);
        }
        else
        {
            return Convert.ToDecimal("0");
        }



    }

    public static string GetRecevieDescription(this string str, int DesType, string chequeNum = "", string ReceiptNum = "", string Date = "")
    {
        string Des = "";
        switch (DesType)
        {
            case 1:
                Des = "دریافت نقدی از" + str;
                break;
            case 2:
                Des = "دریافت طی فیش بانکی به شماره" + ReceiptNum + "از" + str;
                break;
            case 3:
                Des = "دریافت چک به شماره" + chequeNum + "سررسید" + Date + "از" + str;
                break;
            case 4:
                Des = "دریافت وجه / چک از" + str;
                break;
        };
        return Des;
    }

    public static string GetFiscalYearDescription(this string str, int DesType, string Type)
    {
        string Des = "";
        switch (DesType)
        {
            case 1:
                Des = "بستن حساب" + " " + Type + "/" + str;
                break;
            case 2:
                Des = "باز کردن حساب" + " " + Type + "/" + str;
                break;
            case 3:
                Des = "بستن حساب" + " " + str;
                break;
            case 4:
                Des = "باز کردن حساب" + " " + str;
                break;
            case 5:
                Des = "توازن حساب های دریافتنی و پرداختنی اشخاص";
                break;
            case 6:
                Des = "بستن حساب سرمایه اولیه";
                break;
            case 7:
                Des = "باز کردن حساب سرمایه اولیه";
                break;

            case 8:
                Des = "بستن حساب دائم";
                break;
            case 9:
                Des = "سند افتتاحیه";
                break;

        };
        return Des;
    }

    public static bool Contain(this string str, int match)
    {
        try
        {
            string[] div = str.Split(',');
            bool e = false;
            for (int i = 0; i < div.Length; i++)
            {
                int a = int.Parse(div[0]);
                if (a == match)
                {
                    e = true;
                    break;
                }
            }
            return e;
        }
        catch
        {
            return false;
        }
    }
    public static string GetPaymentDescription(this string str, int DesType, string chequeNum = "", string ReceiptNum = "", string Date = "", string name = "")
    {
        string Des = "";
        switch (DesType)
        {
            case 1:
                Des = " پرداخت نقدی به" + " " + str;
                break;
            case 2:
                Des = "پرداخت نقدی بابت" + " " + str;
                break;
            case 3:
                Des = "پرداخت طی فیش بانکی به شماره" + " " + ReceiptNum + "به" + " " + str;
                break;
            case 4:
                Des = "پرداخت طی فیش بانکی به شماره" + " " + ReceiptNum + "بابت" + " " + str;
                break;
            case 5:
                Des = "پرداخت چک به شماره" + " " + chequeNum + " " + "سررسید" + " " + Date + " " + "به" + " " + str;
                break;
            case 6:
                Des = "پرداخت چک به شماره" + " " + chequeNum + " " + "سررسید" + " " + Date + " " + "بابت" + " " + str;
                break;
            case 7:
                Des = "پرداخت وجه / چک به " + " " + str;
                break;
            case 8:
                Des = " پرداخت وجه / چک بابت" + " " + str;
                break;
            case 9:
                Des = " خرج چک دریافتی از " + " " + name + " " + " به شماره" + " " + chequeNum + " " + "سررسید" + " " + Date + " " + "به" + " " + str;
                break;
            case 10:
                Des = " خرج چک دریافتی از" + " " + name + " " + " به شماره" + " " + chequeNum + " " + "سررسید" + " " + Date + " " + "بابت" + " " + str;
                break;
        };
        return Des;
    }

    public static string GetTransferDescription(this string str, string ToName, string FromIssue, string ToIssue)
    {
        string Des = "";

        if (!string.IsNullOrEmpty(FromIssue) && !string.IsNullOrEmpty(ToIssue))
        {
            Des = "انتقال وجه" + " " + str + " " + "به" + " " + ToName + " " + "(شماره ارجاع برداشت :" + " " + FromIssue + " " + ")" + "(شماره ارجاع واریز :" + " " + ToIssue + " " + ")";
        }
        else if (!string.IsNullOrEmpty(ToIssue))
        {
            Des = "انتقال وجه" + " " + str + " " + "به" + " " + ToName + " " + "(شماره ارجاع واریز :" + " " + ToIssue + " " + ")";
        }
        else if (!string.IsNullOrEmpty(FromIssue))
        {

            Des = "انتقال وجه" + " " + str + " " + "به" + " " + ToName + " " + "(شماره ارجاع برداشت :" + " " + FromIssue + " " + ")";
        }
        else
        {
            Des = "انتقال وجه" + " " + str + " " + "به" + " " + ToName + " ";
        }
        return Des;
    }
    public static void ResizeImage(this Stream inputStream, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image img = new Bitmap(inputStream);
        int width = img.Width;
        int height = img.Height;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImage(this System.Drawing.Image img, int width, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByWidth(this Stream inputStream, int width, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image img = new Bitmap(inputStream);
        int height = img.Height * width / img.Width;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByWidth(this System.Drawing.Image img, int width, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        int height = img.Height * width / img.Width;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByHeight(this Stream inputStream, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        System.Drawing.Image img = new Bitmap(inputStream);
        int width = img.Width * height / img.Height;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void ResizeImageByHeight(this System.Drawing.Image img, int height, string savePath, ImageComperssion ic = ImageComperssion.Normal)
    {
        int width = img.Width * height / img.Height;
        System.Drawing.Image result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawImage(img, 0, 0, width, height);
        }
        result.CompressImage(savePath, ic);
    }

    public static void CompressImage(this System.Drawing.Image img, string path, ImageComperssion ic)
    {
        System.Drawing.Imaging.EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt32(ic));
        ImageFormat format = img.RawFormat;
        ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);
        string mimeType = codec == null ? "image/jpeg" : codec.MimeType;
        ImageCodecInfo jpegCodec = null;
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
            {
                jpegCodec = codecs[i];
                break;
            }
        }
        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;
        img.Save(path, jpegCodec, encoderParams);
    }

    public enum ImageComperssion
    {
        Maximum = 50,
        Good = 60,
        Normal = 70,
        Fast = 80,
        Minimum = 90,
    }

    public static int UploadImage(
        this HttpPostedFileBase file,
        string Root,
        string variable,
        ref string fileName,
        int size,
        bool checkType = true)
    {
        try
        {
            if (file != null && file.ContentLength > 0)
            {
                if (file.ContentLength <= size)
                {
                    string fileType = file.ContentType.Split('/')[1].ToLower();

                    bool fileTypeIsOk = checkType ? fileType == "jpeg" || fileType == "jpg" || fileType == "png" : true;

                    if (fileTypeIsOk)
                    {
                        string fName = file.FileName.ImageName(variable);
                        fileName = fName;
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Content\\Upload", AppDomain.CurrentDomain.BaseDirectory));
                        string pathString = Path.Combine(originalDirectory.ToString(), Root);

                        if (!Directory.Exists(pathString))
                            Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, fName);
                        file.SaveAs(path);
                        return 1;
                    }
                    else
                    {
                        //error file type
                        return -2;
                    }
                }
                else
                {
                    //error file size
                    return -1;
                }
            }
            else
            {
                // error not exist
                return 0;
            }
        }
        catch
        {
            return 0;
        }
    }


    public static int deleteImage(this string str, string Root)
    {
        try
        {
            var originalDirectory = new DirectoryInfo(string.Format("{0}Content\\Upload", AppDomain.CurrentDomain.BaseDirectory));
            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), Root);
            var path = string.Format("{0}\\{1}", pathString, str);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        catch
        {
            return 0;
        }

    }

    public static string UploadMessage(this int flag)
    {
        string message = "";

        if (flag == -1)
        {
            message = "عکس انتخابی از لحاظ  سایز مورد قبول نمی باشد";
        }
        else if (flag == -2)
        {
            message = "عکس انتخابی از لحاظ فرمت قابل قبول نمی باشد";
        }
        else
        {
            message = "عکس انتخاب نشده است";
        }
        return message;
    }

    public static string ImageName(this string fName, string SectionId)
    {
        string[] name = fName.Split('.');
        string format = name[name.Length - 1];
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long aaa = (long)(DateTime.UtcNow - epoch).TotalMilliseconds;
        return SectionId.ToString() + "_" + aaa.ToString() + "." + format;

    }

    public static string ToPrice(this object dec)
    {
        string Src = dec.ToString();
        Src = Src.Replace(".0000", "");
        if (!Src.Contains("."))
        {
            Src = Src + ".00";
        }
        string[] price = Src.Split('.');
        if (price[1].Length >= 2)
        {
            price[1] = price[1].Substring(0, 2);
            price[1] = price[1].Replace("00", "");
        }

        string Temp = null;
        int i = 0;
        if ((price[0].Length % 3) >= 1)
        {
            Temp = price[0].Substring(0, (price[0].Length % 3));
            for (i = 0; i <= (price[0].Length / 3) - 1; i++)
            {
                Temp += "," + price[0].Substring((price[0].Length % 3) + (i * 3), 3);
            }
        }
        else
        {
            for (i = 0; i <= (price[0].Length / 3) - 1; i++)
            {
                Temp += price[0].Substring((price[0].Length % 3) + (i * 3), 3) + ",";
            }
            Temp = Temp.Substring(0, Temp.Length - 1);
            // Temp = price(0)
            //If price(1).Length > 0 Then
            //    Return price(0) + "." + price(1)
            //End If
        }
        if (price[1].Length > 0)
        {
            return Temp + "." + price[1];
        }
        else
        {
            return Temp;
        }
    }
    public static string ToStrUnit(this int id)
    {
        gsharing_DamliEntities db = new gsharing_DamliEntities();
        var select = db.Units.Where(x => x.Id == id).FirstOrDefault();
        //var select = _blUnit.Where(x => x.Id == 6);
        string Name = select.Name;
        return Name;
    }

    public static PersianDateTime ToPersianDateTime(this DateTime DT)
    {
        return new PersianDateTime(DT);
    }

    public static DateTime ToMiladiDate(this string date)
    {
        string[] dt = date.Split('/');
        PersianCalendar pc = new PersianCalendar();
        DateTime Date = pc.ToDateTime(int.Parse(dt[0]), int.Parse(dt[1]), int.Parse(dt[2]), 0, 0, 0, 0);
        return Date;

    }

    public static DateTime GetStartDate(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
    }

    public static DateTime GetEndDate(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
    }

    public static PersianDateTime ToPersianDateTime(this string text)
    {
        return new PersianDateTime(Convert.ToDateTime(text));
    }

    public static string GetJaveScriptDate(this string date)
    {
        string[] d = date.Split('/');
        int Month = int.Parse(d[1]) - 1;
        return d[0] + "/" + Month.ToString() + "/" + d[2];
    }
    public static string ToStringDateTime12(this PersianDateTime DT)
    {
        return DT.ToString("yyyy/MM/dd hh:mm tt");
    }

    public static string ToStringDateTime24(this PersianDateTime DT)
    {
        return DT.ToString("yyyy/MM/dd HH:mm");
    }

    public static string ToStringShamsiDate(this PersianDateTime DT)
    {
        string tt = DT.Hour < 12 ? "قبل از ظهر" : "بعد از ظهر";
        return DT.ToString("d MMMM yyyy در h:mm " + tt);
    }

    public static string ToStringDate(this PersianDateTime DT)
    {
        return DT.ToString("yyyy/MM/dd");
    }

    public static string ToStringDateTime12P(this PersianDateTime DT)
    {
        //return DT.ToString("yyyy/MM/dd hh:mm tt").Replace("AM", "ق.ظ").Replace("PM", "ب.ظ");
        string hh = DT.ToString("HH");
        string tt = Convert.ToInt32(hh) < 12 ? "ق.ظ" : "ب.ظ";
        return DT.ToString("yyyy/M/d h:mm " + tt);
    }

    public static string ToStringShamsiDate(this DateTime dt)
    {
        System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();
        int intYear = PC.GetYear(dt);
        int intMonth = PC.GetMonth(dt);
        int intDayOfMonth = PC.GetDayOfMonth(dt);
        DayOfWeek enDayOfWeek = PC.GetDayOfWeek(dt);
        string strMonthName = "";
        string strDayName = "";
        switch (intMonth)
        {
            case 1:
                strMonthName = "فروردین";
                break;
            case 2:
                strMonthName = "اردیبهشت";
                break;
            case 3:
                strMonthName = "خرداد";
                break;
            case 4:
                strMonthName = "تیر";
                break;
            case 5:
                strMonthName = "مرداد";
                break;
            case 6:
                strMonthName = "شهریور";
                break;
            case 7:
                strMonthName = "مهر";
                break;
            case 8:
                strMonthName = "آبان";
                break;
            case 9:
                strMonthName = "آذر";
                break;
            case 10:
                strMonthName = "دی";
                break;
            case 11:
                strMonthName = "بهمن";
                break;
            case 12:
                strMonthName = "اسفند";
                break;
            default:
                strMonthName = "";
                break;
        }


        return (string.Format("{0} {1} {2} {3}", strDayName, intDayOfMonth, strMonthName, intYear));
    }

    public static string GetDescription<T>(this T enumerationValue)
        where T : struct
    {
        Type type = enumerationValue.GetType();
        if (!type.IsEnum)
        {
            throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
        }

        //Tries to find a DescriptionAttribute for a potential friendly name
        //for the enum
        MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
        if (memberInfo.Length > 0)
        {
            object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs.Length > 0)
            {
                //Pull out the description value
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }
        //If we have no description attribute, just return the ToString of the enum
        return enumerationValue.ToString();
    }

    public static bool IsUsedInDb(Exception ex)
    {
        SqlException sqlException = Utilities.GetSqlException(ex);
        if (sqlException != null)
        {
            var exceptionNumber = sqlException.Number;
            if (exceptionNumber == 547)
            {
                return true;
            }
        }

        return false;
    }

    private static SqlException GetSqlException(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            return sqlException;
        }
        else if (exception.InnerException != null)
        {
            return GetSqlException(exception.InnerException);
        }
        else
        {
            return null;
        }
    }
}
