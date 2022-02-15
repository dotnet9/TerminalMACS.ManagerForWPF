using System.IO;
using System.Net.Http;

namespace TerminalMACS.TestDemo.Views.LoLGoal.API;

public class Api
{
    public Api(string region)
    {
        Region = region;
        Key = GetKey("API/Key.txt");
    }

    private string Key { get; }
    private string Region { get; }

    protected HttpResponseMessage GET(string URL)
    {
        using (var client = new HttpClient())
        {
            var result = client.GetAsync(URL);
            result.Wait();

            return result.Result;
        }
    }

    protected string GetURI(string path)
    {
        return "https://" + Region + ".api.riotgames.com/lol/" + path + "?api_key=" + Key;
    }

    public string GetKey(string path)
    {
        if (File.Exists(path))
        {
            var sr = new StreamReader(path);
            return sr.ReadToEnd();
        }

        return "";
    }
}