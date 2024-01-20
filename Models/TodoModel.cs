namespace ToDoApp.Models;

public class TodoModel {
  public int Id { set; get; }
  public string Description { set; get; }
  public DateTime Date { set; get; }

  public TodoModel(int id, string description, DateTime date) {
    Id = id;
    Description = description;
    Date = date;
  }
}