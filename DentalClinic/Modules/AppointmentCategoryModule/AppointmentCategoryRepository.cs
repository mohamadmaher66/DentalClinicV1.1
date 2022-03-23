using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentCategoryModule
{
    public class AppointmentCategoryRepository : GenericRepository<AppointmentCategory, AppointmentCategoryDTO>
    {
        public AppointmentCategoryRepository(IUnitOfWork UoW, IMapper mapper) : base(UoW, mapper) { }

        public IEnumerable<AppointmentCategoryDTO> GetAllLite()
        {
            return _mapper.Map<List<AppointmentCategoryDTO>>(_dbset.OrderBy(m => m.Name));
        }
    }
}
