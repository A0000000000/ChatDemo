using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Message.Domain;
using Message.Utils;
using Thread = System.Threading.Thread;
using Toolbar = Android.Widget.Toolbar;

namespace Message
{
    [Activity(Label = "FriendList", Theme = "@style/FriendListTheme")]
    public class MainPage : Activity, AdapterView.IOnItemClickListener
    {
        public StatusListenBinder MBinder
        {
            set
            {
                mBinder = value;
                mBinder.SetActivity(this);
            }
        }

        private StatusListenBinder mBinder;
        private StatusListenServiceConnection mConnection;
        private NewMessageReceiver receiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_mainPage);
            // Create your application here
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ISharedPreferences preferences = GetSharedPreferences("info", FileCreationMode.Private);
            string username = preferences.GetString("username", "");
            ActionBar.Title = "欢迎您, " + username + "!";
            mConnection = new StatusListenServiceConnection(this);
            Intent intent = new Intent(this, typeof(StatusListenService));
            BindService(intent, mConnection, Bind.AutoCreate);
            StartService(intent);
            receiver = new NewMessageReceiver();
            Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).RegisterReceiver(receiver, new IntentFilter("Message.Message.NewMessage"));
            Runtime.GetRuntime().AddShutdownHook(new Java.Lang.Thread(() =>
            {
                StopService(intent);
                UnbindService(mConnection);
            }));
            LoadFriendList();
        }

        private List<User> friends;
        private List<string> users;
        
        private void LoadFriendList()
        {
            SQLiteUtils db = GlobalData.GetSQLiteUtils();
            friends = db.GetAllFriend();
            users = new List<string>();
            foreach (User user in friends)
            {
                users.Add(user.Username);
            }
            ListView lv_friend = FindViewById<ListView>(Resource.Id.friend_list);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, users);
            lv_friend.Adapter = adapter;
            lv_friend.OnItemClickListener = this;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_search:
                    Intent intent1 = new Intent(this, typeof(FindFriend));
                    StartActivity(intent1);
                    break;
                case Resource.Id.menu_logout:
                    new Thread(() =>
                    {
                        string res = HttpUtils.Post(GlobalData.GetUrl("logout"));
                        if (res.Equals("success", StringComparison.OrdinalIgnoreCase))
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(Application, "登出成功, 请重新登录!", ToastLength.Short).Show();
                                Intent intent2 = new Intent(Application, typeof(Login));
                                Application.StartActivity(intent2);
                            });
                        }
                        else if (res.Equals("failed", StringComparison.OrdinalIgnoreCase))
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(Application, "登出失败, 请重试!", ToastLength.Short).Show();
                            });
                        }
                        else
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(Application, "发生错误, 信息: " + res, ToastLength.Short).Show();
                            });
                        }
                    }).Start();
                    break;
                case Resource.Id.menu_flush:
                    LoadFriendList();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            User user = null;
            foreach (User friend in friends)
            {
                if (users[position].Equals(friend.Username))
                {
                    user = friend;
                    break;
                }
            }
            if (user == null)
            {
                Toast.MakeText(this, "发生错误, 请刷新列表后重试.", ToastLength.Short).Show();
            }
            Intent intent = new Intent(this, typeof(Chat));
            intent.PutExtra("username", user.Username);
            intent.PutExtra("id", user.Id);
            StartActivity(intent);
        }
    }
}