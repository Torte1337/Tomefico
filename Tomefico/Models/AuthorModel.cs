using System;
using System.ComponentModel.DataAnnotations;

namespace Tomefico.Models;

public class AuthorModel
{
    [Key] public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
}
