using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Domain.Entities;
using Todo.Service.Services.Interfaces;

namespace Todo.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;

    public TodoItemController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    // GET: api/<TodoItemController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> Get(int userID)
    {
        try
        {
            var items = await _todoItemService.RetrieveAsync(userID);                

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
            await _todoItemService.CreateAsync(todoItem);

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
            TodoItem todo = await _todoItemService.DetailsAsync(new TodoItem() { TodoItemID = id });
            todo.Name = todoItem.Name;
            todo.IsDone = todoItem.IsDone;
            todo.DeadLine = todoItem.DeadLine;

            if (todo == null)
                return NotFound();

            await _todoItemService.UpdateAsync(todo);

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
            await _todoItemService.DeleteAsync(id);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
