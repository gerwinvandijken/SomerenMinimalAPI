using System.Data.SqlClient;
using Models;

namespace Repositories
{
    public class LecturerRepository
    {
        private SqlConnection dbConnection;

        public LecturerRepository(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public List<Lecturer> GetAll()
        {
            dbConnection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Lecturers", dbConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<Lecturer> lecturers = new List<Lecturer>();
            while (reader.Read())
            {
                Lecturer lecturer = ReadLecturer(reader);
                lecturers.Add(lecturer);
            }
            reader.Close();
            dbConnection.Close();

            return lecturers;
        }

        public Lecturer GetById(int lecturerId)
        {
            dbConnection.Open();
            SqlCommand command = new SqlCommand(
               "SELECT * FROM Lecturers WHERE Id = @Id", dbConnection);

            command.Parameters.AddWithValue("@Id", lecturerId);

            SqlDataReader reader = command.ExecuteReader();
            Lecturer lecturer = null;
            if (reader.Read())
            {
                lecturer = ReadLecturer(reader);
            }
            reader.Close();
            dbConnection.Close();

            return lecturer;
        }

        public void Add(Lecturer lecturer)
        {
            try
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand(
                   "INSERT INTO Lecturers (FirstName, LastName, EmailAddress) " +
                                "VALUES (@FirstName, @LastName, @EmailAddress); " +
                                "SELECT SCOPE_IDENTITY();",
                   dbConnection);

                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@EmailAddress", lecturer.EmailAddress);

                lecturer.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                throw new Exception("Adding lecturer failed!!");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void Update(Lecturer lecturer)
        {
            try
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand(
                   "UPDATE Lecturers SET FirstName = @FirstName, LastName = @LastName, " +
                            "Number = @Number, EmailAddress = @EmailAddress " +
                                "WHERE Id = @Id",
                   dbConnection);

                command.Parameters.AddWithValue("@Id", lecturer.Id);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@EmailAddress", lecturer.EmailAddress);

                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records affected!");
            }
            catch
            {
                throw new Exception("Updating lecturer failed!!");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void Delete(int lecturerId)
        {
            try
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand(
                   "DELETE FROM Lecturers WHERE Id = @Id",
                   dbConnection);

                command.Parameters.AddWithValue("@Id", lecturerId);

                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records affected!");
            }
            catch
            {
                throw new Exception("Deleting lecturer failed!!");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private Lecturer ReadLecturer(SqlDataReader reader)
        {
            // retrieve data from all fields
            int id = (int)reader["Id"];
            string firstName = (string)reader["FirstName"];
            string lastName = (string)reader["LastName"];
            string emailAddress = (string)reader["EmailAddress"];

            // return new Lecturer object 
            return new Lecturer(id, firstName, lastName, emailAddress);
        }
    }
}