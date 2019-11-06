using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Message.Domain;

namespace Message.Utils
{
    class SQLiteUtils
    {
    
        public string DatabaseName => databaseName;

        public SQLiteUtils(Context context, string databaseName)
        {
            this.context = context;
            this.databaseName = databaseName;
            openHelper = new MyOpenHelper(this.context, this.databaseName, null, 1);
        }

        public void AddNewFriend(User user)
        {
            SQLiteDatabase database = openHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put("id", user.Id);
            values.Put("username", user.Username);
            values.Put("birthday", user.Birthday == null ?  "" :user.Birthday.Value.ToString("yyyy/MM/dd"));
            values.Put("email", user.Email);
            values.Put("phone", user.Phone);
            values.Put("status", user.Status);
            database.Insert(MyOpenHelper.FriendTableName, null, values);
        }
        
        public void AddNewMessage(Domain.Message message)
        {
            SQLiteDatabase database = openHelper.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put("id", message.Id);
            values.Put("senderId", message.SenderId);
            values.Put("receiverId", message.ReceiverId);
            values.Put("content", message.Content);
            values.Put("sendTime", message.SendTime.ToString("yyyy/MM/dd"));
            values.Put("status", message.Status);
            database.Insert(MyOpenHelper.MessageTableName, null, values);
        }

        public List<User> GetAllFriend()
        {
            List<User> list = new List<User>();
            SQLiteDatabase database = openHelper.WritableDatabase;
            ICursor cursor = database.Query(MyOpenHelper.FriendTableName, null, null, null, null, null, null);            ;
            if (cursor.MoveToFirst())
            {
                do
                {
                    User user = new User
                    {
                        Id = cursor.GetString(cursor.GetColumnIndex("id")),
                        Username = cursor.GetString(cursor.GetColumnIndex("username")),
                        Birthday = string.IsNullOrEmpty(cursor.GetString(cursor.GetColumnIndex("birthday"))) ? null as DateTime? : DateTime.Parse(cursor.GetString(cursor.GetColumnIndex("birthday"))),
                        Email = cursor.GetString(cursor.GetColumnIndex("email")),
                        Phone = cursor.GetString(cursor.GetColumnIndex("phone")),
                        Status = cursor.GetInt(cursor.GetColumnIndex("status"))
                    };
                    list.Add(user);
                } while (cursor.MoveToNext());
            }
            cursor.Close();
            ContentValues values = new ContentValues();
            values.Put("status", 0);
            database.Update(MyOpenHelper.FriendTableName, values, null, null);
            return list;
        }

        public List<Domain.Message> GetMessageWithFriend(User friend)
        {
            List<Domain.Message> list = new List<Domain.Message>();
            SQLiteDatabase database = openHelper.WritableDatabase;
            ICursor cursor = database.Query(MyOpenHelper.MessageTableName, null, "senderId = ? or receiverId = ?", new string[] {friend.Id, friend.Id }, null, null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    Domain.Message message = new Domain.Message();
                    message.Id = cursor.GetString(cursor.GetColumnIndex("id"));
                    message.SenderId = cursor.GetString(cursor.GetColumnIndex("senderId"));
                    message.ReceiverId = cursor.GetString(cursor.GetColumnIndex("receiverId"));
                    message.Content = cursor.GetString(cursor.GetColumnIndex("content"));
                    message.SendTime = DateTime.Parse(cursor.GetString(cursor.GetColumnIndex("sendTime")));
                    list.Add(message);
                } while (cursor.MoveToNext());
            }
            ContentValues values = new ContentValues();
            values.Put("status", 0);
            database.Update(MyOpenHelper.MessageTableName, values, null, null);
            return list;
        }

        public User GetUserById(string id)
        {
            User user = new User();
            SQLiteDatabase database = openHelper.WritableDatabase;
            ICursor cursor = database.Query(MyOpenHelper.FriendTableName, null, "id = ?", new string[] {id}, null, null, null);
            if (cursor.MoveToFirst())
            {
                user.Id = cursor.GetString(cursor.GetColumnIndex("id"));
                user.Username = cursor.GetString(cursor.GetColumnIndex("username"));
                user.Birthday = string.IsNullOrEmpty(cursor.GetString(cursor.GetColumnIndex("birthday"))) ? null as DateTime? : DateTime.Parse(cursor.GetString(cursor.GetColumnIndex("birthday")));
                user.Email = cursor.GetString(cursor.GetColumnIndex("email"));
                user.Phone = cursor.GetString(cursor.GetColumnIndex("phone"));
                user.Status = cursor.GetInt(cursor.GetColumnIndex("status"));
            }
            return user;
        }

        private Context context;
        private string databaseName;
        private MyOpenHelper openHelper;
    }


}