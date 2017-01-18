using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace DMS_M306.Helpers
{
    public static class QRCodeHelper
    {
        public static IHtmlString GenerateQrCode(this HtmlHelper html, string url, string alt = "QR code", int size = 500, int margin = 0)
        {
            var qrWriter = new BarcodeWriter();
            qrWriter.Format = BarcodeFormat.QR_CODE;
            qrWriter.Options = new EncodingOptions() { Height = size, Width = size, Margin = margin };

            using (var q = qrWriter.Write(url))
            {
                using (var ms = new MemoryStream())
                {
                    q.Save(ms, ImageFormat.Png);
                    var img = new TagBuilder("img");
                    img.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
                    img.Attributes.Add("alt", alt);
                    return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
                }
            }
        }

        public static IHtmlString GenerateFileCode(this HtmlHelper html, int fileId)
        {
            string releaseDeafault = "00000000";
            var hexString = fileId.ToString("X6");
            string qrString = hexString + "-" + releaseDeafault;
            return GenerateQrCode(html, qrString, "DMS-File-QR", 400, 1);
        }

        public static IHtmlString GenerateQrFromString(this HtmlHelper html, string stringForQr)
        {
            return GenerateQrCode(html, stringForQr, "DMS-File-QR", 400, 1);
        }
    }
}