

using Dropbox.Api;
using Dropbox.Api.Files;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Application.FileUpload
{
    public class DropBoxConfig
    {
        public string Token { get; set; }
        public string DestinationFolder { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
    }

    public interface IDropboxHelper
    {
        Task<bool> CheckExists(string rootFolder, string fullPath = null);
        Task CreateFolderAsync(string folder);
        Task<string> DownloadContentString(string file);
        Task<byte[]> Download(FileMetadata file);
        Task<byte[]> Download(string filename);
        Task Upload(string file, byte[] content);
        Task<ListFolderResult> ListFolder(string path);
        Task<List<Metadata>> ListRootFolder();
    }


    public class DropboxHelper : IDropboxHelper, IDisposable
    {
        private readonly string _token;
        private readonly string _destinationFolder;
        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly DropboxClient _client;
        private readonly DropBoxConfig _dropBoxConfig;
        public DropboxHelper(IOptions<DropBoxConfig> dropBoxConfig)
        {
            _dropBoxConfig = dropBoxConfig?.Value;
            if (_dropBoxConfig == null)
            {
                throw new Exception("Unable to injact dropBoxConfig into DropboxHelper class.");
            }
            _token = _dropBoxConfig.Token;
            _destinationFolder = _dropBoxConfig.DestinationFolder;
            _appKey = _dropBoxConfig.AppKey;
            _appSecret = _dropBoxConfig.AppSecret;

            this._client = new DropboxClient(_token);
            var checkFileExists = CheckExists(_destinationFolder).GetAwaiter().GetResult();
            if (!checkFileExists)
            {
                CreateFolderAsync(_destinationFolder).GetAwaiter().GetResult();
            }
        }


        public async Task<bool> CheckExists(string rootFolder, string fullPath = null)
        {
            var listFolders = await ListFolder(rootFolder);
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                fullPath = rootFolder;
            }
            return listFolders.Entries.Any(x => x.PathDisplay.Contains(fullPath));
        }

        public async Task CreateFolderAsync(string folder)
        {
            await _client.Files.CreateFolderV2Async(folder.Replace("\\", "/"));
        }

        // To download a file:
        public async Task<string> DownloadContentString(string file)
        {
            var path = file.Replace("\\", "/");
            using (var response = await _client.Files.DownloadAsync(path))
            {
                return await response.GetContentAsStringAsync();
            }
        }

        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <remarks>This demonstrates calling a download style api in the Files namespace.</remarks>
        /// <param name="folder">The folder path in which the file should be found.</param>
        /// <param name="file">The file to download within <paramref name="folder"/>.</param>
        /// <returns></returns>
        public async Task<byte[]> Download(FileMetadata file)
        {
            var path = file.Name.Replace("\\", "/");
            using (var response = await this._client.Files.DownloadAsync(path))
            {
                return await response.GetContentAsByteArrayAsync();
            }
        }

        public async Task<byte[]> Download(string filename)
        {
            var path = filename.Replace("\\", "/");
            using (var response = await this._client.Files.DownloadAsync(path))
            {
                return await response.GetContentAsByteArrayAsync();
            }
        }

        // To upload a file:
        public async Task Upload(string file, byte[] content)
        {
            var fullFilePath = file.Replace("\\", "/");
            using (var mem = new MemoryStream(content))
            {
                var updated = await _client.Files.UploadAsync(
                    fullFilePath,
                    WriteMode.Overwrite.Instance,
                    body: mem);
            }
        }

        public async Task<bool> FileExists(string filePath)
        {
            var metaData = await _client.Files.GetMetadataAsync(filePath);
            return false;
        }

        /// <summary>
        /// Initializes ssl certificate pinning.
        /// </summary>
        public void InitializeCertPinning()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                var root = chain.ChainElements[chain.ChainElements.Count - 1];
                var publicKey = root.Certificate.GetPublicKeyString();

                return DropboxCertHelper.IsKnownRootCertPublicKey(publicKey);
            };
        }


        /// <summary>
        /// Lists the items within a folder.
        /// </summary>
        /// <remarks>This is a demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="path">The path to list.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        public async Task<ListFolderResult> ListFolder(string path)
        {
            var list = await this._client.Files.ListFolderAsync(path.Replace("\\", "/"));
            return list;
        }

        public async Task<List<Metadata>> ListRootFolder()
        {
            try
            {
                var list = await _client.Files.ListFolderAsync(string.Empty);
                return list.Entries.Where(i => i.IsFolder).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

