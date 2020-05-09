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
    private readonly ToDosRepo _todoRepo;
    private readonly ToDosController _todoController;

    public ToDoTest( ToDoDbContext context )
    {
      _todoRepo = new ToDosRepo( context );
      _todoController = new ToDosController( context );
    }

    public ToDo AddEntry( DateTime date )
    {
      var todoItem = new ToDo() { CreatedDate = date, ExpiredDate = date.AddDays( 1 ), Title = "Entry test", Description = "Entry test description", CompletenessPercentage = 0 };

      return _todoController.AddToDoItem( todoItem ).Value;
    }

    [TestCase]
    public void Test_Add_Entry()
    {
      var todo = new ToDo() { CreatedDate = DateTime.Now, ExpiredDate = DateTime.Now.AddDays( 1 ), Title = "Entry test", Description = "Entry test description", CompletenessPercentage = 0 };

      var result = _todoController.AddToDoItem( todo );

      Assert.Equals( todo.Title, result.Value.Title );

      _todoRepo.Delete( result.Value );
    }

    [TestCase]
    public void Test_Get( ToDoDbContext context )
    {
      var entry = AddEntry( DateTime.Now );
      var result = _todoController.GetToDos();

      Assert.Equals( result.Value.Count(), 1 );

      _todoRepo.Delete( entry );
    }

    [TestCase]
    public void Test_Update( ToDoDbContext context )
    {
      var entry = AddEntry( DateTime.Now );
      var newEntry = new ToDo() { CreatedDate = entry.CreatedDate, ExpiredDate = entry.ExpiredDate, Title = "Entry update test", Description = "Entry update test description", CompletenessPercentage = entry.CompletenessPercentage };

      _todoController.PutToDoItem( entry.Id, newEntry );
      var result = _todoController.GetToDoById( entry.Id );

      Assert.Equals( result.Value.Title, newEntry.Title );

      _todoRepo.Delete( entry );
    }

    [TestCase]
    public void Test_Delete( ToDoDbContext context )
    {
      var entry = AddEntry( DateTime.Now );
      var result = _todoController.DeleteToDoItem( entry.Id );

      Assert.Equals( result.Value.Title, entry.Title );
    }

    [TestCase]
    public void Test_GetByDateToday( ToDoDbContext context )
    {
      var entry1 = AddEntry( DateTime.Now );
      var entry2 = AddEntry( DateTime.Now );
      var entry3 = AddEntry( DateTime.Now );

      var result = _todoController.GetByDate( DateRangeEnum.TODAY.ToString() );

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

      var result = _todoController.GetByDate( DateRangeEnum.TOMORROW.ToString() );

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

      var result = _todoController.GetByDate( DateRangeEnum.THISWEEK.ToString() );

      Assert.Equals( result.Value.Count(), 3 );

      _todoRepo.Delete( entry1 );
      _todoRepo.Delete( entry2 );
      _todoRepo.Delete( entry3 );
    }

    [TestCase]
    public void Test_UpdateCompletenessPercentage( ToDoDbContext context )
    {
      var entry1 = AddEntry( DateTime.Now );
      var result = _todoController.Complete( entry1.Id, 50 );

      Assert.Equals( result.Value.CompletenessPercentage, 50 );

      _todoRepo.Delete( entry1 );
    }

    [TestCase]
    public void Test_UpdateCompleteDone( ToDoDbContext context )
    {
      var entry1 = AddEntry( DateTime.Now );
      var result = _todoController.Complete( entry1.Id );

      // done entry would has completenessPercentage 100
      Assert.Equals( result.Value.CompletenessPercentage, 100 );

      _todoRepo.Delete( entry1 );
    }
  }
}
