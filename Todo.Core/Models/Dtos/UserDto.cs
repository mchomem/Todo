using System;

namespace Todo.Core.Models.Dtos
{
    public class UserDto
    {
        #region Propeties

        public Int32 UserID { get; set; }

        public String Name { get; set; }

        public bool IsActive { get; set; }

        public string Token { get; set; }

        #endregion
    }
}
