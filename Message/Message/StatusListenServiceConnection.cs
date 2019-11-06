using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Message
{
    class StatusListenServiceConnection: Java.Lang.Object, IServiceConnection
    {
        private MainPage activity;

        public StatusListenServiceConnection(MainPage activity)
        {
            this.activity = activity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            this.activity.MBinder = service as StatusListenBinder;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            
        }

    }
}