using System;

namespace Todo.Core.Models.Entities
{
    public class TodoItem
    {
        #region properties

        public Int32? TodoItemID { get; set; }
        public String Name { get; set; }
        public Boolean? IsDone { get; set; }
        public DateTime? DeadLine { get; set; }
        public Int32 CreatedByID { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedIn { get; set; }

        #endregion
    }
}
