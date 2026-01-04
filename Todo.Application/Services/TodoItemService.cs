namespace Todo.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUserRepository _userRepository;

    public TodoItemService(ITodoItemRepository todoItemRepository, IUserRepository userRepository)
    {
        _todoItemRepository = todoItemRepository;
        _userRepository = userRepository;
    }

    public async Task CreateAsync(TodoItemInsertDto todoItemDto)
    {
        var user = await _userRepository.GetAsync(new User() { UserID = todoItemDto.CreatedByID });

        if(user is null)
            throw new Exception("User not found.");

        var todoItem = new TodoItem
        {
            Name = todoItemDto.Name,
            IsDone = todoItemDto.IsDone ?? false,
            DeadLine = todoItemDto.DeadLine,
            CreatedByID = todoItemDto.CreatedByID,
            CreatedBy = user
        };

        await _todoItemRepository.CreateAsync(todoItem);
    }

    public async Task DeleteAsync(int id)
    {
        await _todoItemRepository.DeleteAsync(new TodoItem() { TodoItemID = id });
    }

    public async Task<TodoItemDto> GetAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetAsync(new TodoItem() { TodoItemID = id });

        if(todoItem is null)
            throw new Exception("Todo item not found.");

        // TODO: user mapping as Mapster or AutoMapper
        var todoItemDto = new TodoItemDto
        {
            TodoItemID = todoItem.TodoItemID,
            Name = todoItem.Name,
            IsDone = todoItem.IsDone,
            DeadLine = todoItem.DeadLine,
            CreatedByID = todoItem.CreatedByID,
            CreatedIn = todoItem.CreatedIn
        };

        return todoItemDto;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllByUserIdAsync(int userID)
    {
        var todoItems = await _todoItemRepository.GetAllAsync(new TodoItem() { CreatedBy = new User() { UserID = userID } });

        var todoItemsDto = todoItems.Select(todoItem => new TodoItemDto
        {
            TodoItemID = todoItem.TodoItemID,
            Name = todoItem.Name,
            IsDone = todoItem.IsDone,
            DeadLine = todoItem.DeadLine,
            CreatedByID = todoItem.CreatedByID,
            CreatedIn = todoItem.CreatedIn
        });

        return todoItemsDto;
    }

    public async Task UpdateAsync(int id, TodoItemUpdateDto entity)
    {
        var todo = await _todoItemRepository.GetAsync(new TodoItem() { TodoItemID = id });

        if(todo is null)
            throw new Exception("Todo item not found.");

        todo.Name = entity.Name;
        todo.IsDone = entity.IsDone;
        todo.DeadLine = entity.DeadLine;

        await _todoItemRepository.UpdateAsync(todo);
    }
}
