using AutoMapper;
using DTOs;
using Enums;
using MedicalHistoryModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Request;

namespace DentalClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly MedicalHistoryDSL medicalHistoryDSL;

        public MedicalHistoryController(IMapper _mapper)
        {
            medicalHistoryDSL = new MedicalHistoryDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllMedicalHistorys")]
        public IActionResult GetMedicalHistorys([FromBody] RequestedData<MedicalHistoryDTO> requestedData)
        {
            requestedData.EntityList = medicalHistoryDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAllMedicalHistoryLite")]
        public IActionResult GetAllMedicalHistoryLite([FromBody] RequestedData<MedicalHistoryDTO> requestedData)
        {
            requestedData.EntityList = medicalHistoryDSL.GetAllLite();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetMedicalHistory")]
        public IActionResult GetMedicalHistory([FromBody] RequestedData<MedicalHistoryDTO> requestedData)
        {
            requestedData.Entity = medicalHistoryDSL.GetById(requestedData.Entity.Id);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddMedicalHistory")]
        public IActionResult AddMedicalHistory([FromBody] RequestedData<MedicalHistoryDTO> requestedData)
        {
            medicalHistoryDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم اضافة التاريخ الطبي بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditMedicalHistory")]
        public IActionResult EditMedicalHistory([FromBody] RequestedData<MedicalHistoryDTO> requestedData)
        {
            medicalHistoryDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم تعديل التاريخ الطبي بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteMedicalHistory")]
        public IActionResult DeleteMedicalHistory([FromBody] RequestedData<MedicalHistoryDTO> requestedData)
        {
            requestedData.EntityList = medicalHistoryDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حذف التاريخ الطبي بنجاح" });
            return Ok(requestedData);
        }
    }
}