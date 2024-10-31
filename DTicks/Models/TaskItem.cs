using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTicks.Models
{
    [Table("tasks")]
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        //[Column("created")]
        //public DateTime Created { get; set; }

        //[Column("dueDate")]
        //public DateOnly DueDate { get; set; }

        [Column("isDone")]
        public bool IsDone { get; set; } = false;
    }
}
