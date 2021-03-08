using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Team.Business.Constant;
using Team.Business.DependencyInjections;
using Team.Business.Helpers;
using Team.Business.Request;
using Team.Business.Response;

namespace Team.Business.Repository
{
    public interface IPlayerRepository
    {
        Task<CreatePlayerResponse> CreatePlayerMethod(CreatePlayerRequest m); // Dönüş tipimiz = CreatePlayerResponse
        Task<IEnumerable<CreatePlayerRequest>> GetAllPlayers();
    }

    public class PlayerRepository : IPlayerRepository, ISingletonDependency
    {
        public Task<CreatePlayerResponse> CreatePlayerMethod(CreatePlayerRequest m)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CreatePlayerRequest>> GetAllPlayers()
        {
            try
            {
                await using (var connection = new SqlConnection(GetSettingsFile.ConnectionString))
                {
                    return await connection.QueryAsync<CreatePlayerRequest>(QueryStrings.GetAllPlayer, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}
