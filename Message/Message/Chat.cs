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
using Java.Util;
using Message.Domain;
using Message.Utils;

namespace Message
{
    [Activity(Label = "Chat")]
    public class Chat : AppCompatActivity
    {
        private string username;
        private string id;        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chat);
            // Create your application here
            username = Intent.GetStringExtra("username");
            id = Intent.GetStringExtra("id");
            SupportActionBar.Title = $"与 {username} 的会话";
            EditText input_message = FindViewById<EditText>(Resource.Id.input_sendMessage);
            Button send_message = FindViewById<Button>(Resource.Id.chat_sendMessage);
            Button flush = FindViewById<Button>(Resource.Id.chat_flush);
            LoadMessage();
            send_message.Click += (btnSend, btnE) =>
            {
                string inputText = input_message.Text;
                if (string.IsNullOrEmpty(inputText))
                {
                    return;
                }
                new Thread(() =>
                {
                    string res = HttpUtils.Post(GlobalData.GetUrl("sendMessage"), new Dictionary<string, string>{["targetId"] = id, ["content"] = inputText});
                    if (res.Equals("error:1", StringComparison.OrdinalIgnoreCase))
                    {
                        Toast.MakeText(this, "登录状态异常, 请重新登陆!", ToastLength.Short).Show();
                    }
                    else if (res.Equals("failed", StringComparison.OrdinalIgnoreCase))
                    {
                        Toast.MakeText(this, "发送失败, 请重试!", ToastLength.Short).Show();
                    }
                    else
                    {
                        string[] reses = res.Split("#");
                        SQLiteUtils db = GlobalData.GetSQLiteUtils();
                        Domain.Message message = new Domain.Message
                        {
                            Id = reses[0],
                            SenderId = reses[1],
                            ReceiverId = id,
                            Content = inputText,
                            SendTime = DateTime.Now
                        };
                        db.AddNewMessage(message);
                        RunOnUiThread(() =>
                        {
                            LoadMessage();
                            input_message.Text = "";
                        });
                    }
                }).Start();
            };
            flush.Click += (flushSender, flushE) =>
            {
                LoadMessage();
            };
        }

        private void LoadMessage()
        {
            ListView lv_message = FindViewById<ListView>(Resource.Id.message_list);
            SQLiteUtils db = GlobalData.GetSQLiteUtils();
            User friend = db.GetUserById(id);
            List<Domain.Message> list = db.GetMessageWithFriend(friend);
            List<string> chats = new List<string>();
            foreach (Domain.Message message in list)
            {
                chats.Add($"{(message.SenderId == friend.Id ? friend.Username : "我")}说: {message.Content} (时间: {message.SendTime:yyyy/MM/dd})");
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, chats);
            lv_message.Adapter = adapter;
        }
    }
}