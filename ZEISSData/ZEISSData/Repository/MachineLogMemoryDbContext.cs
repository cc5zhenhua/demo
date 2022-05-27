using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZEISSData.Repository
{
    public class MachineLogMemoryDbContext:DbContext
    {
        public MachineLogMemoryDbContext(DbContextOptions<MachineLogMemoryDbContext> options):base(options){ 
        }

        public  DbSet<MessageEntity> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageEntity>(entity =>
            {
                entity.HasKey(e => e.MessageId);
            });
        }
    }


    public class MessageEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; } 
        public string topic { get; set; }
        public string _ref { get; set; }
        //public string join_ref { get; set; }
        public string _event { get; set; }
        public DateTime timestamp { get; set; }
        public string status { get; set; }
        public string machine_id { get; set; }
        public string id { get; set; }
    }
}
