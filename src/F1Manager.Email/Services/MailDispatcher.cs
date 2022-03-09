using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using F1Manager.Email.Abstractions;
using F1Manager.Email.Configuration;
using F1Manager.Email.Enums;
using F1Manager.Email.Exceptions;
using HexMaster.Email.DomainModels;
using Microsoft.Extensions.Options;

namespace F1Manager.Email.Services;

public class MailDispatcher : IMailDispatcher
{
    private const string DefaultLanguage = "en";
    private const string MailTemplateFileExtension = ".html";
    private const string MailTitleRegularExpression = "<title>(?<title>[\\w\\s]*)</title>";

    private const string EmailMessagesContainerName = "mails";
    private readonly BlobContainerClient _emailsBlobContainer;

    public async Task<bool> Dispatch(Subjects subject, string preferredLanguage, Recipient recipient)
    {
        var sender = new Sender("ekeilholz@outlook.com", "F1 Manager");
        var mailTitle = subject.ToString();
        var mailContent = await DownloadMailTemplate(subject, preferredLanguage);

        var match = Regex.Match(mailContent, MailTitleRegularExpression);
        if (match.Success && match.Groups["title"].Success)
        {
            mailTitle = match.Groups["title"].Value;
        }

        var mailBody = new Body("default", mailContent, true);

        var message = new Message(sender, recipient, mailTitle, mailBody);
        var messageStream = await message.SerializeToStreamAsync();
        var response = await _emailsBlobContainer.UploadBlobAsync($"{Guid.NewGuid()}.mail", messageStream);
        return response.GetRawResponse().Status == 201;
    }


    private async Task<string> DownloadMailTemplate(Subjects subject, string preferredLanguage)
    {
        var templateName = $"{subject.ToString().ToLower()}-{preferredLanguage}{MailTemplateFileExtension}";
        var defaultLanguageTemplateName = $"{subject.ToString().ToLower()}-{DefaultLanguage}{MailTemplateFileExtension}";
        var respourceName = GetType().Assembly.GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith(templateName, StringComparison.CurrentCultureIgnoreCase));
        if (string.IsNullOrWhiteSpace(respourceName))
        {
            respourceName= GetType().Assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(defaultLanguageTemplateName, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(respourceName))
        {
            var resourceStream = GetType().Assembly.GetManifestResourceStream(respourceName);
            if (resourceStream != null)
            {
                var streamReader = new StreamReader(resourceStream);
                return await streamReader.ReadToEndAsync();
            }
        }

        throw new F1ManagerMailException(F1ManagerErrorCode.TemplateNotFound,
            $"The email template {templateName} was not found");

    }


    public MailDispatcher(IOptions<MailDispatcherOptions> config)
    {
        var blobService = new BlobServiceClient(config.Value.AzureStorageAccount);
        _emailsBlobContainer= blobService.GetBlobContainerClient(EmailMessagesContainerName);
    }


}