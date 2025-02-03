using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DBFirst.Models;

public partial class Book
{
    public int N { get; set; }
    [DisplayName("Код")]
    public double? Code { get; set; }
    [DisplayName("Новий?")]
    public bool New { get; set; }
    [DisplayName("Назва")]
    public string? Name { get; set; }
    [DisplayName("Ціна")]
    public decimal? Price { get; set; }
    [DisplayName("Видавництво")]
    public string? Izd { get; set; }
    [DisplayName("Сторінки")]
    public double? Pages { get; set; }
    [DisplayName("Формат")]
    public string? Format { get; set; }
    [DisplayName("Дата видання")]
    public DateTime? Date { get; set; }
    [DisplayName("Перед-запуск")]
    public double? Pressrun { get; set; }
    [DisplayName("Тема")]
    public string? Themes { get; set; }
    [DisplayName("Категорія")]
    public string? Category { get; set; }
}
