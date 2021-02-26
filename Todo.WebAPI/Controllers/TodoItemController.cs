using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<TodoItem>> Get(int userID)
        {
            try
            {
                return _todoItemRepository
                    .Retrieve(new TodoItem() { CreatedBy = new User() { UserID = userID } });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // POST api/<TodoItemController>
        [HttpPost]
        public ActionResult Post(TodoItem todoItem)
        {
            try
            {
                _todoItemRepository.Create(todoItem);
                return Ok(new { message = "Task created." });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // PUT api/<TodoItemController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, TodoItem todoItem)
        {
            try
            {
                TodoItem todo = _todoItemRepository
                    .Details(new TodoItem() { TodoItemID = id });

                todo.Name = todoItem.Name;
                todo.IsDone = todoItem.IsDone;
                todo.DeadLine = todoItem.DeadLine;

                if (todo == null)
                    return NotFound();

                _todoItemRepository.Update(todo);

                return StatusCode(204);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        // DELETE api/<TodoItemController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _todoItemRepository
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
