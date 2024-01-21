namespace ToDoApp.Models;

public class TodoModel {
  public int Id { set; get; }
  public string Description { set; get; }
  public DateTime Date { set; get; }
  public bool Finished { set; get; }

  public TodoModel() {
  }
}