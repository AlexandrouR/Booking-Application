using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SystemsDevelopmentGraphicalUserInterface
{
    internal sealed class DBConnection
    {
        private static volatile DBConnection instance = null;

        private SqlConnection connection;

        private DBConnection()
        {
            connection = new SqlConnection("Server=tcp:gre.database.windows.net," +
                "1433;Database=GreTheatre;User ID=sysadmin;Password=Pierino_123;" +
                "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public static DBConnection GetInstance()
        {
            if (instance == null)
            {
                instance = new DBConnection();
            }
            return instance;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User u)
        {
            string query = @"INSERT INTO [User] (Email, Password, Firstname, LastName, Address, PhoneNumber, Partner)
                                  VALUES (@Email, @Password, @Firstname, @LastName, @Address, @PhoneNumber, @Partner)";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(new SqlParameter("Email", u.Email));
            cmd.Parameters.Add(new SqlParameter("Password", u.Password));
            cmd.Parameters.Add(new SqlParameter("Firstname", u.FirstName));
            cmd.Parameters.Add(new SqlParameter("LastName", u.LastName));
            cmd.Parameters.Add(new SqlParameter("Address", u.Address));
            cmd.Parameters.Add(new SqlParameter("PhoneNumber", u.PhoneNumber));
            cmd.Parameters.Add(new SqlParameter("Partner", '0'));
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStaff(Staff s)
        {
            string UserQuery = @"INSERT INTO [User] (Email, Password, Firstname, LastName, Address, PhoneNumber)
                                  VALUES (@Email, @Password, @Firstname, @LastName, @Address, @PhoneNumber)";
            string StaffQuery = @"INSERT INTO [Staff] (accountEnabled, FK_UserMail)
                                  VALUES (@accountEnabled, @FK_UserMail)";
            connection.Open();
            SqlCommand cmd = new SqlCommand(UserQuery, connection);
            cmd.Parameters.Add(new SqlParameter("Email", s.Email));
            cmd.Parameters.Add(new SqlParameter("Password", s.Password));
            cmd.Parameters.Add(new SqlParameter("Firstname", s.FirstName));
            cmd.Parameters.Add(new SqlParameter("LastName", s.LastName));
            cmd.Parameters.Add(new SqlParameter("Address", s.Address));
            cmd.Parameters.Add(new SqlParameter("PhoneNumber", s.PhoneNumber));
            cmd.ExecuteNonQuery();
            SqlCommand scmd = new SqlCommand(StaffQuery, connection);
            scmd.Parameters.Add(new SqlParameter("AccountEnabled", '0'));
            scmd.Parameters.Add(new SqlParameter("FK_UserMail", s.Email));
            scmd.ExecuteNonQuery();
            connection.Close();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckUserEmailExists(string email)
        {
            bool exists = false;
            try
            {
                string query = @"SELECT UserID FROM [User] WHERE Email='" + email + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        exists = true;
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return exists;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckStaffEmailExists(string staff)
        {
            bool isValid = false;

            string query = @"SELECT FK_UserMail FROM [Staff]";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<string> allUsers = new List<string>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    allUsers.Add(dr.GetString(0));
                }
            }

            dr.Close();
            connection.Close();

            foreach (String s in allUsers)
            {
                if (s == staff)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckUserLoginPassword(string user, string password)
        {
            bool isValid = false;

            string query = @"SELECT Email, Password FROM [User]";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            List<string> allUsers = new List<string>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    allUsers.Add(dr.GetString(0) + " " + dr.GetString(1));
                }
            }

            dr.Close();
            connection.Close();
            string auth = user + " " + password;
            foreach (String s in allUsers)
            {
                if (s == auth)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string LoadUserData(string email)
        {
            string query = @"SELECT * FROM [User]";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            string LoggedUser = "";

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr.GetString(1).Contains(email))
                    {
                        LoggedUser = dr.GetInt32(0).ToString() + "$" + dr.GetString(1) + "$" + dr.GetString(2) + "$" +
                                        dr.GetString(3) + "$" + dr.GetString(4) + "$"
                                         + dr.GetString(5) + "$" + dr.GetString(6) + "$" + dr.GetBoolean(7);
                    }
                }
            }

            dr.Close();
            connection.Close();

            return LoggedUser;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string LoadStaffData(string email)
        {
            string query = @"SELECT * FROM [Staff]";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            string LoggedStaff = "";

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr.GetString(2).Contains(email))
                    {
                        LoggedStaff = dr.GetInt32(0).ToString() + "$" + dr.GetBoolean(1) + "$" + dr.GetString(2);
                    }
                }
            }

            dr.Close();
            connection.Close();

            return LoggedStaff;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Play> LoadAllPlays()
        {
            List<Play> RetreivedList = new List<Play>();
            string query = @"SELECT * FROM [Play] WHERE Archived='false'";
            connection.Open();
            // Load all Plays
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RetreivedList.Add(new Play(dr.GetInt32(0), dr.GetString(1), dr.GetString(2),
                                    dr.GetString(3), dr.GetInt32(4), dr.GetInt32(5), dr.GetInt32(6), dr.GetInt32(7), dr.GetBoolean(8)));
                }
            }
            dr.Close();
            connection.Close();
            return RetreivedList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddPlayToDatabase(Play p)
        {
            SqlParameter param = new SqlParameter();
            string insertPlay = @"INSERT INTO [Play] (PlayTitle,PlayDescription,PlayImage, PriceBandA, PriceBandB, PriceBandC, PartnerDiscount)" +
                                        "VALUES (@PlayTitle,@PlayDescription,@PlayImage,@PriceBandA,@PriceBandB,@PriceBandC,@PartnerDiscount)";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(insertPlay, connection);
                cmd.Parameters.Add(new SqlParameter("PlayTitle", p.PlayTitle));
                cmd.Parameters.Add(new SqlParameter("PlayDescription", p.Description));
                cmd.Parameters.Add(new SqlParameter("PlayImage", p.Image));
                cmd.Parameters.Add(new SqlParameter("PriceBandA", p.PriceBandA));
                cmd.Parameters.Add(new SqlParameter("PriceBandB", p.PriceBandB));
                cmd.Parameters.Add(new SqlParameter("PriceBandC", p.PriceBandC));
                cmd.Parameters.Add(new SqlParameter("PartnerDiscount", p.PartnerDiscount));
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Play Created and Added to the Theatre Database");
            }
            catch (SqlException se)
            {
                MessageBox.Show("Error: " + se);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddPerformanceToPlay(Play Play, Performance p)
        {
            string query = @"INSERT INTO [Performance] (PerformanceDate, DayTime, FK_PlayID)
                                  VALUES (@PerformanceDate, @DayTime, @FK_PlayID)";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.Add(new SqlParameter("PerformanceDate", p.PerformanceDate));
                cmd.Parameters.Add(new SqlParameter("DayTime", p.DayTime));
                cmd.Parameters.Add(new SqlParameter("FK_PlayID", Play.PlayID));
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException se)
            {
                MessageBox.Show("Error: " + se);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Performance> LoadAllPerformancesPlay(int PlayID)
        {
            List<Performance> RetreivedList = new List<Performance>();
            string query = @"SELECT * FROM [Performance] WHERE FK_PlayID='" + PlayID + "'";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RetreivedList.Add(new Performance(dr.GetInt32(0), dr.GetString(1), dr.GetString(2)));
                }
            }
            dr.Close();
            connection.Close();
            return RetreivedList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Performance LoadPerformanceData(int PerformanceID)
        {
            Performance RetreivedPerformance = new Performance();
            string query = @"SELECT * FROM [Performance] WHERE PerformanceID='" + PerformanceID + "'";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RetreivedPerformance = new Performance(dr.GetInt32(0), dr.GetString(1), dr.GetString(2));
                }
            }
            dr.Close();
            connection.Close();
            return RetreivedPerformance;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Seat> LoadBookedSeatsForPerformance(Performance p)
        {
            List<Seat> RetreivedList = new List<Seat>();
            List<int> SeatIDs = new List<int>();
            string query = @"SELECT * FROM [SeatPerformance] WHERE FK_PerformanceID='" + p.PerformanceID + "'";
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    SeatIDs.Add(dr.GetInt32(1));
                }
            }
            dr.Close();

            foreach (int i in SeatIDs)
            {
                string GetSeatIDQuery = @"SELECT * FROM [Seat] WHERE SeatID='" + i + "'";
                SqlCommand scmd = new SqlCommand(GetSeatIDQuery, connection);
                SqlDataReader dR = scmd.ExecuteReader();
                if (dR.HasRows)
                {
                    while (dR.Read())
                    {
                        RetreivedList.Add(new Seat(dR.GetInt32(0), dR.GetString(1), dR.GetString(2)));
                    }
                }
                dR.Close();
            }
            connection.Close();
            return RetreivedList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void BookSelectedTicket(Order o, List<BasketItem> userBasket)
        {
            string AddOrderQuery = @"INSERT INTO [Order] (Date, Total, FirstName, LastName,Address,FK_UserID)
                                  VALUES (@Date, @Total, @FirstName, @LastName, @Address, @FK_UserID)";
            connection.Open();
            SqlCommand ocmd = new SqlCommand(AddOrderQuery, connection);
            ocmd.Parameters.Add(new SqlParameter("Date", o.DateOrder));
            ocmd.Parameters.Add(new SqlParameter("Total", o.Total));
            ocmd.Parameters.Add(new SqlParameter("FirstName", o.Firstname));
            ocmd.Parameters.Add(new SqlParameter("LastName", o.Lastname));
            ocmd.Parameters.Add(new SqlParameter("Address", o.Address));
            ocmd.Parameters.Add(new SqlParameter("FK_UserID", o.UserID));
            ocmd.ExecuteNonQuery();
            connection.Close();
            
            foreach (BasketItem bi in userBasket)
            {
                string GetSeatIDQuery = @"SELECT * FROM [Seat] WHERE SeatNumber='" + bi.Seat.SeatNumber + "'";
                connection.Open();
                SqlCommand scmd = new SqlCommand(GetSeatIDQuery, connection);
                SqlDataReader dr = scmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        bi.Seat.SeatID = dr.GetInt32(0);
                    }
                }
                dr.Close();
                connection.Close();
            }

            string GetOrderIDQuery = @"SELECT OrderID FROM [Order] WHERE Date='" + o.DateOrder + "'" + "AND Total='" + o.Total + "'"
                + "AND FirstName='" + o.Firstname + "'" + "AND LastName='" + o.Lastname + "'" + "AND FK_UserID='" + o.UserID + "'";
            connection.Open();
            SqlCommand gocmd = new SqlCommand(GetOrderIDQuery, connection);
            SqlDataReader dreader = gocmd.ExecuteReader();
            if (dreader.HasRows)
            {
                while (dreader.Read())
                {
                    o.OrderID = dreader.GetInt32(0);
                }
            }
            dreader.Close();
            connection.Close();

            foreach (BasketItem bi in userBasket)
            {
                string BookTicketQuery = @"INSERT INTO [SeatPerformance] (FK_SeatID, FK_PerformanceID, FK_OrderID)
                                  VALUES (@FK_SeatID, @FK_PerformanceID, @FK_OrderID)";
                connection.Open();
                SqlCommand cmd = new SqlCommand(BookTicketQuery, connection);
                cmd.Parameters.Add(new SqlParameter("FK_SeatID", bi.Seat.SeatID));
                cmd.Parameters.Add(new SqlParameter("FK_PerformanceID", bi.PerformanceID));
                cmd.Parameters.Add(new SqlParameter("FK_OrderID", o.OrderID));
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GenerateSeats()
        {
            string query = @"INSERT INTO [Seat] (SeatNumber, Band)
                                  VALUES (@SeatNumber, @Band)";
            connection.Open();
            for (int i = 1; i < 51; i++)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.Add(new SqlParameter("SeatNumber", "A" + i));
                cmd.Parameters.Add(new SqlParameter("Band", "A"));
                cmd.ExecuteNonQuery();
            }
            for (int i = 1; i < 41; i++)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.Add(new SqlParameter("SeatNumber", "B" + i));
                cmd.Parameters.Add(new SqlParameter("Band", "B"));
                cmd.ExecuteNonQuery();
            }
            for (int i = 1; i < 51; i++)
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.Add(new SqlParameter("SeatNumber", "C" + i));
                cmd.Parameters.Add(new SqlParameter("Band", "C"));
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddReview(Review review)
        {
            // Add a review 

            string query = @"INSERT INTO [Reviews] (ReviewTitle, DateAdded,ReviewDescription, Score, FK_UserID, FK_PlayID)
                                  VALUES (@Title, @Date ,@Description, @Score, @FK_UserID, @FK_PlayID)";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(new SqlParameter("Title", review.Title));
            cmd.Parameters.Add(new SqlParameter("Date", review.DateAdded));
            cmd.Parameters.Add(new SqlParameter("Description", review.Description));
            cmd.Parameters.Add(new SqlParameter("Score", review.Score));
            cmd.Parameters.Add(new SqlParameter("FK_UserID", review.FK_UserID));
            cmd.Parameters.Add(new SqlParameter("FK_PlayID", review.FK_PlayID));
            cmd.ExecuteNonQuery();
            connection.Close();
        }
          
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Review> LoadReviews(int PlayID)
        {
            List<Review> RetreivedList = new List<Review>();
            string query = @"SELECT * FROM [Reviews] WHERE FK_PlayID='" + PlayID + "'";
          connection.Open();
           // Load all the reviews associated with the play
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RetreivedList.Add(new Review(dr.GetString(0), dr.GetString(1), dr.GetInt32(2),
                                    dr.GetInt32(3), dr.GetInt32(4), dr.GetString(5), dr.GetInt32(6)));
                }
            }
            dr.Close();
            connection.Close();
            return RetreivedList;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Review> LoadSpecificUserReviews(string UserID)
        {
            List<Review> RetreivedList = new List<Review>();
            string query = @"SELECT * FROM [Reviews] WHERE FK_UserID='" + UserID + "'";
            connection.Open();
           // Load all reviews from user
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RetreivedList.Add(new Review(dr.GetString(0), dr.GetString(1), dr.GetInt32(2),
                                    dr.GetInt32(3), dr.GetInt32(4), dr.GetString(5), dr.GetInt32(6)));
                }
            }
            dr.Close();
            connection.Close();
            return RetreivedList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Play> LoadPlaysAUserAttended(User user)
        {
            List<Play> outputListOfPlays = new List<Play>();
            try
            {
                string query = @"SELECT PlayID, PlayTitle FROM [Play] WHERE PlayID IN (SELECT FK_PlayID FROM Performance WHERE PerformanceID IN(SELECT FK_PerformanceID FROM [SeatPerformance] WHERE EXISTS(SELECT * FROM [Order] WHERE  EXISTS( SELECT UserID FROM [User] WHERE UserID ='" + user.UserID + "'))));";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Play playUserAttended = new Play(dataReader.GetInt32(0),dataReader.GetString(1));
                        outputListOfPlays.Add(playUserAttended);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }
            return outputListOfPlays;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Play> LoadPlaysWithReviews()
        {
            List<Play> listofPlaysWithReviews = new List<Play>();
            try
            {
                string query = @"SELECT PlayTitle FROM [Play] WHERE PlayID IN (SELECT FK_PlayID FROM [Reviews])";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Play playUserAttended = new Play(dataReader.GetString(0));
                        listofPlaysWithReviews.Add(playUserAttended);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }
            return listofPlaysWithReviews;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Review> LoadReviewsForAPlay(string playTitle)
        {
            List<Review> listOfReviewsForAPlay = new List<Review>();
            string query = @"SELECT ReviewTitle, DateAdded, ReviewDescription, Score, FK_UserID, FK_PlayID FROM [Reviews] WHERE FK_PlayID in (SELECT PlayID FROM [Play] WHERE PlayTitle ='" + playTitle + "');";
            connection.Open();
            // Load all reviews from user
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    listOfReviewsForAPlay.Add(new Review(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetInt32(3), dr.GetInt32(4), dr.GetInt32(5)));
                }
            }
            dr.Close();
            connection.Close();
            return listOfReviewsForAPlay;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteSpecificReview(Review review)
        {

            try
            {
                string query = @"DELETE FROM [Reviews] WHERE PK_ReviewID ='" + review.PK_ReviewID + "'";
                connection.Open();
                // Delete passed review
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
           

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Order> LoadAllUserOrders(int userID)
        {
            string query = @"SELECT * FROM [Order] WHERE FK_UserID='"+userID+"'";
            List<Order> ListOfOrders = new List<Order>();

            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ListOfOrders.Add(new Order(dr.GetInt32(0), dr.GetString(1), dr.GetFloat(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetInt32(6)));
                }
            }

            dr.Close();

            foreach (Order o in ListOfOrders)
            {
                string seatquery = @"Select FK_SeatID From SeatPerformance where FK_OrderID=" + o.OrderID + "";
                SqlCommand scmd = new SqlCommand(seatquery, connection);
                SqlDataReader dR = scmd.ExecuteReader();

                if (dR.HasRows)
                {
                    o.Seats = new List<Seat>();
                    while (dR.Read())
                    {
                        o.Seats.Add(new Seat(dR.GetInt32(0)));
                    }
                }
                
                dR.Close();
            }

            foreach (Order o in ListOfOrders)
            {
                foreach (Seat s in o.Seats)
                {
                    string GetSeatIDQuery = @"SELECT * FROM [Seat] WHERE SeatID=" + s.SeatID + "";
                    SqlCommand scmd = new SqlCommand(GetSeatIDQuery, connection);
                    SqlDataReader dar = scmd.ExecuteReader();
                    if (dar.HasRows)
                    {
                        while (dar.Read())
                        {
                            s.SeatNumber = dar.GetString(1);
                            s.Band = dar.GetString(2);
                        }
                    }
                    dar.Close();
                }
                
            }
            connection.Close();

            return ListOfOrders;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetUser(int userID)
        {
            string username = "";
            try
            {
                string query = @"SELECT Firstname FROM [User] WHERE UserID IN(Select FK_UserID FROM [Reviews] WHERE FK_UserID ='" + userID + "');";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        username = dataReader.GetString(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }
            return username;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetMostViewedPlayUrl()
        {
            string url = "";
            List<int> performanceID = new List<int>();
            
            //Get all the performancesID from DB into a list of int
            try
            {
                string query = @"SELECT * FROM [SeatPerformance]";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        performanceID.Add(dataReader.GetInt32(2));
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Sort the list
            int temp;
            for (int j = 0; j <= performanceID.Count - 2; j++)
            {
                for (int i = 0; i <= performanceID.Count - 2; i++)
                {
                    if (performanceID[i] > performanceID[i + 1])
                    {
                        temp = performanceID[i + 1];
                        performanceID[i + 1] = performanceID[i];
                        performanceID[i] = temp;
                    }
                }
            }


            //Get Max occurance
            int max_count = 1; 
            int curr_count = 1;
            int res = performanceID[0];
            for (int i = 1; i < performanceID.Count-1; i++)
            {
                if (performanceID[i] == performanceID[i - 1])
                    curr_count++;
                else
                {
                    if (curr_count > max_count)
                    {
                        max_count = curr_count;
                        res = performanceID[i - 1];
                    }
                    curr_count = 1;
                }
            }
            if (curr_count > max_count)
            {
                max_count = curr_count;
                res = performanceID[performanceID.Count - 1];
            }

            try
            {
                string query = @"SELECT PlayImage FROM [Play] where PlayID in(SELECT FK_PlayID FROM [Performance] WHERE PerformanceID='" + res + "');";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        url = dataReader.GetString(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return url;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetMostViewedPlayID()
        {
            int id =0;
            List<int> performanceID = new List<int>();

            //Get all the performancesID from DB into a list of int
            try
            {
                string query = @"SELECT * FROM [SeatPerformance]";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        performanceID.Add(dataReader.GetInt32(2));
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Sort the list
            int temp;
            for (int j = 0; j <= performanceID.Count - 2; j++)
            {
                for (int i = 0; i <= performanceID.Count - 2; i++)
                {
                    if (performanceID[i] > performanceID[i + 1])
                    {
                        temp = performanceID[i + 1];
                        performanceID[i + 1] = performanceID[i];
                        performanceID[i] = temp;
                    }
                }
            }


            //Get Max occurance
            int max_count = 1;
            int curr_count = 1;
            int res = performanceID[0];
            for (int i = 1; i < performanceID.Count - 1; i++)
            {
                if (performanceID[i] == performanceID[i - 1])
                    curr_count++;
                else
                {
                    if (curr_count > max_count)
                    {
                        max_count = curr_count;
                        res = performanceID[i - 1];
                    }
                    curr_count = 1;
                }
            }
            if (curr_count > max_count)
            {
                max_count = curr_count;
                res = performanceID[performanceID.Count - 1];
            }

            try
            {
                string query = @"SELECT FK_PlayID FROM [Performance] WHERE PerformanceID='" + res + "'";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        id = dataReader.GetInt32(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Play GetPlayFromSeatPerformance(int sID, int orderID)
        {
            Play SelectedPlay = new Play();
            try
            {
                string query = @"SELECT * FROM [Play] WHERE PlayID IN (SELECT FK_PlayID FROM Performance WHERE PerformanceID IN(SELECT FK_PerformanceID FROM [SeatPerformance] WHERE FK_SeatID='" + sID + "' AND FK_OrderID='" + orderID +"'))";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        SelectedPlay = new Play(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetString(3), dataReader.GetInt32(4), dataReader.GetInt32(5), dataReader.GetInt32(6), dataReader.GetInt32(7), dataReader.GetBoolean(8));
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return SelectedPlay;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GetTicketsFromSeatPerformance(int orderID, List<Ticket> SelectedTickets)
        {
            try
            {
                string query = @"Select * From SeatPerformance Where FK_OrderID=" + orderID;
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        SelectedTickets.Add(new Ticket(dataReader.GetInt32(0), dataReader.GetInt32(1), dataReader.GetInt32(2), dataReader.GetInt32(3)));
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "...");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Play GetPlayFromSPO(int sID, int pID, int oID)
        {
            Play p = new Play(); ;
            try
            {
                string query = @"SELECT * FROM [Play] WHERE PlayID IN (SELECT FK_PlayID FROM Performance WHERE PerformanceID IN(SELECT FK_PerformanceID FROM [SeatPerformance] WHERE FK_SeatID="+ sID + "AND FK_OrderID="+ oID +"AND FK_PerformanceID="+pID+"))";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        p = new Play(dr.GetInt32(0), dr.GetString(1), dr.GetString(2),
                                    dr.GetString(3), dr.GetInt32(4), dr.GetInt32(5), dr.GetInt32(6), dr.GetInt32(7), dr.GetBoolean(8));
                    }
                }
                dr.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "...");
            }
            return p;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetPerfDateFromSPO(int pID)
        {
            string date = "";
            try
            {
                string query = @"SELECT PerformanceDate,DayTime FROM Performance WHERE PerformanceID=" + pID;
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        date = dataReader.GetString(0) + " - " + dataReader.GetString(1);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }
            return date;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetSeatNumberFromSeatID(int sID)
        {
            string sn = "";
            try
            {
                string query = @"Select SeatNumber From Seat Where SeatID=" + sID;
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        sn = dataReader.GetString(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "...");
            }
            return sn;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetSeatBandFromSeatID(int sID)
        {
            string band = "";
            try
            {
                string query = @"Select Band From Seat Where SeatID=" + sID;
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        band = dataReader.GetString(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "...");
            }
            return band;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetPerformanceDate(int sID, int orderID)
        {
            string date = "";
            try
            {
                string query = @"SELECT PerformanceDate,DayTime FROM Performance WHERE PerformanceID IN(SELECT FK_PerformanceID FROM [SeatPerformance] WHERE FK_SeatID='" + sID + "' AND FK_OrderID='" + orderID + "')";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        date = dataReader.GetString(0) + " - " + dataReader.GetString(1);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return date;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetLastAddedOrderID()
        {
            int oID = 0;
            try
            {
                string query = @"SELECT TOP 1 OrderID FROM [Order] ORDER BY OrderID DESC";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        oID = dataReader.GetInt32(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return oID;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SearchStaffByEmail(string email)
        {
            bool exists = false;
            try
            {
                string query = @"SELECT accountEnabled FROM [Staff] WHERE FK_UserMail='" + email + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        exists = true;
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return exists;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetPartnerStatus(string email)
        {
            bool Partner = false;
            try
            {
                string query = @"SELECT Partner FROM [User] WHERE Email='" + email + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Partner = dataReader.GetBoolean(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return Partner;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetStaffEnabled(string email)
        {
            bool enabled = false;
            try
            {
                string query = @"SELECT accountEnabled FROM [Staff] WHERE FK_UserMail='"+ email+"';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        enabled = dataReader.GetBoolean(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return enabled;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EnableDisableStaffAccount(string email, int status)
        {
            try
            {
                string query = @"UPDATE [Staff] SET accountEnabled = @accountEnabled where FK_UserMail='" + email + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@accountEnabled", status);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EnableDisablePartner(string email, int status)
        {
            try
            {
                string query = @"UPDATE [User] SET Partner = @Partner where Email='" + email + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Partner", status);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public float GetTotalOrder(int OrderID)
        {
            float total = 0.0F;
            try
            {
                string query = @"SELECT Total FROM [Order] WHERE OrderID="+OrderID;
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        total = dataReader.GetFloat(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return total;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetOrderPartnerStatus(int OrderID)
        {
            bool status = false;
            try
            {
                string query = @"Select [Partner] FROM [User] where UserID in (SELECT FK_UserID FROM [Order] WHERE OrderID=" + OrderID+")";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        status = dataReader.GetBoolean(0);
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return status;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SearchOrderByID(int OrderID)
        {
            bool exists = false;
            try
            {
                string query = @"SELECT Lastname FROM [Order] WHERE OrderID='" + OrderID + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        exists = true;
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return exists;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SearchOrderByLastname(string lastname)
        {
            bool exists = false;
            try
            {
                string query = @"SELECT Lastname FROM [Order] WHERE Lastname='" + lastname + "';";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        exists = true;
                    }
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return exists;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Order LoadOrderForTickets(int OrderID)
        {
            Order order = new Order();
            try
            {
                string query = @"Select * FROM [Order] WHERE OrderID=" + OrderID;
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        order = new Order(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetFloat(2), dataReader.GetString(3), dataReader.GetString(4), dataReader.GetString(5), dataReader.GetInt32(6));
                    }
                }
                dataReader.Close();

                
                string seatquery = @"Select FK_SeatID From SeatPerformance where FK_OrderID=" + order.OrderID + "";
                SqlCommand scmd = new SqlCommand(seatquery, connection);
                SqlDataReader dR = scmd.ExecuteReader();

                if (dR.HasRows)
                {
                    order.Seats = new List<Seat>();
                    while (dR.Read())
                    {
                        order.Seats.Add(new Seat(dR.GetInt32(0)));
                    }
                }

                dR.Close();
                
                
                foreach (Seat s in order.Seats)
                {
                    string GetSeatIDQuery = @"SELECT * FROM [Seat] WHERE SeatID=" + s.SeatID + "";
                    SqlCommand tcmd = new SqlCommand(GetSeatIDQuery, connection);
                    SqlDataReader dar = tcmd.ExecuteReader();
                    if (dar.HasRows)
                    {
                        while (dar.Read())
                        {
                            s.SeatNumber = dar.GetString(1);
                            s.Band = dar.GetString(2);
                        }
                    }
                    dar.Close();
                }

                connection.Close();
                
            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return order;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Order> LoadOrderForTickets(string lastname)
        {
            List<Order> orders = new List<Order>();
            try
            {
                string query = @"Select * FROM [Order] WHERE LastName='" + lastname+"'";
                connection.Open();

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        orders.Add(new Order(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetFloat(2), dataReader.GetString(3), dataReader.GetString(4), dataReader.GetString(5), dataReader.GetInt32(6)));
                    }
                }
                dataReader.Close();

                foreach (Order o in orders)
                {
                    string seatquery = @"SELECT * FROM Seat where SeatID in (SELECT FK_SeatID FROM [SeatPerformance] WHERE FK_OrderID='" + o.OrderID + "')";
                    List<Seat> ListOfSeats = new List<Seat>();
                    SqlCommand scmd = new SqlCommand(seatquery, connection);
                    SqlDataReader dr = scmd.ExecuteReader();
                    o.Seats = new List<Seat>();
                    while (dr.Read())
                    {
                        o.Seats.Add(new Seat(dr.GetInt32(0), dr.GetString(1), dr.GetString(2)));
                    }
                    dr.Close();
                }
                
                connection.Close();

            }
            catch (Exception e)
            {

                Console.WriteLine(e + "Load plays user attended");
            }


            return orders;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUserDetails(User updatedUser ,string oldEmail)
        {

            try
            {
                string query = @"UPDATE [User] SET Email = @Email, Password = @Password, Firstname = @Firstname, Lastname = @Lastname , Address = @Address ,PhoneNumber = @PhoneNumber where Email='" + oldEmail + "';";
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", updatedUser.Email);
                cmd.Parameters.AddWithValue("@Password", updatedUser.Password);
                cmd.Parameters.AddWithValue("@Firstname", updatedUser.FirstName);
                cmd.Parameters.AddWithValue("@Lastname", updatedUser.LastName);
                cmd.Parameters.AddWithValue("@Address", updatedUser.Address);
                cmd.Parameters.AddWithValue("@PhoneNumber", updatedUser.PhoneNumber);
               
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EditPlay(Play updatedPlay, String playTitle)
        {
            try
            {
                string query = @"UPDATE [Play] SET PlayTitle = @title, PlayDescription = @description, PlayImage = @image, PriceBandA = @bandA, PriceBandB = @bandB, PriceBandC = @bandC, PartnerDiscount = @discount, Archived = @archived where PlayTitle='" + playTitle +"';";
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@title", updatedPlay.PlayTitle);
                command.Parameters.AddWithValue("@description", updatedPlay.Description);
                command.Parameters.AddWithValue("@image", updatedPlay.Image);
                command.Parameters.AddWithValue("@bandA", updatedPlay.PriceBandA);
                command.Parameters.AddWithValue("@bandB", updatedPlay.PriceBandB);
                command.Parameters.AddWithValue("@bandC", updatedPlay.PriceBandC);
                command.Parameters.AddWithValue("@discount", updatedPlay.PartnerDiscount);
                command.Parameters.AddWithValue("@archived", updatedPlay.Archived);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
            }
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ArchivePlay(string playTitle)
        {
            try
            {
                string query = @"UPDATE [Play] SET Archived = @archived where PlayTitle='" + playTitle + "';";
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@archived", true);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}