using System;

namespace APBD;

public class Program
{

    public static int ZnajdzMaxWartosc(int[] tablica)
    {
        int maxWartosc = int.MinValue;
        foreach (int number in tablica)
        {
            if(number > maxWartosc)
			{
				maxWartosc = number;
>>>>>>> feature-new
			}
        }
        return maxWartosc;
    }

    public static void Main()
    {
        int[] tablica = {1, 5, 3, 2, 4 };

    int maxWartosc = ZnajdzMaxWartosc(tablica);
        Console.WriteLine($"Maksymalna wartość: {maxWartosc}");
		Console.WriteLine("");
    }
}