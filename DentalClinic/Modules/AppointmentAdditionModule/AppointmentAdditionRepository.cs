using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentAdditionModule
{
    public class AppointmentAdditionRepository : GenericRepository<AppointmentAddition, AppointmentAdditionDTO>
    {
        public AppointmentAdditionRepository(IUnitOfWork UoW, IMapper mapper) : base(UoW, mapper) { }

        public IEnumerable<AppointmentAdditionDTO> GetAllLite()
        {
            return _mapper.Map<List<AppointmentAdditionDTO>>(_dbset.Select(a => new { a.Id, a.Name }).OrderBy(m => m.Name));
        }
    }
}
