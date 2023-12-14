namespace Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string RoomNumber { get; set; }
        public int NumberOfBeds { get; set; }

        public Room(int id, string building, string roomNumber, int numberOfBeds)
        {
            Id = id;
            Building = building;
            RoomNumber = roomNumber;
            NumberOfBeds = numberOfBeds;
        }
    }
}