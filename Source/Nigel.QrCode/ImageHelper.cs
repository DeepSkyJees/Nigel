using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Nigel.QrCode
{
    /// <summary>
    ///     Class ImageHelper.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        ///     The maximum length
        /// </summary>
        private static readonly long maxLength = 10485760; //10*1024*1024

        /// <summary>
        ///     Gets the image format by suffix.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns>SkiaSharp.SKEncodedImageFormat.</returns>
        public static SKEncodedImageFormat GetImageFormatBySuffix(string suffix)
        {
            var format = SKEncodedImageFormat.Jpeg;
            if (string.IsNullOrEmpty(suffix)) return format;
            if (suffix[0] == '.') suffix = suffix.Substring(1);
            if (string.IsNullOrEmpty(suffix)) return format;
            suffix = suffix.ToUpper();
            switch (suffix)
            {
                case "PNG":
                    format = SKEncodedImageFormat.Png;
                    break;
                case "GIF":
                    format = SKEncodedImageFormat.Gif;
                    break;
                case "BMP":
                    format = SKEncodedImageFormat.Bmp;
                    break;
                case "ICON":
                    format = SKEncodedImageFormat.Ico;
                    break;
                case "ICO":
                    format = SKEncodedImageFormat.Ico;
                    break;
                case "DNG":
                    format = SKEncodedImageFormat.Dng;
                    break;
                case "WBMP":
                    format = SKEncodedImageFormat.Wbmp;
                    break;
                case "WEBP":
                    format = SKEncodedImageFormat.Webp;
                    break;
                case "PKM":
                    format = SKEncodedImageFormat.Pkm;
                    break;
                case "KTX":
                    format = SKEncodedImageFormat.Ktx;
                    break;
                case "ASTC":
                    format = SKEncodedImageFormat.Astc;
                    break;
            }

            return format;
        }

        /// <summary>
        ///     Gets the image format by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>SkiaSharp.SKEncodedImageFormat.</returns>
        public static SKEncodedImageFormat GetImageFormatByPath(string path)
        {
            var suffix = "";
            if (Path.HasExtension(path)) suffix = Path.GetExtension(path);
            return GetImageFormatBySuffix(suffix);
        }

        /// <summary>
        ///     Gets the image information.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Tuple&lt;System.Int32, System.Int32, System.Int64, SkiaSharp.SKEncodedImageFormat&gt;.</returns>
        /// <exception cref="System.Exception">
        ///     路径不能为空
        ///     or
        ///     文件不存在
        ///     or
        ///     文件过大
        ///     or
        ///     文件无效
        /// </exception>
        public static Tuple<int, int, long, SKEncodedImageFormat> GetImageInfo(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new Exception("路径不能为空");
            if (!File.Exists(path)) throw new Exception("文件不存在");
            var fileStream =
                new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read); //fileInfo.OpenRead();
            var fileLength = fileStream.Length;
            if (fileLength > maxLength)
            {
                fileStream.Dispose();
                throw new Exception("文件过大");
            }

            var sKManagedStream = new SKManagedStream(fileStream, true);
            var sKBitmap = SKBitmap.Decode(sKManagedStream);
            sKManagedStream.Dispose();

            if (sKBitmap.IsEmpty)
            {
                sKBitmap.Dispose();
                throw new Exception("文件无效");
            }

            var w = sKBitmap.Width;
            var h = sKBitmap.Height;
            return new Tuple<int, int, long, SKEncodedImageFormat>(w, h, fileLength, GetImageFormatByPath(path));
        }

        /// <summary>
        ///     Images the maximum cut by center.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="savePath">The save path.</param>
        /// <param name="saveWidth">Width of the save.</param>
        /// <param name="saveHeight">Height of the save.</param>
        /// <param name="quality">The quality.</param>
        public static void ImageMaxCutByCenter(string path, string savePath, int saveWidth, int saveHeight, int quality)
        {
            var bytes = ImageMaxCutByCenter(path, saveWidth, saveHeight, quality);
            if (bytes == null || bytes.Length < 1) return;
            var saveDirPath = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(saveDirPath)) Directory.CreateDirectory(saveDirPath);
            var fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        /// <summary>
        ///     Images the maximum cut by center.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="saveWidth">Width of the save.</param>
        /// <param name="saveHeight">Height of the save.</param>
        /// <param name="quality">The quality.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ImageMaxCutByCenter(string path, int saveWidth, int saveHeight, int quality)
        {
            byte[] bytes = null;
            if (!File.Exists(path)) return bytes;
            var fileStream =
                new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read); //fileInfo.OpenRead();
            if (fileStream.Length > maxLength)
            {
                fileStream.Dispose();
                return bytes;
            }

            var sKManagedStream = new SKManagedStream(fileStream, true);
            var sKBitmap = SKBitmap.Decode(sKManagedStream);
            sKManagedStream.Dispose();

            if (sKBitmap.IsEmpty) return bytes;

            if (saveWidth < 1) saveWidth = 1;
            if (saveHeight < 1) saveHeight = 1;
            if (quality < 1) quality = 1;
            if (quality > 100) quality = 100;

            var oW = sKBitmap.Width;
            var oH = sKBitmap.Height;
            var cutW = saveWidth;
            var cutH = saveHeight;
            double ratio = 1;
            if (cutW > oW)
            {
                ratio = oW / (double) cutW;
                cutH = Convert.ToInt32(cutH * ratio);
                cutW = oW;
                if (cutH > oH)
                {
                    ratio = oH / (double) cutH;
                    cutW = Convert.ToInt32(cutW * ratio);
                    cutH = oH;
                }
            }
            else if (cutW < oW)
            {
                ratio = oW / (double) cutW;
                cutH = Convert.ToInt32(Convert.ToDouble(cutH) * ratio);
                cutW = oW;
                if (cutH > oH)
                {
                    ratio = oH / (double) cutH;
                    cutW = Convert.ToInt32(cutW * ratio);
                    cutH = oH;
                }
            }
            else
            {
                if (cutH > oH)
                {
                    ratio = oH / (double) cutH;
                    cutW = Convert.ToInt32(cutW * ratio);
                    cutH = oH;
                }
            }

            var startX = oW > cutW ? oW / 2 - cutW / 2 : cutW / 2 - oW / 2;
            var startY = oH > cutH ? oH / 2 - cutH / 2 : cutH / 2 - oH / 2;

            var sKBitmap2 = new SKBitmap(saveWidth, saveHeight);
            var sKCanvas = new SKCanvas(sKBitmap2);
            var sKPaint = new SKPaint
            {
                FilterQuality = SKFilterQuality.Medium,
                IsAntialias = true
            };
            sKCanvas.DrawBitmap(
                sKBitmap,
                new SKRect
                {
                    Location = new SKPoint {X = startX, Y = startY},
                    Size = new SKSize {Height = cutH, Width = cutW}
                },
                new SKRect
                {
                    Location = new SKPoint {X = 0, Y = 0},
                    Size = new SKSize {Height = saveHeight, Width = saveWidth}
                }, sKPaint);
            sKCanvas.Dispose();
            var sKImage2 = SKImage.FromBitmap(sKBitmap2);
            sKBitmap2.Dispose();
            var data = sKImage2.Encode(GetImageFormatByPath(path), quality);
            sKImage2.Dispose();
            bytes = data.ToArray();
            data.Dispose();

            return bytes;
        }

        /// <summary>
        ///     Images the scaling to range.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="savePath">The save path.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="quality">The quality.</param>
        public static void ImageScalingToRange(string path, string savePath, int maxWidth, int maxHeight, int quality)
        {
            var bytes = ImageScalingToRange(path, maxWidth, maxHeight, quality);
            if (bytes == null || bytes.Length < 1) return;
            var saveDirPath = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(saveDirPath)) Directory.CreateDirectory(saveDirPath);
            var fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        /// <summary>
        ///     Images the scaling to range.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="quality">The quality.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ImageScalingToRange(string path, int maxWidth, int maxHeight, int quality)
        {
            byte[] bytes = null;
            if (!File.Exists(path)) return bytes;
            var fileStream =
                new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read); //fileInfo.OpenRead();
            if (fileStream.Length > maxLength)
            {
                fileStream.Dispose();
                return bytes;
            }

            var sKManagedStream = new SKManagedStream(fileStream, true);
            var sKBitmap = SKBitmap.Decode(sKManagedStream);
            sKManagedStream.Dispose();

            if (sKBitmap.IsEmpty) return bytes;

            if (maxWidth < 1) maxWidth = 1;
            if (maxHeight < 1) maxHeight = 1;
            if (quality < 1) quality = 1;
            if (quality > 100) quality = 100;

            var oW = sKBitmap.Width;
            var oH = sKBitmap.Height;
            var nW = oW;
            var nH = oH;

            if (nW < maxWidth && nH < maxHeight) //放大
            {
                if (nW < maxWidth)
                {
                    var r = maxWidth / (double) nW;
                    nW = maxWidth;
                    nH = (int) Math.Floor(nH * r);
                }

                if (nH < maxHeight)
                {
                    var r = maxHeight / (double) nH;
                    nH = maxHeight;
                    nW = (int) Math.Floor(nW * r);
                }
            }

            //限制超出(缩小)
            if (nW > maxWidth)
            {
                var r = maxWidth / (double) nW;
                nW = maxWidth;
                nH = (int) Math.Floor(nH * r);
            }

            if (nH > maxHeight)
            {
                var r = maxHeight / (double) nH;
                nH = maxHeight;
                nW = (int) Math.Floor(nW * r);
            }


            var sKBitmap2 = new SKBitmap(nW, nH);
            var sKCanvas = new SKCanvas(sKBitmap2);
            var sKPaint = new SKPaint
            {
                FilterQuality = SKFilterQuality.Medium,
                IsAntialias = true
            };
            sKCanvas.DrawBitmap(
                sKBitmap,
                new SKRect
                {
                    Location = new SKPoint {X = 0, Y = 0},
                    Size = new SKSize {Height = oH, Width = oW}
                },
                new SKRect
                {
                    Location = new SKPoint {X = 0, Y = 0},
                    Size = new SKSize {Height = nH, Width = nW}
                }, sKPaint);
            sKCanvas.Dispose();
            var sKImage2 = SKImage.FromBitmap(sKBitmap2);
            sKBitmap2.Dispose();
            var data = sKImage2.Encode(GetImageFormatByPath(path), quality);
            sKImage2.Dispose();
            bytes = data.ToArray();
            data.Dispose();

            return bytes;
        }

        /// <summary>
        ///     Images the scaling by oversized.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="savePath">The save path.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="quality">The quality.</param>
        public static void ImageScalingByOversized(string path, string savePath, int maxWidth, int maxHeight,
            int quality)
        {
            var bytes = ImageScalingByOversized(path, maxWidth, maxHeight, quality);
            if (bytes == null || bytes.Length < 1) return;
            var saveDirPath = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(saveDirPath)) Directory.CreateDirectory(saveDirPath);
            var fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        /// <summary>
        ///     Images the scaling by oversized.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="quality">The quality.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ImageScalingByOversized(string path, int maxWidth, int maxHeight, int quality)
        {
            byte[] bytes = null;
            if (!File.Exists(path)) return bytes;
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fileStream.Length > maxLength)
            {
                fileStream.Dispose();
                return bytes;
            }

            var sKManagedStream = new SKManagedStream(fileStream, true);
            var sKBitmap = SKBitmap.Decode(sKManagedStream);
            sKManagedStream.Dispose();

            if (sKBitmap.IsEmpty) return bytes;

            if (maxWidth < 1) maxWidth = 1;
            if (maxHeight < 1) maxHeight = 1;
            if (quality < 1) quality = 1;
            if (quality > 100) quality = 100;

            var oW = sKBitmap.Width;
            var oH = sKBitmap.Height;
            var nW = oW;
            var nH = oH;

            if (oW > maxWidth || oH > maxHeight)
            {
                nW = maxWidth;
                nH = maxHeight;
                double ratio = 1;

                if (nW > 0 && nH > 0)
                {
                    ratio = (double) nW / oW;
                    nH = Convert.ToInt32(oH * ratio);
                    if (maxHeight < nH)
                    {
                        ratio = (double) maxHeight / nH;
                        nW = Convert.ToInt32(nW * ratio);
                        nH = maxHeight;
                    }
                }

                if (nW < 1 && nH < 1)
                {
                    nW = oW;
                    nH = oH;
                }

                if (nW < 1)
                {
                    ratio = (double) nH / oH;
                    nW = Convert.ToInt32(oW * ratio);
                }

                if (nH < 1)
                {
                    ratio = (double) nW / oW;
                    nH = Convert.ToInt32(oH * ratio);
                }

                var sKBitmap2 = new SKBitmap(nW, nH);
                var sKCanvas = new SKCanvas(sKBitmap2);
                var sKPaint = new SKPaint
                {
                    FilterQuality = SKFilterQuality.Medium,
                    IsAntialias = true
                };
                sKCanvas.DrawBitmap(
                    sKBitmap,
                    new SKRect
                    {
                        Location = new SKPoint {X = 0, Y = 0},
                        Size = new SKSize {Height = oH, Width = oW}
                    },
                    new SKRect
                    {
                        Location = new SKPoint {X = 0, Y = 0},
                        Size = new SKSize {Height = nH, Width = nW}
                    }, sKPaint);
                sKCanvas.Dispose();
                sKBitmap.Dispose();
                sKBitmap = sKBitmap2;
            }

            var sKImage = SKImage.FromBitmap(sKBitmap);
            sKBitmap.Dispose();
            var data = sKImage.Encode(GetImageFormatByPath(path), quality);
            sKImage.Dispose();
            bytes = data.ToArray();
            data.Dispose();

            return bytes;
        }

        /// <summary>
        ///     生成二维码(320*320)
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="logoPath">Logo图片路径(缩放到真实二维码区域尺寸的1/6)</param>
        /// <param name="keepWhiteBorderPixelVal">白边处理(负值表示不做处理，最大值不超过真实二维码区域的1/10)</param>
        public static void QRCoder(string text, string savePath, string logoPath = "", int keepWhiteBorderPixelVal = -1)
        {
            var format = GetImageFormatByPath(savePath);
            byte[] bytesLogo = null;
            if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
            {
                var fsLogo = new FileStream(logoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var ms = new MemoryStream();
                fsLogo.CopyTo(ms);
                fsLogo.Dispose();
                bytesLogo = ms.ToArray();
                ms.Dispose();
            }

            var bytes = QRCoder(text, format, bytesLogo, keepWhiteBorderPixelVal);

            if (bytes == null || bytes.Length < 1) return;

            var saveDirPath = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(saveDirPath)) Directory.CreateDirectory(saveDirPath);
            var fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        /// <summary>
        ///     生成二维码(320*320)
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="format">保存格式</param>
        /// <param name="logoImgae">Logo图片(缩放到真实二维码区域尺寸的1/6)</param>
        /// <param name="keepWhiteBorderPixelVal">白边处理(负值表示不做处理，最大值不超过真实二维码区域的1/10)</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] QRCoder(string text, SKEncodedImageFormat format, byte[] logoImgae = null,
            int keepWhiteBorderPixelVal = -1)
        {
            byte[] reval = null;
            var width = 320;
            var height = 320;
            var qRCodeWriter = new QRCodeWriter();
            var hints = new Dictionary<EncodeHintType, object>
            {
                {EncodeHintType.CHARACTER_SET, "utf-8"},
                {EncodeHintType.QR_VERSION, 8},
                {EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.Q}
            };
            var bitMatrix = qRCodeWriter.encode(text, BarcodeFormat.QR_CODE, width, height, hints);
            var w = bitMatrix.Width;
            var h = bitMatrix.Height;
            var sKBitmap = new SKBitmap(w, h);

            var blackStartPointX = 0;
            var blackStartPointY = 0;
            var blackEndPointX = w;
            var blackEndPointY = h;

            #region --绘制二维码(同时获取真实的二维码区域起绘点和结束点的坐标)--

            var sKCanvas = new SKCanvas(sKBitmap);
            var sKColorBlack = SKColor.Parse("000000");
            var sKColorWihte = SKColor.Parse("ffffff");
            sKCanvas.Clear(sKColorWihte);
            var blackStartPointIsNotWriteDown = true;
            for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
            {
                var flag = bitMatrix[x, y];
                if (flag)
                {
                    if (blackStartPointIsNotWriteDown)
                    {
                        blackStartPointX = x;
                        blackStartPointY = y;
                        blackStartPointIsNotWriteDown = false;
                    }

                    blackEndPointX = x;
                    blackEndPointY = y;
                    sKCanvas.DrawPoint(x, y, sKColorBlack);
                }
            }

            sKCanvas.Dispose();

            #endregion

            var qrcodeRealWidth = blackEndPointX - blackStartPointX;
            var qrcodeRealHeight = blackEndPointY - blackStartPointY;

            #region -- 处理白边 --

            if (keepWhiteBorderPixelVal > -1) //指定了边框宽度
            {
                var borderMaxWidth = (int) Math.Floor((double) qrcodeRealWidth / 10);
                if (keepWhiteBorderPixelVal > borderMaxWidth) keepWhiteBorderPixelVal = borderMaxWidth;
                var nQrcodeRealWidth = width - keepWhiteBorderPixelVal - keepWhiteBorderPixelVal;
                var nQrcodeRealHeight = height - keepWhiteBorderPixelVal - keepWhiteBorderPixelVal;

                var sKBitmap2 = new SKBitmap(width, height);
                var sKCanvas2 = new SKCanvas(sKBitmap2);
                sKCanvas2.Clear(sKColorWihte);
                //二维码绘制到临时画布上时无需抗锯齿等处理(避免文件增大)
                sKCanvas2.DrawBitmap(
                    sKBitmap,
                    new SKRect
                    {
                        Location = new SKPoint {X = blackStartPointX, Y = blackStartPointY},
                        Size = new SKSize {Height = qrcodeRealHeight, Width = qrcodeRealWidth}
                    },
                    new SKRect
                    {
                        Location = new SKPoint {X = keepWhiteBorderPixelVal, Y = keepWhiteBorderPixelVal},
                        Size = new SKSize {Width = nQrcodeRealWidth, Height = nQrcodeRealHeight}
                    });

                blackStartPointX = keepWhiteBorderPixelVal;
                blackStartPointY = keepWhiteBorderPixelVal;
                qrcodeRealWidth = nQrcodeRealWidth;
                qrcodeRealHeight = nQrcodeRealHeight;

                sKCanvas2.Dispose();
                sKBitmap.Dispose();
                sKBitmap = sKBitmap2;
            }

            #endregion

            #region -- 绘制LOGO --

            if (logoImgae != null && logoImgae.Length > 0)
            {
                var sKBitmapLogo = SKBitmap.Decode(logoImgae);
                if (!sKBitmapLogo.IsEmpty)
                {
                    var sKPaint2 = new SKPaint
                    {
                        FilterQuality = SKFilterQuality.None,
                        IsAntialias = true
                    };
                    var logoTargetMaxWidth = (int) Math.Floor((double) qrcodeRealWidth / 6);
                    var logoTargetMaxHeight = (int) Math.Floor((double) qrcodeRealHeight / 6);
                    var qrcodeCenterX = (int) Math.Floor((double) qrcodeRealWidth / 2);
                    var qrcodeCenterY = (int) Math.Floor((double) qrcodeRealHeight / 2);
                    var logoResultWidth = sKBitmapLogo.Width;
                    var logoResultHeight = sKBitmapLogo.Height;
                    if (logoResultWidth > logoTargetMaxWidth)
                    {
                        var r = (double) logoTargetMaxWidth / logoResultWidth;
                        logoResultWidth = logoTargetMaxWidth;
                        logoResultHeight = (int) Math.Floor(logoResultHeight * r);
                    }

                    if (logoResultHeight > logoTargetMaxHeight)
                    {
                        var r = (double) logoTargetMaxHeight / logoResultHeight;
                        logoResultHeight = logoTargetMaxHeight;
                        logoResultWidth = (int) Math.Floor(logoResultWidth * r);
                    }

                    var pointX = qrcodeCenterX - (int) Math.Floor((double) logoResultWidth / 2) + blackStartPointX;
                    var pointY = qrcodeCenterY - (int) Math.Floor((double) logoResultHeight / 2) + blackStartPointY;

                    var sKCanvas3 = new SKCanvas(sKBitmap);
                    var sKPaint = new SKPaint
                    {
                        FilterQuality = SKFilterQuality.Medium,
                        IsAntialias = true
                    };
                    sKCanvas3.DrawBitmap(
                        sKBitmapLogo,
                        new SKRect
                        {
                            Location = new SKPoint {X = 0, Y = 0},
                            Size = new SKSize {Height = sKBitmapLogo.Height, Width = sKBitmapLogo.Width}
                        },
                        new SKRect
                        {
                            Location = new SKPoint {X = pointX, Y = pointY},
                            Size = new SKSize {Height = logoResultHeight, Width = logoResultWidth}
                        }, sKPaint);
                    sKCanvas3.Dispose();
                    sKPaint.Dispose();
                    sKBitmapLogo.Dispose();
                }
                else
                {
                    sKBitmapLogo.Dispose();
                }
            }

            #endregion

            var sKImage = SKImage.FromBitmap(sKBitmap);
            sKBitmap.Dispose();
            var data = sKImage.Encode(format, 75);
            sKImage.Dispose();
            reval = data.ToArray();
            data.Dispose();

            return reval;
        }

        /// <summary>
        ///     Qrs the decoder.
        /// </summary>
        /// <param name="qrCodeFilePath">The qr code file path.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">
        ///     文件不存在
        ///     or
        ///     图片文件太大
        /// </exception>
        public static string QRDecoder(string qrCodeFilePath)
        {
            if (!File.Exists(qrCodeFilePath)) throw new Exception("文件不存在");

            var fileStream = new FileStream(qrCodeFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fileStream.Length > maxLength)
            {
                fileStream.Dispose();
                throw new Exception("图片文件太大");
            }

            return QRDecoder(fileStream);
        }

        /// <summary>
        ///     Qrs the decoder.
        /// </summary>
        /// <param name="qrCodeBytes">The qr code bytes.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">
        ///     参数qrCodeBytes不存在
        ///     or
        ///     图片文件太大
        /// </exception>
        public static string QRDecoder(byte[] qrCodeBytes)
        {
            if (qrCodeBytes == null || qrCodeBytes.Length < 1) throw new Exception("参数qrCodeBytes不存在");
            if (qrCodeBytes.Length > maxLength) throw new Exception("图片文件太大");
            var ms = new MemoryStream(qrCodeBytes);
            return QRDecoder(ms);
        }

        /// <summary>
        ///     Qrs the decoder.
        /// </summary>
        /// <param name="qrCodeFileStream">The qr code file stream.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">未识别的图片文件</exception>
        public static string QRDecoder(Stream qrCodeFileStream)
        {
            var sKManagedStream = new SKManagedStream(qrCodeFileStream, true);
            var sKBitmap = SKBitmap.Decode(sKManagedStream);
            sKManagedStream.Dispose();
            if (sKBitmap.IsEmpty)
            {
                sKBitmap.Dispose();
                throw new Exception("未识别的图片文件");
            }

            var w = sKBitmap.Width;
            var h = sKBitmap.Height;
            var ps = w * h;
            var bytes = new byte[ps * 3];
            var byteIndex = 0;
            for (var x = 0; x < w; x++)
            for (var y = 0; y < h; y++)
            {
                var color = sKBitmap.GetPixel(x, y);
                bytes[byteIndex + 0] = color.Red;
                bytes[byteIndex + 1] = color.Green;
                bytes[byteIndex + 2] = color.Blue;
                byteIndex += 3;
            }

            sKBitmap.Dispose();

            var qRCodeReader = new QRCodeReader();
            var rGbLuminanceSource = new RGBLuminanceSource(bytes, w, h);
            var hybridBinarizer = new HybridBinarizer(rGbLuminanceSource);
            var binaryBitmap = new BinaryBitmap(hybridBinarizer);
            var hints = new Dictionary<DecodeHintType, object> {{DecodeHintType.CHARACTER_SET, "utf-8"}};
            var result = qRCodeReader.decode(binaryBitmap, hints);

            return result != null ? result.Text : "";
        }
    }
}