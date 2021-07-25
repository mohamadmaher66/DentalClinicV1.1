using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;
using Enums;

namespace ExpenseModule
{
    public class ExpenseRepository
    {
        private DbContext entities;
        private DbSet<Expense> dbset;
        private readonly IMapper _mapper;

        public ExpenseRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<Expense>();
            _mapper = mapper;
        }

        public IEnumerable<ExpenseDTO> GetAll(GridSettings gridSettings)
        {
            DbSet<Clinic> clinics = entities.Set<Clinic>();
            double.TryParse(gridSettings.SearchText, out double cost);

            IEnumerable<ExpenseDTO> expenseList = from expense in dbset
                                                  join clinic in clinics on expense.ClinicId equals clinic.Id
                                                  where string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                                   (expense.Name.Contains(gridSettings.SearchText)
                                                   || expense.Cost == cost)
                                                  select new ExpenseDTO()
                                                  {
                                                      Id = expense.Id,
                                                      Cost = expense.Cost,
                                                      Name = expense.Name,
                                                      Description = expense.Description,
                                                      ActionDate = expense.ActionDate,
                                                      Type = expense.Type,
                                                      ClinicId = expense.ClinicId,
                                                      ClinicName = clinic.Name
                                                  };


            gridSettings.RowsCount = expenseList.Count();
            return (expenseList.OrderByDescending(m => m.ActionDate)
                                .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                .Take(gridSettings.PageSize));
        }


        public ExpenseDTO GetById(int expenseId)
        {
            return _mapper.Map<ExpenseDTO>(entities.Set<Expense>().AsNoTracking().FirstOrDefault(c => c.Id == expenseId));
        }

        public void Add(ExpenseDTO expense, int userId)
        {
            Expense model = _mapper.Map<Expense>(expense);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = userId;
            model.Clinic = null;

            dbset.Add(model);
        }

        public void Update(ExpenseDTO expense, int userId)
        {
            Expense model = _mapper.Map<Expense>(expense);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = userId;
            model.Clinic = null;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }

        public void Delete(ExpenseDTO expense)
        {
            dbset.Remove(_mapper.Map<Expense>(expense));
        }

        internal IEnumerable<ExpenseDTO> GetExpenseReport(DateTime dateFrom, DateTime dateTo, int clinicId, int userId, ExpenseType type)
        {
            DbSet<Clinic> clinics = entities.Set<Clinic>();
            DbSet<User> users = entities.Set<User>();

            return from expense in dbset
                   join clinic in clinics on expense.ClinicId equals clinic.Id
                   join user in users on expense.CreatedBy equals user.Id
                   where (expense.ActionDate >= dateFrom && expense.ActionDate <= dateTo)
                        && (clinicId == 0 || expense.ClinicId == clinicId)
                        && (userId == 0 || expense.CreatedBy == userId)
                        && (type == ExpenseType.None || expense.Type == type)
                   orderby expense.ActionDate         
                   select new ExpenseDTO()
                   {
                       Id = expense.Id,
                       Cost = expense.Cost,
                       Name = expense.Name,
                       Description = expense.Description,
                       ActionDate = expense.ActionDate,
                       Type = expense.Type,
                       TypeName = expense.Type == ExpenseType.In ? "داخل للعيادة" : "خارج من العيادة",
                       ClinicId = expense.ClinicId,
                       ClinicName = clinic.Name,
                       UserFullName = user.FullName
                   };
        }
    }
}
