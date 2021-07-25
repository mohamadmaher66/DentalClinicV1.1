using AutoMapper;
using ClinicModule;
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
    public class ClinicController : ControllerBase
    {
        private readonly ClinicDSL clinicDSL;

        public ClinicController(IMapper _mapper)
        {
            clinicDSL = new ClinicDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllClinics")]
        public IActionResult GetClinics([FromBody] RequestedData<ClinicDTO> requestedData)
        {
            requestedData.EntityList = clinicDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAllClinicLite")]
        public IActionResult GetAllClinicLite([FromBody] RequestedData<ClinicDTO> requestedData)
        {
            requestedData.EntityList = clinicDSL.GetAllLite();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetClinic")]
        public IActionResult GetClinic([FromBody] RequestedData<ClinicDTO> requestedData)
        {
            requestedData.Entity = clinicDSL.GetById(requestedData.Entity.Id);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddClinic")]
        public IActionResult AddClinic([FromBody] RequestedData<ClinicDTO> requestedData)
        {
            clinicDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم اضافة العيادة بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditClinic")]
        public IActionResult EditClinic([FromBody] RequestedData<ClinicDTO> requestedData)
        {
            clinicDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم تعديل العيادة بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteClinic")]
        public IActionResult DeleteClinic([FromBody] RequestedData<ClinicDTO> requestedData)
        {
            requestedData.EntityList = clinicDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حذف العيادة بنجاح" });
            return Ok(requestedData);
        }
    }
}