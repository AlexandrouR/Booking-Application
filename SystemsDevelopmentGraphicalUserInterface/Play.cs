using System.Collections.Generic;

namespace SystemsDevelopmentGraphicalUserInterface
{
    public class Play
    {
        private int playID;
        private string playTitle;
        private string image;
        private string description;
        private int priceBandA;
        private int priceBandB;
        private int priceBandC;
        private int partnerDiscount;
        private bool archived;

        
        private int TotalTicketsSold { get; set; }

        internal List<Performance> Performances { get => performances; set => performances = value; }
        public int PlayID { get => playID; set => playID = value; }
        public string PlayTitle { get => playTitle; set => playTitle = value; }
        public string Image { get => image; set => image = value; }
        public string Description { get => description; set => description = value; }
        public int PriceBandA { get => priceBandA; set => priceBandA = value; }
        public int PriceBandB { get => priceBandB; set => priceBandB = value; }
        public int PriceBandC { get => priceBandC; set => priceBandC = value; }
        public int PartnerDiscount { get => partnerDiscount; set => partnerDiscount = value; }
        public bool Archived { get => archived; set => archived = value; }

        private List<Performance> performances = new List<Performance>();

        // Empty constructor
        public Play()
        {
        }

        // Constructor used when creating a new play
        public Play(string playTitle, string description, string imageURL, int pA, int pB, int pC, int partnerDiscount, bool archived)
        {
            this.PlayTitle = playTitle;
            this.Image = imageURL;
            this.Description = description;
            this.PriceBandA = pA;
            this.PriceBandB = pB;
            this.PriceBandC = pC;
            this.PartnerDiscount = partnerDiscount;
            this.Archived = archived;
        }

        // Constructor used when pulling data from DB
        public Play(int playID, string playTitle, string description, string imageURL, int pA, int pB, int pC, int partnerDiscount, bool archived)
        {
            this.PlayID = playID;
            this.PlayTitle = playTitle;
            this.Image = imageURL;
            this.Description = description;
            this.PriceBandA = pA;
            this.PriceBandB = pB;
            this.PriceBandC = pC;
            this.PartnerDiscount = partnerDiscount;
            this.Archived = archived;
        }
        public Play(string PlayTitle)
        {
            this.playTitle = PlayTitle;
        }

        public Play( int PlayID, string PlayTitle)
        {
            this.playTitle = PlayTitle;
            this.playID = PlayID;
        }
    }
}