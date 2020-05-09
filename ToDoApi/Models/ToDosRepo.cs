using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace ToDoApi.Models
{
  public class ToDosRepo
  {
    private readonly ToDoDbContext _context;

    public ToDosRepo( ToDoDbContext context )
    {
      _context = context;
    }

    public DbSet<ToDo> Get()
    {
      return _context.ToDoItems;
    }

    public ToDo Get( int id )
    {
      return _context.ToDoItems.Find( id );
    }

    public IEnumerable<ToDo> Get( DateTime date )
    {
      return Get().Where( a => a.CreatedDate.Date == date.Date );
    }

    public IEnumerable<ToDo> Get( DateTime dateStart, DateTime dateEnd )
    {
      return Get().Where( a => a.CreatedDate >= dateStart.Date && a.CreatedDate <= dateEnd.Date );
    }

    public ToDo Insert( ToDo todo )
    {
      _context.ToDoItems.Add( todo );
      _context.SaveChanges();

      return todo;
    }

    public void Update( ToDo todo )
    {
      _context.Entry( todo ).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void Delete( ToDo todo )
    {
      _context.ToDoItems.Remove( todo );
      _context.SaveChanges();
    }

    public void Update( int id, int completenessPercentage )
    {
      var todo = Get( id );

      todo.CompletenessPercentage = completenessPercentage;
      _context.Entry( todo ).State = EntityState.Modified;
      _context.SaveChanges();
    }
  }
}
