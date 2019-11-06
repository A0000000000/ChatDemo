using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Message.Utils;

namespace Message
{
    [Service]
    public class StatusListenService : Service
    {
        public StatusListenService()
        {
            mBinder = new StatusListenBinder(this);
        }
        public override IBinder OnBind(Intent intent)
        {
            return mBinder;
        }

        public int Timer
        {
            get => timer;
            set => timer = value;
        }

        public MainPage Activity
        {
            set => activity = value;
        }
        public override void OnCreate()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(timer);
                    string res = HttpUtils.Post(GlobalData.GetUrl("getStatus"));
                    if (res.Equals("1", StringComparison.OrdinalIgnoreCase) || res.Equals("2", StringComparison.OrdinalIgnoreCase) || res.Equals("3", StringComparison.OrdinalIgnoreCase))
                    {
                        Intent intent = new Intent("Message.Message.NewMessage");
                        intent.PutExtra("type", res);
                        Android.Support.V4.Content.LocalBroadcastManager.GetInstance(activity).SendBroadcast(intent);
                    }
                    else
                    {
                        if (!res.Equals("0", StringComparison.OrdinalIgnoreCase))
                        {
                            activity.RunOnUiThread(() =>
                            {
                                Toast.MakeText(activity, "发生错误, 信息:" + res, ToastLength.Short).Show();
                            });
                        }
                    }
                }
            }).Start();
            base.OnCreate();
        }

        public override void OnDestroy()
        {           
            base.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return base.OnStartCommand(intent, flags, startId);
        }


        private int timer = 3000;
        private StatusListenBinder mBinder;
        private MainPage activity;


    }
}