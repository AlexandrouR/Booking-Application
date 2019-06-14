using System.Collections.Generic;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Performance
    {
        //private List<Ticket> Tickets;
        //private int RemainingTickets { get; set; }

        private int performanceID;
        private string performanceDate;
        private string dayTime;
        private List<Seat> bookedSeats;


        public Performance()
        {
        }

        public Performance(string time, string dTime)
        {
            this.performanceDate = time;
            this.dayTime = dTime;
        }

        public Performance(int id, string time, string dTime)
        {
            this.performanceID = id;
            this.performanceDate = time;
            this.dayTime = dTime;
        }

        public int PerformanceID { get => performanceID; set => performanceID = value; }
        public string PerformanceDate { get => performanceDate; set => performanceDate = value; }
        public string DayTime { get => dayTime; set => dayTime = value; }
        internal List<Seat> Seats { get => bookedSeats; set => bookedSeats = value; }
    }
}