using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;


namespace ToDoApi.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class ToDosController : ControllerBase
  {
    private readonly ToDosRepo _todoRepo;

    public ToDosController( ToDoDbContext context )
    {
      _todoRepo = new ToDosRepo( context );
    }

    //GET: api/todos
    [HttpGet]
    public ActionResult<IEnumerable<ToDo>> GetToDos()
    {
      return _todoRepo.Get();
    }

    //GET: api/todos/n
    [HttpGet( "{id}" )]
    public ActionResult<ToDo> GetToDoById( int id )
    {
      var toDoItem = _todoRepo.Get( id );

      if( toDoItem == null )
        return NotFound();

      return toDoItem;
    }

    //POST: api/todos
    [HttpPost]
    public ActionResult<ToDo> AddToDoItem( ToDo todo )
    {
      var todoR = _todoRepo.Insert( todo );

      return CreatedAtAction( "GetToDoById", new ToDo { Id = todoR.Id }, todoR );
    }

    //PUT: api/todos/n
    [HttpPut( "{id}" )]
    public ActionResult PutToDoItem( int id, ToDo todo )
    {
      if( id != todo.Id )
        return BadRequest();

      var todoItem = _todoRepo.Get( id );

      if( todoItem == null )
        return NotFound();

      _todoRepo.Update( todo );

      return NoContent();
    }

    //DELETE: api/todos/n
    [HttpDelete( "{id}" )]
    public ActionResult<ToDo> DeleteToDoItem( int id )
    {
      var todoItem = _todoRepo.Get( id );

      if( todoItem == null )
        return NotFound();

      _todoRepo.Delete( todoItem );

      return todoItem;
    }

    //GET: api/todos/n
    [Route( "[action]/{dateRangeType}" )]
    [HttpGet]
    public ActionResult<IEnumerable<ToDo>> GetByDate( string dateRangeType )
    {
      IEnumerable<ToDo> toDoItems = null;

      if( !Enum.TryParse( dateRangeType.ToUpper(), out DateRangeEnum dateRangeEnum ) )
        return NotFound();

      if( dateRangeEnum == DateRangeEnum.TODAY )
      {
        toDoItems = _todoRepo.Get( DateTime.Now );
      }
      else if( dateRangeEnum == DateRangeEnum.TOMORROW )
      {
        toDoItems = _todoRepo.Get( DateTime.Now.AddDays( 1 ) );
      }
      else if( dateRangeEnum == DateRangeEnum.THISWEEK )
      {
        var sunday = DateTime.Today.AddDays( -(int)DateTime.Today.DayOfWeek );
        var saturday = DateTime.Today.AddDays( -(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday );

        toDoItems = _todoRepo.Get( sunday, saturday );
      }

      if( toDoItems == null || toDoItems.Count() < 1 )
        return NotFound();

      return toDoItems.ToList();
    }

  }
}
