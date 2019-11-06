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

namespace Message.Domain
{
    class Message
    {
        public string Id
        {
            get => id;
            set => id = value;
        }

        public string SenderId
        {
            get => senderId;
            set => senderId = value;
        }

        public string ReceiverId
        {
            get => receiverId;
            set => receiverId = value;
        }

        public string Content
        {
            get => content;
            set => content = value;
        }

        public DateTime SendTime
        {
            get => sendTime;
            set => sendTime = value;
        }

        public int Status
        {
            get => status;
            set => status = value;
        }

        private string id;
        private string senderId;
        private string receiverId;
        private string content;
        private DateTime sendTime;
        private int status;
    }
}