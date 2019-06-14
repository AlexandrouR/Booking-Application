namespace SystemsDevelopmentGraphicalUserInterface
{
    public class User
    {
        private int userID;
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private string address;
        private string phoneNumber;
        private bool partner;

        public User(int userID, string email, string password, string firstName, string lastName, string address, string phoneNumber,bool partner)

        {
            this.UserID = userID;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Partner = partner;
        }

        public User(string email, string password, string firstName, string lastName, string address, string phoneNumber)

        {
            this.Email = email;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
        }

        public int UserID { get => userID; set => userID = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Address { get => address; set => address = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public bool Partner { get => partner; set => partner = value; }
    }
}