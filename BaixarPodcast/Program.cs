using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace BaixarPodcast
{
    class Program
    {
        static void Main(string[] args)
        {

            downloadPodcast("http://jovemnerd.com.br/categoria/nerdcast/feed/");
        }

        static void Main(string ajuda)
        {
            if (Equals(ajuda, "--help") || Equals(ajuda, "--ajuda") || Equals(ajuda,"-h"))
            {
                Console.WriteLine("Lista de comandos");
                Console.WriteLine("--help Listagem de comandos");
                Console.WriteLine("-h Listagem de comandos");
                Console.WriteLine("-url url do feed do podcast");
                Console.WriteLine("===================================");
                Console.Read();
            }
        }

        static void Main(string parametro, string url, params string[] args)
        {
            if (!Equals(url, "-url")) return;
            downloadPodcast(url);


        }

        private static void downloadPodcast(string url, string urlPath="C:\\Podcast\\")
        {
            Console.WriteLine("Começando os procedimentos");
            Console.WriteLine("Tentando recuperar a  url");
            var reader = XmlReader.Create(url);
            Console.WriteLine("Feed recuperado, fazendo a conversão");
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            Console.WriteLine("==========================================");
            Console.WriteLine("Começando o download");

            foreach (var item in feed.Items)
            {
                var cont = 0;
                foreach (var link in item.Links.Where(c => c.Uri.ToString().Contains(".mp3")).ToList())
                {
                    Console.WriteLine("Iniciando download"); 
                    Console.WriteLine("Arquivo: {0}",item.Title.Text);
                    Console.WriteLine("Url: {0}", link.Uri.ToString());
                    
                    if (cont < 1)
                    {
                        var wClient = new WebClient();
                        var nome = String.Format("{0}{1}", item.Title.Text.Replace(" ","-"), ".mp3");
                        wClient.DownloadFile(link.Uri.ToString(), urlPath+nome);
                        //wClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                        //wClient.DownloadFileAsync(link.Uri, urlPath+nome);
                        //System.Diagnostics.Process.Start(urlPath + nome);
                        
                        cont++;
                        Console.WriteLine("Download do arquivo {0} finalizado",urlPath+nome);
                    }
                    else
                    {
                        var wClient = new WebClient();
                        var nome = String.Format("{0}{1}-{2}", item.Title.ToString().Replace(" ", "-"), cont, ".mp3");
                        wClient.DownloadFile(link.Uri.ToString(), urlPath + nome);
                    }
                    Console.WriteLine("=============================================================");
                }
            }
            
        }


        static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write(e.ProgressPercentage.ToString()+"-");
        }

    }
}
