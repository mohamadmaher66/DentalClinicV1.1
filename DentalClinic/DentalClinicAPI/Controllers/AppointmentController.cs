using AutoMapper;
using AppointmentModule;
using DTOs;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Request;
using System.IO;
using System.Net.Http.Headers;
using System;

namespace DentalAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentDSL appointmentDSL;

        public AppointmentController(IMapper _mapper)
        {
            appointmentDSL = new AppointmentDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllAppointments")]
        public IActionResult GetAppointments([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            requestedData.EntityList = appointmentDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAllAppointmentsDashboard")]
        public IActionResult GetAllAppointmentsDashboard([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            requestedData.EntityList = appointmentDSL.GetAllDashboard(requestedData.Entity);
            requestedData.DetailsList = appointmentDSL.GetDashboardDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAppointment")]
        public IActionResult GetAppointment([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            requestedData.Entity = appointmentDSL.GetById(requestedData.Entity.Id);
            requestedData.DetailsList = appointmentDSL.GetDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddAppointment")]
        public IActionResult AddAppointment([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            appointmentDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم اضافة الكشف بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditAppointment")]
        public IActionResult EditAppointment([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            appointmentDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم تعديل الكشف بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteAppointment")]
        public IActionResult DeleteAppointment([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            requestedData.EntityList = appointmentDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حذف الكشف بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("SaveState")]
        public IActionResult SaveState([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            appointmentDSL.SaveState(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حفظ الكشف بنجاح" });
            return Ok(requestedData);
        }
        
        [HttpPost]
        [Route("GetAppointmentDetailsLists")]
        public IActionResult GetAppointmentDetailsLists([FromBody] RequestedData<AppointmentDTO> requestedData)
        {
            requestedData.DetailsList = appointmentDSL.GetDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("UploadImages")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Images", "Attachments");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = DateTime.Now.ToString("ddMMyyHHmmss") + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}