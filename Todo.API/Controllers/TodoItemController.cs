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
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAsync(int userID)
    {
        try
        {
            var items = await _todoItemService.GetAllByUserIdAsync(userID);
            return Ok(items);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(TodoItemInsertDto todoItem)
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
    public async Task<ActionResult> PutAsync(int id, TodoItemUpdateDto todoItem)
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

    [HttpPut("complete/{id}")]
    public async Task<ActionResult> PutMarkTaskAsCompleteAsync(int id)
    {
        try
        {
            await _todoItemService.MarkTaskAsComplete(id);
            return StatusCode(204);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
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
