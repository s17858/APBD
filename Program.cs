namespace APBD;

public class Program
{

    public static double ObliczSrednia(int[] tablica)
    {
        int suma = 0;
        foreach (int liczba in tablica)
        {
            suma += liczba;
        }

        double srednia = (double)suma / tablica.Length;
        return srednia;
    }

    public static void Main()
    {
        int[] tablica = {1, 2, 3, 4, 5 };

    double srednia = ObliczSrednia(tablica);
        Console.WriteLine($"Srednia: {srednia:F2}");
    }
}