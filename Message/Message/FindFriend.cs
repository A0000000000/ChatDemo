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
using Message.Domain;
using Message.Utils;
using Org.Json;

namespace Message
{
    [Activity(Label = "FindFriend")]
    public class FindFriend : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_findFriend);
            EditText username = FindViewById<EditText>(Resource.Id.et_username);
            Button search = FindViewById<Button>(Resource.Id.btn_search);
            TextView result = FindViewById<TextView>(Resource.Id.tv_result);
            Button add = FindViewById<Button>(Resource.Id.btn_add);
            search.Click += (searchSender, searchE) =>
            {
                ISharedPreferences preferences = GetSharedPreferences("info", FileCreationMode.Private);
                string name = preferences.GetString("username", "");
                if (username.Text.Trim().Equals(name))
                {
                    Toast.MakeText(this, "不能添加自己为好友!", ToastLength.Short).Show();
                    return;
                }
                new Thread(() =>
                {
                    string res = HttpUtils.Post(GlobalData.GetUrl("searchFriend"), new Dictionary<string, string>{["username"] = username.Text.Trim()});
                    if (res.Equals("none", StringComparison.OrdinalIgnoreCase))
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(Application, "查无此人!", ToastLength.Short).Show();
                        });
                    }
                    else
                    {                        
                        User user = new User();
                        JSONObject obj = new JSONObject(res);
                        user.Id = obj.GetString("id").Trim();
                        user.Username = obj.GetString("username").Trim();
                        string birthday = obj.GetString("birthday");
                        user.Birthday = string.IsNullOrEmpty(birthday?.Trim()) ? null as DateTime? : DateTime.Parse(birthday.Trim());
                        user.Email = obj.GetString("email").Trim();
                        user.Phone = obj.GetString("phone").Trim();
                        user.Status = 0;
                        RunOnUiThread(() =>
                        {
                            result.Text = "找到好友: " + user.Username;
                            result.Visibility = ViewStates.Visible;
                            add.Visibility = ViewStates.Visible;
                            add.Click += (addSender, addE) =>
                            {
                                new Thread(() =>
                                {
                                    string str = HttpUtils.Post(GlobalData.GetUrl("addFriend"), new Dictionary<string, string> {["id"] = user.Id});
                                    if (str.Equals("success", StringComparison.OrdinalIgnoreCase))
                                    {
                                        SQLiteUtils db = GlobalData.GetSQLiteUtils();
                                        db.AddNewFriend(user);
                                        RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(Application, "添加成功!", ToastLength.Short).Show();
                                            Intent intent = new Intent(Application, typeof(MainPage));
                                            result.Visibility = ViewStates.Invisible;
                                            add.Visibility = ViewStates.Invisible;
                                            Application.StartActivity(intent);
                                        });
                                    }
                                    else if (str.Equals("repeat", StringComparison.OrdinalIgnoreCase))
                                    {
                                        RunOnUiThread(() =>
                                        {
                                            Toast.MakeText(Application, "目标已是好友!", ToastLength.Short).Show();
                                        });
                                    }
                                    else
                                    {
                                        string[] reses = str.Split(":");
                                        if (reses.Length == 2 && reses[0].Equals("failed", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (reses[1].Equals("6", StringComparison.OrdinalIgnoreCase))
                                            {
                                                RunOnUiThread(() =>
                                                {
                                                    Toast.MakeText(Application, "登录状态异常, 请重新登录后重试!", ToastLength.Short).Show();
                                                });
                                            }
                                            else if(reses[1].Equals("7", StringComparison.OrdinalIgnoreCase))
                                            {
                                                RunOnUiThread(() =>
                                                {
                                                    Toast.MakeText(Application, "目标用户不存在!", ToastLength.Short).Show();
                                                });
                                            }
                                            else
                                            {
                                                RunOnUiThread(() =>
                                                {
                                                    Toast.MakeText(Application, "服务器繁忙, 请稍后再试!", ToastLength.Short).Show();
                                                });
                                            }
                                        }
                                        else
                                        {
                                            RunOnUiThread(() =>
                                            {
                                                Toast.MakeText(Application, "未知错误, 错误信息: " + str, ToastLength.Short).Show();
                                            });
                                        }
                                    }

                                }).Start();
                            };
                        });
                    }
                }).Start();
            };
        }
    }
}