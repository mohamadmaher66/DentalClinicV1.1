using AutoMapper;
using AppointmentAdditionModule;
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
    public class AppointmentAdditionController : ControllerBase
    {
        private readonly AppointmentAdditionDSL appointmentAdditionDSL;

        public AppointmentAdditionController(IMapper _mapper)
        {
            appointmentAdditionDSL = new AppointmentAdditionDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllAppointmentAdditions")]
        public IActionResult GetAppointmentAdditions([FromBody] RequestedData<AppointmentAdditionDTO> requestedData)
        {
            requestedData.EntityList = appointmentAdditionDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAllAppointmentAdditionLite")]
        public IActionResult GetAllAppointmentAdditionLite([FromBody] RequestedData<AppointmentAdditionDTO> requestedData)
        {
            requestedData.EntityList = appointmentAdditionDSL.GetAllLite();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAppointmentAddition")]
        public IActionResult GetAppointmentAddition([FromBody] RequestedData<AppointmentAdditionDTO> requestedData)
        {
            requestedData.Entity = appointmentAdditionDSL.GetById(requestedData.Entity.Id);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddAppointmentAddition")]
        public IActionResult AddAppointmentAddition([FromBody] RequestedData<AppointmentAdditionDTO> requestedData)
        {
            appointmentAdditionDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم الاضافة بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditAppointmentAddition")]
        public IActionResult EditAppointmentAddition([FromBody] RequestedData<AppointmentAdditionDTO> requestedData)
        {
            appointmentAdditionDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم التعديل بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteAppointmentAddition")]
        public IActionResult DeleteAppointmentAddition([FromBody] RequestedData<AppointmentAdditionDTO> requestedData)
        {
            requestedData.EntityList = appointmentAdditionDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم الحذف بنجاح" });
            return Ok(requestedData);
        }
    }
}