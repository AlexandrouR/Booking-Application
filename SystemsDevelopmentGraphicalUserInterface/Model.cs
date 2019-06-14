using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsDevelopmentGraphicalUserInterface
{
    class Model
    {
        private User loggedUser;
        private Staff loggedStaff;
        private Play selected_play;
        private Performance selected_performance;
        private List<BasketItem> userBasket;
        private List<BasketItem> staffOrderBasket;
        private List<Play> list_of_plays;
        private List<Performance> list_of_performances;
        private List<Seat> selected_seats;
        List<Play> listOfPlaysUserAttended = new List<Play>();
        List<Review> listOfReviews = new List<Review>();
        List<Order> userOrders;
        List<Ticket> ticketsToPrint = new List<Ticket>();

        public User LoggedUser { get => loggedUser; set => loggedUser = value; }
        public Staff LoggedStaff { get => loggedStaff; set => loggedStaff = value; }
        internal Play Selected_play { get => selected_play; set => selected_play = value; }
        internal Performance Selected_performance { get => selected_performance; set => selected_performance = value; }
        internal List<BasketItem> UserBasket { get => userBasket; set => userBasket = value; }
        internal List<Play> List_of_plays { get => list_of_plays; set => list_of_plays = value; }
        internal List<Performance> List_of_performances { get => list_of_performances; set => list_of_performances = value; }
        internal List<Seat> Selected_seats { get => selected_seats; set => selected_seats = value; }
        internal List<Order> UserOrders { get => userOrders; set => userOrders = value; }
        public List<BasketItem> StaffOrderBasket { get => staffOrderBasket; set => staffOrderBasket = value; }
        public List<Play> ListOfPlaysUserAttended { get => listOfPlaysUserAttended; set => listOfPlaysUserAttended = value; }
        internal List<Ticket> TicketsToPrint { get => ticketsToPrint; set => ticketsToPrint = value; }
        public List<Review> ListOfReviews { get => listOfReviews; set => listOfReviews = value; }

        public Model()
        {
        }
    }
}
