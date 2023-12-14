using System.Data.SqlClient;
using Models;

namespace Repositories
{
    public class RoomRepository
    {
        private SqlConnection dbConnection;

        public RoomRepository(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public List<Room> GetAll()
        {
            dbConnection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Rooms", dbConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<Room> rooms = new List<Room>();
            while (reader.Read())
            {
                Room room = ReadRoom(reader);
                rooms.Add(room);
            }
            reader.Close();
            dbConnection.Close();

            return rooms;
        }

        private Room ReadRoom(SqlDataReader reader)
        {
            // retrieve data from all fields
            int id = (int)reader["Id"];
            string building = (string)reader["Building"];
            string number = (string)reader["Number"];
            int numberOfBeds = (int)reader["NumberOfBeds"];

            // return new Room object 
            return new Room(id, building, number, numberOfBeds);
        }
    }
}
