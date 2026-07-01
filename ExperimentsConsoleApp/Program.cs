using System.Diagnostics;
using AVL_TreeLib;
class Program
{
    static void Main()
    {
        int n = 100000;
        var rnd = new Random();

        int[] array = new int[n];
        for (int i = 0; i < n; i++)
            array[i] = rnd.Next();


        //AVL

        var avl = new AVL_Tree<int, int>();

        var sw = Stopwatch.StartNew();

        foreach (var item in array)
            avl[item] = 0;

        for (int i = n / 2; i < 3 * n / 4; i++)
            avl.Remove(array[i]);

        foreach (var item in array)
            avl.ContainsKey(item);

        sw.Stop();

        Console.WriteLine($"AVL: {sw.ElapsedMilliseconds} ms");


        //SortedDictionary

        var sorted = new SortedDictionary<int, int>();

        sw.Restart();

        foreach (var item in array)
            sorted[item] = 0;

        for (int i = n / 2; i < 3 * n / 4; i++)
            sorted.Remove(array[i]);

        foreach (var item in array)
            sorted.ContainsKey(item);

        sw.Stop();

        Console.WriteLine($"SortedDictionary: {sw.ElapsedMilliseconds} ms");
    }

}
