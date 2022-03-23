using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace MedicalHistoryModule
{
    public class MedicalHistoryRepository : GenericRepository<MedicalHistory, MedicalHistoryDTO>
    {
        public MedicalHistoryRepository(UnitOfWork UoW, IMapper mapper) : base(UoW, mapper) { }
        public IEnumerable<MedicalHistoryDTO> GetAllLite()
        {
            return _mapper.Map<List<MedicalHistoryDTO>>(_dbset.OrderBy(m => m.Name));
        }
    }
}
