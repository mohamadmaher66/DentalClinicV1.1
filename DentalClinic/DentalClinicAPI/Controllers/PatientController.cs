using AutoMapper;
using PatientModule;
using DTOs;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Request;

namespace DentalPatientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly PatientDSL patientDSL;

        public PatientController(IMapper _mapper)
        {
            patientDSL = new PatientDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllPatients")]
        public IActionResult GetPatients([FromBody] RequestedData<PatientDTO> requestedData)
        {
            requestedData.EntityList = patientDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetPatient")]
        public IActionResult GetPatient([FromBody] RequestedData<PatientDTO> requestedData)
        {
            requestedData.Entity = patientDSL.GetById(requestedData.Entity.Id);
            requestedData.DetailsList = patientDSL.GetDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddPatient")]
        public IActionResult AddPatient([FromBody] RequestedData<PatientDTO> requestedData)
        {
            patientDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم اضافة المريض بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditPatient")]
        public IActionResult EditPatient([FromBody] RequestedData<PatientDTO> requestedData)
        {
            patientDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم تعديل المريض بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeletePatient")]
        public IActionResult DeletePatient([FromBody] RequestedData<PatientDTO> requestedData)
        {
            requestedData.EntityList = patientDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حذف المريض بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetPatientDetailsLists")]
        public IActionResult GetPatientDetailsLists([FromBody] RequestedData<PatientDTO> requestedData)
        {
            requestedData.DetailsList = patientDSL.GetDetailsLists();
            return Ok(requestedData);
        }
    }
}