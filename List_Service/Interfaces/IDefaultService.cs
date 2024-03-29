﻿using Microsoft.AspNetCore.Http;

namespace List_Service.Interfaces
{
    public interface IDefaultService<RequestType,ResponseType> where RequestType : class? where ResponseType : class? 
    {
        Task<int> Add(ResponseType item);

        Task<IQueryable<RequestType>> GetByUserId();

        Task<IQueryable<RequestType>> GetAll();

        Task<List<int>> Remove(List<int> ids);

        Task<bool> RemoveFromDb(List<int> ids);

        Task<int> Update(ResponseType item, int itemId);
    }
}
