using AutoMapper;
using ExpenseModule;
using DTOs;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Request;

namespace DentalExpenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseDSL expenseDSL;

        public ExpenseController(IMapper _mapper)
        {
            expenseDSL = new ExpenseDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllExpenses")]
        public IActionResult GetExpenses([FromBody] RequestedData<ExpenseDTO> requestedData)
        {
            requestedData.EntityList = expenseDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetExpense")]
        public IActionResult GetExpense([FromBody] RequestedData<ExpenseDTO> requestedData)
        {
            requestedData.Entity = expenseDSL.GetById(requestedData.Entity.Id);
            requestedData.DetailsList = expenseDSL.GetDetailsLists();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddExpense")]
        public IActionResult AddExpense([FromBody] RequestedData<ExpenseDTO> requestedData)
        {
            expenseDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم اضافة المصاريف بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditExpense")]
        public IActionResult EditExpense([FromBody] RequestedData<ExpenseDTO> requestedData)
        {
            expenseDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم تعديل المصاريف بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteExpense")]
        public IActionResult DeleteExpense([FromBody] RequestedData<ExpenseDTO> requestedData)
        {
            requestedData.EntityList = expenseDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حذف المصاريف بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetExpenseDetailsLists")]
        public IActionResult GetExpenseDetailsLists([FromBody] RequestedData<ExpenseDTO> requestedData)
        {
            requestedData.DetailsList = expenseDSL.GetDetailsLists();
            return Ok(requestedData);
        }
    }
}