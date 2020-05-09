using System;
using NUnit.Framework;
using ToDoApi.Controllers;
using ToDoApi.Models;
using System.Linq;

namespace NunitTest
{
  [TestFixture]
  public class ToDoTest
  {
    private readonly ToDoDbContext _context;
    private readonly ToDosRepo _todoRepo;

    public ToDoTest( ToDoDbContext context )
    {
      //_context = context;
      _todoRepo = new ToDosRepo( context );
    }

    public ToDo AddEntry( DateTime date )
    {
      var controller = new ToDosController( _context );
      var todoItem = new ToDo() { CreatedDate = date, ExpiredDate = date.AddDays( 1 ), Title = "Entry test", Description = "Entry test description", CompletenessPercentage = 0 };

      return controller.AddToDoItem( todoItem ).Value;
    }

    [TestCase]
    public void Test_Add_Entry()
    {
      var controller = new ToDosController( _context );

      var todo = new ToDo() { CreatedDate = DateTime.Now, ExpiredDate = DateTime.Now.AddDays( 1 ), Title = "Entry test", Description = "Entry test description", CompletenessPercentage = 0 };

      var result = controller.AddToDoItem( todo );

      Assert.Equals( todo.Title, result.Value.Title );

      _todoRepo.Delete( result.Value );
    }

    [TestCase]
    public void Test_Get( ToDoDbContext context )
    {
      var entry = AddEntry( DateTime.Now );
      var controller = new ToDosController( _context );
      var result = controller.GetToDos();

      Assert.Equals( result.Value.Count(), 1 );

      _todoRepo.Delete( entry );
    }

    [TestCase]
    public void Test_Update( ToDoDbContext context )
    {
      var entry = AddEntry( DateTime.Now );
      var controller = new ToDosController( _context );

      var newEntry = new ToDo() { CreatedDate = entry.CreatedDate, ExpiredDate = entry.ExpiredDate, Title = "Entry update test", Description = "Entry update test description", CompletenessPercentage = entry.CompletenessPercentage };

      controller.PutToDoItem( entry.Id, newEntry );
      var result = controller.GetToDoById( entry.Id );
      Assert.Equals( result.Value.Title, newEntry.Title );

      _todoRepo.Delete( entry );
    }

    [TestCase]
    public void Test_Delete( ToDoDbContext context )
    {
      var entry = AddEntry( DateTime.Now );
      var controller = new ToDosController( _context );

      var result = controller.DeleteToDoItem( entry.Id );

      Assert.Equals( result.Value.Title, entry.Title );
    }

    [TestCase]
    public void Test_GetByDateToday( ToDoDbContext context )
    {
      var entry1 = AddEntry( DateTime.Now );
      var entry2 = AddEntry( DateTime.Now );
      var entry3 = AddEntry( DateTime.Now );

      var controller = new ToDosController( _context );
      var result = controller.GetByDate( DateRangeEnum.TODAY.ToString() );

      Assert.Equals( result.Value.Count(), 3 );

      _todoRepo.Delete( entry1 );
      _todoRepo.Delete( entry2 );
      _todoRepo.Delete( entry3 );
    }

    [TestCase]
    public void Test_GetByDateTomorrow( ToDoDbContext context )
    {
      var entry1 = AddEntry( DateTime.Now.AddDays( 1 ) );
      var entry2 = AddEntry( DateTime.Now.AddDays( 1 ) );
      var entry3 = AddEntry( DateTime.Now.AddDays( 1 ) );

      var controller = new ToDosController( _context );
      var result = controller.GetByDate( DateRangeEnum.TOMORROW.ToString() );

      Assert.Equals( result.Value.Count(), 3 );

      _todoRepo.Delete( entry1 );
      _todoRepo.Delete( entry2 );
      _todoRepo.Delete( entry3 );
    }

    [TestCase]
    public void Test_GetByDateThisWeek( ToDoDbContext context )
    {
      var sunday = DateTime.Today.AddDays( -(int)DateTime.Today.DayOfWeek );
      var saturday = DateTime.Today.AddDays( -(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday );

      var entry1 = AddEntry( sunday );
      var entry2 = AddEntry( sunday.AddDays( 2 ) );
      var entry3 = AddEntry( saturday );

      var controller = new ToDosController( _context );
      var result = controller.GetByDate( DateRangeEnum.THISWEEK.ToString() );

      Assert.Equals( result.Value.Count(), 3 );

      _todoRepo.Delete( entry1 );
      _todoRepo.Delete( entry2 );
      _todoRepo.Delete( entry3 );
    }
  }
}
