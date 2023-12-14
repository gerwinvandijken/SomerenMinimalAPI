using System.Data.SqlClient;
using Models;

namespace Repositories
{
    public class StudentRepository
    {
        private SqlConnection dbConnection;

        public StudentRepository(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public List<Student> GetAll()
        {
            dbConnection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Students", dbConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<Student> students = new List<Student>();
            while (reader.Read())
            {
                Student customer = ReadStudent(reader);
                students.Add(customer);
            }
            reader.Close();
            dbConnection.Close();

            return students;
        }

        public Student GetById(int studentId)
        {
            dbConnection.Open();
            SqlCommand command = new SqlCommand(
               "SELECT * FROM Students WHERE Id = @Id", dbConnection);

            command.Parameters.AddWithValue("@Id", studentId);

            SqlDataReader reader = command.ExecuteReader();
            Student student = null;
            if (reader.Read())
            {
                student = ReadStudent(reader);
            }
            reader.Close();
            dbConnection.Close();

            return student;
        }

        public void Add(Student student)
        {
            try
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand(
                   "INSERT INTO Students (FirstName, LastName, Number, EmailAddress) " +
                                "VALUES (@FirstName, @LastName, @Number, @EmailAddress); " +
                                "SELECT SCOPE_IDENTITY();",
                   dbConnection);

                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Number", student.Number);
                command.Parameters.AddWithValue("@EmailAddress", student.EmailAddress);

                student.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                throw new Exception("Adding student failed!!");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void Update(Student student)
        {
            try
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand(
                   "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, " +
                            "Number = @Number, EmailAddress = @EmailAddress " +
                                "WHERE Id = @Id",
                   dbConnection);

                command.Parameters.AddWithValue("@Id", student.Id);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Number", student.Number);
                command.Parameters.AddWithValue("@EmailAddress", student.EmailAddress);

                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records affected!");
            }
            catch
            {
                throw new Exception("Updating student failed!!");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void Delete(int studentId)
        {
            try
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand(
                   "DELETE FROM Students WHERE Id = @Id",
                   dbConnection);

                command.Parameters.AddWithValue("@Id", studentId);

                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records affected!");
            }
            catch
            {
                throw new Exception("Deleting student failed!!");
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private Student ReadStudent(SqlDataReader reader)
        {
            // retrieve data from all fields
            int id = (int)reader["Id"];
            string firstName = (string)reader["FirstName"];
            string lastName = (string)reader["LastName"];
            string number = (string)reader["Number"];
            string emailAddress = (string)reader["EmailAddress"];

            // return new Student object 
            return new Student(id, firstName, lastName, number, emailAddress);
        }
    }
}