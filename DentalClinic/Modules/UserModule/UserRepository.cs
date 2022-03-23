using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;
using System.Linq.Expressions;

namespace UserModule
{
    public class UserRepository : GenericRepository<User, UserDTO>
    {
        public UserRepository(UnitOfWork UoW, IMapper mapper) : base(UoW, mapper) { }

        public override IEnumerable<UserDTO> GetAll(GridSettings gridSettings, Expression<Func<User, bool>> predicate)
        {
            IEnumerable<User> userList = _dbset.Where(predicate)
                                    .Select(u => new User { Id = u.Id, Username = u.Username, Address = u.Address, 
                                            FullName = u.FullName, IsActive = u.IsActive, Phone = u.Phone, Role = u.Role });

            gridSettings.RowsCount = userList.Count();
            return _mapper.Map<List<UserDTO>>(userList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }
        public IEnumerable<UserDTO> GetAllLite()
        {
            return _mapper.Map<List<UserDTO>>(_dbset.Where(x => x.IsActive == true)
                                                    .Select(u => new User { Id = u.Id, Username = u.Username, FullName = u.FullName})
                                                    .OrderBy(m => m.FullName));
        }
        public IEnumerable<UserDTO> GetAllDoctorsLite()
        {
            return _mapper.Map<List<UserDTO>>(_dbset.Where(x => x.Role == Enums.RoleEnum.Doctor && x.IsActive == true)
                                                    .OrderBy(m => m.FullName));
        }
        public override UserDTO GetById(int userId)
        {
            return _mapper.Map<UserDTO>(_entities.Set<User>().AsNoTracking()
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
        public UserDTO Login(string username, string password)
        {
            return _mapper.Map<UserDTO>(_dbset
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
