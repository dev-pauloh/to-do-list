using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Extentions;
using Todo.Models;
using Todo.ViewModels;
using Todo.ViewModels.Todos;

namespace Todo.Controllers
{
    [Authorize]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpGet("v1/todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices] TodoDataContext context)
        {
            try
            {
                var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

                var todos = await context
                    .Todos
                    .Where(x => x.UserId == user.Id)
                    .ToListAsync();
                return Ok(new ResultViewModel<List<TodoModel>>(todos));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<TodoModel>>("Falha interna do servidor"));
            }
        }

        [HttpGet("v1/todos/done/{done:bool}")]
        public async Task<IActionResult> GetDoneAsync(
            [FromServices] TodoDataContext context,
            [FromRoute] bool done)
        {
            try
            {
                var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

                var todos = await context
                    .Todos
                    .Where(x => x.UserId == user.Id)
                    .Where(x => x.Done == done)
                    .ToListAsync();
                return Ok(new ResultViewModel<List<TodoModel>>(todos));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<TodoModel>>("Falha interna do servidor"));
            }
        }



        [HttpPost("v1/todos")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorTodoViewModel model,
            [FromServices] TodoDataContext context)
        {
            try
            {
                if (!ModelState.IsValid) //Com a validação ModelState automático desabilitado, essa linha é obrigatória
                    return BadRequest(new ResultViewModel<TodoModel>(ModelState.GetErrors()));
                var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

                var todo = new TodoModel
                {
                    Id = 0,
                    Title = model.Title,
                    Done = model.Done,
                    UserId = user.Id
                };
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();

                return Created($"v1/todos/{todo.Id}", new ResultViewModel<TodoModel>(todo));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<TodoModel>("Não foi possível incluir a tarefa"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<TodoModel>("Falha interna do servidor"));
            }
        }


        [HttpPut("v1/todos/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorTodoViewModel model,
            [FromServices] TodoDataContext context)
        {
            try
            {
                var todo = await context
                    .Todos
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (todo == null)
                    return NotFound(new ResultViewModel<TodoModel>("Conteúdo não encontrado"));

                todo.Title = model.Title;
                todo.Done = model.Done;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<TodoModel>(todo));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<TodoModel>("Não foi possível alterar a tarefa"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<TodoModel>("Falha interna do servidor"));
            }


        }

        [HttpDelete("v1/todos/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
           [FromRoute] int id,
           [FromServices] TodoDataContext context)
        {
            try
            {
                var todo = await context
                    .Todos
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (todo == null)
                    return NotFound(new ResultViewModel<TodoModel>("Conteúdo não encontrado"));

                context.Todos.Remove(todo);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<TodoModel>(todo));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<TodoModel>("Não foi possível excluir a tarefa"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<TodoModel>("Falha interna do servidor"));
            }
        }
    }
}