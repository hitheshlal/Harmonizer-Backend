using System.Threading.Tasks;
using Harmonizer.DTO;
using Harmonizer.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmonizer.Controllers
{
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpPost]
        [Route("create task")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDTO)
        {
            if (taskDTO == null)
            {
                return BadRequest("Invalid request.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskRepository.CreateTask(taskDTO);

                return Ok(result);
            }
            catch (ArgumentException ex)  
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPut]
        [Route("EditTask")]
        public async Task<IActionResult>EditTask(int taskid, [FromBody] TaskDTO taskDTO)
        {
            if (taskDTO == null)
            {
                return BadRequest("Invalid request.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskRepository.EditTask(taskid, taskDTO);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetTaskById")]
        public async Task<IActionResult> GetTaskById(int taskid)
        {
            if (taskid == null || taskid <= 0)
            {
                return BadRequest("Task id is Invalid");
            }

            try
            {
                var task = await _taskRepository.GetTaskById(taskid);
                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("DeleteTask")]

        public async Task<IActionResult> DeleteTask(int taskId)
        {
            if(taskId == null || taskId <= 0)
            {
                return BadRequest("Task id is Invalid");
            }

            try
            {
                var task = await _taskRepository.DeleteTask(taskId);
                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            

        }

        [HttpGet]
        [Route("GetTaskByUserId")]
        public async Task<IActionResult> GetTaskByUserId(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _taskRepository.GetTaskByUserId(userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("updatestatus")]  
        public async Task<IActionResult> UpdateOrderItemStatus([FromBody] updateStatusRequest request)
        {
            try
            {
                var result = await _taskRepository.UpdateStatus(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status");
            }
        }

    }
}
