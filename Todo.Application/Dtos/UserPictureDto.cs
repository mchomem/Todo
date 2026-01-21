namespace Todo.Application.Dtos
{
    public class UserPictureDto
    {
        public int? UserPictureID { get; set; }
        public byte[]? Picture { get; set; }
        public int? PictureFromUserID { get; set; }
    }

    public class UserPictureInsertDto
    {
        public required byte[] Picture { get; init; }
        public required int PictureFromUserID { get; init; }
    }
}
