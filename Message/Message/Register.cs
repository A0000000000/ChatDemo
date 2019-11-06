using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Message.Domain;
using Message.Utils;

namespace Message
{
    [Activity(Label = "Register")]
    public class Register : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);
            // Create your application here
            Button register = FindViewById<Button>(Resource.Id.registerButton);
            EditText username = FindViewById<EditText>(Resource.Id.input_username);
            EditText password = FindViewById<EditText>(Resource.Id.input_password);
            EditText repassword = FindViewById<EditText>(Resource.Id.input_repassword);
            EditText birthday = FindViewById<EditText>(Resource.Id.input_birthday);
            EditText email = FindViewById<EditText>(Resource.Id.input_email);
            EditText phone = FindViewById<EditText>(Resource.Id.input_phone);            
            register.Click += (registerSender, registerE) =>
            {
                if (password.Text.Trim() != repassword.Text.Trim())
                {
                    Toast.MakeText(this, "输入的两次密码不一致!", ToastLength.Short).Show();
                    return;
                }
                try
                {
                    User user = new User
                    {
                        Username = username.Text.Trim(),
                        Password = password.Text.Trim(),
                        Birthday = string.IsNullOrEmpty(birthday.Text.Trim()) ? null as DateTime? : DateTime.ParseExact(birthday.Text.Trim(), "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                        Email = email.Text.Trim(),
                        Phone = phone.Text.Trim()
                    };
                    new Thread(() =>
                    {
                        string res = HttpUtils.Post(GlobalData.GetUrl("checkUsername"), new Dictionary<string, string> { ["username"] = user.Username });
                        if (res.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            string result = HttpUtils.Post(GlobalData.GetUrl("register"), new Dictionary<string, string>
                            {
                                ["username"] = user.Username, ["password"] = user.Password, ["birthday"] = user.Birthday?.ToString("yyyy/MM/dd"),
                                ["email"] = user.Email, ["phone"] = user.Phone
                            });
                            if (result.Equals("success", StringComparison.OrdinalIgnoreCase))
                            {
                                RunOnUiThread(() =>
                                {
                                    Toast.MakeText(Application, "注册成功! 请登录.", ToastLength.Short).Show();
                                    Intent intent = new Intent(Application, typeof(Login));
                                    Application.StartActivity(intent);
                                });
                            }
                            else
                            {
                                string[] reses = result.Split(":");
                                if (reses.Length == 2 && reses[0].Equals("failed", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (reses[1].Equals("1", StringComparison.OrdinalIgnoreCase))
                                    {
                                        RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(Application, "该名称太火了, 请换一个低调一点的吧.", ToastLength.Short).Show();
                                        });
                                    }
                                    else if (reses[1].Equals("2", StringComparison.OrdinalIgnoreCase))
                                    {
                                        RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(Application, "服务器忙, 请稍后再试.", ToastLength.Short).Show();
                                        });
                                    }
                                    else
                                    {
                                        RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(Application, "未知错误, 请联系开发者!", ToastLength.Short).Show();
                                        });
                                    }
                                }
                                else
                                {
                                    RunOnUiThread(() =>
                                    {
                                        Toast.MakeText(Application, "未知错误, 错误信息: " + result, ToastLength.Short).Show();
                                    });
                                }
                            }
                        }
                        else
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(Application, "用户名冲突!", ToastLength.Short).Show();
                            });
                        }

                    }).Start();
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Short).Show();
                }
            };
        }
    }
}