using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RazorPaySample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            RazorPayload payload = new RazorPayload();
            payload.amount = 2000;
            payload.currency = "INR";
            payload.receipt = GenerateRefNo();


            MessagingCenter.Send<RazorPayload>(payload, "PayNow");
        }
        public string GenerateRefNo()
        {
            string refNo = "";
            Random ran = new Random();
            String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int length = 6;
            String random = "";
            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(26);
                random = random + b.ElementAt(a);
            }

            int d = ran.Next(100000, 999999);

            refNo = $"{random}{d}";
            return refNo;
        }
    }
}
