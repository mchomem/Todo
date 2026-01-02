namespace Todo.API.Controllers;

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

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, TodoItem todoItem)
    {
        try
        {
            await _todoItemService.UpdateAsync(id, todoItem);
            return StatusCode(204);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

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
