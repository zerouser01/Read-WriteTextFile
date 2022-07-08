using System.Diagnostics;
using System.Text.RegularExpressions;
using test1;

class Program
{

    static void Main(string[] args)
    {
        string path_read = "C:\\gore-ot-uma.txt";
        string path_write_async = "C:\\Users\\rasto\\Desktop\\result_async.txt";
        string path_write_sync = "C:\\Users\\rasto\\Desktop\\result_sync.txt";
        
        ReadWriter readWriter = new ReadWriter();
        Stopwatch stopWatch = new Stopwatch();

        stopWatch.Start();
        readWriter.ReadAsync(path_read, path_write_async);
        stopWatch.Stop();
        Console.WriteLine("Затраченное время async: " + stopWatch.Elapsed);

        stopWatch = new Stopwatch();
        stopWatch.Start();
        readWriter.Read(path_read, path_write_sync);
        stopWatch.Stop();
        Console.WriteLine("Затраченное время sync: " + stopWatch.Elapsed);

    }
}

