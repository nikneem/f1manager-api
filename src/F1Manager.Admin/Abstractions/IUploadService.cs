using System.Threading.Tasks;
using F1Manager.Admin.DataTransferObjects;

namespace F1Manager.Admin.Abstractions
{
    public interface IUploadService
    {
        Task<SasTokenDto> GetSasToken(SasTokenRequestDto dto);

    }
}