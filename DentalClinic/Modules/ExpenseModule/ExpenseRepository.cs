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
using System.Linq.Expressions;

namespace ExpenseModule
{
    public class ExpenseRepository : GenericRepository<Expense, ExpenseDTO>
    {
        public ExpenseRepository(UnitOfWork UoW, IMapper mapper) : base(UoW, mapper) { }

        public override IEnumerable<ExpenseDTO> GetAll(GridSettings gridSettings, Expression<Func<Expense, bool>> predicate)
        {
            DbSet<Clinic> clinics = _entities.Set<Clinic>();
            
            IQueryable<ExpenseDTO> expenseList = from expense in _dbset.Include(e => e.Clinic)
                                                 .Where(predicate).AsNoTracking()
                                                 select new ExpenseDTO()
                                                 {
                                                     Id = expense.Id,
                                                     Cost = expense.Cost,
                                                     Name = expense.Name,
                                                     Description = expense.Description,
                                                     ActionDate = expense.ActionDate,
                                                     Type = expense.Type,
                                                     ClinicId = expense.ClinicId,
                                                     ClinicName = expense.Clinic.Name
                                                 };

            gridSettings.RowsCount = expenseList.Count();
            return expenseList.OrderByDescending(m => m.ActionDate)
                              .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                              .Take(gridSettings.PageSize);
        }

        public override ExpenseDTO GetById(int id)
        {
            return _mapper.Map<ExpenseDTO>(_entities.Set<Expense>().Include(c => c.Clinic).FirstOrDefault(e => e.Id == id));
        }

        public override void Add(ExpenseDTO dto, int userId)
        {
            Expense expense = _mapper.Map<Expense>(dto);
            expense.CreationDate = DateTime.Now;
            expense.CreatedBy = userId;

            expense.Clinic = null;
            _dbset.Add(expense);
        }

        internal IEnumerable<ExpenseDTO> GetExpenseReport(DateTime dateFrom, DateTime dateTo, int clinicId, int userId, ExpenseType type)
        {
            DbSet<Clinic> clinics = _entities.Set<Clinic>();
            DbSet<User> users = _entities.Set<User>();

            return from expense in _dbset
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
