using System;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Staff : User
    {
        private int staffID;
        private Boolean accountEnabled;

        public Staff(int staffID, int userID, string email, string password, string firstName, string lastName, string address, string phoneNumber,bool partner, Boolean accountEnabled) :
        base(userID, email, password, firstName, lastName, address, phoneNumber, partner)
        {
            this.staffID = staffID;
            this.accountEnabled = accountEnabled;
        }

        public Staff(string email, string password, string firstName, string lastName, string address, string phoneNumber) :
        base(email, password, firstName, lastName, address, phoneNumber)
        {
            this.accountEnabled = false;
        }
        
        public int StaffID { get => staffID; set => staffID = value; }
        public bool AccountEnabled { get => accountEnabled; set => accountEnabled = value; }

    }
}