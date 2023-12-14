namespace Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public Student(int id, string number, string firstName, string lastName, string emailAddress)
        {
            Id = id;
            Number = number;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
    }
}