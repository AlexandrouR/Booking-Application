using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsDevelopmentGraphicalUserInterface
{
    class Ticket
    {
        public int SeatPerformanceID;
        public int SeatID;
        public int PerformanceID;
        public int OrderID;

        public Ticket()
        {

        }

        public Ticket(int spID, int sID, int pID, int oID)
        {
            this.SeatPerformanceID = spID;
            this.SeatID = sID;
            this.PerformanceID = pID;
            this.OrderID = oID;
        }
    }
}
