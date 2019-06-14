using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Review
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public int FK_UserID { get; set; } // This is the FK for the user that wrote the review
        public int FK_PlayID { get; set; } // This is the FK for the performance that the review belongs to
        public int PK_ReviewID { get; set; }
        public string DateAdded { get; set; }

        public Review(string title, string date,string description, int score, int FK_userID, int FK_playID)
        {
            Title = title;
            Description = description;
            Score = score;
            FK_UserID = FK_userID;
            FK_PlayID = FK_playID;
            DateAdded = date;
        }

        public  Review(string title, string description, int score, int userID, int playID)
        {
            Title = title;
            Description = description;
            Score = score;
            FK_UserID = userID;
            FK_PlayID = playID;
        }

        public Review(string PlayTitle, string reviewDescription, int reviewScore, int userID)
        {
            this.Title = PlayTitle;
            this.Description = reviewDescription;
            this.Score = reviewScore;
            this.FK_UserID = userID;
            this.DateAdded = DateTime.UtcNow.Date.ToString();
        }

        public Review(string title, string description, int score, int userID, int playID, string dateAdded, int reviewID) : this(title, description, score, userID, playID)
        {
            DateAdded = dateAdded;
            PK_ReviewID = reviewID;
        }

        public Review(int userID,string dateAdded, string description, int score)
        {
            this.FK_UserID = userID;
            this.Description = description;
            this.DateAdded = dateAdded;
            this.Score = score;
        }
        
    }
}
