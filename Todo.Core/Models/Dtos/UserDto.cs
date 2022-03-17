using System;

namespace Todo.Core.Models.Dtos
{
    [Serializable]
    public class UserDto
    {
        #region Propeties

        public Int32 UserID { get; set; }

        public String Name { get; set; }

        public bool IsActive { get; set; }

        public byte[] Picture { get; set; }

        public string Token { get; set; }

        #endregion
    }
}
