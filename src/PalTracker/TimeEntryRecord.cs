using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalTracker
{
    [Table("time_entries")]
    public class TimeEntryRecord
    {
        [Column("id")]
        public long? Id { get; set; }

        [Column("project_id")]
        public long ProjectId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("hours")]
        public int Hours { get; set; }

        public TimeEntryRecord()
        {
        }
    }
}