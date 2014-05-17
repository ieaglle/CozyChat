// Oleksandr Babii
// 16/05/2014 19:07 
// 

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CozyChat.Model
{
    public class CozyChatContext : DbContext
    {
        public CozyChatContext()
            : base("CozyChatContext")
        {

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ChatRoom>()
                .HasMany(m => m.Users)
                .WithMany(m => m.ChatRooms)
                .Map(m =>
                {
                    m.ToTable("ChatRoomUserMappings");
                    m.MapLeftKey("ChatRoomId");
                    m.MapRightKey("UserId");
                });
        }

        public virtual DbSet<ChatRoom> ChatRooms { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}