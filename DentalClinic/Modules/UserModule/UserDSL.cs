using Infrastructure;
using System;
using System.Linq;
using DBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;
using Microsoft.Data.SqlClient;

namespace UserModule
{
    public class UserDSL
    {
        UserRepository userRepository;
        UnitOfWork UoW;

        public UserDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            userRepository = new UserRepository(UoW, _mapper);
        }

        public List<UserDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                return userRepository.GetAll(gridSettings).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserDTO> GetAllLite()
        {
            try
            {
                return userRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<UserDTO> GetAllDoctorsLite()
        {
            try
            {
                return userRepository.GetAllDoctorsLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public UserDTO GetById(int userId)
        {
            try
            {
                return userRepository.GetById(userId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(UserDTO user, int userId)
        {
            try
            {
                userRepository.Add(user, userId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                HandleErrorMessage(e);
            }
        }

        public void Update(UserDTO user, int userId)
        {
            try
            {
                userRepository.Update(user, userId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                HandleErrorMessage(e);
            }
        }
        public List<UserDTO> Delete(UserDTO user, GridSettings gridSettings)
        {
            try
            {
                userRepository.Delete(user);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetAll(gridSettings);
        }

        public UserDTO Login(string username, string password)
        {
            try
            {
                return userRepository.Login(username, password);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private void HandleErrorMessage(Exception ex)
        {
            if(ex.InnerException is SqlException sqlEx 
                && (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                && ex.InnerException.Message.ToLower().Contains("username"))
            {
                throw new Exception(" اسم المستخدم موجود بالفعل !", ex);
            }
            else
            {
                throw ex;
            }
        }
    }
}
