using AutoMapper;
using DBModels;
using System;
using System.Linq;

namespace DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppointmentAddition, AppointmentAdditionDTO>().ReverseMap();
            CreateMap<AppointmentCategory, AppointmentCategoryDTO>().ReverseMap();
            CreateMap<Attachment, AttachmentDTO>().ReverseMap();
            CreateMap<Expense, ExpenseDTO>().ReverseMap();
            CreateMap<MedicalHistory, MedicalHistoryDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<AppointmentTooth, AppointmentToothDTO>().ReverseMap();
            CreateMap<Clinic, ClinicDTO>().ReverseMap();
            CreateMap<Patient, PatientDTO>()
                    .ForMember(dto => dto.MedicalHistoryList, PMH => PMH.MapFrom(MH => MH.PatientMedicalHistoryList.Select(cs => cs.MedicalHistory)))
                    .ReverseMap();
            CreateMap<Appointment, AppointmentDTO>()
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
