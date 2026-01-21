namespace Todo.API.Controllers;

/// <summary>
/// Controller responsible for managing to do items operations including creation, retrieval, update, and deletion.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoItemController"/> class.
    /// </summary>
    /// <param name="todoItemService">The to do item service.</param>
    public TodoItemController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    /// <summary>
    /// Retrieves all to do items for a specific user.
    /// </summary>
    /// <param name="userID">The identifier of the user.</param>
    /// <returns>Returns a collection of to do items belonging to the specified user.</returns>
    /// <response code="200">Returns the list of to do items.</response>
    /// <response code="500">If an error occurs while retrieving the items.</response>
    [HttpGet]
    public async Task<IActionResult> GetAsync(int userID)
    {
        var items = await _todoItemService.GetAllByUserIdAsync(userID);
        var response = new ApiResponse<IEnumerable<TodoItemDto>>(items);
        return Ok(response);
    }

    /// <summary>
    /// Creates a new to do item.
    /// </summary>
    /// <param name="todoItem">The to do item data to create.</param>
    /// <returns>Returns a success message if the task is created successfully.</returns>
    /// <response code="200">Returns a success message.</response>
    /// <response code="500">If an error occurs while creating the task.</response>
    [HttpPost]
    public async Task<IActionResult> PostAsync(TodoItemInsertDto todoItem)
    {
        await _todoItemService.CreateAsync(todoItem);
        var response = new ApiResponse<string>("A new todo created.");
        return Ok(response);
    }

    /// <summary>
    /// Updates an existing to do item.
    /// </summary>
    /// <param name="id">The identifier of the to do item to update.</param>
    /// <param name="todoItem">The updated to do item data.</param>
    /// <returns>Returns no content if the update is successful.</returns>
    /// <response code="204">If the to do item was updated successfully.</response>
    /// <response code="500">If an error occurs while updating the item.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, TodoItemUpdateDto todoItem)
    {
        await _todoItemService.UpdateAsync(id, todoItem);
        var response = new ApiResponse<string>("Todo item updated successfully.");
        return Ok(response);
    }

    /// <summary>
    /// Marks a to do item as complete.
    /// </summary>
    /// <param name="id">The identifier of the to do item to mark as complete.</param>
    /// <returns>Returns no content if the task is marked as complete successfully.</returns>
    /// <response code="204">If the task was marked as complete successfully.</response>
    /// <response code="500">If an error occurs while marking the task as complete.</response>
    [HttpPut("complete/{id}")]
    public async Task<IActionResult> PutMarkTaskAsCompleteAsync(int id)
    {
        await _todoItemService.MarkTaskAsComplete(id);
        var response = new ApiResponse<string>("Todo item marked as complete successfully.");
        return Ok(response);
    }

    /// <summary>
    /// Deletes a to do item.
    /// </summary>
    /// <param name="id">The identifier of the to do item to delete.</param>
    /// <returns>Returns success if the item is deleted.</returns>
    /// <response code="200">If the to do item was deleted successfully.</response>
    /// <response code="500">If an error occurs while deleting the item.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _todoItemService.DeleteAsync(id);
        var response = new ApiResponse<string>("Todo item deleted successfully.");
        return Ok(response);
    }
}
