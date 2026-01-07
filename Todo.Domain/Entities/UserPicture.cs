namespace Todo.Domain.Entities;

[Serializable]
public class UserPicture
{
    private UserPicture() { }

    public UserPicture(byte[]? picture, int? pictureFromUserID, User user)
    {
        Picture = picture;
        PictureFromUserID = pictureFromUserID;
        User = user;
    }

    public int UserPictureID { get; private set; }
    public byte[]? Picture { get; private set; }
    public int? PictureFromUserID { get; private set; }
    
    [JsonIgnore]
    public User? User { get; set; }

    public void Update(byte[] picture)
    {
        Picture = picture;
    }
}
