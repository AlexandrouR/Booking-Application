namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Seat
    {
        private int seatID;
        private string seatNumber;
        private string band;

        public int SeatID { get => seatID; set => seatID = value; }
        public string SeatNumber { get => seatNumber; set => seatNumber = value; }
        public string Band { get => band; set => band = value; }

        public Seat()
        {
        }

        public Seat(string sn)
        {
            this.SeatNumber = sn;
        }

        public Seat(int sid)
        {
            this.SeatID = sid;
        }

        public Seat(string sn, string b)
        {
            this.SeatNumber = sn;
            this.Band = b;
        }

        public Seat(int id, string sn, string b)
        {
            this.SeatID = id;
            this.SeatNumber = sn;
            this.Band = b;
        }
    }
}