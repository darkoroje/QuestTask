using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestTask.DbObjects;

namespace QuestTask.Controllers
{
    [Route("client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientDbService dbService;

        public ClientController(IClientDbService dbService)
        {
            this.dbService = dbService;
        }

        /// <summary>
        /// Returns client with the specified Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Root> GetClient(int clientId)
        {
            var doc = await dbService.FindClient(clientId);
            return doc;
        }

        /// <summary>
        /// Returns all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<List<Root>> GetClients()
        {
            var docs = await dbService.FindAllClients();
            return docs;
        }

        /// <summary>
        /// Deletes client with specified Id
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Number of updated records</returns>
        [HttpDelete]
        public async Task<long> DeleteClient(int clientId)
        {
            var deleteCount = await dbService.DeleteClient(clientId);
            return deleteCount;
        }

        /// <summary>
        /// Updates complete client
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Number of updated records</returns>
        [HttpPut]
        public async Task<long> UpdateClient(Root client)
        {
            var updateCount = await dbService.UpdateClient(client);
            return updateCount;
        }
    }
}
