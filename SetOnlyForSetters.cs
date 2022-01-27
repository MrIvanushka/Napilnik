
public static void CreateObject()
{
    //Создание объекта на карте
}

public static void GenerateChance()
{
    _chance = Random.Range(0, 100);
}

public static int ScoreSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}