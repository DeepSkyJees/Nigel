using System;
using System.IO;

namespace Nigel.QrCode.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image");
            var path = Path.Combine(savePath, "dou.png"); //size=595*842
            ImageHelper.ImageMaxCutByCenter(path, $"{savePath}/New/dou1.png", 1024, 768, 75); //size=1024*768
            ImageHelper.ImageMaxCutByCenter(path, $"{savePath}/New/dou2.png", 768, 1024, 75); //size=768*1024
            ImageHelper.ImageScalingToRange(path, $"{savePath}/New/dou3.png", 1024, 768, 75); //size=542*768
            ImageHelper.ImageScalingToRange(path, $"{savePath}/New/dou4.png", 768, 1024, 75); //size=724*1024
            ImageHelper.ImageScalingByOversized(path, $"{savePath}/New/dou5.png", 640, 320, 75); //size=226*320
            ImageHelper.ImageScalingByOversized(path, $"{savePath}/New/dou6.png", 320, 640, 75); //size=320*453

            var qrCodeSavePath = Path.Combine(savePath, "New/hello.png");
            var qrCodeLogoPath = Path.Combine(savePath, "logo.png");
            var qrCodeWhiteBorderPixelVal = 5;
            var qrCodeText = "Hello Friend";
            ImageHelper.QRCoder(qrCodeText, qrCodeSavePath, qrCodeLogoPath, qrCodeWhiteBorderPixelVal);
            var result = ImageHelper.QRDecoder(qrCodeSavePath);
            Console.WriteLine(result);

            var imageInfo = ImageHelper.GetImageInfo(qrCodeLogoPath);
            Console.WriteLine(
                $"WIDTH={imageInfo.Item1}&HEIGHT={imageInfo.Item2}&LENGTH={imageInfo.Item3}&FORMAT={imageInfo.Item4}");
        }
    }
}