#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExamPrepFinal.Models;
namespace ExamPrepFinal.Models;
public class User
{    
    [Key]    
    public int UserId { get; set; }
    
    [Required]    
    public string FirstName { get; set; }
    
    [Required]        
    public string LastName { get; set; }     
    
    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }    
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } 
    
   
    public DateTime CreatedAt {get;set;} = DateTime.Now;   
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
     [NotMapped]
    // There is also a built-in attribute for comparing two fields we can use!
    [Compare("Password")]
    public string PasswordConfirm { get; set; }
    public List<Ide>? idete {get;set;}
    public List<Kontribut>? kontributet {get;set;}
}
