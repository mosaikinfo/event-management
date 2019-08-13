using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EventManagement.Shared.Mvc
{
    /// <summary>
    /// Download a QR Code as image file.
    /// </summary>
    public class QrCodeResult : FileContentResult
    {
        /// <summary>
        /// Download a QR Code as image file.
        /// </summary>
        /// <param name="qrCodeValue">The value to store within the qr code.</param>
        public QrCodeResult(string qrCodeValue) 
            : base(CreateQrCode(qrCodeValue), "image/png")
        {
        }

        private static byte[] CreateQrCode(string qrCodeValue)
        {
            var generator = new QRCodeGenerator();
            var qrCodeData = generator.CreateQrCode(qrCodeValue, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            Bitmap bitmap = qrCode.GetGraphic(3);
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }
    }
}
