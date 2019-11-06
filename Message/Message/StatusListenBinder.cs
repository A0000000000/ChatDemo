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
    public class StatusListenBinder: Binder
    {
        public StatusListenBinder(StatusListenService service)
        {
            this.service = service;
        }

        public void EditServiceTimer(int timer)
        {
            service.Timer = timer;
        }

        public void SetActivity(MainPage activity)
        {
            service.Activity = activity;
        }

        private StatusListenService service;
    }
}