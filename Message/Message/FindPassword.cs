using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    [Activity(Label = "FindPassword")]
    public class FindPassword : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_findPassword);
            // Create your application here
            EditText et_username = FindViewById<EditText>(Resource.Id.find_username);
            EditText et_email = FindViewById<EditText>(Resource.Id.find_email);
            EditText et_password = FindViewById<EditText>(Resource.Id.find_password);
            EditText et_repassword = FindViewById<EditText>(Resource.Id.find_repassword);
            Button changePassword = FindViewById<Button>(Resource.Id.findPasswordButton);
            changePassword.Click += (changePasswordSender, changePasswordE) =>
            {
                string username = et_username.Text.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    Toast.MakeText(this, "用户名不能为空!", ToastLength.Short).Show();
                    return;
                }
                if (et_password.Text.Trim() != et_repassword.Text.Trim())
                {
                    Toast.MakeText(this, "两次密码不一致!", ToastLength.Short).Show();
                    return;
                }
                string password = et_password.Text.Trim();
                if (string.IsNullOrEmpty(password))
                {
                    Toast.MakeText(this, "密码不能为空!", ToastLength.Short).Show();
                    return;
                }
                string email = et_email.Text.Trim();
                if (string.IsNullOrEmpty(email))
                {
                    Toast.MakeText(this, "邮箱不能为空!", ToastLength.Short).Show();
                    return;
                }
                if (!(Regex.IsMatch(email, "^[a-z0-9A-Z]+[- | a-z0-9A-Z . _]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$")))
                {
                    Toast.MakeText(this, "邮箱不合法!", ToastLength.Short).Show();
                    return;
                }
                new Thread(() =>
                {
                    string res = HttpUtils.Post(GlobalData.GetUrl("checkUsername"), new Dictionary<string, string> { ["username"] = username });
                    if (res.Equals("N", StringComparison.OrdinalIgnoreCase))
                    {
                        string result = HttpUtils.Post(GlobalData.GetUrl("changePassword"), new Dictionary<string, string> {["username"] = username, ["email"] = email, ["password"] = password });
                        if (result.Equals("success", StringComparison.OrdinalIgnoreCase))
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(Application, "密码修改成功, 请重新登录!", ToastLength.Short).Show();
                                Intent intent = new Intent(Application, typeof(Login));
                                Application.StartActivity(intent);
                            });
                        }
                        else
                        {
                            string[] reses = result.Split(":");
                            if (reses.Length == 2 && reses[0].Equals("failed", StringComparison.OrdinalIgnoreCase))
                            {
                                if (reses[1].Equals("3", StringComparison.OrdinalIgnoreCase))
                                {
                                    RunOnUiThread(() =>
                                    {
                                        Toast.MakeText(Application, "用户名和邮箱不匹配!", ToastLength.Short).Show();
                                    });
                                }
                                else if(reses[1].Equals("4", StringComparison.OrdinalIgnoreCase))
                                {
                                    RunOnUiThread(() =>
                                    {
                                        Toast.MakeText(Application, "服务器忙, 请稍后再试!", ToastLength.Short).Show();
                                    });
                                }
                                else
                                {
                                    RunOnUiThread(() =>
                                    {
                                        Toast.MakeText(Application, "未知错误! 请联系开发者", ToastLength.Short).Show();
                                    });
                                }
                            }
                            else
                            {
                                RunOnUiThread(() =>
                                {
                                    Toast.MakeText(Application, "发生错误, 错误信息: " + result, ToastLength.Short).Show();
                                });
                            }
                        }
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(Application, "用户不存在!", ToastLength.Short).Show();
                        });
                    }
                }).Start();
            };
        }
    }
}