﻿using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;

namespace List_Service.Services
{
    public class CustomListService : ICustomListService
    {
        private readonly ICustomListRepository _customListRepository;
        private readonly IMapper _mapper;
        public CustomListService(ICustomListRepository customListRepository,IMapper mapper)
        {
            _mapper= mapper;
            _customListRepository = customListRepository;
        }

        public async Task<int> Add(CreateCustomList item, int userId)
        {
            item.Name.Trim();

            if (await _customListRepository.CheckIfNameExist(item.Name,userId))
                throw new ValidProblemException($"{item.Name} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Name))
                throw new ValidProblemException($"{item.Name} - Not valide");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.UserId= userId;
            
            await _customListRepository.Add(itemToDb);

            return itemToDb.Id;
        }

        public async Task<IQueryable<CustomListView>> Get(int userid)
        {
            var items = await _customListRepository.Get(userid);

            return items.ToList().Select(x => _mapper.Map<CustomListView>(x)).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids, int userId)
        {
            return await _customListRepository.Remove(ids,userId);            
        }

        public async Task<int> Update(CreateCustomList item, int userId, int listId)
        {
            item.Name.Trim();

            if (await _customListRepository.CheckIfNameExist(item.Name, userId))
                throw new ValidProblemException($"{item.Name} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Name))
                throw new ValidProblemException($"{item.Name} - Not valide");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.Id = listId;
            itemToDb.UserId = userId;

            if (!await _customListRepository.Update(itemToDb, userId))
                throw new NotFoundException($"{listId} - can't be Foud");

            return itemToDb.Id;
        }
    }
}