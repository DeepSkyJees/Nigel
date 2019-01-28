using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Nigel.Basic.Utility
{
    public static class FtpUtility
    {
        /// <summary>
        ///     Uploads the FTP file.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool UploadFtpFile(string address, string fileName, string filePath)
        {
            MakeDirectory(address);
            using (var webClient = new WebClient())
            {
                webClient.UploadFile($"{address}/{fileName}", filePath);
                return true;
            }
        }

        /// <summary>
        ///     Makes the directory.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MakeDirectory(string address)
        {
            if (RemoteFtpDirExists(address))
                return true;
            FtpWebResponse res = null;
            try
            {
                var req = (FtpWebRequest)WebRequest.Create(address);
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
        ///     Deletes the FTP file.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DeleteFtpFile(string address, string fileName)
        {
            var req = (FtpWebRequest)WebRequest.Create($"{address}/{fileName}");
            req.Method = WebRequestMethods.Ftp.DeleteFile;
            req.GetResponse();
            return true;
        }

        /// <summary>
        ///     Downloads the FTP file.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DownloadFtpFile(string address, string fileName)
        {
            var downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "monitor");
            using (var webClient = new WebClient())
            {
                if (!Directory.Exists(downloadPath))
                    Directory.CreateDirectory(downloadPath);
                webClient.DownloadFile($"{address}/{fileName}", Path.Combine(downloadPath, fileName));
            }

            return true;
        }


        /// <summary>
        ///     Remotes the FTP dir exists.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool RemoteFtpDirExists(string address)
        {
            var req = (FtpWebRequest)WebRequest.Create(address);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse res = null;
            try
            {
                res = (FtpWebResponse)req.GetResponse();
                var code = res.StatusCode;
                res.Close();
                return true;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                res?.Close();
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }


        /// <summary>
        ///     Uploads the FTP file.
        /// </summary>
        /// <param name="ftpUrl">The FTP URL.</param>
        /// <param name="ftpDirectoryFullPath">The FTP directory full path.</param>
        /// <param name="localDirectoryFullPath">The local directory full path.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void UploadFtpFile(string ftpUrl, string ftpDirectoryFullPath, string localDirectoryFullPath,
            string fileName)
        {
            var hasDirectory = CreateFtpDirectory(ftpUrl, ftpDirectoryFullPath);
            if (!hasDirectory.IsNotEmptyOrNullOrWhite())
            {
                var ftpWebRequest = CreateFtpWebRequest(ftpUrl, ftpDirectoryFullPath, fileName,
                    WebRequestMethods.Ftp.UploadFile);
                var fileInfo = new FileInfo($"{localDirectoryFullPath}/{fileName}");
                var fileStream = fileInfo.OpenRead();

                var bufferLength = 2048;
                var buffer = new byte[bufferLength];

                var uploadStream = ftpWebRequest.GetRequestStream();
                var contentLength = fileStream.Read(buffer, 0, bufferLength);

                while (contentLength != 0)
                {
                    uploadStream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }

                uploadStream.Close();
                fileStream.Close();
            }
        }

        /// <summary>
        ///     Downloads the FTP files.
        /// </summary>
        /// <param name="ftpWebRequest">The FTP web request.</param>
        /// <param name="ftpDirectoryFullPath">The FTP directory full path.</param>
        /// <param name="localDirectoryFullPath">The local directory full path.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="fileNameList">The file name list.</param>
        /// <exception cref="ArgumentNullException">ftpWebRequest</exception>
        public static void DownloadFtpFiles(
            FtpWebRequest ftpWebRequest,
            string ftpDirectoryFullPath,
            string localDirectoryFullPath,
            string fileExtension,
            List<string> fileNameList)
        {
            if (ftpWebRequest == null) throw new ArgumentNullException(nameof(ftpWebRequest));

            ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            foreach (var fileName in fileNameList)
            {
                var localFilePath = Path.Combine(localDirectoryFullPath, fileName);
                using (var webClient = new WebClient())
                {
                    if (!Directory.Exists(localDirectoryFullPath))
                        Directory.CreateDirectory(localDirectoryFullPath);
                    webClient.DownloadFile($"{ftpDirectoryFullPath}/{fileName}", localFilePath);
                }
            }
        }


        /// <summary>
        ///     Gets the FTP file name list.
        /// </summary>
        /// <param name="ftpWebRequest">The FTP web request.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        /// <exception cref="ArgumentNullException">ftpWebRequest</exception>
        public static List<string> GetFtpFileNameList(FtpWebRequest ftpWebRequest, string fileExtension)
        {
            if (ftpWebRequest == null) throw new ArgumentNullException(nameof(ftpWebRequest));

            var lines = new List<string>();
            using (var listResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
            using (var listStream = listResponse.GetResponseStream())
            using (var listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    var line = listReader.ReadLine();
                    if (line != null)
                    {
                        var tokens = line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                        var name = tokens[3];
                        if (!line.EndsWith(fileExtension)) continue;
                        lines.Add(name);
                    }
                }

                return lines;
            }
        }

        /// <summary>
        ///     Deletes the FTP files then directory.
        /// </summary>
        /// <param name="ftpUrl">The FTP URL.</param>
        /// <param name="ftpDirectoryFullPath">The FTP directory full path.</param>
        /// <param name="fileExtension">The file extension.</param>
        public static void DeleteFtpFilesThenDirectory(string ftpUrl, string ftpDirectoryFullPath,
            string fileExtension = "*.*")
        {
            var getFtpFilesWebRequest = CreateFtpWebRequest(ftpUrl, ftpDirectoryFullPath);
            var fileList = GetFtpFileNameList(getFtpFilesWebRequest, fileExtension);
            foreach (var file in fileList)
            {
                var deleteFtpFileRequest = CreateFtpWebRequest(ftpUrl, ftpDirectoryFullPath, file);
                var deleteDtpFileResponse = (FtpWebResponse)deleteFtpFileRequest.GetResponse();
                deleteDtpFileResponse.Close();
            }

            var deleteFtpDirectoryRequest = CreateFtpWebRequest(ftpUrl, ftpDirectoryFullPath);
            var deleteFtpDirectoryResponse = (FtpWebResponse)deleteFtpDirectoryRequest.GetResponse();
            deleteFtpDirectoryResponse.Close();
        }


        /// <summary>
        ///     Creates the FTP directory.
        /// </summary>
        /// <param name="ftpUrl">The FTP URL.</param>
        /// <param name="ftpDirectoryFullPath">The FTP directory full path.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static string CreateFtpDirectory(string ftpUrl, string ftpDirectoryFullPath,
            string userName = "Anonymous", string password = "")
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri($"{ftpUrl}/{ftpDirectoryFullPath}"));
                ftpRequest.UsePassive = true;
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.UseBinary = true;
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;


                var makeDirectoryResponse = (FtpWebResponse)ftpRequest.GetResponse();
                var ftpStream = makeDirectoryResponse.GetResponseStream();
                ftpStream?.Close();
                makeDirectoryResponse.Close();
                return string.Empty;
            }
            catch (WebException e)
            {
                var response = (FtpWebResponse)e.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return response.StatusDescription;
                }

                throw e;
            }
        }


        /// <summary>
        ///     Downloads the FTP file.
        /// </summary>
        /// <param name="ftpUrl">The FTP URL.</param>
        /// <param name="ftpDirectoryPath">The FTP directory path.</param>
        /// <param name="localDirectoryPath">The local directory path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public static string DownloadFtpFile(string ftpUrl, string ftpDirectoryPath, string localDirectoryPath,
            string fileName)
        {
            try
            {
                var request =
                    CreateFtpWebRequest(ftpUrl, ftpDirectoryPath, fileName, WebRequestMethods.Ftp.DownloadFile);

                var response = (FtpWebResponse)request.GetResponse();

                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                var outputStream = new FileStream($"{localDirectoryPath}/{fileName}", FileMode.Create);
                var Length = 2048;
                var buffer = new byte[Length];
                var bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    outputStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }

                reader.Close();
                response.Close();
                return string.Empty;
            }
            catch (WebException e)
            {
                var response = (FtpWebResponse)e.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return response.StatusDescription;
                }

                throw;
            }
        }

        /// <summary>
        ///     Creates the FTP web request.
        /// </summary>
        /// <param name="ftpUrl">The FTP URL.</param>
        /// <param name="ftpDirectoryFullPath">The FTP directory full path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="webRequestMethod">The web request method.</param>
        /// <returns>FtpWebRequest.</returns>
        public static FtpWebRequest CreateFtpWebRequest(
            string ftpUrl,
            string ftpDirectoryFullPath,
            string fileName = "",
            string webRequestMethod = WebRequestMethods.Ftp.DeleteFile)
        {
            FtpWebRequest ftpRequest = null;
            if (fileName.IsNotEmptyOrNullOrWhite())
                ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri($"{ftpUrl}/{ftpDirectoryFullPath}/{fileName}"));
            else
                ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri($"{ftpUrl}/{ftpDirectoryFullPath}"));
            ftpRequest.UsePassive = true;
            ftpRequest.Credentials = new NetworkCredential("Anonymous", "");
            ftpRequest.UseBinary = true;
            ftpRequest.Method = webRequestMethod;
            return ftpRequest;
        }

        /// <summary>
        ///     Creates the FTP web request.
        /// </summary>
        /// <param name="ftpUrl">The FTP URL.</param>
        /// <param name="ftpDirectoryFullPath">The FTP directory full path.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="ftpWebRequest">The FTP web request.</param>
        /// <returns>FtpWebRequest.</returns>
        public static FtpWebRequest CreateFtpWebRequest(string ftpUrl,
            string ftpDirectoryFullPath,
            string userName,
            string password,
            string ftpWebRequest)
        {
            var ftpRequest =
                (FtpWebRequest)WebRequest.Create(new Uri($"{ftpUrl}/{ftpDirectoryFullPath}"));
            ftpRequest.UsePassive = true;
            ftpRequest.Credentials = new NetworkCredential(userName, password);
            ftpRequest.UseBinary = true;
            ftpRequest.Method = ftpWebRequest;
            return ftpRequest;
        }
    }
}