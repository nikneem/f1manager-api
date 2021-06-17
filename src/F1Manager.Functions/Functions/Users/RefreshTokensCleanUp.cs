using System.Threading.Tasks;
using F1Manager.Users.Repositories;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace F1Manager.Functions.Functions.Users
{
    public  class RefreshTokensCleanUp
    {

        [FunctionName("RefreshTokensCleanUp")]
        public  async Task Run(
            [TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer,
            [Table(RefreshTokensRepository.TableName)] CloudTable table)
        {
            await RefreshTokensRepository.Cleanup(table);
        }

    }
}
