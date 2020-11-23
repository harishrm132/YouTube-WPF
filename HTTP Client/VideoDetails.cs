using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Youtube_WPF
{
    class VideoDetails
    {

        public class Youtube
        {
            public string titleUrl { get; set; }
        }

        public class Data1
        {
            public List<Items> items { get; set; }
            public class Items
            {
                public string ID { get; set; } //ID
                public Snippet snippet { get; set; }
                public class Snippet
                {
                    public string title { get; set; } //Video Name
                    public string channelTitle { get; set; } //ChannelName
                    public string categoryId { get; set; } //ChannelCatogeryID

                }
            }
        }

        // HttpClient is intended to be instantiated once per application, rather than per-use.
        static readonly HttpClient client = new HttpClient();
        public static List<Data1.Items> Conc = new List<Data1.Items>();

        //This will give us the full name path of the executable file:
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //This will strip just the working path name:
        public static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

        static async Task Getdetails(string link)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string responseBody = await client.GetStringAsync(link);
                var m = JsonConvert.DeserializeObject<Data1>(responseBody);

                Conc.AddRange(m.items);

            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("\nException Caught!");
                MessageBox.Show("Message :{0} ", e.Message);
            }
        }

        
        public async Task GetVideo(String api_key)
        {
            String link = "https://www.googleapis.com/youtube/v3/videos";
            link = link + "?part=snippet&key=" + api_key;

            string filepath = MainWindow.Path;
            string lines = System.IO.File.ReadAllText(filepath);

            List<Youtube> m = JsonConvert.DeserializeObject<List<Youtube>>(lines);
            List<string> IDlist = new List<string>();

            //POSSIBLE ID VALUES
            foreach (Youtube s in m)
            {
                string ID = s.titleUrl;
                if (ID != null)
                {
                    int found = ID.IndexOf("=");
                    ID = ID.Substring(found + 1);
                    IDlist.Add(ID);
                }
            }

            for (int i = 0; i < IDlist.Count - 50; i += 50)
            {
                await Getdetails(link + "&id=" + String.Join(",", IDlist.GetRange(i, 50)));
            }


            //open file stream
            using (StreamWriter file = File.CreateText(strWorkPath + "/VideoDetails.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, Conc);
            }

        }

    }
}
