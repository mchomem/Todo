namespace Todo.Application.Dtos
{
    public class UserPictureDto
    {
        public int? UserPictureID { get; set; }
        public byte[]? Picture { get; set; }
        public int? PictureFromUserID { get; set; }
    }
}
