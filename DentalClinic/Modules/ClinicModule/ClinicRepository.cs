using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using System.Collections.Generic;
using System.Linq;

namespace ClinicModule
{
    public class ClinicRepository : GenericRepository<Clinic, ClinicDTO>
    {
        public ClinicRepository(UnitOfWork UoW, IMapper mapper) :base (UoW, mapper) { }

        public IEnumerable<ClinicDTO> GetAllLite()
        {
            return _mapper.Map<List<ClinicDTO>>(_dbset.OrderBy(m => m.Name));
        }
    }
}
