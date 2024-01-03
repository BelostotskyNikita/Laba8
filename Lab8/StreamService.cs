using System.Runtime.Serialization.Json;
using System.Text.Json;
namespace Lab8
{
    public class StreamService
    {
        Semaphore sph = new Semaphore(1, 1);
        public async Task WriteToStreamAsync(Stream stream, IEnumerable<Immovables> data, IProgress<string> progress)
        {
            try
            {
                sph.WaitOne();
                progress.Report("Поток записи " + Thread.CurrentThread.ManagedThreadId + " запущен");
                for (int i = 1; i <= 100; i++)
                {
                    //await Task.Delay(10);
                    Thread.Sleep(10);
                    progress.Report("Поток "+Thread.CurrentThread.ManagedThreadId + ": "+i+"%");
                }
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<Immovables>));
                js.WriteObject(stream, data);
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            progress.Report("Поток записи " + Thread.CurrentThread.ManagedThreadId + " закончил работу");
            sph.Release();
        }
        public async Task CopyFromStreamAsync(Stream stream, string filename, IProgress<string> progress)
        {
            sph.WaitOne();
            progress.Report("Поток чтения " + Thread.CurrentThread.ManagedThreadId + " запущен");
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();
            await File.WriteAllTextAsync(filename, json);
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(10);
                progress.Report("Поток " + Thread.CurrentThread.ManagedThreadId + ": " + i + "%");
            }
            progress.Report("Поток чтения " + Thread.CurrentThread.ManagedThreadId + " закончил работу");
            sph.Release();
        }
        public async Task<int> GetStatisticsAsync(string fileName, Func<Immovables, bool> filter)
        {
            int count = 0;
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var persons = await JsonSerializer.DeserializeAsync<List<Immovables>>(fs);
                if (persons == null) throw new ArgumentNullException();
                foreach (var p in persons)
                    if (filter(p)) count++;
            }
            return count;
        }
    }
}