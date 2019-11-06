using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Message.Domain
{
    class User
    {
        public string Id
        {
            get => id;
            set => id = value;
        }
        public string Username
        {
            get => username;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("用户名不能为空!");
                }
                username = value;
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("密码不能为空!");
                }
                password = value;
            }
        }

        public DateTime? Birthday
        {
            get => birthday;
            set => birthday = value;
        }

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("邮箱不能为空!");
                }

                if (Regex.IsMatch(value, "^[a-z0-9A-Z]+[- | a-z0-9A-Z . _]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$"))
                {
                    email = value;
                }
                else
                {
                    throw new Exception("邮箱不合法!");
                }
                
            }
        }

        public string Phone
        {
            get => phone;
            set => phone = value;
        }

        public int Status
        {
            get => status;
            set => status = value;
        }

        private string id;
        private string username;
        private string password;
        private Nullable<DateTime> birthday;
        private string email;
        private string phone;
        private int status;
    }
}