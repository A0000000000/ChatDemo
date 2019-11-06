using System.IO;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Message.Utils;

namespace Message
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            GlobalData.initAssets(Assets);
            GlobalData.initSQLite(ApplicationContext);
            Button registerButton = FindViewById<Button>(Resource.Id.register);
            Button loginButton = FindViewById<Button>(Resource.Id.login);
            registerButton.Click += (registerSender, registerE) =>
            {
                Intent intent = new Intent(this, typeof(Register));
                StartActivity(intent);                
            };
            loginButton.Click += (loginSender, loginE) =>
            {
                Intent intent = new Intent(this, typeof(Login));
                StartActivity(intent);
            };
        }
    }
}