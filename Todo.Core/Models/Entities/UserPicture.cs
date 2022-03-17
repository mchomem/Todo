using System;
using System.Text.Json.Serialization;

namespace Todo.Core.Models.Entities
{
    [Serializable]
    public class UserPicture
    {
        public int? UserPictureID { get; set; }
        public byte[] Picture { get; set; }
        public int? PictureFromUserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
