using AutoMapper;
using AppointmentCategoryModule;
using DTOs;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Request;

namespace DentalClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentCategoryController : ControllerBase
    {
        private readonly AppointmentCategoryDSL appointmentCategoryDSL;

        public AppointmentCategoryController(IMapper _mapper)
        {
            appointmentCategoryDSL = new AppointmentCategoryDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllAppointmentCategorys")]
        public IActionResult GetAppointmentCategorys([FromBody] RequestedData<AppointmentCategoryDTO> requestedData)
        {
            requestedData.EntityList = appointmentCategoryDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAllAppointmentCategoryLite")]
        public IActionResult GetAllAppointmentCategoryLite([FromBody] RequestedData<AppointmentCategoryDTO> requestedData)
        {
            requestedData.EntityList = appointmentCategoryDSL.GetAllLite();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAppointmentCategory")]
        public IActionResult GetAppointmentCategory([FromBody] RequestedData<AppointmentCategoryDTO> requestedData)
        {
            requestedData.Entity = appointmentCategoryDSL.GetById(requestedData.Entity.Id);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddAppointmentCategory")]
        public IActionResult AddAppointmentCategory([FromBody] RequestedData<AppointmentCategoryDTO> requestedData)
        {
            appointmentCategoryDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم الاضافة بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditAppointmentCategory")]
        public IActionResult EditAppointmentCategory([FromBody] RequestedData<AppointmentCategoryDTO> requestedData)
        {
            appointmentCategoryDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم التعديل بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteAppointmentCategory")]
        public IActionResult DeleteAppointmentCategory([FromBody] RequestedData<AppointmentCategoryDTO> requestedData)
        {
            requestedData.EntityList = appointmentCategoryDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم الحذف بنجاح" });
            return Ok(requestedData);
        }
    }
}