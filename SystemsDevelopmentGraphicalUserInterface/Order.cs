using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Order
    {
        private int orderID;
        private string dateOrder;
        private float total;
        private string firstname;
        private string lastname;
        private string address;
        private int userID;
        private List<Seat> seats;

        public Order()
        {

        }

        public Order(int oID, string dO, float t, string fN, string lN, string a, int uID)
        {
            this.orderID = oID;
            this.dateOrder = dO;
            this.total = t;
            this.firstname = fN;
            this.lastname = lN;
            this.address = a;
            this.userID = uID;
        }

        public Order(string dO, float t, string fN, string lN, string a, int uID, List<Seat> s)
        {
            this.dateOrder = dO;
            this.total = t;
            this.firstname = fN;
            this.lastname = lN;
            this.address = a;
            this.userID = uID;
            this.seats = s;
        }

        public Order(int oID, string dO, float t, string fN, string lN, string a, int uID, List<Seat> s)
        {
            this.orderID = oID;
            this.dateOrder = dO;
            this.total = t;
            this.firstname = fN;
            this.lastname = lN;
            this.address = a;
            this.userID = uID;
            this.seats = s;
        }

        public int OrderID { get => orderID; set => orderID = value; }
        public string DateOrder { get => dateOrder; set => dateOrder = value; }
        public float Total { get => total; set => total = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Address { get => address; set => address = value; }
        public int UserID { get => userID; set => userID = value; }
        internal List<Seat> Seats { get => seats; set => seats = value; }
    }
}
