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


    /// <summary>
    /// Checks if a number is in use,
    /// </summary>
    /// <param name="num">Number to check </param>
    /// <returns> bool: true if number is not in use</returns>
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
        if (name is null || name.Length < 1)
        {
            return "Navn mangler";
        }


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





    public async Task<IActionResult> TrekkVinner()
    {

        var usersFromDb = await _storageService.GetAllAsync();
        var v = View();
        v.ViewData["Count"] = usersFromDb.Count();

        return v;
    }


    public async Task<ActionResult<string>> PickWinners()
    {
        var usersFromDb = await _storageService.GetAllAsync();
        return null;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

