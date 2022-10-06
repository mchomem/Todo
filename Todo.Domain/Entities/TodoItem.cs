namespace Todo.Domain.Entities
{
    [Serializable]
    public class TodoItem
    {
        #region properties

        public int? TodoItemID { get; set; }
        public string? Name { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? DeadLine { get; set; }
        public int CreatedByID { get; set; }
        public User? CreatedBy { get; set; }
        public DateTime? CreatedIn { get; set; }

        #endregion
    }
}
