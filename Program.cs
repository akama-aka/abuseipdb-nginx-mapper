using System.Net;

namespace ipabusedb_mmdb;

class Program
{
    static void Main(string[] args)
    {
        var client = new HttpClient();
        string[] nginxFormatedList = new string[0];
        string path = Directory.GetCurrentDirectory();
        var response =
            client.GetAsync(
                "https://raw.githubusercontent.com/borestad/blocklist-abuseipdb/refs/heads/main/abuseipdb-s100-30d.ipv4");
        response.Wait();
        if (response.IsCompletedSuccessfully)
        {
            var content = response.Result.Content.ReadAsStringAsync();
            //var content = temp;
            string[] adresses = content.Result.Split(new string[]  { Environment.NewLine }, StringSplitOptions.None);
            //string[] adresses = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        
            foreach (var i in adresses)
            {
                if (i.StartsWith('#') || i == String.Empty) continue;
                string[] ip = i.Split(' ');
                IPAddress address = IPAddress.Parse(ip[0]);
                Console.WriteLine(address);
                string nginx = $"\"{address}\" 1;";
                Console.WriteLine(nginx);
                nginxFormatedList = nginxFormatedList.Append(nginx).ToArray();
            }
            Console.WriteLine(nginxFormatedList);
            var nginxFile = new StreamWriter(Path.Combine(path, "nginx.conf"), true);
            nginxFile.WriteLine("map $http_x_forwarded_for $mitigate_ip_bad {\n"+ string.Join("\n", nginxFormatedList) + "\n}");
            nginxFile.Close();
        }
        else
        {
            Console.WriteLine("Error");
            Console.WriteLine(response.Result.StatusCode);
            
        }
    }
}