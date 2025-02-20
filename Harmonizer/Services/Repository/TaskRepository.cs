using Harmonizer.Data;
using Harmonizer.DTO;
using Harmonizer.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Harmonizer.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Harmonizer.Services.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDBContext _db;

        public TaskRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateTask(TaskDTO taskDTO)
        {
            var user = await _db.Users.FindAsync(taskDTO.UserId);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var status = await _db.Statuses.FindAsync(taskDTO.StatusId);

            if (status == null)
            {
                throw new ArgumentException("Status not found.");
            }

            var task = new TaskEnitity
            {
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                CreatedAt = DateTime.UtcNow,
                DueDate = taskDTO.DueDate?.ToUniversalTime(),
                UserId = taskDTO.UserId,
                StatusId = taskDTO.StatusId
            };

            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return true;

        }


        //public async Task<object> GetTaskById(int taskid)
        //{
        //    var task = await _db.Tasks.FindAsync(taskid);

        //    if (task == null)
        //    {
        //        throw new ArgumentException("Task not found.");
        //    }

        //    return task;
        //}

        public async Task<IEnumerable<TaskEnitity>> GetTaskByUserId(int userId)
        {
            var tasks = await _db.Tasks
                         .Where(t => t.UserId == userId)
                         .OrderBy(o => o.CreatedAt)
                         .ToListAsync();

            return tasks;
        }

        public async Task<bool> UpdateStatus(updateStatusRequest updateStatusRequest)
        {
            var task = await _db.Tasks.FindAsync(updateStatusRequest.Id);
            if (task == null)
            {
                throw new ArgumentException("Task not found" );
            }
            task.StatusId = updateStatusRequest.StatusId;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool>EditTask(int taskId, TaskDTO taskDTO)
        {
            var task = await _db.Tasks.FindAsync(taskId);

            if (task == null)
            {
                throw new ArgumentException("Task not found.");
            }

            var user = await _db.Users.FindAsync(taskDTO.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            
            task.Title = taskDTO.Title;
            task.Description = taskDTO.Description;
            task.DueDate = taskDTO.DueDate;
            task.UserId = taskDTO.UserId;
           

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTask(int taskId)
        {
            var task = await _db.Tasks.FindAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }
            _db.Tasks.Remove(task);
            _db.SaveChangesAsync();

            return true;
        }

        public async Task<TaskEnitity> GetTaskById(int taskid)
        {
            var task = await _db.Tasks.FindAsync(taskid);

            if (task == null)
            {
                throw new ArgumentException("Task not found.");
            }

            return task;
        }

        //public async Task<TaskEnitity> GetTaskById(int taskid)
        //{
        //    var task = await _db.Tasks.FindAsync(taskId);
        //    if (task == null)
        //    {
        //        throw new ArgumentException("Task not found");
        //    }
        //    var Task = new TaskEnitity
        //    {
        //        Title = task.Title,
        //        Description = task.Description,
        //        CreatedAt = task.CreatedAt,
        //        StatusId = task.StatusId
        //    };
        //    return Task;



        //}
    }
}
