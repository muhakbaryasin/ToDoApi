using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace ToDoApi.Models
{
  public class ToDosRepo
  {
    private readonly ToDoDbContext _context;

    // constructur: passing ToDoDbContenxt
    public ToDosRepo( ToDoDbContext context )
    {
      _context = context;
    }

    // Get all ToDo entries
    public DbSet<ToDo> Get()
    {
      return _context.ToDoItems;
    }

    // Get ToDo by id
    public ToDo Get( int id )
    {
      return _context.ToDoItems.Find( id );
    }

    // Get all ToDo entries of a certain date
    public IEnumerable<ToDo> Get( DateTime date )
    {
      return Get().Where( a => a.CreatedDate.Date == date.Date );
    }

    // Get all ToDo entries of a certain date range
    public IEnumerable<ToDo> Get( DateTime dateStart, DateTime dateEnd )
    {
      return Get().Where( a => a.CreatedDate >= dateStart.Date && a.CreatedDate <= dateEnd.Date );
    }

    // Insert new entry of ToDo
    public ToDo Insert( ToDo todo )
    {
      _context.ToDoItems.Add( todo );
      _context.SaveChanges();

      return todo;
    }

    // Update data of a ToDo 
    public void Update( ToDo todo )
    {
      _context.Entry( todo ).State = EntityState.Modified;
      _context.SaveChanges();
    }

    // Update data completeness percentage of a ToDo by id
    public void Update( int id, int completenessPercentage )
    {
      var todo = Get( id );

      todo.CompletenessPercentage = completenessPercentage;
      _context.Entry( todo ).State = EntityState.Modified;
      _context.SaveChanges();
    }

    // Delete a ToDo entry
    public void Delete( ToDo todo )
    {
      _context.ToDoItems.Remove( todo );
      _context.SaveChanges();
    }
  }
}
