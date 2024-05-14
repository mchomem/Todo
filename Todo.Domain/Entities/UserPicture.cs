namespace Todo.Domain.Entities;

[Serializable]
public class UserPicture
{
    public int? UserPictureID { get; set; }
    public byte[]? Picture { get; set; }
    public int? PictureFromUserID { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}
