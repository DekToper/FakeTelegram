namespace Server
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TelegramModel : DbContext
    {
        public TelegramModel()
            : base("name=TelegramModel1")
        {
        }

        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Groups>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Groups)
                .Map(m => m.ToTable("UsersGroups").MapLeftKey("GroupId").MapRightKey("UserId"));

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Users1)
                .WithMany(e => e.Users2)
                .Map(m => m.ToTable("UserUser").MapLeftKey("Friend2_UserId").MapRightKey("Friend1_UserId"));
        }
    }
}
