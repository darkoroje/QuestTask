using QuestTask;
using QuestTask.DbObjects;
using QuestTask.Services;

namespace Tests
{
    // These tests assume there is a MongoDb running locally, and test the service with sample data
    // More tests could be added, and the controller could also be tested

    public class ClientDbServiceTests
    {
        readonly IClientDbService clientDbService;
        readonly Root[] sampleData;

        public ClientDbServiceTests()
        {
            DBSetup.InitializeDbWithSampleData();
            var data = DBSetup.GetSampleData();
            if (data == null)
                throw new Exception("Unable to get sample data");
            sampleData = data;
            clientDbService = new ClientDbService();
        }

        /// <summary>
        /// Make sure we can get all elements and they the returned data is as expected
        /// </summary>
        [Fact]
        public async void GetAll()
        {
            var clients = await clientDbService.FindAllClients();
            Assert.True(clients.Count() == sampleData.Count());
            foreach (var client in clients)
            {
                Assert.Contains(sampleData, elem => elem.id == client.id);
            }
        }

        [Fact]
        public async void ExpectedClientExists()
        {
            var client = await clientDbService.FindClient(10);
            Assert.NotNull(client);
        }

        [Fact]
        public async void GetingNonExistantClientCausesException()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await clientDbService.FindClient(99));
        }

        [Fact]
        public async void CanDeleteAClient()
        {
            await clientDbService.DeleteClient(10);
            var newClients = await clientDbService.FindAllClients();
            Assert.True(newClients.Count() == sampleData.Count() - 1);
        }

        [Fact]
        public async void CanUpdateAClient()
        {
            var clientToUpdate = sampleData.First();
            clientToUpdate.name = "Test name";
            await clientDbService.UpdateClient(clientToUpdate);
            var updatedClient = await clientDbService.FindClient(clientToUpdate.id);
            Assert.True(updatedClient.id == clientToUpdate.id);
        }
    }
}