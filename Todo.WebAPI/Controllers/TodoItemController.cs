using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemController(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        // GET: api/<TodoItemController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get(int userID)
        {
            try
            {
                var items = await _todoItemRepository
                    .Retrieve(new TodoItem() { CreatedBy = new User() { UserID = userID } });

                return Ok(items);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // POST api/<TodoItemController>
        [HttpPost]
        public async Task<ActionResult> Post(TodoItem todoItem)
        {
            try
            {
                await _todoItemRepository.Create(todoItem);
                return Ok(new { message = "Task created." });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // PUT api/<TodoItemController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, TodoItem todoItem)
        {
            try
            {
                TodoItem todo = (TodoItem)await _todoItemRepository
                    .Details(new TodoItem() { TodoItemID = id });

                todo.Name = todoItem.Name;
                todo.IsDone = todoItem.IsDone;
                todo.DeadLine = todoItem.DeadLine;

                if (todo == null)
                    return NotFound();

                await _todoItemRepository.Update(todo);

                return StatusCode(204);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // DELETE api/<TodoItemController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _todoItemRepository
                    .Delete(new TodoItem() { TodoItemID = id });

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
