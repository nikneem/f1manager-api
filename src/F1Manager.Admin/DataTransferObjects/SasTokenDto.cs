using System;
using System.Collections.Generic;
using System.Text;

namespace F1Manager.Admin.DataTransferObjects
{
    public class SasTokenDto
    {
        public string StorageAccountUrl { get; set; }
        public string SasToken { get; set; }
        public string ConstainerName { get; set; }
        public string BlobName { get; set; }
    }
}
