using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;
using ProgramCRUD.Model;

namespace ProgramCRUD.Data
{
    public class ProgramService
    {
        private readonly string CosmosDBAccountUri = "https://localhost:8081";
        private readonly string CosmosDBAccountPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private readonly string CosmosDbName = "ProgramManagementDB";
        private readonly string CosmosDbContainerName = "Programs";

        private Container ContainerClient()
        {
            CosmosClient cosmosDBClient = new CosmosClient(CosmosDBAccountUri, CosmosDBAccountPrimaryKey);
            Container containerClient = cosmosDBClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return containerClient;
        }

        public async Task AddProgram(ProgramModel program)
        {
            try
            {
                program.id = Guid.NewGuid();
                var container = ContainerClient();
                var response = await container.CreateItemAsync(program, new PartitionKey(program.Title));

                // return Ok(response);
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }

        }
        public async Task<List<ProgramModel>> GetProgramDetails()
        {
            List<ProgramModel> programs = new List<ProgramModel>();
            try
            {
                var container = ContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<ProgramModel> queryResultSetIterator = container.GetItemQueryIterator<ProgramModel>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<ProgramModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (ProgramModel program in currentResultSet)
                    {
                        programs.Add(program);
                    }
                }
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }
            return programs;
        }

        public async Task<ProgramModel> GetProgramDetailsById(string? proframId, string? partitionKey)
        {
            try
            {
                var container = ContainerClient();
                ItemResponse<ProgramModel> response = await container.ReadItemAsync<ProgramModel>(proframId, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {

                throw new Exception("Exception ", ex);
            }
        }
        public async Task UpdateProgram(ProgramModel pro)
        {
            try
            {
                var container = ContainerClient();
                ItemResponse<ProgramModel> res = await container.ReadItemAsync<ProgramModel>(Convert.ToString(pro.id), new PartitionKey(pro.Title));
                //Get Existing Item
                var existingItem = res.Resource;

                //Replace existing item values with new values 
                existingItem.Title = pro.Title;
                existingItem.Summary = pro.Summary;
                existingItem.Description = pro.Description;
                existingItem.Skills = pro.Skills;
                existingItem.Benifits = pro.Benifits;
                existingItem.Criteria = pro.Criteria;
                existingItem.Type = pro.Type;
                existingItem.Start = pro.Start;
                existingItem.Open = pro.Close ;
                existingItem.Duration = pro.Duration;
                existingItem.Location = pro.Location;
                existingItem.Qualification = pro.Qualification;
                existingItem.MaxNumOfApplications = pro.MaxNumOfApplications;
                string? id = Convert.ToString(existingItem.id);
                var updateRes = await container.ReplaceItemAsync(existingItem, id, new PartitionKey(pro.Title));
                //return updateRes.Resource
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        public async Task DeleteProgram(string? empId, string? partitionKey)
        {
            try
            {
                var container = ContainerClient();
                var response = await container.DeleteItemAsync<ProgramModel>(empId, new PartitionKey(partitionKey));
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }
    }
}
