﻿namespace Todo.Application.Services.Interfaces;

public interface IUserPictureService
{
    public Task CreateAsync(UserPicture entity);
    public Task DeleteAsync(UserPicture entity);
    public Task<UserPicture> DetailsAsync(UserPicture entity);
    public Task<IEnumerable<UserPicture>> RetrieveAsync(UserPicture entity);
    public Task UpdateAsync(UserPicture entity);
}
