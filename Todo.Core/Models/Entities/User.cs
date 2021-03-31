using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Todo.Core.Models.Entities
{
    public class User
    {
        #region Propeties

        public Int32? UserID { get; set; }
        public String Name { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public ICollection<TodoItem> TodoItems { get; set; }

        public UserPicture Picture { get; set; }

        #endregion
    }
}
