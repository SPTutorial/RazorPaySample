using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Razorpay;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Org.Json;
using Xamarin.Forms;

namespace RazorPaySample.Droid
{
    [Activity(Label = "RazorPaySample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPaymentResultWithDataListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            MessagingCenter.Subscribe<RazorPayload>(this,"PayNow" ,(payload) =>
               {
                   string username = "rzp_test_co1QTfvqLJyWXn";
                   string password = "iAhjtNtHYHrQOQPE09X5XBGC";
                   PayViaRazor(payload, username, password);
               });
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnPaymentError(int p0, string p1, PaymentData p2)
        {
           //
        }

        public void OnPaymentSuccess(string p0, PaymentData p1)
        {
            //
        }

        public async void PayViaRazor(RazorPayload payload,string username,string password)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.razorpay.com/v1/orders"))
                {
                    var plainTextBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                    var basicAuthKey = Convert.ToBase64String(plainTextBytes);

                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {basicAuthKey}");

                    string jsonData = JsonConvert.SerializeObject(payload);
                    request.Content = new StringContent(jsonData);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    string jsonResp = await response.Content.ReadAsStringAsync();
                    RazorResp resp = JsonConvert.DeserializeObject<RazorResp>(jsonResp);

                    if (!string.IsNullOrEmpty(resp.id))
                    {
                        // checkout
                        Checkout checkout = new Checkout();
                        checkout.SetImage(0);

                        checkout.SetKeyID(username);
                        try
                        {
                            JSONObject options = new JSONObject();

                            options.Put("name", "Merchant Name");
                            options.Put("description", $"Reference No. {payload.receipt}");
                            options.Put("image", "https://s3.amazonaws.com/rzp-mobile/images/rzp.png");
                            options.Put("order_id", resp.id);//from response of step 3.
                            options.Put("theme.color", "#3399cc");
                            options.Put("currency", "INR");
                            options.Put("amount", payload.amount);//pass amount in currency subunits
                            options.Put("prefill.email", "spaltutorial@gmail.com");
                            options.Put("prefill.contact", "1234567890");
                            
                            checkout.Open(this, options);
                        }
                        catch (Exception e)
                        {
                           // Log.e(TAG, "Error in starting Razorpay Checkout", e);
                        }

                    }
                    else
                    {
                        Toast.MakeText(this, "Payment Error", ToastLength.Short).Show();
                    }
                }
            }
        }
    }
}