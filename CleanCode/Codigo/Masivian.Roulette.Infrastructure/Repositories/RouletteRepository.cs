using Masivian.Roulette.Domain.Config;
using Masivian.Roulette.Domain.Entities;
using Masivian.Roulette.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Masivian.Roulette.Infrastructure.Repositories
{
    public class RouletteRepository : IRouletteRepository
    {

        private readonly IOptions<GlobalSettings> settings;
        public RouletteRepository(IOptions<GlobalSettings> settings)
        {
            this.settings = settings;
        }
        public async Task<List<SpinWheel>> GetAllRoulletes()
        {
            List<SpinWheel> roulettes = new List<SpinWheel>();
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Get_Roulettes", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var roulette = new SpinWheel
                    {
                        Id = !Convert.IsDBNull(reader["KeyRoulette"]) ? reader["KeyRoulette"].ToString() : "",
                        isOpen = Convert.ToBoolean(reader["IsOpen"]),
                        dateOpen = !Convert.IsDBNull(reader["dateOpen"]) ? DateTime.Parse(reader["dateOpen"].ToString()) : new DateTime(),
                        dateClose = !Convert.IsDBNull(reader["dateClose"]) ? DateTime.Parse(reader["dateClose"].ToString()) : new DateTime(),
                    };
                    roulettes.Add(roulette);
                }
            }
            return roulettes;
        }
        public async Task<List<Bet>> getBetsRoulette(string idRoulette)
        {
            List<Bet> bets = new List<Bet>();
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Get_Bets_Roulette", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@KeyRoulette", SqlDbType.VarChar).Value = idRoulette;
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var bet = new Bet
                    {
                        Amount = !Convert.IsDBNull(reader["Amount"]) ? double.Parse(reader["Amount"].ToString()) : 0,
                        TypeBet = (TypeBet)int.Parse(reader["TypeBet"].ToString()),
                        Number = !Convert.IsDBNull(reader["Number"]) ? int.Parse(reader["Number"].ToString()) : 0,
                        BetColor = !Convert.IsDBNull(reader["BetColor"]) ? (ColorBet)int.Parse(reader["BetColor"].ToString()) : ColorBet.Undefined,
                        UserBet = !Convert.IsDBNull(reader["UserBet"]) ? reader["UserBet"].ToString() : "",
                        RouletteId = idRoulette
                    };
                    bets.Add(bet);
                }
            }
            return bets;
        }
        public async Task<string> SaveRoulette(SpinWheel roulette)
        {
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Create_Roulette", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@KeyRoulette", roulette.Id);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return roulette.Id;
        }
        public async Task<SpinWheel> getRoulette(string idRoulette)
        {
            SpinWheel roulette = null;
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Get_Roulette_X_Id", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@KeyRoulette", SqlDbType.VarChar).Value = idRoulette;
                await connection.OpenAsync();
                DataSet dataRoulette = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                await Task.Run(() => adapter.Fill(dataRoulette));
                if (dataRoulette.Tables.Count > 0 & dataRoulette.Tables[0].Rows.Count > 0)
                {
                    roulette = new SpinWheel();
                    roulette.Id = !Convert.IsDBNull(dataRoulette.Tables[0].Rows[0]["KeyRoulette"]) ? dataRoulette.Tables[0].Rows[0]["KeyRoulette"].ToString() : "";
                    roulette.isOpen = !Convert.IsDBNull(dataRoulette.Tables[0].Rows[0]["IsOpen"]) ? Boolean.Parse(dataRoulette.Tables[0].Rows[0]["IsOpen"].ToString()) : false;
                }
            }

            return roulette;
        }
        public async Task<OutcomingMessage> OpenRoulette(SpinWheel roulette)
        {
            OutcomingMessage response;
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Open_Roulette", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@KeyRoulette", roulette.Id);
                command.Parameters.AddWithValue("@dateOpen", roulette.dateOpen);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                response = new OutcomingMessage() { message = "Operación Exitosa" };
            }
            return response;
        }
        public async Task<bool> SaveBet(Bet bet)
        {
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Create_Bet", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@KeyRoulette", bet.RouletteId);
                command.Parameters.AddWithValue("@Amount", bet.Amount);
                command.Parameters.AddWithValue("@TypeBet", bet.TypeBet);
                command.Parameters.AddWithValue("@Number", bet.Number);
                command.Parameters.AddWithValue("@BetColor", bet.BetColor);
                command.Parameters.AddWithValue("@UserBet", bet.UserBet);
                command.Parameters.AddWithValue("@DateBet", bet.DateBet);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return true;
        }


        public async Task<bool> CloseRoulette(SpinWheel roulette)
        {
            bool response;
            using (SqlConnection connection = new SqlConnection(settings.Value.ConnectionsStrings.SqlConnectionString))
            {
                SqlCommand command = new SqlCommand("Masivian_Close_Roulette", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@KeyRoulette", roulette.Id);
                command.Parameters.AddWithValue("@dateClose", roulette.dateClose);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                response = true;
            }
            return response;
        }
    }
}
