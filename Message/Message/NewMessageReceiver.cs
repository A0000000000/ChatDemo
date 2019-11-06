using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Message.Domain;
using Message.Utils;
using Org.Json;

namespace Message
{
    [BroadcastReceiver(Enabled = true)]
    public class NewMessageReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string type = intent.GetStringExtra("type");
            if (type.Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                NewFriend(context);
                MakeNotification("新通知", "您有一位新好友", context);
            }
            else if(type.Equals("2", StringComparison.OrdinalIgnoreCase))
            {
                NewMessage(context);
                MakeNotification("新通知", "您有一条新消息", context);
            }
            else if(type.Equals("3", StringComparison.OrdinalIgnoreCase))
            {
                NewFriend(context);
                MakeNotification("新通知", "您有一位新好友", context);
                NewMessage(context);
                MakeNotification("新通知", "您有一条新消息", context);
            }
            else
            {
                Toast.MakeText(context, "未知广播类型: " + type, ToastLength.Short).Show();
            }
        }

        private void NewFriend(Context context)
        {
            new Thread(() =>
            {
                string xmlRes = HttpUtils.Post(GlobalData.GetUrl("getNewFriend"));
                XmlDocument xml = new XmlDocument();
                xml.Load(new MemoryStream(Encoding.ASCII.GetBytes(xmlRes)));
                XmlNodeList users = xml.ChildNodes[0].ChildNodes;
                for (int i = 0; i < users.Count; i++)
                {
                    XmlNodeList user = users[i].ChildNodes;
                    User u = new User { Status = 1 };
                    foreach (XmlNode node in user)
                    {
                        switch (node.Name)
                        {
                            case "id":
                                u.Id = node.InnerText;
                                break;
                            case "username":
                                u.Username = node.InnerText;
                                break;
                            case "birthday":
                                u.Birthday = string.IsNullOrEmpty(node.InnerText) ? null as DateTime? : DateTime.Parse(node.InnerText);
                                break;
                            case "email":
                                u.Email = node.InnerText;
                                break;
                            case "phone":
                                u.Phone = node.InnerText;
                                break;
                            default:
                                break;
                        }
                    }
                    GlobalData.GetSQLiteUtils().AddNewFriend(u);
                }
            }).Start();
        }

        private void NewMessage(Context context)
        {
            new Thread(() =>
            {
                SQLiteUtils db = GlobalData.GetSQLiteUtils();
                string jsonArray = HttpUtils.Post(GlobalData.GetUrl("getNewMessage"));
                JSONArray array = new JSONArray(jsonArray);
                for (int i = 0; i < array.Length(); i++)
                {
                    JSONObject obj = array.GetJSONObject(i);
                    Domain.Message message = new Domain.Message();
                    message.Id = obj.GetString("id");
                    message.SenderId = obj.GetString("senderId");
                    message.ReceiverId = obj.GetString("receiverId");
                    message.Content = obj.GetString("content");
                    message.SendTime = DateTime.Parse(obj.GetString("sendTime"));
                    db.AddNewMessage(message);
                }
            }).Start();
        }

        public void MakeNotification(string title, string content, Context context)
        {
            NotificationManager manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel("1", "default", NotificationImportance.Default);
                channel.EnableLights(true);
                channel.LightColor = Color.Green;
                channel.SetShowBadge(true);
                manager.CreateNotificationChannel(channel);
            }
            Notification notification = new NotificationCompat.Builder(context)
                .SetContentTitle(title)
                .SetContentText(content)
                .SetWhen(JavaSystem.CurrentTimeMillis())
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .Build();
            manager.Notify(1, notification);
        }
    }
}