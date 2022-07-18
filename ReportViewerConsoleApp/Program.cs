using DAL.Model;
using DAL.Services;

var conferenceService = new ConferenceService();

Console.WriteLine("Alailable commands:");
Console.WriteLine("\t showconf [all][number]");
Console.WriteLine("\t showconfusers [number]");
Console.WriteLine("\t showconfequip [number]");

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
        if (commands.Length != 2)
        {
            throw new ArgumentException();
        }
        switch (commands[0])
        {
            case "showconf": ShowConference(commands[1]); break;
            case "showconfusers": ShowUsersOfConference(commands[1]); break;
            case "showconfequip": ShowEquipment(commands[1]); break;
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
