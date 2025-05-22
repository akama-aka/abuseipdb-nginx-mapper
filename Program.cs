using System.Net;

namespace ipabusedb_mmdb;

class Program
{
    static void Main(string[] args)
    {
        var client = new HttpClient();
        string[] nginxFormatedList = new string[0];
        string path = Directory.GetCurrentDirectory();
        Console.WriteLine("Fetching AbuseIPDB Score 100 30 Days IP List...");
        var response =
            client.GetAsync(
                "https://raw.githubusercontent.com/borestad/blocklist-abuseipdb/refs/heads/main/abuseipdb-s100-30d.ipv4");
        response.Wait();
        if (response.IsCompletedSuccessfully)
        {
            var content = response.Result.Content.ReadAsStringAsync();
            string[] adresses = content.Result.Split(new string[]  { Environment.NewLine }, StringSplitOptions.None);
            Console.WriteLine("Format the result and Writing the List...");
            foreach (var i in adresses)
            {
                try
                {
                    if (i.StartsWith('#') || i == String.Empty) continue;
                    string[] ip = i.Split(' ');
                    IPAddress address = IPAddress.Parse(ip[0]);
                    string nginx = $"\"{address}\" 1;";
                    nginxFormatedList = nginxFormatedList.Append(nginx).ToArray();
                } catch (Exception e)
                {
                    Console.WriteLine($"An Error occurred:\n{e}");
                }
            }
            Console.WriteLine("Writing List...");
            var nginxFile = new StreamWriter(Path.Combine(path, "nginx.conf"), true);
            nginxFile.WriteLine("map $http_x_forwarded_for $mitigate_ip_bad {\n"+ string.Join("\n", nginxFormatedList) + "\n}");
            nginxFile.Close();
            Console.WriteLine($"Done. File is located in {Path.Combine(path, "nginx.conf")}");
        }
        else
        {
            Console.WriteLine("An Error occurred:");
            Console.WriteLine(response.Result.StatusCode);
            
        }
    }
}