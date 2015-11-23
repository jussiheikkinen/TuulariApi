using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TuulariApi.Models
{
    public class TuulariApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TuulariApiContext() : base("name=TuulariApiContext")
        {
        }

        public System.Data.Entity.DbSet<TuulariApi.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<TuulariApi.Models.Announcement> Announcements { get; set; }

        public System.Data.Entity.DbSet<TuulariApi.Models.Attendance> Attendances { get; set; }

        public System.Data.Entity.DbSet<TuulariApi.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<TuulariApi.Models.Event> Events { get; set; }

        public System.Data.Entity.DbSet<TuulariApi.Models.Location> Locations { get; set; }

        public System.Data.Entity.DbSet<TuulariApi.Models.UserData> UserDatas { get; set; }
    }
}
