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
                usersFromDb.Select( o => new UserDto(o.GUID,o.Name?? "N/A",o.SelectedNumber) )
                );
            return View(vm);
        }

        return View();
    }


    private async Task<bool> NumberIsAvalible(int num)
    {
        var usersFromDb = await _storageService.GetAllAsync();

        if (usersFromDb.Any())
        {
            return !usersFromDb.Any(o => o.SelectedNumber == num);




        }
        return true;
    }

    

    public async Task<ActionResult<string>> PostNewPick(string name, int selectedNumber)
    {
        User_DataBaseEntry newUser = new User_DataBaseEntry( Guid.NewGuid().ToString() )
        {
            Name = name,
            SelectedNumber = selectedNumber
        };

        if (! await NumberIsAvalible(newUser.SelectedNumber)) {

            return "Tallet er i bruk";
        }

        var createdEntity = _storageService.UpsertEntityAsync(newUser);

        if (createdEntity != null)
            return "Bruker reggistrert";
        else
            throw new Exception("Failed to save changes to db");
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

