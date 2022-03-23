using AutoMapper;
using DBModels;
using System.Linq;

namespace DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppointmentAddition, AppointmentAdditionDTO>().ReverseMap();
            //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.AppointmentAdditionId))
            CreateMap<AppointmentCategory, AppointmentCategoryDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.AppointmentCategoryId))
                .ReverseMap();

            CreateMap<Attachment, AttachmentDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.AppointmentId))
                //.ForMember(dto => dto.AppointmentId, model => model.MapFrom(m => m.Appointment.AppointmentId))
                .ReverseMap();

            CreateMap<Expense, ExpenseDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.ExpenseId))
                .ForMember(dto => dto.ClinicId, model => model.MapFrom(m => m.Clinic.Id))
                .ForMember(dto => dto.ClinicName, model => model.MapFrom(m => m.Clinic.Name))
                .ReverseMap();

            CreateMap<User, UserDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.UserId))
                .ReverseMap();

            CreateMap<AppointmentTooth, AppointmentToothDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.AppointmentToothId))
                //.ForMember(dto => dto.AppointmentId, model => model.MapFrom(m => m.Appointment.AppointmentId))
                .ReverseMap();

            CreateMap<Clinic, ClinicDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.ClinicId))
                .ReverseMap();

            CreateMap<Patient, PatientDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.PatientId))
                //.ForMember(dto => dto.MedicalHistoryList, PMH => PMH.MapFrom(MH => MH.PatientMedicalHistoryList.Select(cs => cs.MedicalHistory).ToList()))
                .ForMember(dto => dto.MedicalHistoryList, PMH => PMH.MapFrom(MH => MH.PatientMedicalHistoryList))
                .ReverseMap()
                .ForMember(d => d.PatientMedicalHistoryList, opt => opt.MapFrom(s => s.MedicalHistoryList
                    .Select(c => new PatientMedicalHistory { PatientId = s.Id, MedicalHistoryId = c.Id })));

            CreateMap<MedicalHistory, MedicalHistoryDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.MedicalHistoryId))
                .ReverseMap();

            CreateMap<PatientMedicalHistory, MedicalHistoryDTO>()
                .ForMember(dto => dto.Id, model => model.MapFrom(m => m.MedicalHistoryId))
                //.IncludeMembers(x => x.Patient)
                .ReverseMap();

            CreateMap<PatientMedicalHistory, PatientDTO>()
                .ForMember(dto => dto.Id, model => model.MapFrom(m => m.PatientId))
                //.IncludeMembers(x => x.Patient)
                .ReverseMap();

            CreateMap<Appointment, AppointmentDTO>()
                //.ForMember(dto => dto.Id, model => model.MapFrom(m => m.AppointmentId))
                .ForMember(dto => dto.Patient, pat => pat.MapFrom(model => model.Patient))
                .ForMember(dto => dto.Clinic, clinic => clinic.MapFrom(c => c.Clinic))
                .ForMember(dto => dto.Category, cat => cat.MapFrom(c => c.Category))
                .ForMember(dto => dto.User, doc => doc.MapFrom(c => c.User))
                .ForMember(dto => dto.AppointmentAdditionList, AAA => AAA.MapFrom(AA => AA.AppointmentAppointmentAdditionList.Select(aa => aa.AppointmentAddition)))
                .ForMember(dto => dto.AttachmentList, AAA => AAA.MapFrom(AA => AA.AttachmentList))
                .ForMember(dto => dto.ToothList, AAA => AAA.MapFrom(AA => AA.AppointmentToothList))
                .ReverseMap();

        }
    }
}
