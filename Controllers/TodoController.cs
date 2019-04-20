using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TodoController:ControllerBase
    {
        private readonly TodoContext  con;

        public TodoController(TodoContext context){
            con=context;
            if(con.TodoItems.Count()==0)
            {
                con.TodoItems.Add(new TodoItem{Name= "ok! Stenio lista vazia"});
                con.SaveChanges();
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>>GetTodoItems()
        {
            return await con.TodoItems.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem= await con.TodoItems.FindAsync(id);
            if(todoItem==null)
            {
                return NotFound();
            }
            return todoItem;
        }
        [HttpPost]
        public async Task<ActionResult<TodoItem>>PostTodoItem(TodoItem item)
        {
            con.TodoItems.Add(item);
            await con.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem),new {id= item.Id},item);


        }
        [HttpPut("{id}")]
        public async Task<IActionResult>PutTodoItem(long id,TodoItem item)
        {
            if(id!=item.Id)
            {
                return BadRequest();
            }
            con.Entry(item).State=EntityState.Modified;
            await con.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteTodoItem(long id)
        {
            var todoItem =await con.TodoItems.FindAsync(id);
            if(todoItem==null)
            {
                return NotFound();
            }
            con.TodoItems.Remove(todoItem);
            await con.SaveChangesAsync();
            return NoContent();
        }





    }






}