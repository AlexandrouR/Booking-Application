using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Controller
    {
        Model mdlControl = new Model();
        private DBConnection SqlConnection = DBConnection.GetInstance();

        public Controller()
        {
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsStaffAccount(string email)
        {
            bool IsStaff = false;
            string[] SplittedEmail = email.Split('@');
            if (SplittedEmail[1].Equals("spqr.com"))
            {
                IsStaff = true;
            }
            return IsStaff;
        }

        public bool CheckPasswordComplexity(string password)
        {
            string patternPassword = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckIfUserEmailExist(string email)
        {
            return SqlConnection.CheckUserEmailExists(email);
        }

        public bool CheckUserPassword(string email, string password)
        {
            return SqlConnection.CheckUserLoginPassword(email, password);
        }

        public string LoadUserData(string email)
        {
            return SqlConnection.LoadUserData(email);
        }

        public string LoadStaffData(string email)
        {
            return SqlConnection.LoadStaffData(email);
        }

        public bool CheckStaffEmailExists(string email)
        {
            return SqlConnection.CheckStaffEmailExists(email);
        }

        public void AddStaffAccount(Staff newStaff)
        {
            SqlConnection.AddStaff(newStaff);
        }

        public void AddUserAccount(User newUser)
        {
            SqlConnection.AddUser(newUser);
        }

        public void UpdateUserDetails(User newStaff, string oldEmail)
        {
            SqlConnection.UpdateUserDetails(newStaff, oldEmail);

        }

        public List<Seat> GetSelectedSeats()
        {
            return mdlControl.Selected_seats;
        }

        public void ClearSelectedSeats()
        {
            mdlControl.Selected_seats.Clear();
        }

        public void InitialiseSelectedSeats()
        {
            mdlControl.Selected_seats = new List<Seat>();
        }

        public void AddSeatToSelectedSeats(string SeatNumber, string band)
        {
            mdlControl.Selected_seats.Add(new Seat(SeatNumber, band));
        }

        public string AddSeatToSummary()
        {
            string toPrint = "";
            int total = 0;
            foreach (Seat s in mdlControl.Selected_seats)
            {
                if (s.Band == "A")
                {
                    total = total + mdlControl.Selected_play.PriceBandA;
                    toPrint = toPrint + "Selected Seat: " + s.SeatNumber + " - " + "Cost: " + mdlControl.Selected_play.PriceBandA + "£" + "\n";
                }
                if (s.Band == "B")
                {
                    total = total + mdlControl.Selected_play.PriceBandB;
                    toPrint = toPrint + "Selected Seat: " + s.SeatNumber + " - " + "Cost: " + mdlControl.Selected_play.PriceBandB + "£" + "\n";
                }
                if (s.Band == "C")
                {
                    total = total + mdlControl.Selected_play.PriceBandC;
                    toPrint = toPrint + "Selected Seat: " + s.SeatNumber + " - " + "Cost: " + mdlControl.Selected_play.PriceBandC + "£" + "\n";
                }
            }

            toPrint = toPrint + "\n" + "You Total is: " + total.ToString() + "£";
            return toPrint;
        }

        public bool isUserLogged()
        {
            if (mdlControl.LoggedUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isStaffLogged()
        {
            if (mdlControl.LoggedStaff == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public User GetLoggedUser()
        {
            return mdlControl.LoggedUser;
        }

        public Staff GetLoggedUserStaff()
        {
            return mdlControl.LoggedStaff;
        }

        public List<Play> GetAllPlays()
        {
            return mdlControl.List_of_plays;
        }

        public Play GetSelectedPlay()
        {
            return mdlControl.Selected_play;
        }

        public void SetSelectedPlay(Play p)
        {
            mdlControl.Selected_play = p;
        }

        public void LoadAllPerformancesForPlay(int Play)
        {
            mdlControl.List_of_performances = SqlConnection.LoadAllPerformancesPlay(Play);
        }

        public Performance GetSelectedPerformance()
        {
            return mdlControl.Selected_performance;
        }

        public List<Performance> GetAllPerformancesForPlay()
        {
            return mdlControl.List_of_performances;
        }

        public void SetSelectedPerformance(Performance p)
        {
            mdlControl.Selected_performance = p;
        }

        public void AddPlayToDatabase(Play newPlay)
        {
            SqlConnection.AddPlayToDatabase(newPlay);
        }

        public void LoadAllPlays()
        {
            mdlControl.List_of_plays = SqlConnection.LoadAllPlays();
        }

        public int GetMostViewedPlay()
        {
            return SqlConnection.GetMostViewedPlayID();
        }

        public string GetMostViewedPlayUrl()
        {
            return SqlConnection.GetMostViewedPlayUrl();
        }

        public void AddPerformanceToPlay(Play p, Performance newPerformance)
        {
            SqlConnection.AddPerformanceToPlay(p, newPerformance);
        }

        public void LogStaff(int staffID, int userID, string email, string password, string firstName, string lastName, string address, string phoneNumber, bool partner, bool accountEnabled)
        {
            mdlControl.LoggedStaff = new Staff(staffID, userID, email, password, firstName, lastName, address, phoneNumber, partner, accountEnabled);
        }

        public void LogUser(int userID, string email, string password, string firstName, string lastName, string address, string phoneNumber, bool partner)
        {
            mdlControl.LoggedUser = new User(userID, email, password, firstName, lastName, address, phoneNumber, partner);
        }

        public void LogOutStaff()
        {
            mdlControl.LoggedStaff = null;
            mdlControl.StaffOrderBasket = null;
            mdlControl.Selected_seats = null;
            mdlControl.Selected_performance = null;
        }

        public void LogOutUser()
        {
            mdlControl.LoggedUser = null;
            mdlControl.ListOfPlaysUserAttended = null;
            mdlControl.Selected_performance = null;
            mdlControl.Selected_seats = null;
            mdlControl.UserBasket = null;
            mdlControl.UserOrders = null;
        }

        public bool CheckIfStaffExists(string email)
        {
            return SqlConnection.SearchStaffByEmail(email);
        }

        public bool GetStaffEnableStatus(string email)
        {
            return SqlConnection.GetStaffEnabled(email);
        }

        public bool GetPartnerEnableStatus(string email)
        {
            return SqlConnection.GetPartnerStatus(email);
        }

        public void EnableDisableStaffAccount(string email, int v)
        {
            SqlConnection.EnableDisableStaffAccount(email, v);
        }

        public void EnableDisablePartnerAccount(string email, int v)
        {
            SqlConnection.EnableDisablePartner(email, v);
        }

        public void InitializeUserBasket()
        {
            mdlControl.UserBasket = new List<BasketItem>();
        }

        public void ClearUserBasket()
        {
            mdlControl.UserBasket.Clear();
        }

        public void ClearStaffOrderBasket()
        {
            mdlControl.StaffOrderBasket.Clear();
        }

        public void AddToUserBasket(BasketItem b)
        {
            mdlControl.UserBasket.Add(b);
        }

        public void RemoveFromUserBasket(BasketItem b)
        {
            mdlControl.UserBasket.Remove(b);
        }

        public List<BasketItem> GetUserBasket()
        {
            return mdlControl.UserBasket;
        }

        public void AddNewReview(string title, string date, string description, int score, int FK_userID, int FK_playID)
        {
            Review newReview = new Review(title, date, description, score, FK_userID, FK_playID);
            SqlConnection.AddReview(newReview);
        }

        public List<Order> GetUserOrder()
        {
            return mdlControl.UserOrders;
        }

        public Order LoadOrderForTickets(int orderID)
        {
            return SqlConnection.LoadOrderForTickets(orderID);
        }

        public List<Order> LoadListOfOrderForTickets(string lastname)
        {
            return SqlConnection.LoadOrderForTickets(lastname);
        }

        public bool CheckIfOrderExistsByID(int orderID)
        {
            return SqlConnection.SearchOrderByID(orderID);
        }

        public bool CheckIfOrderExistsByLastname(string lastname)
        {
            return SqlConnection.SearchOrderByLastname(lastname);
        }

        public void SetUserOrder()
        {
            mdlControl.UserOrders = SqlConnection.LoadAllUserOrders(mdlControl.LoggedUser.UserID);
        }

        public bool isStaffEnabled()
        {
            if (mdlControl.LoggedStaff.AccountEnabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GenerateUserInvoice(int OrderID)
        {
            Order SelectedOrder = new Order(); ;
            foreach (Order o in mdlControl.UserOrders)
            {
                if (o.OrderID == OrderID)
                {
                    SelectedOrder = o;
                }
            }

            //Fonts
            // If the user does not have the fonts on his computer there might be an error. When generating the tickets from the server or host machine 
            //Change the BaseFont.NOT_EMBEDDED to BaseFont.EMBEDDED. This will increase the size of the pdf file but will embedd the font in the file
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            // Change the path when you are generating it // The PDFTEST.PDF needs to be unique for now otherwise it will override the previous PDF
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FullName;
            System.IO.FileStream fs = new FileStream(path + $"\\Documents\\Invoice{OrderID}.pdf", FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddAuthor("Greenwich Community Theatre");
            document.AddCreator("Greenwich Community Theatre");
            document.AddKeywords("Order Invoice");
            document.AddSubject("Displaying the booking information");
            document.AddTitle("GCT Invoice " + SelectedOrder.OrderID);
            // Open the document to enable you to write to the document
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(f_cn, 20);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Greenwich Community Theatre", 20, 800, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Order id " + SelectedOrder.OrderID, 400, 800, 0);
            cb.SetFontAndSize(f_cn, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Greenwich Community Theatre", 20, 700, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PostCode: SE10 8ES", 20, 685, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Road: Cromss Hill", 20, 670, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "City: London", 20, 655, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Contact Number: 020 8858 7725", 20, 640, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Billing Information", 20, 600, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name: " + SelectedOrder.Firstname + " " + SelectedOrder.Lastname, 20, 585, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Address: " + SelectedOrder.Address, 20, 570, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Contact Number: " + mdlControl.LoggedUser.PhoneNumber, 20, 555, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Email Adress: " + mdlControl.LoggedUser.Email, 20, 540, 0);

            cb.EndText();

            //PdfContentByte cb = writer.DirectContent;
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 550;

            table.AddCell("SEAT NUMBER");
            table.AddCell("PLAY");
            table.AddCell("PERFORMANCE DATE");
            table.AddCell("COST");

            // now we add a cell with rowspan 2
            //This is where we would put the for each loop
            bool identicalSeatId = false;
            for (int i = 0; i < SelectedOrder.Seats.Count; i++)
            {
                for (int j = 0; j < SelectedOrder.Seats.Count - 1; j++)
                {
                    if (SelectedOrder.Seats[i].SeatID == SelectedOrder.Seats[j + 1].SeatID)
                    {
                        identicalSeatId = true;
                        break;
                    }
                }
            }

            if (identicalSeatId == false)
            {
                foreach (Seat s in SelectedOrder.Seats)
                {
                    table.AddCell($"{s.SeatNumber}");
                    Play p = SqlConnection.GetPlayFromSeatPerformance(s.SeatID, SelectedOrder.OrderID);
                    table.AddCell($"{p.PlayTitle}");
                    string date = SqlConnection.GetPerformanceDate(s.SeatID, SelectedOrder.OrderID);
                    table.AddCell($"{date}");
                    int price = 0;
                    if (s.Band == "A")
                    {
                        price = p.PriceBandA;
                    }
                    else if (s.Band == "B")
                    {
                        price = p.PriceBandB;
                    }
                    else if (s.Band == "C")
                    {
                        price = p.PriceBandC;
                    }
                    table.AddCell($"{price}£");
                }
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                float total = SqlConnection.GetTotalOrder(OrderID);
                bool partner = SqlConnection.GetOrderPartnerStatus(OrderID);
                if (partner)
                {
                    table.AddCell($"TOTAL: {total}£ \n ** You have a Partner Discount **");
                }
                else
                {
                    table.AddCell($"TOTAL: {total}£");
                }

                table.WriteSelectedRows(0, -1, 20, 500, cb);
            }
            else
            {
                SqlConnection.GetTicketsFromSeatPerformance(SelectedOrder.OrderID, mdlControl.TicketsToPrint);
                foreach (Ticket sp in mdlControl.TicketsToPrint)
                {
                    string seatNumber = SqlConnection.GetSeatNumberFromSeatID(sp.SeatID);
                    string seatBand = SqlConnection.GetSeatBandFromSeatID(sp.SeatID);
                    table.AddCell($"{seatNumber}");
                    Play p = SqlConnection.GetPlayFromSPO(sp.SeatID, sp.PerformanceID, sp.OrderID);
                    table.AddCell($"{p.PlayTitle}");
                    string date = SqlConnection.GetPerfDateFromSPO(sp.PerformanceID);
                    table.AddCell($"{date}");
                    int price = 0;
                    if (seatBand == "A")
                    {
                        price = p.PriceBandA;
                    }
                    else if (seatBand == "B")
                    {
                        price = p.PriceBandB;
                    }
                    else if (seatBand == "C")
                    {
                        price = p.PriceBandC;
                    }
                    table.AddCell($"{price}£");
                }
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                float total = SqlConnection.GetTotalOrder(OrderID);
                bool partner = SqlConnection.GetOrderPartnerStatus(OrderID);
                if (partner)
                {
                    table.AddCell($"TOTAL: {total}£ \n ** You have a Partner Discount **");
                }
                else
                {
                    table.AddCell($"TOTAL: {total}£");
                }

                table.WriteSelectedRows(0, -1, 20, 500, cb);

            }

            document.Close();
            // Close the writer instance
            writer.Close();
            // Always close open filehandles explicity
            fs.Close();
            System.Diagnostics.Process.Start(path + $@"\\Documents\\Invoice{OrderID}.pdf");
        }

        public void GenerateStaffOrderInvoice(int OrderID)
        {
            Order SelectedOrder = SqlConnection.LoadOrderForTickets(OrderID);

            //Fonts
            // If the user does not have the fonts on his computer there might be an error. When generating the tickets from the server or host machine 
            //Change the BaseFont.NOT_EMBEDDED to BaseFont.EMBEDDED. This will increase the size of the pdf file but will embedd the font in the file
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            // Change the path when you are generating it // The PDFTEST.PDF needs to be unique for now otherwise it will override the previous PDF
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FullName;
            System.IO.FileStream fs = new FileStream(path + $"\\Documents\\Invoice{OrderID}.pdf", FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddAuthor("Greenwich Community Theatre");
            document.AddCreator("Greenwich Community Theatre");
            document.AddKeywords("Order Invoice");
            document.AddSubject("Displaying the booking information");
            document.AddTitle("GCT Invoice " + SelectedOrder.OrderID);
            // Open the document to enable you to write to the document
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(f_cn, 20);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Greenwich Community Theatre", 20, 800, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Order id " + SelectedOrder.OrderID, 400, 800, 0);
            cb.SetFontAndSize(f_cn, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Greenwich Community Theatre", 20, 700, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PostCode: SE10 8ES", 20, 685, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Road: Cromss Hill", 20, 670, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "City: London", 20, 655, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Contact Number: 020 8858 7725", 20, 640, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Billing Information", 20, 600, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name: " + SelectedOrder.Firstname + " " + SelectedOrder.Lastname, 20, 585, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Address: " + SelectedOrder.Address, 20, 570, 0);

            cb.EndText();

            //PdfContentByte cb = writer.DirectContent;
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 550;

            table.AddCell("SEAT NUMBER");
            table.AddCell("PLAY");
            table.AddCell("PERFORMANCE DATE");
            table.AddCell("COST");

            bool identicalSeatId = false;
            for (int i = 0; i < SelectedOrder.Seats.Count; i++)
            {
                for (int j = 0; j < SelectedOrder.Seats.Count - 1; j++)
                {
                    if (SelectedOrder.Seats[i].SeatID == SelectedOrder.Seats[j + 1].SeatID)
                    {
                        identicalSeatId = true;
                        break;
                    }
                }
            }
            // now we add a cell with rowspan 2
            //This is where we would put the for each loop
            if (identicalSeatId == false)
            {
                foreach (Seat s in SelectedOrder.Seats)
                {
                    table.AddCell($"{s.SeatNumber}");
                    Play p = SqlConnection.GetPlayFromSeatPerformance(s.SeatID, SelectedOrder.OrderID);
                    table.AddCell($"{p.PlayTitle}");
                    string date = SqlConnection.GetPerformanceDate(s.SeatID, SelectedOrder.OrderID);
                    table.AddCell($"{date}");
                    int price = 0;
                    if (s.Band == "A")
                    {
                        price = p.PriceBandA;
                    }
                    else if (s.Band == "B")
                    {
                        price = p.PriceBandB;
                    }
                    else if (s.Band == "C")
                    {
                        price = p.PriceBandC;
                    }
                    table.AddCell($"{price}£");
                }
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                float total = SqlConnection.GetTotalOrder(OrderID);
                table.AddCell($"TOTAL: {total}£");

                table.WriteSelectedRows(0, -1, 20, 500, cb);
            }
            else
            {
                SqlConnection.GetTicketsFromSeatPerformance(SelectedOrder.OrderID, mdlControl.TicketsToPrint);
                foreach (Ticket sp in mdlControl.TicketsToPrint)
                {
                    string seatNumber = SqlConnection.GetSeatNumberFromSeatID(sp.SeatID);
                    string seatBand = SqlConnection.GetSeatBandFromSeatID(sp.SeatID);
                    table.AddCell($"{seatNumber}");
                    Play p = SqlConnection.GetPlayFromSPO(sp.SeatID, sp.PerformanceID, sp.OrderID);
                    table.AddCell($"{p.PlayTitle}");
                    string date = SqlConnection.GetPerfDateFromSPO(sp.PerformanceID);
                    table.AddCell($"{date}");
                    int price = 0;
                    if (seatBand == "A")
                    {
                        price = p.PriceBandA;
                    }
                    else if (seatBand == "B")
                    {
                        price = p.PriceBandB;
                    }
                    else if (seatBand == "C")
                    {
                        price = p.PriceBandC;
                    }
                    table.AddCell($"{price}£");
                }
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                float total = SqlConnection.GetTotalOrder(OrderID);
                bool partner = SqlConnection.GetOrderPartnerStatus(OrderID);
                if (partner)
                {
                    table.AddCell($"TOTAL: {total}£ \n ** You have a Partner Discount **");
                }
                else
                {
                    table.AddCell($"TOTAL: {total}£");
                }

                table.WriteSelectedRows(0, -1, 20, 500, cb);

            }


            document.Close();
            // Close the writer instance
            writer.Close();
            // Always close open filehandles explicity
            fs.Close();
            System.Diagnostics.Process.Start(path + $@"\\Documents\\Invoice{OrderID}.pdf");
        }

        public List<BasketItem> GetStaffOrderBasket()
        {
            return mdlControl.StaffOrderBasket;
        }

        public void AddToStaffOrderBasket(BasketItem bi)
        {
            mdlControl.StaffOrderBasket.Add(bi);
        }

        public void InitialiseStaffOrderBasket(List<BasketItem> bi)
        {
            mdlControl.StaffOrderBasket = bi;
        }

        public void CreateTickets(Order order)
        {
            //Fonts
            // If the user does not have the fonts on his computer there might be an error. When generating the tickets from the server or host machine 
            //Change the BaseFont.NOT_EMBEDDED to BaseFont.EMBEDDED. This will increase the size of the pdf file but will embedd the font in the file
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            // Change the path when you are generating it // The PDFTEST.PDF needs to be unique for now otherwise it will override the previous PDF
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FullName;
            System.IO.FileStream fs = new FileStream(path + $"\\Documents\\Tickets{order.OrderID}.pdf", FileMode.Create);
            Document document = new Document(PageSize.A7, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddAuthor("Greenwich Community Theatre");
            document.AddCreator("Greenwich Community Theatre");
            document.AddKeywords("PDF Performance Ticket");
            document.AddSubject("Displaying the booking information");
            document.AddTitle("Change the title of the Ticket HERE");
            // Open the document to enable you to write to the document
            document.Open();

            bool identicalSeatId = false;
            for (int i = 0; i < order.Seats.Count; i++)
            {
                for (int j = 0; j < order.Seats.Count - 1; j++)
                {
                    if (order.Seats[i].SeatID == order.Seats[j + 1].SeatID)
                    {
                        identicalSeatId = true;
                        break;
                    }
                }
            }
            if (identicalSeatId == false)
            {
                foreach (Seat s in order.Seats)
                {
                    document.NewPage();
                    PdfContentByte cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(f_cn, 15);
                    Play p = SqlConnection.GetPlayFromSeatPerformance(s.SeatID, order.OrderID);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Order Number: " + order.OrderID, 40, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name: " + order.Firstname + " " + order.Lastname, 60, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Play: " + p.PlayTitle, 80, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Performance Date: " + SqlConnection.GetPerformanceDate(s.SeatID, order.OrderID), 100, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Seat: " + s.SeatNumber, 120, 10, 90);
                    int price = 0;
                    if (s.Band == "A")
                    {
                        price = p.PriceBandA;
                    }
                    else if (s.Band == "B")
                    {
                        price = p.PriceBandB;
                    }
                    else if (s.Band == "C")
                    {
                        price = p.PriceBandC;
                    }
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ticket Price: " + price + "£", 140, 10, 90);
                    cb.EndText();
                    // Set line width
                    cb.SetLineWidth(0f);
                    // From x,y
                    cb.MoveTo(170, 0);
                    // To x,y
                    cb.LineTo(170, 245);
                    // Draw line
                    cb.Stroke();
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("https://i.postimg.cc/8cwHhnfZ/spqr-png-3.png");
                    img.SetAbsolutePosition(150, 230);
                    img.ScaleAbsolute(60, 60);
                    cb.AddImage(img);
                }
            }
            else
            {
                SqlConnection.GetTicketsFromSeatPerformance(order.OrderID, mdlControl.TicketsToPrint);
                foreach (Ticket sp in mdlControl.TicketsToPrint)
                {
                    document.NewPage();
                    PdfContentByte cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(f_cn, 15);
                    Play p = SqlConnection.GetPlayFromSPO(sp.SeatID, sp.PerformanceID, sp.OrderID);
                    string seatBand = SqlConnection.GetSeatBandFromSeatID(sp.SeatID);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Order Number: " + order.OrderID, 40, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Name: " + order.Firstname + " " + order.Lastname, 60, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Play: " + p.PlayTitle, 80, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Performance Date: " + SqlConnection.GetPerfDateFromSPO(sp.PerformanceID), 100, 10, 90);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Seat: " + SqlConnection.GetSeatNumberFromSeatID(sp.SeatID), 120, 10, 90);
                    int price = 0;
                    if (seatBand == "A")
                    {
                        price = p.PriceBandA;
                    }
                    else if (seatBand == "B")
                    {
                        price = p.PriceBandB;
                    }
                    else if (seatBand == "C")
                    {
                        price = p.PriceBandC;
                    }
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Ticket Price: " + price + "£", 140, 10, 90);
                    cb.EndText();
                    // Set line width
                    cb.SetLineWidth(0f);
                    // From x,y
                    cb.MoveTo(170, 0);
                    // To x,y
                    cb.LineTo(170, 245);
                    // Draw line
                    cb.Stroke();
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("https://i.postimg.cc/8cwHhnfZ/spqr-png-3.png");
                    img.SetAbsolutePosition(150, 230);
                    img.ScaleAbsolute(60, 60);
                    cb.AddImage(img);
                }
            }
        
            document.Close();
            System.Diagnostics.Process.Start(path + $@"\\Documents\\Tickets{order.OrderID}.pdf");
        }

        public void LoadAllPlaysUserAttended()
        {
            mdlControl.ListOfPlaysUserAttended = SqlConnection.LoadPlaysAUserAttended(mdlControl.LoggedUser);
        }

        public List<Play> AllPlaysUserAttended()
        {
            return mdlControl.ListOfPlaysUserAttended;
        }

        public void AddOrderToBatabase(Order newOrder,List<BasketItem> basket)
        {
            SqlConnection.BookSelectedTicket(newOrder, basket);
        }

        public int GetLastAddedOrder()
        {
            int orderID = 0;
            orderID = SqlConnection.GetLastAddedOrderID();
            return orderID;
        }

        public List<Performance> LoadAllPerformancesByPlayID(int playID)
        {
            return SqlConnection.LoadAllPerformancesPlay(playID);
        }

        public List<Play> ReturnPlaysWithReview()
        {
            return SqlConnection.LoadPlaysWithReviews();
        }

        public List<Review> LoadReviewsForAPlay(string playTitle)
        {
            return mdlControl.ListOfReviews = SqlConnection.LoadReviewsForAPlay(playTitle);
        }

        public List<Seat> LoadBookedSeatsForPerformance()
        {
            return SqlConnection.LoadBookedSeatsForPerformance(mdlControl.Selected_performance);
        }

        public string GetUserOfReview(int FK_UserID)
        {
            return SqlConnection.GetUser(FK_UserID);
        }

        public void UpdatePlay(Play updatedPlay, string playTitle)
        {
            SqlConnection.EditPlay(updatedPlay, playTitle);
        }

        public void ArchivePlay(string playTitle)
        {
            SqlConnection.ArchivePlay(playTitle);
        }
    }
}