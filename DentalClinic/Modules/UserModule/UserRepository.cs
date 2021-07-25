using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace UserModule
{
    public class UserRepository
    {
        private DbContext entities;
        private DbSet<User> dbset;
        private readonly IMapper _mapper;

        public UserRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<User>();
            _mapper = mapper;
        }

        public IEnumerable<UserDTO> GetAll(GridSettings gridSettings)
        {
            IEnumerable<User> userList = dbset.Where(x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                    (x.FullName.Contains(gridSettings.SearchText)
                                    || x.Address.Contains(gridSettings.SearchText)
                                    || x.Username.Contains(gridSettings.SearchText)
                                    || x.Phone.Contains(gridSettings.SearchText)
                                    ))
                                    .Select(u => new User { Id = u.Id , Username = u.Username, Address = u.Address, 
                                            FullName = u.FullName, IsActive = u.IsActive, Phone = u.Phone, Role = u.Role });

            gridSettings.RowsCount = userList.Count();
            return _mapper.Map<List<UserDTO>>(userList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }
        public IEnumerable<UserDTO> GetAllLite()
        {
            return _mapper.Map<List<UserDTO>>(dbset.Where(x => x.IsActive == true)
                                                    .Select(u => new User { Id = u.Id, Username = u.Username, FullName = u.FullName})
                                                    .OrderBy(m => m.FullName));
        }
        public IEnumerable<UserDTO> GetAllDoctorsLite()
        {
            return _mapper.Map<List<UserDTO>>(dbset.Where(x => x.Role == Enums.RoleEnum.Doctor && x.IsActive == true)
                                                    .OrderBy(m => m.FullName));
        }
        public UserDTO GetById(int userId)
        {
            return _mapper.Map<UserDTO>(entities.Set<User>().AsNoTracking()
                                        .Select(u => new User
                                        {
                                            Id = u.Id,
                                            Username = u.Username,
                                            Address = u.Address,
                                            FullName = u.FullName,
                                            IsActive = u.IsActive,
                                            Phone = u.Phone,
                                            Role = u.Role
                                        }).FirstOrDefault(c => c.Id == userId));
        }

        public void Add(UserDTO user, int userId)
        {
            User model = _mapper.Map<User>(user);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = userId;
            dbset.Add(model);
        }

        public void Update(UserDTO user, int userId)
        {
            User model = _mapper.Map<User>(user);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = userId;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;

            if (string.IsNullOrEmpty(user.Password))
            {
                entities.Entry(model).Property(m => m.Password).IsModified = false;
            }
        }
        public void Delete(UserDTO user)
        {
            dbset.Remove(_mapper.Map<User>(user));
        }

        public UserDTO Login(string username, string password)
        {
            return _mapper.Map<UserDTO>(dbset
                            .Where(u => u.Username == username && u.Password == password && u.IsActive == true)
                            .Select(u => new User
                            {
                                Id = u.Id,
                                Username = u.Username,
                                FullName = u.FullName,
                                Role = u.Role
                            })
                            .FirstOrDefault());
        }
    }
}
