namespace Lab8
{
    class Program
    {

        static async Task Main(string[] args)
        {
            StreamService ss = new StreamService();
            MemoryStream ms = new MemoryStream();
            Progress<string> progress = new Progress<string>();
            progress.ProgressChanged += (s, e) => Console.WriteLine(e);
            List<Immovables> Imms = new List<Immovables>();
            for (int i = 0; i < 1000; i++) Imms.Add(new Immovables(i));
            Task task1 = ss.WriteToStreamAsync(ms, Imms, progress);
            await Task.Delay(1000);
            Task task2 = ss.CopyFromStreamAsync(ms, "qwerty.json", progress);
            await Task.WhenAll(task1, task2);
            int a = await ss.GetStatisticsAsync("qwerty.json", (obj) => obj.PropertyValue>35);
            Console.WriteLine(a);
        }
    }
}