using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

public interface IPostRepository<PostDto>
{
    Task<GetBlogPostResponse> GetByIdAsync(string id);
    Task<GetAllBlogPostsResponse> GetAllAsync();
    Task<AddBlogPostResponse> AddAsync(AddBlogPostRequest request);
    Task<UpdateBlogPostResponse> UpdateAsync(UpdateBlogPostRequest request);
    Task<DeleteBlogPostResponse> DeleteAsync(string id);
}

