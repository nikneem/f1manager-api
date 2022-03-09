using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HexMaster.Email.Abstractions.Services;
using HexMaster.Email.DomainModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;

namespace F1Manager.Functions.Functions.Email
{
    public class EmailSenderFunction
    {
        private readonly IMailService _mailService;

        [FunctionName("EmailSenderFunction")]
        public  async Task Run([BlobTrigger("mails/{name}")] CloudBlockBlob blob, string name, ILogger log)
        {
            log.LogInformation("Received message {name} to send as email", name);

            await using var input = new MemoryStream();
            await blob.DownloadToStreamAsync(input);
            var mailMessage = await Message.FromStreamAsync(input);

            await _mailService.SendAsync(mailMessage);

            if (!mailMessage.Recipients.All(r => r.IsCompleted))
            {
                log.LogError("Not all recipients were reached by the mail service. One or more messages failed.");
            }

            await blob.DeleteIfExistsAsync();
        }

        public EmailSenderFunction(IMailService mailService)
        {
            _mailService = mailService;
        }
    }
}
