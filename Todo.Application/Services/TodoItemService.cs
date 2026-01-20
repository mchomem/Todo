namespace Todo.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TodoItemService(ITodoItemRepository todoItemRepository, IUserRepository userRepository, IMapper mapper)
    {
        _todoItemRepository = todoItemRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(TodoItemInsertDto todoItemDto)
    {
        var user = await _userRepository.GetAsync(todoItemDto.CreatedByID);

        if (user is null)
            throw new UserNotFoundException();

        var todoItem = new TodoItem(todoItemDto.Name, todoItemDto.DeadLine, todoItemDto.CreatedByID, user);
        await _todoItemRepository.CreateAsync(todoItem);
    }

    public async Task DeleteAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetAsync(id);

        if(todoItem is null)
            throw new TodoItemNotFoundException();

        await _todoItemRepository.DeleteAsync(todoItem);
    }

    public async Task<TodoItemDto> GetAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetAsync(id);

        if(todoItem is null)
            throw new TodoItemNotFoundException();

        var todoItemDto = _mapper.Map<TodoItemDto>(todoItem);
        return todoItemDto;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllByUserIdAsync(int userID)
    {
        Expression<Func<TodoItem, bool>> expressionFilter = x => x.CreatedByID == userID;

        var todoItems = await _todoItemRepository.GetAllAsync(expressionFilter);
        var todoItemsDto = _mapper.Map<IEnumerable<TodoItemDto>>(todoItems);
        return todoItemsDto;
    }

    public async Task UpdateAsync(int id, TodoItemUpdateDto entity)
    {
        var todo = await _todoItemRepository.GetAsync(id);

        if(todo is null)
            throw new TodoItemNotFoundException();

        todo.Update(entity.Name, entity.IsDone, entity.DeadLine);

        await _todoItemRepository.UpdateAsync(todo);
    }

    public async Task MarkTaskAsComplete(int id)
    {
        var todo = await _todoItemRepository.GetAsync(id);
        
        if(todo is null)
            throw new TodoItemNotFoundException();

        todo.MaskAsDone();
        await _todoItemRepository.UpdateAsync(todo);
    }
}
