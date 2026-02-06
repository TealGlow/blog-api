using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

public interface IUserRepository<UserDto>
{
    Task<GetUserResponse> GetByIdAsync(string id);
    // Task<GetAllUsersResponse> GetAllAsync();
    Task<AddUserResponse> AddAsync(AddUserRequest request);
    // Task<UpdateUserResponse> UpdateAsync(UpdateUserRequest request);
    // Task<DeleteUserResponse> DeleteAsync(string id);
}

