using System.Collections.Generic;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class BasketItem
    {
        private int userID;
        private int playID;
        private int performanceID;
        private Seat seat;

        public int UserID { get => userID; set => userID = value; }
        public int PlayID { get => playID; set => playID = value; }
        public int PerformanceID { get => performanceID; set => performanceID = value; }
        internal Seat Seat { get => seat; set => seat = value; }

        public BasketItem()
        {

        }

        public BasketItem(int u, int pID, int perfID, Seat se)
        {
            this.userID = u;
            this.playID = pID;
            this.performanceID = perfID;
            this.seat = se;
        }
        
        
    }
}