using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsDevelopmentGraphicalUserInterface
{
    class PaymentCheck : IPayment
    {

        private readonly Random RandomNumber = new Random();
        
        public bool GetTransactionResult()

        {
            if (RandomNumber.Next(0, 10) != 5)
            {
                PaymentOK();
                return true;
            }

            else
            {
                PaymentDeclined();
                return false;
            }
        }
            
        public void PaymentDeclined()
        {
            System.Windows.Forms.MessageBox.Show("Payment Declined");
        }
            
        public void PaymentOK()
        {
            System.Windows.Forms.MessageBox.Show("Payment Accepted - Order Processed");
        }
        
    }
}
