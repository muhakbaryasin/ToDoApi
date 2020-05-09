﻿using System;

namespace ToDoApi.Models
{
  public class ToDo
  {
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int CompletenessPercentage { get; set; }
  }
}
