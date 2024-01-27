#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using ExamPrepFinal.Models;
namespace ExamPrepFinal.Models;
public class Ide
{
    [Key]
    public int IdeId { get; set; }
    public string Name { get; set; } 
    public int Shuma { get; set; }
    public int? UserId {get;set;}
    public string? ImagePath { get; set; }
    public DateTime DeadLine {get;set;}
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public User? Creator {get;set;}
    public List<Kontribut>? kontributet {get;set;}
}
                
