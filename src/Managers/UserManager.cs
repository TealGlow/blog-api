using System;
using Microsoft.AspNetCore.DataProtection.Repositories;

/// <summary>
/// The UserManager class is responsible for managing user-related operations in the application. 
/// </summary>
public class UserManager
{
    private readonly IUserRepository<UserDto> _repository;

    public UserManager(IUserRepository<UserDto> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retrieves a single user by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be retrieved.</param>
    /// <returns>A GetUserResponse containing the details of the requested user.</returns>
    public async Task<GetUserResponse> GetUsersAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }


    /// <summary>
    /// Adds a new user to the database. 
    /// </summary>
    /// <param name="request"></param>
    /// <returns>
    /// An AddUserResponse indicating the result of the add operation.
    /// </returns>
    public async Task<AddUserResponse> AddUserAsync(AddUserRequest request)
    {
        return await _repository.AddAsync(request);

    }
}