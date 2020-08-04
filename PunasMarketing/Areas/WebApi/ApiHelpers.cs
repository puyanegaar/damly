using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;


namespace PunasMarketing.Areas.WebApi
{
    public enum EntityType
    {
        Customer = 1,
        Personnel = 2,
        Marketer = 3,
        Offer = 4,
        Product = 5
    }

    public class ApiHelpers
    {
        public static string GetImageVirtualPath(EntityType entityType, string imageName)
        {
            string virtualPath = "~/Content/Upload/Image/";
            switch (entityType)
            {
                case EntityType.Customer:
                    virtualPath += "Customers/";
                    break;

                case EntityType.Personnel:
                    virtualPath += "Personnels/";
                    break;

                case EntityType.Marketer:
                    virtualPath += "Marketers/";
                    break;

                case EntityType.Offer:
                    virtualPath += "Offers/";
                    break;

                case EntityType.Product:
                    virtualPath += "Products/";
                    break;

                default:
                    return string.Empty;
            }

            virtualPath += imageName;
            return virtualPath;
        }

        public static bool SaveImage(EntityType entityType, string base64Image, string imageName)
        {
            try
            {
                var virtualPath = GetImageVirtualPath(entityType, imageName);
                string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

                byte[] imageBytes = Convert.FromBase64String(base64Image);
                File.WriteAllBytes(physicalPath, imageBytes);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteImage(EntityType entityType, string imageName)
        {
            #region Delete previous image file

            var virtualPath = ApiHelpers.GetImageVirtualPath(entityType, imageName);
            var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

            try
            {
                File.Delete(physicalPath);
            }
            catch
            {
                // ignored
            }

            #endregion
        }
    }
}