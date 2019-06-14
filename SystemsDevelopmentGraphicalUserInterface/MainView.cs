using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public partial class MainView : Form
    {
        // Pattern instances
        private Controller ctrl = new Controller();

        // Variables used to update GUI
        private int countMaxMin = 0;
        private bool SeatingLayoutCreated = false;
        private bool StaffSeatingLayoutCreated = false;
        private bool ItemsAddedToBasket = false;
        private bool StaffItemsAddedToBasket = false;
        private bool CashAtTheTill = false;
        float OrderTotal;

        public MainView()
        {
            InitializeComponent();
            ctrl.LoadAllPlays();
            OpenHomePanel();
        }

        //--------------------- BUTTON ACTIONS -------------------------
        private void PlaysButton_Click(object sender, EventArgs e)
        {
            CloseAllPanels();
            PlayView_Panel.Visible = true;

            if (staffOptionsPanel.Visible == true)
            {
                staffOptionsPanel.Visible = false;
            }
            changeStaffLocation();

            currentSeparatorPanel.Visible = true;

            foreach (TableLayoutPanel tlp in PlayTable_Layout.Controls)
            {
                PlayTable_Layout.Controls.Remove(tlp);
            }

            TableLayoutPanel PlaysTableLayout = new TableLayoutPanel();
            PlaysTableLayout.RowStyles.Clear();
            PlaysTableLayout.Controls.Clear();
            PlaysTableLayout.Size = new Size(PlayTable_Layout.Width - 25, PlayTable_Layout.Height - 20);
            PlaysTableLayout.ColumnCount = 5;
            PlaysTableLayout.RowCount = ctrl.GetAllPlays().Count;

            PlaysTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            PlaysTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            PlaysTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500));
            PlaysTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            PlaysTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));

            foreach (Play p in ctrl.GetAllPlays())
            {
                // Adds a row
                PlaysTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / PlaysTableLayout.RowCount));
                // Add the image
                PlaysTableLayout.Controls.Add(new PictureBox()
                {
                    Name = p.PlayTitle + "PBox",
                    ImageLocation = p.Image,
                    Size = new Size(200, 100),
                    SizeMode = PictureBoxSizeMode.StretchImage
                });

                // Adds the title - This columns size has to be dependent on the size of the text that the database holds for the description in order to fit in the tablelayout panel cell
                PlaysTableLayout.Controls.Add(new Label()
                {
                    Text = p.PlayTitle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true,
                    Anchor = AnchorStyles.None,
                    Font = new Font("Palatino Linotype", 14)
                });

                // Adds the description
                PlaysTableLayout.Controls.Add(new Label()
                {
                    Text = p.Description,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true,
                    Size = new Size(50, 50),
                    Anchor = AnchorStyles.None,
                    Font = new Font("Palatino Linotype", 10)
                });

                // Add the review button
                Button NewReviewButton = new Button();
                NewReviewButton.Click += (s, ev) => { OpenReviewForAPlayPanel(p.PlayID); };
                NewReviewButton.Text = "Check Reviews";
                NewReviewButton.TextAlign = ContentAlignment.MiddleCenter;
                NewReviewButton.Anchor = AnchorStyles.None;
                NewReviewButton.Size = new Size(60, 40);

                PlaysTableLayout.Controls.Add(NewReviewButton);
                NewReviewButton.Name = p.PlayTitle + "Review_Button";

                // Add the book button
                Button NewBookButton = new Button();
                NewBookButton.Click += (s, ev) => { OpenBookingPanel(p.PlayID); };
                NewBookButton.Text = "Book Tickets";
                NewBookButton.TextAlign = ContentAlignment.MiddleCenter;
                NewBookButton.Anchor = AnchorStyles.None;
                NewBookButton.Size = new Size(60, 40);
                PlaysTableLayout.Controls.Add(NewBookButton);
                NewBookButton.Name = p.PlayTitle + "Book_Button";
            }
            PlayTable_Layout.Controls.Add(PlaysTableLayout);

            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorPanel.Height = Plays_Button.Height;
            currentSeparatorPanel.Top = Plays_Button.Top;
            userOptionsPanel.Visible = false;
            currentSeparatorUserPanel.Visible = false;
        }

        private void AboutUsButton_Click(object sender, EventArgs e)
        {
            if (staffOptionsPanel.Visible == true)
            {
                staffOptionsPanel.Visible = false;
            }
            changeStaffLocation();
            currentSeparatorPanel.Visible = true;

            CloseAllPanels();
            AboutUs_Panel.Visible = true;
            Map_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorPanel.Height = AboutUs_Button.Height;
            currentSeparatorPanel.Top = AboutUs_Button.Top;
            userOptionsPanel.Visible = false;
            currentSeparatorUserPanel.Visible = false;
        }

        private void UserAreaButton_Click(object sender, EventArgs e)
        {
            if (ctrl.isUserLogged() == false)
            {
                MessageBox.Show("You are not logged in");
            }
            else
            {
                if (staffOptionsPanel.Visible == true)
                {
                    staffOptionsPanel.Visible = false;
                }
                changeStaffLocation();

                setAddEditRemoveButtonsVisibility(false);
                currentSeparatorPanel.Height = UserArea_Button.Height;
                currentSeparatorPanel.Top = UserArea_Button.Top;
                userOptionsPanel.Visible = true;
                currentSeparatorUserPanel.Visible = true;
                StaffArea_Button.Location = new Point(9, 308);
                userOptionsPanel.Location = new Point(0, 179);
            }
        }

        private void StaffAreaButton_Click(object sender, EventArgs e)
        {
            if (ctrl.isStaffLogged() == false)
            {
                MessageBox.Show("Access Denied - You are not logged in as a Staff Member");
            }

            else
            {
                if (ctrl.isStaffEnabled() == false)
                {
                    MessageBox.Show("Access Denied - You staff account is not enabled");
                }
                else
                {
                    changeStaffLocation();
                    currentSeparatorPanel.Visible = true;
                    setAddEditRemoveButtonsVisibility(true);
                    currentSeparatorPanel.Height = StaffArea_Button.Height;
                    currentSeparatorPanel.Top = StaffArea_Button.Top;
                    userOptionsPanel.Visible = false;
                    currentSeparatorUserPanel.Visible = false;
                    staffOptionsPanel.Location = new Point(0, 220);
                    staffOptionsPanel.Visible = true;
                }
            }
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            if (staffOptionsPanel.Visible == true)
            {
                staffOptionsPanel.Visible = false;
            }

            OpenHomePanel();
            changeStaffLocation();
            currentSeparatorPanel.Visible = true;
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorPanel.Height = Home_Button.Height; //this takes the separator to
            currentSeparatorPanel.Top = Home_Button.Top;     //the current button
            userOptionsPanel.Visible = false;
            currentSeparatorUserPanel.Visible = false;
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            currentSeparatorUserPanel.Height = History_Button.Height;
            currentSeparatorUserPanel.Top = History_Button.Top;
            CloseAllPanels();
            ctrl.SetUserOrder();
            ListViewItem item;
            Order_ListView.Visible = true;
            Order_ListView.Items.Clear();

            foreach (Order o in ctrl.GetUserOrder())
            {
                string[] array = new string[7];
                array[0] = o.OrderID.ToString();
                array[1] = o.DateOrder;
                array[2] = o.Total.ToString();
                array[3] = o.Firstname;
                array[4] = o.Lastname;
                array[5] = o.Address;
                int c = 0;
                foreach (Seat s in o.Seats)
                {
                    c++;
                }
                array[6] = c.ToString();
                item = new ListViewItem(array);
                Order_ListView.Items.Add(item);
            }
            History_Panel.Visible = true;
        }

        private void YourAccountButton_Click(object sender, EventArgs e)
        {
            currentSeparatorUserPanel.Height = YourAccount_Button.Height;
            currentSeparatorUserPanel.Top = YourAccount_Button.Top;
            setAddEditRemoveButtonsVisibility(false);
            CloseAllPanels();
            UserAccount_Panel.Visible = true;
            UserUpdateFN.Text = ctrl.GetLoggedUser().FirstName;
            UserUpdateLN.Text = ctrl.GetLoggedUser().LastName;
            UserUpdateEmail.Text = ctrl.GetLoggedUser().Email;
            UserUpdateAddress.Text = ctrl.GetLoggedUser().Address;
            UserUpdatePhone.Text = ctrl.GetLoggedUser().PhoneNumber;
        }

        private void YourBasketButton_Click(object sender, EventArgs e)
        {
            double total = 0;
            currentSeparatorUserPanel.Height = YourBasket_Button.Height;
            currentSeparatorUserPanel.Top = YourBasket_Button.Top;
            CloseAllPanels();
            Basket_ListView.Items.Clear(); 
            ListViewItem itm;
            try
            {
                foreach (BasketItem bi in ctrl.GetUserBasket())
                {
                    string[] arr = new string[5];
                    foreach (Play p in ctrl.GetAllPlays())
                    {
                        if (p.PlayID == bi.PlayID)
                        {
                            arr[0] = p.PlayTitle;
                            if (bi.Seat.Band == "A")
                            {
                                arr[4] = p.PriceBandA.ToString();
                                total = total + p.PriceBandA;
                            }
                            else if (bi.Seat.Band == "B")
                            {
                                arr[4] = p.PriceBandB.ToString();
                                total = total + p.PriceBandB;
                            }
                            else if (bi.Seat.Band == "C")
                            {
                                arr[4] = p.PriceBandC.ToString() + "£";
                                total = total + p.PriceBandC;
                            }
                            p.Performances = ctrl.LoadAllPerformancesByPlayID(p.PlayID);
                            foreach (Performance perf in p.Performances)
                            {
                                if (perf.PerformanceID == bi.PerformanceID)
                                {
                                    arr[1] = perf.PerformanceDate;
                                    arr[2] = perf.DayTime;
                                }
                            }
                        }
                    }

                    arr[3] = bi.Seat.SeatNumber;
                    itm = new ListViewItem(arr);
                    Basket_ListView.Items.Add(itm);
                }
                if (Basket_ListView.Items.Count >= 20)
                {
                    double discount = 0.05;
                    total = total - (total * discount);
                    TotalOrder_Label.Text = "Your total order: " + total + "£. With an applied discount of 5%.";
                } else
                {
                    TotalOrder_Label.Text = "Your total order: " + total + "£";
                }
                Basket_Panel.Visible = true;
            }
            catch (NullReferenceException nre)
            {
                MessageBox.Show("The basket is empty");
            }

        }

        private void CreateReview_Button_Click(object sender, EventArgs e)
        {
            currentSeparatorUserPanel.Height = AddReview_Button.Height;
            currentSeparatorUserPanel.Top = AddReview_Button.Top;
            PlaysUserAttendedCB.Items.Clear();
            CloseAllPanels();
            ctrl.LoadAllPlaysUserAttended();

            foreach (Play play in ctrl.AllPlaysUserAttended())
            {
                PlaysUserAttendedCB.Items.Add(play.PlayTitle);
            }
            CreateReviewPanel.Visible = true;
        }

        private void ManagePlaysButton_Click(object sender, EventArgs e)
        {
            setAddEditRemoveButtonsVisibility(true);
            currentSeparatorStaffPanel.Top = ManagePlays_Button.Top;
            CloseAllPanels();
        }

        private void ManageReviewsButton_Click(object sender, EventArgs e)
        {
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorStaffPanel.Height = ManageReviews_Button.Height;
            currentSeparatorStaffPanel.Top = ManageReviews_Button.Top;
            CloseAllPanels();
            AllPlaysReviewsCB.Items.Clear();
            //populates combobox with only the plays that actually have at least one review
            List<Play> listOfPlaysWithReviews = ctrl.ReturnPlaysWithReview();
            foreach (Play play in listOfPlaysWithReviews)
            {
                AllPlaysReviewsCB.Items.Add(play.PlayTitle);
            }
            ReviewsPanel.Visible = true;
        }

        private void PrintTickets_Button_Click(object sender, EventArgs e)
        {
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorStaffPanel.Height = PrintTickets_Button.Height;
            currentSeparatorStaffPanel.Top = PrintTickets_Button.Top;
            SearchOrder_ListView.Items.Clear();
            SearchOrderID_TextBox.Enabled = true;
            SearchOrderID_TextBox.Text = "";
            CloseAllPanels();
            PrintTickets_Panel.Visible = true;
        }

        private void PlaceOrderButton_Click(object sender, EventArgs e)
        {
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorStaffPanel.Height = placeOrderButton.Height;
            currentSeparatorStaffPanel.Top = placeOrderButton.Top;
            CloseAllPanels();
            OpenStaffBookingPanel();
        }

        private void MyAccountButton_Click(object sender, EventArgs e)
        {
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorStaffPanel.Height = myAccountButton.Height;
            currentSeparatorStaffPanel.Top = myAccountButton.Top;
            CloseAllPanels();
            ManageStaff_Panel.Visible = true;
            StaffUpdateFN.Text = ctrl.GetLoggedUserStaff().FirstName;
            StaffUpdateLN.Text = ctrl.GetLoggedUserStaff().LastName;
            StaffUpdateEMAIL.Text = ctrl.GetLoggedUserStaff().Email;
            StaffUpdateADD.Text = ctrl.GetLoggedUserStaff().Address;
            StaffUpdatePHONE.Text = ctrl.GetLoggedUserStaff().PhoneNumber;



        }

        private void Add_Play_Button_Click(object sender, EventArgs e)
        {
            CloseAllPanels();
            PlayManagement_Panel.Visible = true;
        }

        private void SignIn_Button_Click(object sender, EventArgs e)
        {
            if (!ctrl.IsValidEmail(LoginUsername_Textbox.Text))
            {
                MessageBox.Show("Email format not valid");
            }
            else if (!ctrl.CheckIfUserEmailExist(LoginUsername_Textbox.Text))
            {
                MessageBox.Show("User not found");
            }
            else if (!ctrl.CheckUserPassword(LoginUsername_Textbox.Text, LoginPassword_Textbox.Text))
            {
                MessageBox.Show("Password not correct");
            }
            else
            {
                string[] userToInst = ctrl.LoadUserData(LoginUsername_Textbox.Text).Split('$');
                if (ctrl.IsStaffAccount(LoginUsername_Textbox.Text))
                {
                    if (ctrl.CheckStaffEmailExists(LoginUsername_Textbox.Text))
                    {
                        string[] staffToInst = ctrl.LoadStaffData(LoginUsername_Textbox.Text).Split('$');
                        Boolean IsEnabled = true;
                        if (staffToInst[1] == "False")
                        {
                            IsEnabled = false;
                        }
                        bool partner = false;
                        if (userToInst[7] == "True")
                        {
                            partner = true;
                        }
                        ctrl.LogStaff(Convert.ToInt32(staffToInst[0]), Convert.ToInt32(userToInst[0]), userToInst[1], userToInst[2], userToInst[3], userToInst[4], userToInst[5], userToInst[6], partner, IsEnabled);
                        MessageBox.Show("Login Succesfull for " + ctrl.GetLoggedUserStaff().Email);
                        ctrl.InitialiseStaffOrderBasket(new List<BasketItem>());
                        ctrl.InitialiseSelectedSeats();
                        signInPanel.Visible = false;
                        SignIn_Label.Visible = false;
                        SignOut_Label.Visible = true;
                        Home_Panel.Visible = true;
                    }
                }
                else
                {
                    bool partner = false;
                    if (userToInst[7] == "True")
                    {
                        partner = true;
                    }
                    ctrl.LogUser(Convert.ToInt32(userToInst[0]), userToInst[1], userToInst[2], userToInst[3], userToInst[4], userToInst[5], userToInst[6], partner);
                    MessageBox.Show("Login Succesfull for " + ctrl.GetLoggedUser().Email);
                    ctrl.InitializeUserBasket();
                    ctrl.InitialiseSelectedSeats();
                    signInPanel.Visible = false;
                    SignOut_Label.Visible = true;
                    SignIn_Label.Visible = false;
                    Home_Panel.Visible = true;
                }
            }
        }

        private void CreateAccount_Button_Click(object sender, EventArgs e)
        {
            if (!ctrl.IsValidEmail(RegisterEmail_TextBox.Text))
            {
                MessageBox.Show("Not Valid Email Format");
            }
            else if (RegisterPassword_TextBox.Text != RegisterRepeatPassword_TextBox.Text)
            {
                MessageBox.Show("Passwords do not match");
            }
            else if (!ctrl.CheckPasswordComplexity(RegisterRepeatPassword_TextBox.Text))
            {
                MessageBox.Show("Password not complex enough");
            }
            else if (ctrl.IsStaffAccount(RegisterEmail_TextBox.Text))
            {
                Staff newStaff = new Staff(RegisterEmail_TextBox.Text, RegisterPassword_TextBox.Text, RegisterFirstname_TextBox.Text,
                                     RegisterLastname_TextBox.Text, RegisterAddress_TextBox.Text, RegisterPhoneNumber_TextBox.Text);
                try
                {
                    ctrl.AddStaffAccount(newStaff);
                    MessageBox.Show("Account created");
                    registerUserPanel.Visible = false;
                    signInPanel.Visible = true;
                }
                catch (Exception err)
                {
                    MessageBox.Show("Account could not be created: " + err);
                }
            }
            else
            {
                User newUser = new User(RegisterEmail_TextBox.Text, RegisterPassword_TextBox.Text, RegisterFirstname_TextBox.Text,
                                     RegisterLastname_TextBox.Text, RegisterAddress_TextBox.Text, RegisterPhoneNumber_TextBox.Text);
                try
                {
                    ctrl.AddUserAccount(newUser);
                    MessageBox.Show("Account created");
                    registerUserPanel.Visible = false;
                    signInPanel.Visible = true;
                }
                catch (Exception err)
                {
                    MessageBox.Show("Account could not be created: " + err);
                }
            }
        }

        private void Remove_Play_Button_Click(object sender, EventArgs e)
        {
            CloseAllPanels();
            RemovePlayPanel.Visible = true;
            selectPlayToRemoveCb.Items.Clear();
            foreach (Play p in ctrl.GetAllPlays())
            {
                selectPlayToRemoveCb.Items.Add(p.PlayTitle);
            }
            
        }

        private void Create_Play_Button_Click(object sender, EventArgs e)
        {
            Play NewPlay = new Play(PlayTitle_TextBox.Text, PlayDescription_Text.Text, PlayImage_TextBox.Text, Convert.ToInt32(PriceBandA_TextBox.Text), Convert.ToInt32(PriceBandB_TextBox.Text), Convert.ToInt32(PriceBandC_TextBox.Text), Convert.ToInt32(AddPartnerDiscount_TextBox.Text), false);
            bool Exists = false;
            foreach (Play p in ctrl.GetAllPlays())
            {
                if (p.PlayTitle.Equals(PlayTitle_TextBox.Text))
                {
                    Exists = true;
                    MessageBox.Show("Play already exists");
                }
            }
            if (Exists == false)
            {
                ctrl.AddPlayToDatabase(NewPlay);
                ctrl.GetAllPlays().Add(NewPlay);
                ctrl.LoadAllPlays();
            }
            CloseAllPanels();
        }

        private void Edit_Play_Button_Click(object sender, EventArgs e)
        {
            CloseAllPanels();
            PlayListPlayPanel_ComboBox.Items.Clear();
            EditPlay_Panel.Visible = true;
            foreach (Play p in ctrl.GetAllPlays())
            {
                PlayListPlayPanel_ComboBox.Items.Add(p.PlayTitle);
            }
        }

        private void CreatePerformance_Button_Click(object sender, EventArgs e)
        {
            String performanceTime;

            if (Matinee_RB.Checked)
            {
                performanceTime = "Matinee";
            }
            else
            {
                performanceTime = "Evening";
            }

            Performance NewPerformance = new Performance(NewPerformance_TimePicker.Value.ToShortDateString(), performanceTime);
            string sPlay = PlayListPlayPanel_ComboBox.SelectedItem.ToString();
            foreach (Play s in ctrl.GetAllPlays())
            {
                if (s.PlayTitle == sPlay)
                {
                    bool Exists = false;
                    foreach (Performance pe in s.Performances)
                    {
                        if ((pe.PerformanceDate == NewPerformance.PerformanceDate) && (pe.DayTime == NewPerformance.DayTime))
                        {
                            Exists = true;
                        }
                    }
                    if (Exists == false)
                    {
                        s.Performances.Add(NewPerformance);
                        ctrl.AddPerformanceToPlay(s, NewPerformance);
                    }
                    else
                    {
                        MessageBox.Show("Performance already exists");
                    }
                }
            }

            EditPlayExistingPerformance_CB.Items.Add("Day: " + NewPerformance.PerformanceDate + " " + "Time: " + NewPerformance.DayTime);
        }

        private void AddToBasket_Button_Click(object sender, EventArgs e)
        {
            if (ctrl.isUserLogged() == false)
            {
                MessageBox.Show("Denied - Sign in Required || If you are a staff member please use your dedicated area");
            }
            else
            {
                if (ctrl.GetSelectedSeats().Count != 0)
                {
                    foreach (Seat s in ctrl.GetSelectedSeats())
                    {
                        ctrl.AddToUserBasket(new BasketItem(ctrl.GetLoggedUser().UserID, ctrl.GetSelectedPlay().PlayID, ctrl.GetSelectedPerformance().PerformanceID, s));
                    }
                    MessageBox.Show("Order Added to the Basket");
                    ItemsAddedToBasket = true;
                }
                else
                {
                    MessageBox.Show("Please Select Seats");
                }
                
            }
        }

        private void CheckOut_Button_Click(object sender, EventArgs e)
        {
            if ((ctrl.isUserLogged() == false))
            {
                MessageBox.Show("Denied - Signin Required || If you are a staff member please use your dedicated area");
            }
            else if (ctrl.GetSelectedSeats().Count < 1)
            {
                MessageBox.Show("Denied - You have not selected any seat");
            }
            else
            {
                CloseAllPanels();
                OpenCheckoutPanel();
            }
        }

        private void ClearSelectedSeats_Button_Click(object sender, EventArgs e)
        {
            if (ctrl.isStaffLogged() || ctrl.isUserLogged())
            {
                ClearAllSelectedSeats();
                SelectionForBasket_RichTextBox.Text = "";
                ctrl.GetSelectedSeats().Clear();
            }
        }

        private void SearchStaffAccount_button_Click(object sender, EventArgs e)
        {
            Staff_ListView.Items.Clear();
            if (ctrl.CheckIfStaffExists(SearchEmailAccount_TextBox.Text))
            {
                string[] array = new string[2];
                array[0] = SearchEmailAccount_TextBox.Text;
                array[1] = ctrl.GetStaffEnableStatus(SearchEmailAccount_TextBox.Text).ToString();
                ListViewItem itm = new ListViewItem(array);
                Staff_ListView.Items.Add(itm);
            }
            else
            {
                MessageBox.Show("User Not Found");
            }
        }

        private void EnableStaff_Button_Click(object sender, EventArgs e)
        {
            ctrl.EnableDisableStaffAccount(SearchEmailAccount_TextBox.Text, 1);
            MessageBox.Show("Staff account Enabled");
            Staff_ListView.Items.Clear();
        }

        private void DisableStaff_Button_Click(object sender, EventArgs e)
        {
            ctrl.EnableDisableStaffAccount(SearchEmailAccount_TextBox.Text, 0);
            MessageBox.Show("Staff account Disabled");
            Staff_ListView.Items.Clear();
        }

        private void SearchUserAccount_Button_Click(object sender, EventArgs e)
        {
            Partner_ListView.Items.Clear();
            if (ctrl.CheckIfUserEmailExist(SearchPartnerAccount_TextBox.Text))
            {
                string[] array = new string[2];
                array[0] = SearchPartnerAccount_TextBox.Text;
                array[1] = ctrl.GetPartnerEnableStatus(SearchPartnerAccount_TextBox.Text).ToString();
                ListViewItem itm = new ListViewItem(array);
                Partner_ListView.Items.Add(itm);
            }
            else
            {
                MessageBox.Show("User Not Found");
            }
        }

        private void DisablePartner_Button_Click(object sender, EventArgs e)
        {
            ctrl.EnableDisablePartnerAccount(SearchPartnerAccount_TextBox.Text, 0);
            MessageBox.Show("Partner Account Disabled");
            Partner_ListView.Items.Clear();
        }

        private void EnablePartner_Button_Click(object sender, EventArgs e)
        {
            ctrl.EnableDisablePartnerAccount(SearchPartnerAccount_TextBox.Text, 1);
            MessageBox.Show("Partner Account Enabled");
            Partner_ListView.Items.Clear();
        }

        private void StaffAddToBasket_Button_Click(object sender, EventArgs e)
        {
            foreach (Seat s in ctrl.GetSelectedSeats())
            {
                BasketItem bi = new BasketItem(ctrl.GetLoggedUserStaff().StaffID, ctrl.GetSelectedPlay().PlayID, ctrl.GetSelectedPerformance().PerformanceID, s);
                ctrl.AddToStaffOrderBasket(bi);
            }
            StaffItemsAddedToBasket = true;
            MessageBox.Show("Added to Basket - Add more tickets to the order or go to checkout");
            ctrl.ClearSelectedSeats();
            
        }

        private void StaffProceedToCheckOut_Button_Click(object sender, EventArgs e)
        {
            if (ctrl.GetStaffOrderBasket() == null)
            {
                MessageBox.Show("Your basket is empty and no seat is being selected");
            }
            else
            {
                OpenStaffCheckoutPanel();
            }
        }

        private void StaffBookTicket_Button_Click(object sender, EventArgs e)
        {
            if ((StaffOrderFirstname_TextBox.Text == "") || (StaffOrderLastname_TextBox.Text == "") || (StaffOrderPhoneNumber_TextBox.Text == "") || (StaffOrderAddress_TextBox.Text == ""))
            {
                MessageBox.Show("Please Fill All The Fields");
            }
            else if ((CashAtTheTill == false) && ((StaffCheckOut_CardName_TextBox.Text == "") || (StaffCheckOut_CardNumber_TextBox.Text == "") || (StaffCheckOut_CardExpiry_TextBox.Text == "") || (StaffCheckOut_CardCVV_TextBox.Text == "")))
            {
                MessageBox.Show("Please Add Card Details");
            }
            else
            {
                if (new PaymentCheck().GetTransactionResult())
                {
                    List<Seat> allBasketSeats = new List<Seat>();
                    foreach (BasketItem bi in ctrl.GetStaffOrderBasket())
                    {
                        allBasketSeats.Add(bi.Seat);
                    }
                    Order newOrder = new Order(DateTime.UtcNow.ToString(), OrderTotal, StaffOrderFirstname_TextBox.Text, StaffOrderLastname_TextBox.Text, StaffOrderAddress_TextBox.Text, ctrl.GetLoggedUserStaff().UserID, allBasketSeats);
                    ctrl.AddOrderToBatabase(newOrder,ctrl.GetStaffOrderBasket());
                    ctrl.ClearStaffOrderBasket();
                    ctrl.ClearSelectedSeats();
                    int lastEntry = ctrl.GetLastAddedOrder();
                    ctrl.GenerateStaffOrderInvoice(lastEntry);
                    CloseAllPanels();
                    StaffItemsAddedToBasket = false;
                    OrderTotal = 0;
                    StaffOrderSummary_ListView.Items.Clear();
                    OpenStaffBookingPanel();
                }
            }
        }

        private void SearchOrderID_Button_Click(object sender, EventArgs e)
        {
            int orderID;
            SearchOrder_ListView.Items.Clear();
            try
            {
                orderID = Convert.ToInt32(SearchOrderID_TextBox.Text);
                PrintAllTickets_Button.Enabled = true;
                GenerateReceiptFromTickets_Button.Enabled = true;
                if (ctrl.CheckIfOrderExistsByID(orderID))
                {
                    Order loadOrder = new Order();
                    loadOrder = ctrl.LoadOrderForTickets(orderID);
                    string[] array = new string[7];
                    array[0] = loadOrder.OrderID.ToString();
                    array[1] = loadOrder.DateOrder;
                    array[2] = loadOrder.Total.ToString();
                    array[3] = loadOrder.Firstname;
                    array[4] = loadOrder.Lastname;
                    array[5] = loadOrder.Address;
                    int c = 0;
                    foreach (Seat s in loadOrder.Seats)
                    {
                        c++;
                    }
                    array[6] = c.ToString();
                    ListViewItem item = new ListViewItem(array);
                    SearchOrder_ListView.Items.Add(item);
                }
                else
                {
                    MessageBox.Show("Order Not Found");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Type only a integer" + err);
            }
        }

        private void SearchOrderLastname_Button_Click(object sender, EventArgs e)
        {
            string lastname;
            SearchOrder_ListView.Items.Clear();
            try
            {
                lastname = SearchOrderLastName_TextBox.Text;
                PrintAllTickets_Button.Enabled = true;
                GenerateReceiptFromTickets_Button.Enabled = true;
                if (ctrl.CheckIfOrderExistsByLastname(lastname))
                {
                    List<Order> loadOrders = new List<Order>();
                    loadOrders = ctrl.LoadListOfOrderForTickets(lastname);
                    foreach (Order o in loadOrders)
                    {
                        string[] array = new string[7];
                        array[0] = o.OrderID.ToString();
                        array[1] = o.DateOrder;
                        array[2] = o.Total.ToString();
                        array[3] = o.Firstname;
                        array[4] = o.Lastname;
                        array[5] = o.Address;
                        int c = 0;
                        foreach (Seat s in o.Seats)
                        {
                            c++;
                        }
                        array[6] = c.ToString();
                        ListViewItem item = new ListViewItem(array);
                        SearchOrder_ListView.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Order Not Found");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Type only a integer" + err);
            }
        }

        private void GenerateReceiptFromTickets_Button_Click(object sender, EventArgs e)
        {
            for (int i = SearchOrder_ListView.Items.Count - 1; i >= 0; i--)
            {
                if (SearchOrder_ListView.Items[i].Selected)
                {
                    ctrl.GenerateStaffOrderInvoice(Convert.ToInt32(SearchOrder_ListView.Items[i].Text));
                }
            }
        }

        private void ResetSearchByID_Button_Click(object sender, EventArgs e)
        {
            SearchOrderID_TextBox.Enabled = true;
            PrintAllTickets_Button.Enabled = false;
            GenerateReceiptFromTickets_Button.Enabled = false;
        }

        private void PrintAllTickets_button_Click(object sender, EventArgs e)
        {
            Order loadOrder = new Order();
            for (int i = SearchOrder_ListView.Items.Count - 1; i >= 0; i--)
            {
                if (SearchOrder_ListView.Items[i].Selected)
                {
                    loadOrder = ctrl.LoadOrderForTickets(Convert.ToInt32(SearchOrder_ListView.Items[i].Text));
                    ctrl.CreateTickets(loadOrder);
                }
            }
        }

        private void EditPlay_Button_Click(object sender, EventArgs e)
        {
            Play updatedPlay = new Play(EditPlayTitle_TextBox.Text,
                                                EditPlayDescription_TextBox.Text,
                                                EditPlayImage_TextBox.Text,
                                                Int32.Parse(EditPriceBandA_TextBox.Text),
                                                Int32.Parse(EditPriceBandB_TextBox.Text),
                                                Int32.Parse(EditPriceBandC_TextBox.Text),
                                                Int32.Parse(EditPlayPartnerDiscount_TextBox.Text),
                                                false
                                                );

            ctrl.UpdatePlay(updatedPlay, PlayListPlayPanel_ComboBox.SelectedItem.ToString());
            MessageBox.Show("Play Edited Sucessfully!");
        }

        private void BookNow_Button_Click(object sender, EventArgs e)
        {
            OpenBookingPanel(ctrl.GetMostViewedPlay());
        }

        private void RemoveSelectedSeatsFromTheBasket_Click(object sender, EventArgs e)
        {
            for (int i = Basket_ListView.Items.Count - 1; i >= 0; i--)
            {
                if (Basket_ListView.Items[i].Selected)
                {
                    int playID = 0;
                    int performanceID = 0;
                    foreach (Play p in ctrl.GetAllPlays())
                    {
                        string sItem = Basket_ListView.Items[i].Text;
                        if (p.PlayTitle == sItem)
                        {
                            playID = p.PlayID;
                            p.Performances = ctrl.LoadAllPerformancesByPlayID(p.PlayID);
                            foreach (Performance perf in p.Performances)
                            {
                                if ((perf.PerformanceDate == Basket_ListView.Items[i].SubItems[1].Text) &&
                                        (perf.DayTime == Basket_ListView.Items[i].SubItems[2].Text))
                                {
                                    performanceID = perf.PerformanceID;
                                }
                            }
                            break;
                        }
                    }
                    string sN = Basket_ListView.Items[i].SubItems[3].Text;
                    BasketItem itemToRemoveFromBasket = new BasketItem();
                    foreach (BasketItem bi in ctrl.GetUserBasket())
                    {
                        if ((bi.PlayID == playID) && (bi.PerformanceID == performanceID) && (bi.Seat.SeatNumber == sN))
                        {
                            itemToRemoveFromBasket = bi;
                        }
                    }
                    ctrl.RemoveFromUserBasket(itemToRemoveFromBasket);
                    Basket_ListView.Items[i].Remove();
                }
            }
        }

        private void CompleteReviewButton_Click(object sender, EventArgs e)
        {
            int playIDForReview = 0;
            int playRating = 0;
            if (PlaysUserAttendedCB.SelectedIndex != -1)
            {
                foreach (Play play in ctrl.AllPlaysUserAttended())
                {
                    if (PlaysUserAttendedCB.SelectedItem.ToString() == play.PlayTitle)
                    {
                        playIDForReview = play.PlayID;
                    }
                }
                if (OneStarRadiobutton.Checked)
                {
                    playRating = 1;
                    ctrl.AddNewReview(PlaysUserAttendedCB.SelectedItem.ToString(), DateTime.UtcNow.ToString(), ReviewDescriptionTextbox.Text, playRating, ctrl.GetLoggedUser().UserID, playIDForReview);
                    MessageBox.Show("Review Added Successfully.");
                    ReviewDescriptionTextbox.Text = "";
                }
                else if (TwoStarRadiobutton.Checked)
                {
                    playRating = 2;
                    ctrl.AddNewReview(PlaysUserAttendedCB.SelectedItem.ToString(), DateTime.UtcNow.ToString(), ReviewDescriptionTextbox.Text, playRating, ctrl.GetLoggedUser().UserID, playIDForReview);
                    MessageBox.Show("Review Added Successfully.");
                    ReviewDescriptionTextbox.Text = "";
                }
                else if (ThreeStarRadiobutton.Checked)
                {
                    playRating = 3;
                    ctrl.AddNewReview(PlaysUserAttendedCB.SelectedItem.ToString(), DateTime.UtcNow.ToString(), ReviewDescriptionTextbox.Text, playRating, ctrl.GetLoggedUser().UserID, playIDForReview);
                    MessageBox.Show("Review Added Successfully.");
                    ReviewDescriptionTextbox.Text = "";
                }
                else if (FourStarRadiobutton.Checked)
                {
                    playRating = 4;
                    ctrl.AddNewReview(PlaysUserAttendedCB.SelectedItem.ToString(), DateTime.UtcNow.ToString(), ReviewDescriptionTextbox.Text, playRating, ctrl.GetLoggedUser().UserID, playIDForReview);
                    MessageBox.Show("Review Added Successfully.");
                    ReviewDescriptionTextbox.Text = "";
                }
                else if (FiveStarRadiobutton.Checked)
                {
                    playRating = 5;
                    ctrl.AddNewReview(PlaysUserAttendedCB.SelectedItem.ToString(), DateTime.UtcNow.ToString(), ReviewDescriptionTextbox.Text, playRating, ctrl.GetLoggedUser().UserID, playIDForReview);
                    MessageBox.Show("Review Added Successfully.");
                    ReviewDescriptionTextbox.Text = "";
                }
                else
                {
                    MessageBox.Show("Please rate the play!");
                }
            }
            else
            {
                MessageBox.Show("Please Select a Play");
            }
            


        }

        private void Pay_Button_Click(object sender, EventArgs e)
        {
            if (ctrl.isUserLogged() == false)
            {
                MessageBox.Show("Denied - Signin Required || If you are a staff member please use your dedicated area");
            }
            else
            {
                if (new PaymentCheck().GetTransactionResult())
                {
                    List<Seat> allBasketSeats = new List<Seat>();

                    foreach (BasketItem bi in ctrl.GetUserBasket())
                    {
                        allBasketSeats.Add(bi.Seat);
                    }
                    Order newOrder = new Order(DateTime.UtcNow.ToString(), OrderTotal, ctrl.GetLoggedUser().FirstName, ctrl.GetLoggedUser().LastName, ctrl.GetLoggedUser().Address, ctrl.GetLoggedUser().UserID, allBasketSeats);
                    ctrl.AddOrderToBatabase(newOrder,ctrl.GetUserBasket());
                    ctrl.ClearUserBasket();
                    ctrl.SetUserOrder();
                    int lastEntry = ctrl.GetLastAddedOrder();
                    ctrl.GenerateUserInvoice(lastEntry);
                    CloseAllPanels();
                    OrderTotal = 0;
                    Checkout_Summary_ListView.Items.Clear();
                    Home_Panel.Visible = true;
                }
            }
        }

        private void BasketCheckOut_Button_Click(object sender, EventArgs e)
        {
            if (ctrl.GetUserBasket().Count != 0 )
            {
                OpenCheckoutPanel();
            }
            else
            {
                MessageBox.Show("Your basket is empty");
            }
        }

        //--------------------LABEL ACTIONS------------------------------
        private void NoAccountClick_Label_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CloseAllPanels();
            registerUserPanel.Visible = true;
            //registerUserPanel.Location = new Point(600, 135);
            SignIn_Label.Visible = false;
        }

        private void SignOut_Label_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have logged out");
            ctrl.LogOutUser();
            ctrl.LogOutStaff();
            SignIn_Label.Visible = true;
            SignOut_Label.Visible = false;
            CloseAllPanels();
            staffOptionsPanel.Visible = false;


        }

        private void SignUp_Label_Click(object sender, EventArgs e)
        {
            CloseAllPanels();
            signInPanel.Visible = true;
        }

        private void Label2_Click_1(object sender, EventArgs e)
        {
            registerUserPanel.Visible = false;
            signInPanel.Visible = true;
        }

        //--------------------PICTURE BOX ACTIONS------------------------
        private void CloseProgram_PictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MaximizeProgram_PictureBox_Click(object sender, EventArgs e)
        {
            if (countMaxMin == 0)
            {
                this.WindowState = FormWindowState.Maximized;
                countMaxMin++;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                countMaxMin = 0;
            }
        }

        private void MinimizeProgram_PictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Facebook_PictureBox_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/GreenwichTheatreLondon/?");
        }

        private void Twitter_PictureBox_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("https://twitter.com/GreenwichTheatr");
        }

        private void Insta_PictureBox_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/greenwichtheatre/");
        }

        private void GenerateReceipt_Click(object sender, EventArgs e)
        {
            for (int i = Order_ListView.Items.Count - 1; i >= 0; i--)
            {
                if (Order_ListView.Items[i].Selected)
                {
                    ctrl.GenerateUserInvoice(Convert.ToInt32(Order_ListView.Items[i].Text));
                }
            }
        }

        //-------------------- TEXT BOX ACTIONS   ----------------------------------
        private void RegisterPassword_TextBox_TextChanged(object sender, EventArgs e)
        {
            LoginPassword_Textbox.Text = "";
            LoginPassword_Textbox.PasswordChar = '*';
        }

        private void RegisterRepeatPassword_TextBox_TextChanged(object sender, EventArgs e)
        {
            LoginPassword_Textbox.Text = "";
            LoginPassword_Textbox.PasswordChar = '*';
        }

        private void ShowPassword_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPassword_CheckBox.Checked == true)
            {
                LoginPassword_Textbox.PasswordChar = '\0';
            }
            else
            {
                LoginPassword_Textbox.PasswordChar = '*';
            }
        }

        private void LoginPassword_Textbox_TextChanged(object sender, EventArgs e)
        {
            if (ShowPassword_CheckBox.Checked == true)
            {
                LoginPassword_Textbox.PasswordChar = '\0';
            }
            else
            {
                LoginPassword_Textbox.PasswordChar = '*';
            }
        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
            LoginUsername_Textbox.Text = "";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            LoginPassword_Textbox.Text = "";
            LoginPassword_Textbox.PasswordChar = '*';
        }

        //-------------------- ComboBox Index Change ------------------------------
        private void Select_Performance_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctrl.GetLoggedUser() == null)
            {
                
            }else
            {

                ctrl.ClearSelectedSeats();
            }
            List<Seat> BasketSeats = new List<Seat>();
            foreach (Performance p in ctrl.GetAllPerformancesForPlay())
            {
                if (Select_Performance_CB.SelectedItem.Equals(("Day: " + p.PerformanceDate + " " + "Time: " + p.DayTime)))
                {
                    ctrl.SetSelectedPerformance(p);
                    if (ctrl.GetUserBasket() != null)
                    {
                        foreach (BasketItem bi in ctrl.GetUserBasket())
                        {
                            if (bi.PerformanceID == p.PerformanceID)
                            {
                                BasketSeats.Add(bi.Seat);
                            }
                        }
                    }
                }
            }
            List<Seat> BookedSeats = ctrl.LoadBookedSeatsForPerformance();
            
            UserUpdateSeatingLayout(BookedSeats,BasketSeats);
        }

        private void PlayListPlayPanel_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditPlayExistingPerformance_CB.Items.Clear();

            string sPlay = PlayListPlayPanel_ComboBox.SelectedItem.ToString();
            foreach (Play s in ctrl.GetAllPlays())
            {
                if (s.PlayTitle == sPlay)
                {
                    EditPlayTitle_TextBox.Text = s.PlayTitle;
                    EditPlayDescription_TextBox.Text = s.Description;
                    EditPlayImage_TextBox.Text = s.Image;
                    EditPriceBandA_TextBox.Text = s.PriceBandA.ToString();
                    EditPriceBandB_TextBox.Text = s.PriceBandB.ToString();
                    EditPriceBandC_TextBox.Text = s.PriceBandC.ToString();
                    EditPlayPartnerDiscount_TextBox.Text = s.PartnerDiscount.ToString();
                    s.Performances = ctrl.LoadAllPerformancesByPlayID(s.PlayID);

                    foreach (Performance pe in s.Performances)
                    {
                        EditPlayExistingPerformance_CB.Items.Add("Day: " + pe.PerformanceDate + " " + "Time: " + pe.DayTime);
                    }
                }
            }
            ReadMoreDescriptionEditPlay.Visible = true;
        }

        private void AllPlaysReviewsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Review> listOfReviews = ctrl.LoadReviewsForAPlay(AllPlaysReviewsCB.SelectedItem.ToString());
            ListViewItem item;
            ReviewListView.Visible = true;
            ReviewListView.Items.Clear();

            foreach (Review review in listOfReviews)
            {
                string[] array = new string[5];
                array[0] = ctrl.GetUserOfReview(review.FK_UserID);
                array[1] = review.DateAdded;
                array[2] = review.Description;
                array[3] = review.Score.ToString();
                item = new ListViewItem(array);
                ReviewListView.Items.Add(item);
            }
        }

        private void PlaysStaffBooking_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrl.ClearSelectedSeats();
            PerformanceStaffBooking_CB.Items.Clear();
            ctrl.SetSelectedPlay(new Play());
            foreach (Play p in ctrl.GetAllPlays())
            {
                if (p.PlayTitle == PlaysStaffBooking_CB.SelectedItem.ToString())
                {
                    ctrl.SetSelectedPlay(p);
                    ctrl.LoadAllPerformancesForPlay(p.PlayID);
                }
            }

            StaffBandA_Price_Label.Text = "Band A Price: " + ctrl.GetSelectedPlay().PriceBandA + "£";
            StaffBandB_Price_Label.Text = "Band B Price: " + ctrl.GetSelectedPlay().PriceBandB + "£";
            StaffBandC_Price_Label.Text = "Band C Price: " + ctrl.GetSelectedPlay().PriceBandC + "£";

            foreach (Performance p in ctrl.GetAllPerformancesForPlay())
            {
                PerformanceStaffBooking_CB.Items.Add("Day: " + p.PerformanceDate + " " + "Time: " + p.DayTime);
            }
            PerformanceStaffBooking_CB.Text = "";
            PerformanceStaffBooking_CB.SelectedIndex = -1;
        }

        private void PerformanceStaffBooking_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrl.ClearSelectedSeats();
            foreach (Performance p in ctrl.GetAllPerformancesForPlay())
            {
                if (PerformanceStaffBooking_CB.SelectedItem.Equals(("Day: " + p.PerformanceDate + " " + "Time: " + p.DayTime)))
                {
                    ctrl.SetSelectedPerformance(p);
                }
            }
            List<Seat> BookedSeats = ctrl.LoadBookedSeatsForPerformance();
            StaffUpdateSeatingLayout(BookedSeats);
        }

        private void LastHourDiscount_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (LastHourDiscount_CheckBox.Checked)
            {
                //cost = (float)(p.PriceBandA) - (((float)(p.PriceBandA) / d) * (float)p.PartnerDiscount);
                OrderTotal = OrderTotal - ((OrderTotal / 100) * 10);
                StaffCheckoutTotal_Label.Text = "Your total is: £ " + OrderTotal;
            }
            if (!LastHourDiscount_CheckBox.Checked)
            {
                OrderTotal = 0.0F;
                foreach (BasketItem bi in ctrl.GetStaffOrderBasket())
                {
                    foreach (Play p in ctrl.GetAllPlays())
                    {
                        if (p.PlayID == bi.PlayID)
                        {
                            if (bi.Seat.Band == "A")
                            {
                                OrderTotal = OrderTotal + p.PriceBandA;
                            }
                            else if (bi.Seat.Band == "B")
                            {
                                OrderTotal = OrderTotal + p.PriceBandB;
                            }
                            else if (bi.Seat.Band == "C")
                            {
                                OrderTotal = OrderTotal + p.PriceBandC;
                            }
                        }
                    }
                }
                StaffCheckoutTotal_Label.Text = "Your total is: £ " + OrderTotal;
            }
        }

        private void CashTill_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CashTill_CheckBox.Checked)
            {
                CashAtTheTill = true;
                StaffCheckOut_CardName_TextBox.Enabled = false;
                StaffCheckOut_CardNumber_TextBox.Enabled = false;
                StaffCheckOut_CardExpiry_TextBox.Enabled = false;
                StaffCheckOut_CardCVV_TextBox.Enabled = false;
            }
            else if (!CashTill_CheckBox.Checked)
            {
                CashAtTheTill = false;
                StaffCheckOut_CardName_TextBox.Enabled = true;
                StaffCheckOut_CardNumber_TextBox.Enabled = true;
                StaffCheckOut_CardExpiry_TextBox.Enabled = true;
                StaffCheckOut_CardCVV_TextBox.Enabled = true;
            }


        }

        private void ExpressDelivery_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ExpressDelivery_CheckBox.Checked)
            {
                OrderTotal = OrderTotal + 10;
                TotalCost_Label.Text = "Total: £ " + OrderTotal ;
            }
            if (!ExpressDelivery_CheckBox.Checked)
            {
                OrderTotal = 0.0F;
                foreach (BasketItem bi in ctrl.GetUserBasket())
                {
                    foreach (Play p in ctrl.GetAllPlays())
                    {
                        if (p.PlayID == bi.PlayID)
                        {
                            if (bi.Seat.Band == "A")
                            {
                                if (ctrl.GetLoggedUser().Partner == true)
                                {
                                    float d = 100.0F;
                                    float cost = 0.0F;
                                    cost = (float)(p.PriceBandA) - (((float)(p.PriceBandA) / d) * (float)p.PartnerDiscount);
                                    OrderTotal = OrderTotal + cost;
                                }
                                else
                                {
                                    OrderTotal = OrderTotal + p.PriceBandA;
                                }
                            }
                            else if (bi.Seat.Band == "B")
                            {
                                if (ctrl.GetLoggedUser().Partner == true)
                                {
                                    float d = 100.0F;
                                    float cost = 0.0F;
                                    cost = (float)(p.PriceBandB) - (((float)(p.PriceBandB) / d) * (float)p.PartnerDiscount);
                                    OrderTotal = OrderTotal + cost;
                                }
                                else
                                {
                                    OrderTotal = OrderTotal + p.PriceBandB;
                                }
                            }
                            else if (bi.Seat.Band == "C")
                            {
                                if (ctrl.GetLoggedUser().Partner == true)
                                {
                                    float d = 100.0F;
                                    float cost = 0.0F;
                                    cost = (float)(p.PriceBandC) - (((float)(p.PriceBandC) / d) * (float)p.PartnerDiscount);
                                    OrderTotal = OrderTotal + cost;
                                }
                                else
                                {
                                    OrderTotal = OrderTotal + p.PriceBandC;
                                }
                            }
                        }
                    }
                }
                TotalCost_Label.Text = "Total: £ " + OrderTotal ;
            }
        }

        private void StaffUpdateSeatingLayout(List<Seat> bs)
        {
            foreach (Control btn in StaffBandA.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor != Color.Bisque)
                    {
                        btn.BackColor = Color.Bisque;
                    }
                    if (btn.Enabled != true)
                    {
                        btn.Enabled = true;
                    }
                }
            }
            foreach (Control btn in StaffBandB.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor != Color.Bisque)
                    {
                        btn.BackColor = Color.Bisque;
                    }

                    if (btn.Enabled != true)
                    {
                        btn.Enabled = true;
                    }
                }
            }
            foreach (Control btn in StaffBandC.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor != Color.Bisque)
                    {
                        btn.BackColor = Color.Bisque;
                    }

                    if (btn.Enabled != true)
                    {
                        btn.Enabled = true;
                    }
                }
            }

            foreach (Control btn in StaffBandA.Controls)
            {
                if (btn is Button)
                {
                    foreach (Seat s in bs)
                    {
                        if (s.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                }
            }
            foreach (Control btn in StaffBandB.Controls)
            {
                if (btn is Button)
                {
                    foreach (Seat s in bs)
                    {
                        if (s.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                }
            }
            foreach (Control btn in StaffBandC.Controls)
            {
                if (btn is Button)
                {
                    foreach (Seat s in bs)
                    {
                        if (s.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                }
            }
        }

        private void UserUpdateSeatingLayout(List<Seat> bs, List<Seat> basketSeats)
        {
            foreach (Control btn in Band_A.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor != Color.Bisque)
                    {
                        btn.BackColor = Color.Bisque;
                    }
                    if (btn.Enabled != true)
                    {
                        btn.Enabled = true;
                    }
                }
            }
            foreach (Control btn in Band_B.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor != Color.Bisque)
                    {
                        btn.BackColor = Color.Bisque;
                    }

                    if (btn.Enabled != true)
                    {
                        btn.Enabled = true;
                    }
                }
            }
            foreach (Control btn in Band_C.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor != Color.Bisque)
                    {
                        btn.BackColor = Color.Bisque;
                    }

                    if (btn.Enabled != true)
                    {
                        btn.Enabled = true;
                    }
                }
            }

            foreach (Control btn in Band_A.Controls)
            {
                if (btn is Button)
                {
                    foreach (Seat s in bs)
                    {
                        if (s.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                    foreach (Seat se in basketSeats)
                    {
                        if (se.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Orange;
                            btn.Enabled = false;
                        }
                    }
                }
            }
            foreach (Control btn in Band_B.Controls)
            {
                if (btn is Button)
                {
                    foreach (Seat s in bs)
                    {
                        if (s.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                    foreach (Seat se in basketSeats)
                    {
                        if (se.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Orange;
                            btn.Enabled = false;
                        }
                    }
                }
            }
            foreach (Control btn in Band_C.Controls)
            {
                if (btn is Button)
                {
                    foreach (Seat s in bs)
                    {
                        if (s.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Red;
                            btn.Enabled = false;
                        }
                    }
                    foreach (Seat se in basketSeats)
                    {
                        if (se.SeatNumber == btn.Name)
                        {
                            btn.BackColor = Color.Orange;
                            btn.Enabled = false;
                        }
                    }
                }
                
            }
        }

        //-------------------- Panel Methods ------------------------------
        public void OpenBookingPanel(int Play)
        {
            CloseAllPanels();
            ctrl.SetSelectedPlay(new Play());
            foreach (Play p in ctrl.GetAllPlays())
            {
                if (p.PlayID == Play)
                {
                    ctrl.SetSelectedPlay(p);
                }
            }
            ChooseSeat_Label.Text = "Select Seat for the Play: " + ctrl.GetSelectedPlay().PlayTitle;
            Select_Performance_CB.Items.Clear();

            ClearAllSeats();
            SelectionForBasket_RichTextBox.Text = "";
            ctrl.LoadAllPerformancesForPlay(Play);
            foreach (Performance p in ctrl.GetAllPerformancesForPlay())
            {
                Select_Performance_CB.Items.Add("Day: " + p.PerformanceDate + " " + "Time: " + p.DayTime);
            }
            Select_Performance_CB.Text = "";
            Select_Performance_CB.SelectedIndex = -1;

            if (SeatingLayoutCreated == false)
            {
                CreateSeatingLayout(Band_C, "C");
                CreateSeatingLayout(Band_B, "B");
                CreateSeatingLayout(Band_A, "A");
                SeatingLayoutCreated = true;
            }
            BandA_Price_Label.Text = "Band A Price: " + ctrl.GetSelectedPlay().PriceBandA + "£";
            BandB_Price_Label.Text = "Band B Price: " + ctrl.GetSelectedPlay().PriceBandB + "£";
            BandC_Price_Label.Text = "Band C Price: " + ctrl.GetSelectedPlay().PriceBandC + "£";
            Choose_Seat_Panel.Visible = true;
        }

        public void OpenCheckoutPanel()
        {
            CloseAllPanels();
            OrderTotal = 0.0F;
            ListViewItem itm;

            if (ItemsAddedToBasket == false)
            {
                foreach (Seat s in ctrl.GetSelectedSeats())
                {
                    ctrl.AddToUserBasket(new BasketItem(ctrl.GetLoggedUser().UserID, ctrl.GetSelectedPlay().PlayID, ctrl.GetSelectedPerformance().PerformanceID, s));
                    ItemsAddedToBasket = true;
                }
            }

            foreach (BasketItem bi in ctrl.GetUserBasket())
            {
                string[] arr = new string[5];
                foreach (Play p in ctrl.GetAllPlays())
                {
                    if (p.PlayID == bi.PlayID)
                    {
                        arr[0] = p.PlayTitle;
                        if (bi.Seat.Band == "A")
                        {
                            arr[4] = p.PriceBandA.ToString();
                            if (ctrl.GetLoggedUser().Partner == true)
                            {
                                float d = 100.0F;
                                float cost = 0.0F;
                                cost = (float)(p.PriceBandA) - (((float)(p.PriceBandA) / d) * (float)p.PartnerDiscount);
                                OrderTotal = OrderTotal + cost;
                            }
                            else
                            {
                                OrderTotal = OrderTotal + p.PriceBandA;
                            }
                        }
                        else if (bi.Seat.Band == "B")
                        {
                            arr[4] = p.PriceBandB.ToString();
                            if (ctrl.GetLoggedUser().Partner == true)
                            {
                                float d = 100.0F;
                                float cost = 0.0F;
                                cost = (float)(p.PriceBandB) - (((float)(p.PriceBandB) / d) * (float)p.PartnerDiscount);
                                OrderTotal = OrderTotal + cost;
                            }
                            else
                            {
                                OrderTotal = OrderTotal + p.PriceBandB;
                            }
                        }
                        else if (bi.Seat.Band == "C")
                        {
                            arr[4] = p.PriceBandC.ToString() + "£";
                            if (ctrl.GetLoggedUser().Partner == true)
                            {
                                float d = 100.0F;
                                float cost = 0.0F;
                                cost = (float)(p.PriceBandC) - (((float)(p.PriceBandC) / d) * (float)p.PartnerDiscount);
                                OrderTotal = OrderTotal + cost;
                            }
                            else
                            {
                                OrderTotal = OrderTotal + p.PriceBandC;
                            }
                        }
                        p.Performances = ctrl.LoadAllPerformancesByPlayID(p.PlayID);
                        foreach (Performance perf in p.Performances)
                        {
                            if (perf.PerformanceID == bi.PerformanceID)
                            {
                                arr[1] = perf.PerformanceDate;
                                arr[2] = perf.DayTime;
                            }
                        }
                    }
                }

                arr[3] = bi.Seat.SeatNumber;
                itm = new ListViewItem(arr);
                Checkout_Summary_ListView.Items.Add(itm);

            }
            if (ctrl.GetLoggedUser().Partner == true)
            {
                DiscountCheckOut_Label.Text = "You have a Partner discount";
            } else {
                DiscountCheckOut_Label.Text = "You are not elegible to any discount";
            }
           
            TotalCost_Label.Text = "Total: " + OrderTotal + "£";
            Checkout_FirstName_TextBox.Text = ctrl.GetLoggedUser().FirstName;
            Checkout_FirstName_TextBox.Enabled = false;
            Checkout_LastName_TextBox.Text = ctrl.GetLoggedUser().LastName;
            Checkout_LastName_TextBox.Enabled = false;
            Checkout_Address_TextBox.Text = ctrl.GetLoggedUser().Address;
            Checkout_Address_TextBox.Enabled = false;
            Checkout_PhoneNumber_TextBox.Text = ctrl.GetLoggedUser().Address;
            Checkout_PhoneNumber_TextBox.Enabled = false;

            Checkout_Panel.Visible = true;
        }

        public void OpenStaffCheckoutPanel()
        {
            StaffOrderSummary_ListView.Items.Clear();
            StaffOrderFirstname_TextBox.Clear();
            StaffOrderLastname_TextBox.Clear();
            StaffOrderAddress_TextBox.Clear();
            StaffOrderPhoneNumber_TextBox.Clear();
            StaffCheckOut_CardName_TextBox.Clear();
            StaffCheckOut_CardNumber_TextBox.Clear();
            StaffCheckOut_CardExpiry_TextBox.Clear();
            StaffCheckOut_CardCVV_TextBox.Clear();
            if (CashTill_CheckBox.Checked == true)
            {
                CashTill_CheckBox.Checked = false;
            }
            if (LastHourDiscount_CheckBox.Checked == true)
            {
                CashTill_CheckBox.Checked = false;
            }
            CloseAllPanels();
            OrderTotal = 0.0F;
            ListViewItem itm;

            if (StaffItemsAddedToBasket == false)
            {
                foreach (Seat s in ctrl.GetSelectedSeats())
                {
                    BasketItem bi = new BasketItem(ctrl.GetLoggedUserStaff().StaffID, ctrl.GetSelectedPlay().PlayID, ctrl.GetSelectedPerformance().PerformanceID, s);
                    ctrl.AddToStaffOrderBasket(bi);
                }
                StaffItemsAddedToBasket = true;
            }

            foreach (BasketItem bi in ctrl.GetStaffOrderBasket())
            {
                string[] arr = new string[5];
                foreach (Play p in ctrl.GetAllPlays())
                {
                    if (p.PlayID == bi.PlayID)
                    {
                        arr[0] = p.PlayTitle;
                        if (bi.Seat.Band == "A")
                        {
                            arr[4] = p.PriceBandA.ToString();
                            OrderTotal = OrderTotal + p.PriceBandA;
                        }
                        else if (bi.Seat.Band == "B")
                        {
                            arr[4] = p.PriceBandB.ToString();
                            OrderTotal = OrderTotal + p.PriceBandB;
                        }
                        else if (bi.Seat.Band == "C")
                        {
                            arr[4] = p.PriceBandC.ToString() + "£";
                            OrderTotal = OrderTotal + p.PriceBandC;
                        }
                        p.Performances = ctrl.LoadAllPerformancesByPlayID(p.PlayID);
                        foreach (Performance perf in p.Performances)
                        {
                            if (perf.PerformanceID == bi.PerformanceID)
                            {
                                arr[1] = perf.PerformanceDate;
                                arr[2] = perf.DayTime;
                            }
                        }
                    }
                }

                arr[3] = bi.Seat.SeatNumber;
                itm = new ListViewItem(arr);
                StaffOrderSummary_ListView.Items.Add(itm);
            }
            StaffCheckoutTotal_Label.Text = "Your total is:" + OrderTotal;
            StaffCheckOut_Panel.Visible = true;
        }

        public void OpenHomePanel()
        {
            CloseAllPanels();
            Home_PictureBox.ImageLocation = ctrl.GetMostViewedPlayUrl();
            Home_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Home_Panel.Visible = true;
        }

        public void OpenStaffBookingPanel()
        {
            CloseAllPanels();
            ctrl.LoadAllPlays();
            PlaysStaffBooking_CB.Items.Clear();
            PerformanceStaffBooking_CB.Items.Clear();
            ClearAllStaffOrderSeats();
            StaffSelectionForBasket_RichTextBox.Text = "";

            foreach (Play p in ctrl.GetAllPlays())
            {
                PlaysStaffBooking_CB.Items.Add(p.PlayTitle);
            }

            if (StaffSeatingLayoutCreated == false)
            {
                CreateSeatingLayout(StaffBandC, "C");
                CreateSeatingLayout(StaffBandB, "B");
                CreateSeatingLayout(StaffBandA, "A");
                StaffSeatingLayoutCreated = true;
            }
            StaffOrder_Panel.Visible = true;
        }

        private void OpenReviewForAPlayPanel(int PlayID)
        {
            CloseAllPanels();

            foreach (TableLayoutPanel tlp in PlayTable_Layout.Controls)
            {
                PlayTable_Layout.Controls.Remove(tlp);
            }

            //get the play the user clicked to view the reviews
            ctrl.SetSelectedPlay(new Play());
            foreach (Play play in ctrl.GetAllPlays())
            {
                if (play.PlayID == PlayID)
                {
                    ctrl.SetSelectedPlay(play);
                }
            }
            ReviewsForAPlayPanelHeading.Text = "Reviews for " + ctrl.GetSelectedPlay().PlayTitle;

            List<Review> listOfReviews = ctrl.LoadReviewsForAPlay(ctrl.GetSelectedPlay().PlayTitle);
            ListViewItem item;
            PlayReview_ListView.Visible = true;
            PlayReview_ListView.Items.Clear();
            int score = 0;
            foreach (Review review in listOfReviews)
            {
                string[] array = new string[5];
                array[0] = ctrl.GetUserOfReview(review.FK_UserID);
                array[1] = review.DateAdded;
                array[2] = review.Description;
                array[3] = review.Score.ToString();
                item = new ListViewItem(array);
                PlayReview_ListView.Items.Add(item);
                score = score + review.Score;
            }
            if (listOfReviews.Count > 0)
            {
                score = score / listOfReviews.Count;
            }

            PlayScore_Label.Text = "Play Average Score: " + score;
            setAddEditRemoveButtonsVisibility(false);
            currentSeparatorPanel.Height = Plays_Button.Height;
            currentSeparatorPanel.Top = Plays_Button.Top;
            userOptionsPanel.Visible = false;
            currentSeparatorUserPanel.Visible = false;
            ReviewsForAPlayPanel.Visible = true;
        }

        //-------------------- GUI UTILITIES ------------------------------
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void homeButton_MouseHover(object sender, EventArgs e)
        {
            currentSeparatorPanel.BackColor = Color.Peru;
        }

        private void changeStaffLocation()
        {
            if (StaffArea_Button.Location == new Point(9, 308))
            {
                StaffArea_Button.Location = new Point(12, 179);
            }
        }

        private void setAddEditRemoveButtonsVisibility(bool boolValue)
        {
            Add_Play_Button.Visible = boolValue;
            Edit_Play_Button.Visible = boolValue;
            Remove_Play_Button.Visible = boolValue;
        }

        public void CloseAllPanels()
        {
            foreach (Control c in Pay_Checkout_Button.Controls)
            {
                if (c is Panel && c.Visible == true)
                {
                    c.Visible = false;
                }
            }
        }

        //Redundancy "Control" can be "Button"
        private void ClearAllSelectedSeats()
        {
            foreach (Control btn in Band_A.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor == Color.Green)
                    {
                        btn.BackColor = Color.Bisque;
                        btn.Enabled = true;
                    }
                }
            }
            foreach (Control btn in Band_B.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor == Color.Green)
                    {
                        btn.BackColor = Color.Bisque;
                        btn.Enabled = true;
                    }
                }
            }
            foreach (Control btn in Band_C.Controls)
            {
                if (btn is Button)
                {
                    if (btn.BackColor == Color.Green)
                    {
                        btn.BackColor = Color.Bisque;
                        btn.Enabled = true;
                    }
                }
            }
        }

        private void ClearAllSeats()
        {
            foreach (Control btn in Band_A.Controls)
            {
                if (btn is Button)
                {
                    btn.BackColor = Color.Bisque;
                    btn.Enabled = true;
                }
            }
            foreach (Control btn in Band_B.Controls)
            {
                if (btn is Button)
                {
                    btn.BackColor = Color.Bisque;
                    btn.Enabled = true;
                }
            }
            foreach (Control btn in Band_C.Controls)
            {
                if (btn is Button)
                {
                    btn.BackColor = Color.Bisque;
                    btn.Enabled = true;
                }
            }
        }

        private void ClearAllStaffOrderSeats()
        {
            foreach (Control btn in StaffBandA.Controls)
            {
                if (btn is Button)
                {
                    btn.BackColor = Color.Bisque;
                    btn.Enabled = true;
                }
            }
            foreach (Control btn in StaffBandB.Controls)
            {
                if (btn is Button)
                {
                    btn.BackColor = Color.Bisque;
                    btn.Enabled = true;
                }
            }
            foreach (Control btn in StaffBandC.Controls)
            {
                if (btn is Button)
                {
                    btn.BackColor = Color.Bisque;
                    btn.Enabled = true;
                }
            }
        }

        private void CreateSeatingLayout(TableLayoutPanel Band, string type)
        {
            int c = (Band.RowCount * Band.ColumnCount);
            for (int i = 0; i < Band.RowCount; i++)
            {
                for (int j = 0; j < Band.ColumnCount; j++)
                {
                    Button btn = new Button();
                    btn.Click += new EventHandler(SelectSeat_Click);
                    btn.Text = type + c;
                    btn.Name = type + c;
                    Band.Controls.Add(btn, j, i);
                    c--;
                }
            }
        }

        protected void SelectSeat_Click(object sender, EventArgs e)
        {
            if (ctrl.GetSelectedPerformance() == null)
            {
                MessageBox.Show("Please Select a Performance");
            }
            else
            {
                if ((ctrl.GetLoggedUser() != null) || (ctrl.GetLoggedUserStaff() != null))
                {
                    Button btn = sender as Button;
                    btn.BackColor = Color.Green;
                    btn.Enabled = false;
                    char[] band = btn.Name.ToCharArray();
                    ctrl.AddSeatToSelectedSeats(btn.Text, band[0].ToString());
                    if (StaffOrder_Panel.Visible == false)
                    {
                        SelectionForBasket_RichTextBox.Text = ctrl.AddSeatToSummary();
                    }
                    else if (StaffOrder_Panel.Visible == true)
                    {
                        StaffSelectionForBasket_RichTextBox.Text = ctrl.AddSeatToSummary();
                    }
                }
                else
                {
                    MessageBox.Show("Please Login or register to select a seat");
                }
               
            }
        }

        private void DifferentAddress_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (DifferentAddress_Checkbox.Checked)
            {
                Checkout_FirstName_TextBox.Enabled = true;
                Checkout_FirstName_TextBox.Text = "";
                Checkout_LastName_TextBox.Enabled = true;
                Checkout_LastName_TextBox.Text = "";
                Checkout_Address_TextBox.Enabled = true;
                Checkout_Address_TextBox.Text = "";
                Checkout_PhoneNumber_TextBox.Enabled = true;
                Checkout_PhoneNumber_TextBox.Text = "";
            }
            if (!DifferentAddress_Checkbox.Checked)
            {
                Checkout_FirstName_TextBox.Text = ctrl.GetLoggedUser().FirstName;
                Checkout_FirstName_TextBox.Enabled = false;
                Checkout_LastName_TextBox.Text = ctrl.GetLoggedUser().LastName;
                Checkout_LastName_TextBox.Enabled = false;
                Checkout_Address_TextBox.Text = ctrl.GetLoggedUser().Address;
                Checkout_Address_TextBox.Enabled = false;
                Checkout_PhoneNumber_TextBox.Text = ctrl.GetLoggedUser().Address;
                Checkout_PhoneNumber_TextBox.Enabled = false;
            }
        }

        private void CollectTheatre_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CollectTheatre_CheckBox.Checked)
            {
                Checkout_FirstName_TextBox.Visible = false;
                Checkout_LastName_TextBox.Visible = false;
                Checkout_Address_TextBox.Visible = false;
                Checkout_PhoneNumber_TextBox.Visible = false;
            }
            if (!CollectTheatre_CheckBox.Checked)
            {
                Checkout_FirstName_TextBox.Visible = true;
                Checkout_LastName_TextBox.Visible = true;
                Checkout_Address_TextBox.Visible = true;
                Checkout_PhoneNumber_TextBox.Visible = true;
            }
        }

        //Allows us to relocate the window
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void StaffUpdateInfo_Click(object sender, EventArgs e)
        {
            if (ctrl.GetLoggedUserStaff().Password == StaffUpdateOLDPASS.Text)
            {
                Staff newStaff = new Staff(
                    ctrl.GetLoggedUserStaff().Email,
                    ctrl.GetLoggedUserStaff().Password,
                    StaffUpdateFN.Text,
                    StaffUpdateLN.Text,
                    StaffUpdateADD.Text,
                    StaffUpdatePHONE.Text);
                if (StaffUpdatePASS.Text == StaffUpdateREPASS.Text)
                {
                    newStaff.Password = StaffUpdatePASS.Text;
                    ctrl.UpdateUserDetails(newStaff, ctrl.GetLoggedUserStaff().Email);
                    StaffUpdateStatus.Text = "Your information have been updated";
                }
                else
                {
                    newStaff.Password = ctrl.GetLoggedUserStaff().Password;
                    ctrl.UpdateUserDetails(newStaff, ctrl.GetLoggedUserStaff().Email);
                    StaffUpdateStatus.Text = "Your information have been updated except the password.";
                }
            }
            else
            {
                StaffUpdateStatus.Text = "Make sure you provide the old password before making any changes";
            }



        }

        private void ManageUser_Account_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ManageStaff_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserUpdateComfirm_Click(object sender, EventArgs e)
        {
            if (ctrl.GetLoggedUser().Password == UserUpdateOldPASS.Text)
            {
                User newUser = new User(
                    ctrl.GetLoggedUser().Email,
                    ctrl.GetLoggedUser().Password,
                    UserUpdateFN.Text,
                    UserUpdateLN.Text,
                    UserUpdateAddress.Text,
                    UserUpdatePhone.Text);
                if (UserUpdateNewPass.Text == UserUpdateNewPassREPEAT.Text)
                {
                    newUser.Password = UserUpdateNewPass.Text;
                    ctrl.UpdateUserDetails(newUser, ctrl.GetLoggedUser().Email);
                    UserUpdateLabel.Text = "Your information have been updated";
                }
                else
                {
                    newUser.Password = ctrl.GetLoggedUser().Password;
                    ctrl.UpdateUserDetails(newUser, ctrl.GetLoggedUser().Email);
                    UserUpdateLabel.Text = "Your information have been updated except the password.";
                }
            }
            else
            {
                UserUpdateLabel.Text = "Make sure you provide the old password before making any changes";
            }


        }

        private void ReadMoreDescriptionEditPlay_Click(object sender, EventArgs e)
        {
            foreach (Play play in ctrl.GetAllPlays())
            {
                if (play.PlayTitle == PlayListPlayPanel_ComboBox.SelectedText)
                {
                    MessageBox.Show(play.Description);
                } else
                {
                    Console.WriteLine(play.PlayTitle + " is not the selected play. ");
                }
            }
        }

        private void StundetElderDiscountStaff_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            float previousOrderTotal = OrderTotal;
            if (StundetElderDiscountStaff_Checkbox.Checked)
            {
                previousOrderTotal -= (float)(previousOrderTotal * 0.2);
                StaffCheckoutTotal_Label.Text = "Your total is: £ " + previousOrderTotal;
            }
            else
            {
                StaffCheckoutTotal_Label.Text = "Your total is: £ " + OrderTotal;
            }
        }
        
        private void ArchivePlayButton_Click(object sender, EventArgs e)
        {
            try
            {
                string PlayToArchive = selectPlayToRemoveCb.SelectedItem.ToString();
                ctrl.ArchivePlay(PlayToArchive);
                MessageBox.Show("Play Archived Sucessfully!");
                ctrl.LoadAllPlays();
            }
            catch (Exception err)
            {
                Console.WriteLine("Somethin");
            }
        }
    }
}