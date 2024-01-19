#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using ExamPrepFinal.Models;
namespace ExamPrepFinal.Models;
public class Kontribut
{
    [Key]
    public int KontributId { get; set; }
    public int? UserId {get;set;}
    public int? IdeId {get;set;}
    public int Shuma { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public User? User {get;set;}
    public Ide? Ide {get;set;}
}
                
