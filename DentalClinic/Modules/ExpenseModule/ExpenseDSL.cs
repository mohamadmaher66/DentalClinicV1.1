using Infrastructure;
using System;
using System.Linq;
using AppDBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;
using Enums;
using ClinicModule;

namespace ExpenseModule
{
    public class ExpenseDSL
    {
        ExpenseRepository expenseRepository;
        UnitOfWork UoW;
        IMapper mapper;

        public ExpenseDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            expenseRepository = new ExpenseRepository(UoW, _mapper);
            mapper = _mapper;
        }

        public List<ExpenseDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                double.TryParse(gridSettings.SearchText, out double cost);

                return expenseRepository.GetAll(gridSettings, x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                                   (x.Name.Contains(gridSettings.SearchText)
                                                   || x.Cost == cost)).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ExpenseDTO GetById(int expenseId)
        {
            try
            {
                return expenseRepository.GetById(expenseId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(ExpenseDTO expense, int userId)
        {
            try
            {
                expenseRepository.Add(expense, userId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(ExpenseDTO expense, int userId)
        {
            try
            {
                expenseRepository.Update(expense, userId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ExpenseDTO> Delete(ExpenseDTO expense, GridSettings gridSettings)
        {
            try
            {
                expenseRepository.Delete(expense);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetAll(gridSettings);
        }

        public List<DetailsList> GetDetailsLists()
        {
            try
            {
                List<ClinicDTO> clinicList = new ClinicDSL(mapper).GetAllLite();
                DetailsList donorsList = new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.Clinic,
                    List = clinicList
                };
                List<DetailsList> detailsList = new List<DetailsList>();
                detailsList.Add(donorsList);

                return detailsList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ExpenseDTO> GetExpenseReport(DateTime dateFrom, DateTime dateTo, int clinicId, int userId, ExpenseType type)
        {
            try
            {
                return expenseRepository.GetExpenseReport(dateFrom, dateTo, clinicId, userId, type).ToList();
            }
            catch (Exception e) { throw e; }
        }
    }
}
