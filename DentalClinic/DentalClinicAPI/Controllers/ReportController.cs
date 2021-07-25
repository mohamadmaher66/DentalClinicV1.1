using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ReportModule;
using Request;

namespace DentalClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None, Duration = 0)]
    public class ReportController : ControllerBase
    {
        private readonly ReportDSL reportDSL;

        public ReportController(IMapper _mapper, IHostEnvironment hostEnvironment)
        {
            reportDSL = new ReportDSL(_mapper, hostEnvironment);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        [HttpPost]
        [Route("GetExpenseReport")]
        public IActionResult GetReceivedDonationReport([FromBody] RequestedData<ExpenseFilterDTO> requestedData)
        {
            var reportString = reportDSL.GetExpenseReport(requestedData.Entity);
            return File(reportString, System.Net.Mime.MediaTypeNames.Application.Octet, "ExpenseReport" + ".pdf");
        }

        [HttpPost]
        [Route("GetExpenseDetailsLists")]
        public IActionResult GetExpenseDetailsLists([FromBody] RequestedData<ExpenseFilterDTO> requestedData)
        {
            requestedData.DetailsList = reportDSL.GetExpenseDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAppointmentReport")]
        public IActionResult GetAppointmentReport([FromBody] RequestedData<AppointmentFilterDTO> requestedData)
        {
            var reportString = reportDSL.GetAppointmentReport(requestedData.Entity);
            return File(reportString, System.Net.Mime.MediaTypeNames.Application.Octet, "AppointmentReport" + ".pdf");
        }

        [HttpPost]
        [Route("GetAppointmentDetailsLists")]
        public IActionResult GetAppointmentDetailsLists([FromBody] RequestedData<ExpenseFilterDTO> requestedData)
        {
            requestedData.DetailsList = reportDSL.GetAppointmentDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetTotalExpenseReport")]
        public IActionResult GetTotalExpenseReport([FromBody] RequestedData<TotalExpenseFilterDTO> requestedData)
        {
            var reportString = reportDSL.GetTotalExpenseReport(requestedData.Entity);
            return File(reportString, "application/pdf");
        }

    }
}