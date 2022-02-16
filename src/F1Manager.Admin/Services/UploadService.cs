using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using F1Manager.Admin.Abstractions;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.DataTransferObjects;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Services
{
    public sealed class UploadService: IUploadService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _storageConnectionString;

        public async Task<SasTokenDto> GetSasToken(SasTokenRequestDto dto)
        {
                var blobName = $"{Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower()}";
                var blobContainerClient = new BlobContainerClient(_storageConnectionString, Defaults.BlobStorageUploadContainer);
                await blobContainerClient.CreateIfNotExistsAsync();
                var blobClient = new BlobClient(_storageConnectionString, Defaults.BlobStorageUploadContainer, blobName);


                if (blobClient.CanGenerateSasUri)
                {
                    // Create a SAS token that's valid for one hour.
                    var sasBuilder = new BlobSasBuilder
                    {
                        BlobContainerName = Defaults.BlobStorageUploadContainer,
                        BlobName = blobName,
                        StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Write);

                    var sasUri = blobClient.GenerateSasUri(sasBuilder);
                    Console.WriteLine("SAS URI for blob container is: {0}", sasUri);
                    Console.WriteLine();

                    return new SasTokenDto
                    {
                        StorageAccountUrl = sasUri.Authority,
                        ConstainerName = Defaults.BlobStorageUploadContainer,
                        BlobName = blobName,
                        SasToken = sasUri.Query
                    };
                }
            return null;
        }

        public UploadService(IOptions<AdminOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _storageConnectionString = options.Value.AzureStorageAccount;
        }
    }
}
