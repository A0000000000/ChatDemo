using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Message.Utils;

namespace Message
{
    [Activity(Label = "Login")]
    public class Login : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            // Create your application here
            Button login = FindViewById<Button>(Resource.Id.loginButton);
            Button findPassword = FindViewById<Button>(Resource.Id.findPasswordButton);
            EditText et_username = FindViewById<EditText>(Resource.Id.login_username);
            EditText et_password = FindViewById<EditText>(Resource.Id.login_password);
            CheckBox rememberPassword = FindViewById<CheckBox>(Resource.Id.rememberPassword);
            ISharedPreferences pref = base.GetSharedPreferences("info", FileCreationMode.Private);
            et_username.Text = pref.GetString("username", string.Empty);
            et_password.Text = pref.GetString("password", string.Empty);
            rememberPassword.Checked = pref.GetBoolean("rememberPassword", false);
            login.Click += (loginSender, loginE) =>
            {
                string username = et_username.Text.Trim();
                string password = et_password.Text.Trim();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Toast.MakeText(this, "用户名或密码不能为空!", ToastLength.Short).Show();
                    return;
                }
                ISharedPreferencesEditor editor = base.GetSharedPreferences("info", FileCreationMode.Private).Edit();
                editor.PutString("username", username);
                if (rememberPassword.Checked)
                {
                    editor.PutString("password", password);
                    editor.PutBoolean("rememberPassword", true);
                }
                else
                {
                    editor.PutString("password", string.Empty);
                    editor.PutBoolean("rememberPassword", false);
                }
                editor.Apply();
                new Thread(() =>
                {
                    string res = HttpUtils.Post(GlobalData.GetUrl("login"), new Dictionary<string, string> {["username"] = username, ["password"] = password});
                    if (res.Equals("success", StringComparison.OrdinalIgnoreCase))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(Application, "登录成功!", ToastLength.Short).Show();
                            Intent intent = new Intent(Application, typeof(MainPage));
                            Application.StartActivity(intent);
                        });
                    }
                    else if(res.Equals("failed", StringComparison.OrdinalIgnoreCase))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(Application, "用户名或密码错误!", ToastLength.Short).Show();
                        });
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(Application, "未知错误, 错误信息: " + res, ToastLength.Short).Show();
                        });
                    }
                }).Start();
            };
            findPassword.Click += (findPasswordSender, findPasswordE) =>
            {
                Intent intent = new Intent(this, typeof(FindPassword));
                StartActivity(intent);
            };
        }
    }
}