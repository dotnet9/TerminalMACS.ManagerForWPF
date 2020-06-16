using System.IO;
using System.Net.Http;

namespace TerminalMACS.TestDemo.Views.LoLGoal.API
{
    public class Api
    {
        private string Key { get; set; }
        private string Region { get; set; }

        public Api(string region)
        {
            Region = region;
            Key = GetKey("API/Key.txt");
        }

        protected HttpResponseMessage GET(string URL)
        {
            using (HttpClient client = new HttpClient())
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
            if (System.IO.File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                return sr.ReadToEnd();
            }
            else
            {
                return "";
            }
        }
    }
}
