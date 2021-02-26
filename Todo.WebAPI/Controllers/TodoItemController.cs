using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Todo.Core.Models.DataBase.Repositories;
using Todo.Core.Models.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Todo.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        // GET: api/<TodoItemController>
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get(int userID)
        {
            try
            {
                return new TodoItemRepository()
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
                new TodoItemRepository().Create(todoItem);
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
                TodoItem todo = new TodoItemRepository()
                    .Details(new TodoItem() { TodoItemID = id });

                todo.Name = todoItem.Name;
                todo.IsDone = todoItem.IsDone;
                todo.DeadLine = todoItem.DeadLine;

                new TodoItemRepository().Update(todo);

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
                new TodoItemRepository()
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
