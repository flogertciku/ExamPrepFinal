﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExamPrepFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepFinal.Controllers;
// Name this anything you want with the word "Attribute" at the end
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Auth", "Home", null);
        }
    }
}



public class HomeController : Controller
{
    private MyContext _context; 
     private readonly IEmailSender _emailSender;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,MyContext context,IEmailSender emailSender)
    {
        _logger = logger;
         _context = context;  
          _emailSender = emailSender;
    }
    [SessionCheck]
    public IActionResult Index()
    {
        ViewBag.idete = _context.Idete.Include(e=>e.kontributet).Include(e=>e.Creator).Where(e=> e.DeadLine > DateTime.Now).ToList();
        ViewBag.userId = HttpContext.Session.GetInt32("UserId");
        
        return View();
    }
    [HttpGet("Auth")]
    public IActionResult Auth(){
        return View("Auth");
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(User forma){
        if (ModelState.IsValid)
        {
             PasswordHasher<User> Hasher = new PasswordHasher<User>();   
            // Updating our newUser's password to a hashed version         
            forma.Password = Hasher.HashPassword(forma, forma.Password);  
                    // generate a new code 
                    // forma.validateEmail = newCode
            //Save your user object to the database 
            // await _emailSender.SendEmailAsync(forma.Email, "VerifyEmail", $"Your Email {forma.Email} is used, and verify it using this coe : passcode.");
            _context.Add(forma);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Auth");
    }


     [HttpPost("LogIn")]
    public IActionResult LogIn(LogIn forma){
        if (ModelState.IsValid)
        {
              User? userInDb = _context.Users.FirstOrDefault(u => u.Email == forma.Email);  
            // Updating our newUser's password to a hashed version         
            if(userInDb == null)        
        {            
            // Add an error to ModelState and return to View!            
            ModelState.AddModelError("Email", "Invalid Email");            
            return View("Auth");        
        }      
         PasswordHasher<LogIn> hasher = new PasswordHasher<LogIn>();                    
        // Verify provided password against hash stored in db        
        var result = hasher.VerifyHashedPassword(forma, userInDb.Password, forma.Password); 
            if(result == 0)        
        {    
             ModelState.AddModelError("Passwordi", "Invalid Passwordi");            
            return View("Auth");        
            // Handle failure (this should be similar to how "existing email" is handled)        
        }
            //Save your user object to the database 
           HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("Index");
        }
        return View("Auth");
    }
    [SessionCheck]
    [HttpGet("Ide")]
    public IActionResult Ide(){
        return View();
    }
     [SessionCheck]
    [HttpPost("AddIde")]
    public async Task<IActionResult> AddIde(IFormFile imageFile,Ide form){
        if (ModelState.IsValid)
        {
          if (imageFile != null && imageFile.Length > 0)
{
            var fileName = Path.GetFileName(imageFile.FileName);
            // Relative path inside the wwwroot folder
            var relativePath = Path.Combine("images", fileName);

            // Absolute path to where the file will be saved on the server
            var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            using (var fileStream = new FileStream(absolutePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            // Set the relative path to be stored in the database
            form.ImagePath = relativePath; 
        }
            form.UserId = HttpContext.Session.GetInt32("UserId");
            User loggedUser = _context.Users.FirstOrDefault(e=>e.UserId==HttpContext.Session.GetInt32("UserId") );
             // Assuming the user's email is stored in User.Identity
            await _emailSender.SendEmailAsync(loggedUser.Email, "New Ide Created", $"Your Ide {form.Name} has been created.");

            _context.Add(form);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Ide");

    }
    [SessionCheck]
    [HttpGet("Delete/{itemId}")]
    public IActionResult Delete(int itemId){
        Ide ideja = _context.Idete.FirstOrDefault(e=>e.IdeId==itemId);
        List<Kontribut> kontributet = _context.Kontributet.Where(e=> e.IdeId == itemId).ToList();
        _context.RemoveRange(kontributet);
        _context.Remove(ideja);
        _context.SaveChanges();
        return View();
    }
    [HttpGet("DetailsIde/{itemId}")]
    public IActionResult DetailsIde(int itemId){
        Ide ideja = _context.Idete.Include(e=>e.kontributet).ThenInclude(e=>e.User).Include(e=>e.Creator).FirstOrDefault(e=>e.IdeId==itemId);
       
        return View(ideja);
    }

    // TODO
    [HttpPost("AddMoney/{itemId}")]
    // public IActionResult AddMoney(int vlera,int itemId){
    //     Ide ideja = _context.Idete.FirstOrDefault(e=>e.IdeId==itemId);


    // }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
