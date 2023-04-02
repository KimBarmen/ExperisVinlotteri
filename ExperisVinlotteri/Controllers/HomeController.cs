using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExperisVinlotteri.Models;
using ExperisVinlotteri.Services;

namespace ExperisVinlotteri.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITableStorageService _storageService;

    public HomeController(ILogger<HomeController> logger, ITableStorageService tableStorageService)
    {
        _logger = logger;
        _storageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService)); ;
    }

    public async Task<IActionResult> Index()
    {

        var usersFromDb = await _storageService.GetAllAsync();

        if (usersFromDb.Any())
        {
            var vm = new ListOfUsers(
                usersFromDb.Select( o => new UserDto(o.GUID,o.Name?? "N/A",o.SelectedNumber.GetValueOrDefault()) )
                );
            return View(vm);
        }


       


        return View();
    }


    public ActionResult<bool> PostNewPick(string name, int selectedNumber)
    {
        User_DataBaseEntry newUser = new User_DataBaseEntry( Guid.NewGuid().ToString() )
        {
            Name = name,
            SelectedNumber = selectedNumber
        };

        var createdEntity = _storageService.UpsertEntityAsync(newUser);

        return createdEntity != null; 
    }

    public IActionResult TrekkVinner()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

