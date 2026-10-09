namespace DelegateHomework;

public class Classroom
{
    public string Name { get; }
    public int Seats { get; }
    public bool HasProjector { get; }
    public bool IsLectureHall { get; }
    public bool IsOpenStudyRoom { get; }

    public Classroom(string name, int seats, bool hasProjector, bool isLectureHall, bool isOpenStudyRoom)
    {
        Name = name;
        Seats = seats;
        HasProjector = hasProjector;
        IsLectureHall = isLectureHall;
        IsOpenStudyRoom = isOpenStudyRoom;
    }

    public override string ToString()
    {
        return Name;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var list = CreateClassroomsList();

        Func<Classroom, bool> isSuitableForLectureFor100People =
            classroom => classroom.IsLectureHall
                         && classroom.HasProjector
                         && classroom.Seats >= 100;

        while (true)
        {
            Thread.Sleep(2000);
            Print("");
            Print("1 — Показать лекционные аудитории");
            Print("2 — Отсортировать аудитории по убыванию мест");
            Print("3 — Посчитать аудитории с проектором");
            Print("4 — Найти аудитории для лекции на 100 человек");
            Print("0 — Выход");
            Print("Выберите действие: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    PrintCollection(
                        FindAllClassrooms(list, classroom => classroom.IsLectureHall));
                    break;

                case "2":
                    PrintCollection(
                        Sort(list, classroom => classroom.Seats, true));
                    break;

                case "3":
                    Print(CountClassrooms(
                        list, classroom => classroom.HasProjector));
                    break;

                case "4":
                    Process(list, isSuitableForLectureFor100People, Print);
                    break;

                case "0":
                    return;

                default:
                    Print("Введите число от 0 до 4.");
                    break;
            }
        }
    }
    
    public static void Process<T>(List<T> items, Func<T, bool> condition, Action<T> action)
    {
        foreach (var item in items)
        {
            if (condition(item))
            {
                action(item);
            }
        }
    }
    
    public static List<Classroom> FindAllClassrooms(List<Classroom> classrooms, Func<Classroom, bool> predicate)
    {
        return classrooms.Where(predicate).ToList();
    }
    
    public static List<Classroom> Sort<TKey>(List<Classroom> classrooms, Func<Classroom, TKey> keySelector, bool isDescending = false)
    {
        return (isDescending
            ? classrooms.OrderByDescending(keySelector)
            : classrooms.OrderBy(keySelector))
            .ToList();
    }
    
    public static int CountClassrooms(List<Classroom> classrooms, Func<Classroom, bool> predicate)
    {
        return classrooms.Where(predicate).Count();
    }

    private static List<Classroom> CreateClassroomsList()
    {
        return
        [
            new Classroom("Р-044", 50, true, false, false),
            new Classroom("Р-209", 25, false, false, false),
            new Classroom("Р-325", 90, true, true, false),
            new Classroom("Р-143", 30, true, false, true),
            new Classroom("Р-339", 100, true, true, false)
        ];
    }

    private static void Print<T>(T str)
    {
        Console.WriteLine(str);
    }
    
    private static void PrintCollection(IEnumerable<Classroom> collection)
    {
        Print(string.Join(", ", collection));
    }
}

