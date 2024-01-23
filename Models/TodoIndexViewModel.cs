using System.Collections.Generic;

namespace ToDoApp.Models;

public class TodoIndexViewModel
{
  public IEnumerable<TodoModel> NotDone { get; set; }
  public IEnumerable<TodoModel> Done { get; set; }

  public TodoIndexViewModel(IEnumerable<TodoModel> notDone, IEnumerable<TodoModel>  done) {
    NotDone = notDone;
    Done = done;
  }
}
