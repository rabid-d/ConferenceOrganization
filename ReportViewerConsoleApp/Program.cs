using DAL.Model;
using DAL.Services;
using static System.Linq.Enumerable;

var conferenceService = new ConferenceService();
var userService = new UserService();

Task savePhotoTask = null;
var tokenSource = new CancellationTokenSource();
CancellationToken ct = tokenSource.Token;
string newPathToPhoto = "";
Guid newUserId;

Console.WriteLine("Alailable commands:");
Console.WriteLine("\t showconf [all][number]");
Console.WriteLine("\t showconfusers [number]");
Console.WriteLine("\t showconfequip [number]");
Console.WriteLine("\t adduser");
Console.WriteLine("\t exit");

while (true)
{
    try
    {
        Console.Write("Enter your command: ");
        var line = Console.ReadLine();
        if (line == "")
        {
            throw new ArgumentNullException();
        }
        var commands = line.Split(' ');
        var twoArgumentCommands = new List<string>() { "showconf", "showconfusers", "showconfequip" };
        if (twoArgumentCommands.Contains(commands[0]) & commands.Length != 2)
        {
            throw new ArgumentException();
        }
        switch (commands[0])
        {
            case "showconf": ShowConference(commands[1]); break;
            case "showconfusers": ShowUsersOfConference(commands[1]); break;
            case "showconfequip": ShowEquipment(commands[1]); break;
            case "adduser": AddUser(); break;
            case "exit": Exit(); break;
            default : throw new ArgumentException();
        }
    }
    catch(ArgumentNullException)
    {
        Console.WriteLine("You didn't enter anything");
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Wrong command or arguments");
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("Nothing found");
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("Image not found");
    }
    catch (Exception)
    {
        Console.WriteLine("Something went wrong");
    }
}

void ShowConference(string confArgument)
{
    if (confArgument == "all")
    {
        var confs = conferenceService.GetAllConferences();
        foreach (var conference in confs)
        {
            PrintConfToConsole(conference);
        }
        return;
    }
    var conf = conferenceService.GetConferenceByNumber(confArgument);
    if (conf == null)
    {
        throw new InvalidOperationException();
    } else
    {
        PrintConfToConsole(conf);
    }
}

void PrintConfToConsole(Conference conf)
{
    Console.WriteLine($"{conf.Name} {conf.DateStart} - {conf.DateEnd}");
    foreach (var section in conf.Sections)
    {
        Console.WriteLine("\t ----------");
        Console.WriteLine($"\t Section: {section.Name}");
        Console.WriteLine($"\t Chairperson: {section.Chairperson.FullName}");
        Console.WriteLine($"\t Room: {section.Room}");
        foreach (var talk in section.Talks)
        {
            Console.Write($"\t\t — Talk: {talk.Theme};");
            Console.Write($" {talk.Speaker.FullName};");
            Console.WriteLine($" {talk.DateStart.ToShortTimeString()} - {talk.DateEnd.ToShortTimeString()}");
        }
    }
}

void ShowUsersOfConference(string confArgument)
{
    var conf = conferenceService.GetConferenceByNumber(confArgument);
    if (conf == null)
    {
        throw new InvalidOperationException();
    }
    else
    {
        Console.WriteLine($"Persons in conference: «{conf.Name}»:");
        Console.WriteLine("Chairpersons:");
        foreach (var section in conf.Sections)
        {
            Console.WriteLine($"\t{section.Chairperson.FullName}; {section.Chairperson.Work}; {section.Chairperson.Position}");
        }
        Console.WriteLine("Speakers:");
        foreach (var section in conf.Sections)
        {
            foreach (var talk in section.Talks)
            {
                Console.WriteLine($"\t{talk.Speaker.FullName}; {talk.Speaker.Work}; {talk.Speaker.Position}");
            }
        }
    }
}

void ShowEquipment(string confArgument)
{
    var conf = conferenceService.GetConferenceByNumber(confArgument);
    if (conf == null)
    {
        throw new InvalidOperationException();
    }
    else
    {
        var equipment = new HashSet<Equipment>();
        Console.WriteLine($"Needed equipment for conference: «{conf.Name}»:");
        foreach (var section in conf.Sections)
        {
            foreach (var talk in section.Talks)
            {
                foreach (var equip in talk.Equipment)
                {
                    equipment.Add(equip);
                }
            }
        }
        foreach (var equip in equipment)
        {
            Console.WriteLine(equip.Name);
            foreach (var section in conf.Sections)
            {
                foreach (var talk in section.Talks)
                {
                    if (talk.Equipment.Contains(equip))
                    {
                        Console.WriteLine($"\t Room: {section.Room} Time: {talk.DateStart.ToShortTimeString()} - {talk.DateEnd.ToShortTimeString()}");
                    }
                }
            }
        }
    }
}

void AddUser()
{
    Console.Write("Enter new user full name: ");
    var fullName = Console.ReadLine();
    Console.Write("Enter degree: ");
    var degree = Console.ReadLine();
    Console.Write("Enter work place: ");
    var work = Console.ReadLine();
    Console.Write("Enter work position: ");
    var position = Console.ReadLine();
    Console.Write("Enter professional biography: ");
    var biography = Console.ReadLine();
    Console.Write("Enter path to photo: ");
    var pathToPhoto = Console.ReadLine();

    newUserId = Guid.NewGuid();
    newPathToPhoto = GetNewUserPathToPhoto(fullName, newUserId.ToString(), pathToPhoto);
    SavePhoto(pathToPhoto, newPathToPhoto);

    userService.AddUser(newUserId, fullName, degree, work, position, biography);
    if (userService.IsUserExists(newUserId.ToString()))
    {
        Console.WriteLine("User successfully added.");
    }
    else
    {
        throw new Exception();
    }
}

void SavePhoto(string pathToPhoto, string newPathToPhoto)
{
    savePhotoTask = Task.Run(() =>
    {
        if (!File.Exists(pathToPhoto))
        {
            throw new FileNotFoundException();
        }
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos"));
        File.Copy(pathToPhoto, newPathToPhoto);
        foreach (var _ in Range(0, 20)) // Simulation of long running task.
        {
            Thread.Sleep(1000);
            if (ct.IsCancellationRequested)
            {
                if (File.Exists(newPathToPhoto))
                {
                    File.Delete(newPathToPhoto);
                }
                break;
            }
        }
        if (!ct.IsCancellationRequested)
        {
            Console.WriteLine("Photo successfully uploaded.");
            userService.UpdateUserPhoto(newUserId.ToString(), newPathToPhoto);
        }
    }, tokenSource.Token);
}

string GetNewUserPathToPhoto(string fullName, string id, string pathToPhoto)
{
    var extension = Path.GetExtension(pathToPhoto);
    var newPhotoName = fullName.Trim().ToLower().Replace(' ', '_') + "_" + id;
    string[] paths = { AppDomain.CurrentDomain.BaseDirectory, "photos", newPhotoName};
    var newPath = Path.Combine(paths);
    newPath = Path.ChangeExtension(newPath, extension);
    return newPath;
}

void Exit()
{
    if (savePhotoTask?.Status == TaskStatus.Running)
    {
        Console.Write("The photo is currently uploading to the server. Exit anyway? y/n: ");
        var answer = Console.ReadLine();
        if (answer.ToLower() == "y")
        {
            tokenSource.Cancel();
            savePhotoTask.Wait();
            Environment.Exit(0);
        }
    } else
    {
        Environment.Exit(0);
    }
}
