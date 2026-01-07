namespace Todo.Application.ProfileMappings;

public static class ProfileMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.Picture, src => src.Picture != null ? src.Picture.Picture : null)
            .TwoWays();

        config.NewConfig<User, UserInsertDto>().TwoWays();
        config.NewConfig<User, UserUpdateDto>().TwoWays();
        config.NewConfig<User, UserChangePasswordDto>().TwoWays();

        config.NewConfig<TodoItem, TodoItemDto>().TwoWays();
        config.NewConfig<TodoItem, TodoItemInsertDto>().TwoWays();
        config.NewConfig<TodoItem, TodoItemUpdateDto>().TwoWays();

        config.NewConfig<UserPicture, UserPictureDto>().TwoWays();
        config.NewConfig<UserPicture, UserPictureInsertDto>().TwoWays();
    }
}
