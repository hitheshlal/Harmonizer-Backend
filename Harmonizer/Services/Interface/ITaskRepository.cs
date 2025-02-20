using Harmonizer.DTO;
using Harmonizer.Model;
using Microsoft.AspNetCore.Mvc;

namespace Harmonizer.Services.Interface
{
    public interface ITaskRepository
    {
        Task<bool> CreateTask(TaskDTO taskDTO);
        Task<IEnumerable<TaskEnitity>> GetTaskByUserId(int userId);

        Task<bool> UpdateStatus(updateStatusRequest updateStatusRequest);

        Task<bool> EditTask(int taskId, TaskDTO taskDTO);
        Task<bool> DeleteTask(int taskId);
        Task<TaskEnitity> GetTaskById(int taskid);
    }
}
