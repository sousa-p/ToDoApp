using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models;

public class TodoModel {
  public int Id { set; get; }

  [Required]
  public string Title { set; get; }

  [Required]
  public string Description { set; get; }
  
  [DisplayName("Date to Done")]
  public DateTime DateToDone { set; get; }

  public bool Done { set; get; } = false;

  public string? User { set; get; }
}