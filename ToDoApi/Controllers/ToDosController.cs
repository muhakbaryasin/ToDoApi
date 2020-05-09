using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using Microsoft.EntityFrameworkCore;


namespace ToDoApi.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class ToDosController : ControllerBase
  {
    private readonly ToDoDbContext _context;

    public ToDosController( ToDoDbContext context )
    {
      _context = context;
    }

    //GET: api/todos
    [HttpGet]
    public ActionResult<IEnumerable<ToDo>> GetToDos()
    {
      return _context.ToDoItems;
    }

    //GET: api/todos/n
    [HttpGet( "{id}" )]
    public ActionResult<ToDo> GetToDo( int id )
    {
      var commandItem = _context.ToDoItems.Find( id );

      if( commandItem == null )
        return NotFound();

      return commandItem;
    }

    //POST: api/todos
    [HttpPost]
    public ActionResult<ToDo> AddToDoItem( ToDo todo )
    {
      _context.ToDoItems.Add( todo );
      _context.SaveChanges();

      return CreatedAtAction( "GetToDo", new ToDo { Id = todo.Id }, todo );
    }

    //PUT: api/commands/n
    [HttpPut( "{id}" )]
    public ActionResult PutToDoItem( int id, ToDo todo )
    {
      if( id != todo.Id )
        return BadRequest();

      _context.Entry( todo ).State = EntityState.Modified;
      _context.SaveChanges();

      return NoContent();
    }

    [HttpDelete( "{id}" )]
    public ActionResult<ToDo> DeleteCommandItem( int id )
    {
      var todoItem = _context.ToDoItems.Find( id );

      if( todoItem == null )
        return NotFound();

      _context.ToDoItems.Remove( todoItem );
      _context.SaveChanges();

      return todoItem;
    }
  }
}
