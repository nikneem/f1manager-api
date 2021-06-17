using System.Threading.Tasks;
using F1Manager.Users.Repositories;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace F1Manager.Functions.Functions.Users
{
    public  class LoginAttemptsCleanUp
    {


        [FunctionName("LoginAttemptsCleanUp")]
        public  async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer,
            [Table(LoginsRepository.TableName)] CloudTable table)
        {
            await LoginsRepository.Cleanup(table);
        }

    }
}
