using QuestTask.DbObjects;

namespace QuestTask
{
    public interface IClientDbService
    {
        Task<Root> FindClient(int clientId);
        Task<List<Root>> FindAllClients();
        Task<long> DeleteClient(int clientId);
        Task<long> UpdateClient(Root client);

    }
}
