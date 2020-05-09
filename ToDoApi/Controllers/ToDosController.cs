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

    /* return all To Do(es) */
    //GET: api/todos
    [HttpGet]
    public ActionResult<IEnumerable<ToDo>> GetToDos()
    {
      return _todoRepo.Get();
    }

    /* return a To Do by Id */
    //GET: api/todos/n
    [HttpGet( "{id}" )]
    public ActionResult<ToDo> GetToDoById( int id )
    {
      var toDoItem = _todoRepo.Get( id );

      if( toDoItem == null )
        return NotFound();

      return toDoItem;
    }

    /* add a To Do */
    //POST: api/todos
    [HttpPost]
    public ActionResult<ToDo> AddToDoItem( ToDo todo )
    {
      var todoR = _todoRepo.Insert( todo );

      return CreatedAtAction( "GetToDoById", new ToDo { Id = todoR.Id }, todoR );
    }

    /* update a To Do by Id */
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

    /* delete a To Do by Id */
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

    /* get all To Do(es) in range of date */
    //GET: api/todos/getbydate/daterangeenum
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

    /* update a To Do completeness percentage by Id */
    //PUT: api/todos/complete/n/value/n
    [Route( "[action]/{id}/value/{percentage}" )]
    [HttpPut]
    public ActionResult<ToDo> Complete(int id, int percentage)
    {
      var todoItem = _todoRepo.Get( id );

      if( todoItem == null )
        return NotFound();

      _todoRepo.Update( id, percentage );

      return _todoRepo.Get( id );
    }

    /* update a To Do completeness percentage to 100 by Id */
    //PUT: api/todos/complete/n/
    [Route( "[action]/{id}" )]
    [HttpPut]
    public ActionResult<ToDo> Complete( int id )
    {
      var todoItem = _todoRepo.Get( id );

      if( todoItem == null )
        return NotFound();

      // progress DONE means 100 completeness
      _todoRepo.Update( id, 100 );

      return _todoRepo.Get( id );
    }

  }
}
