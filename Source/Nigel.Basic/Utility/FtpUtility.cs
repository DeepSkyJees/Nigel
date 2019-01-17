using System;
using System.IO;
using System.Net;

namespace Nigel.Basic.Utility
{
    public static class FtpUtility
    {
        /// <summary>
        /// Uploads the FTP file.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Upload(string address, string fileName, string filePath)
        {
            MakeDirectory(address);
            using (WebClient webClient = new WebClient())
            {
                webClient.UploadFile($"{address}/{fileName}", filePath);
                return true;
            }
        }

        /// <summary>
        /// Makes the directory.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MakeDirectory(string address)
        {
            if (RemoteDirExists(address))
                return true;
            FtpWebResponse res = null;
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(address);
                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                res = (FtpWebResponse)req.GetResponse();
                res.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res?.Close();
                throw e;
            }

        }

        /// <summary>
        /// Deletes the FTP file.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Delete(string address, string fileName)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create($"{address}/{fileName}");
            req.Method = WebRequestMethods.Ftp.DeleteFile;
            req.GetResponse();
            return true;
        }

        /// <summary>
        /// Downloads the FTP file.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Download(string address, string fileName)
        {
            string downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "monitor");
            using (WebClient webClient = new WebClient())
            {
                if (!Directory.Exists(downloadPath))
                    Directory.CreateDirectory(downloadPath);
                webClient.DownloadFile($"{address}/{fileName}", Path.Combine(downloadPath, fileName));
            }
            return true;
        }

        /// <summary>
        /// Remotes the FTP dir exists.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool RemoteDirExists(string address)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(address);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse res = null;
            try
            {
                res = (FtpWebResponse)req.GetResponse();
                res.Close();
                return true;
            }
            catch (InvalidOperationException e)
            {
                res?.Close();
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}