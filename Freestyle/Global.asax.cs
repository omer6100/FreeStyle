using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Freestyle.Contexts;
using System.Net.Http;
using System.Web.UI.WebControls;

namespace Freestyle
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private MusicContext db = new MusicContext();
        private static readonly  HttpClient cli = new HttpClient();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Thread postingThread = new Thread(PostToFacebook);
            postingThread.IsBackground = false;
            postingThread.Start();

            Application["postingThread"] = postingThread;
        }

        private async void PostToFacebook()
        {
            while (true)
            {
                var topArtist = (from artist in db.Artists
                                orderby artist.AvgScore descending 
                                    select artist).FirstOrDefault();
                var topAlbum = (from album in db.Albums
                                    orderby album.AvgScore descending 
                                    select album).FirstOrDefault();
                var message = "Hi There Freestyle Fans! Its Time to Update You on our Highest Rated Albums and Artists. " +
                              "This great "+topAlbum.Genre+" Album " + " from " + topAlbum.Artist+ ", "  + topAlbum.Title  + ", reigns supreme with an Average Score of " +
                                string.Format("{0:0.#}", topAlbum.AvgScore * 1.0) + "! Also, our top rated Artist hails from " + topArtist.OriginCountry + 
                              ", with an outstanding Average Score of " + string.Format("{0:0.#}", topArtist.AvgScore * 1.0) +", its the fantastic " + topArtist.Name + "!";


                var values = new Dictionary<string, string>
                {
                    {"message", message},
                    {"access_token", "EAAK6RyueSUsBANjI09ierCMOUgRHdlGZAZBTIEoSZA0SmkQfRoQpH5hvpfZCilMj5TjK2BKzBgZABzIDgza6D0Y7Py1aCNW9Wjdtl8q4hFBUy8lI6v1X63q8bmPhX1b95TROAK2PcG79NCYdi0H1uP2p9YOCychB8idHu9ALVwDllyr7tnoyU"}
                };
                var content = new FormUrlEncodedContent(values);
                var response = await cli.PostAsync("https://graph.facebook.com/588623418489384/feed", content);
                var responseString = await response.Content.ReadAsStringAsync();

                
                Console.WriteLine(responseString);
                Thread.Sleep(60*60*1000);
            }
        }

        protected void Application_End()
        {
               Thread postingThread = (Thread) Application["postingThread"];
               if (postingThread != null && postingThread.IsAlive)
               {
                   postingThread.Abort();
               }
        }
    }
}
