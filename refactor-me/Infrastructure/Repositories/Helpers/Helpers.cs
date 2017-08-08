﻿using System.Data.SqlClient;
using System.Web;

namespace RefactorMe.Infrastructure.Repositories
{
    public static class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connstr);
        }
    }
}