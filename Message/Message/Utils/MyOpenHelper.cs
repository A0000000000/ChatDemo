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

namespace Message.Utils
{
    class MyOpenHelper : SQLiteOpenHelper
    {
        private Context context;

        public static readonly string FriendTableName = "t_friend";
        public static readonly string MessageTableName = "t_message";

        private static readonly string friendTable = "create table t_friend(" +
                             "id text primary key," +
                             "username text," +
                             "birthday date," +
                             "email text," +
                             "phone text," +
                             "status integer" +
                             ")";
        private static readonly string messageTable = "create table t_message(" +
                              "id text primary key," +
                              "senderId text," +
                              "receiverId text," +
                              "content text," +
                              "sendTime date," +
                              "status integer" +
                              ")";
        public MyOpenHelper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MyOpenHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version) : base(context, name, factory, version)
        {
            this.context = context;
        }

        public MyOpenHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version, IDatabaseErrorHandler errorHandler) : base(context, name, factory, version, errorHandler)
        {
            this.context = context;
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            
            db.ExecSQL(friendTable);
            db.ExecSQL(messageTable);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            
        }
    }
}