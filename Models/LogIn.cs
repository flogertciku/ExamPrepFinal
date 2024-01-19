#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExamPrepFinal.Models;
namespace ExamPrepFinal.Models;
public class LogIn
{    

     
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }    
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } 
    
   

}
