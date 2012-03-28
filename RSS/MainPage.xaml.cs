using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Phone.Tasks;
using System.Xml.Linq;

namespace RSS
{
    public static class Parametres 
    {
        public static string currentContentParams;
    }

    public class RSSItem
    {
        public String Description { get; set; }
        public String Link { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public String PublishDate { get; set; }
    }


    public partial class MainPage : PhoneApplicationPage
    {

        private List<RSSItem> flux;
        private string currentContent;

        // Constructeur
        public MainPage()
        {
            InitializeComponent();
            this.flux = new List<RSSItem>();
        }

        private void feedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //MessageBox.Show("tesst");
            int x = this.feedListBox.SelectedIndex;

            if (x != -1)
            {
                //RSSItem it = this.flux[x];
                //string param = it.Content.ToString();
                //NavigationService.Navigate(new Uri("/Page1.xaml?msg=" + param, UriKind.RelativeOrAbsolute));
                this.currentContent = "";
                this.currentContent = this.flux[x].Content.ToString();
                this.MyButton.Opacity = 1;
            }
            
            /*ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItem != null)
            {
                // Get the SyndicationItem that was tapped.
                SyndicationItem sItem = (SyndicationItem)listBox.SelectedItem;

                // Set up the page navigation only if a link actually exists in the feed item.
                if (sItem.Links.Count > 0)
                {
                    // Get the associated URI of the feed item.
                    Uri uri = sItem.Links.FirstOrDefault().Uri;

                    // Create a new WebBrowserTask Launcher to navigate to the feed item. 
                    // An alternative solution would be to use a WebBrowser control, but WebBrowserTask is simpler to use. 
                    WebBrowserTask webBrowserTask = new WebBrowserTask();
                    webBrowserTask.URL = uri.ToString();
                    webBrowserTask.Show();
                }
            }
             */
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("tgezezezf");
            //string param = "VALLOT LE NOOBZA !";
            //this.feedListBox.Items.

            //int x = this.feedListBox.SelectedIndex;
            //MessageBox.Show(x.ToString());

            MessageBox.Show(this.currentContent);
            Parametres.currentContentParams = this.currentContent;
            string param = "test";


            NavigationService.Navigate(new Uri("/Page1.xaml?msg=" + param, UriKind.RelativeOrAbsolute));
        }

        private void loadFeedButton_Click(object sender, RoutedEventArgs e)
        {
            // WebClient is used instead of HttpWebRequest in this code sample because 
            // the implementation is simpler and easier to use, and we do not need to use 
            // advanced functionality that HttpWebRequest provides, such as the ability to send headers.
            WebClient webClient = new WebClient();

            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel. 
            //

            webClient.DownloadStringAsync(new System.Uri("http://www.exponaute.com/feed/"));
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    MessageBox.Show(e.Error.Message);
                });
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned. 
                this.State["feed"] = e.Result;
                UpdateFeedList(e.Result);
            }
        }

        // This method sets up the feed and binds it to our ListBox. 
        private void UpdateFeedList(string feedXML)
        {
            // Load the feed into a SyndicationFeed instance.
            StringReader stringReader = new StringReader(feedXML);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XElement xmlFeeds = XElement.Parse(feedXML);
            List<XElement> elements = xmlFeeds.Descendants("item").ToList();
            //List<RSSItem> flux = new List<RSSItem>();

            XNamespace contentNamespace = "http://purl.org/rss/1.0/modules/content/";

            foreach (XElement rssItem in elements)
            {
                //XElement x = new XElement(contentNamespace + "content:encoded", rssItem);

                //MessageBox.Show(x.Value);
                //MessageBox.Show(rssItem.ToString());
                //string test = rssItem.Element("description").Value;

                RSSItem x = new RSSItem();
                string titre = rssItem.Element("title").Value;
                string description = rssItem.Element("description").Value;
                string date = rssItem.Element("pubDate").Value;
                string content = rssItem.Element(contentNamespace + "encoded").Value;
                x.Title = titre;
                x.Description = description;
                x.Content = content;
                //this.feedListBox.Items.Add(x);
                this.flux.Add(x);
                //string test = rssItem.Element(contentNamespace + "encoded").Value;
                
                //string fixedString = HttpUtility.HtmlDecode(test);
                //MessageBox.Show(fixedString);
                //this.webBrowser1.NavigateToString(test);
                //this.webBrowser1.IsScriptEnabled = true;

                
                //string test = rssItem.Element(XName.Get("encoded", "http://www.exponaute.com/feed/")).Value;
                //string test = rssItem.Value;


                //MessageBox.Show(test);
                
                //string content = rssItem.Element(contentNamespace + "encoded").Value;
                //string content = rssItem.Element(contentNamespace + "encoded").Value;
                //MessageBox.Show(test.Value);
                //flux.Add(content);
            }

            this.feedListBox.ItemsSource = this.flux;
           
            
      
            /*from feed in xmlFeeds.Descendants("item")
                                           select new FeedItem
                                               {
                                                   Content = feed.Element(XName.Get("encoded", "")).Value
                                               };
             * /

            /*
            xmlReader.MoveToContent();
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.CDATA:
                        string x = xmlReader.Value;
                        feedListBox.Items.Add(x);
                        //MessageBox.Show(xmlReader.Value);
                        break;
                }    
     
            }
             */

            //SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

            // In Windows Phone OS 7.1, WebClient events are raised on the same type of thread they were called upon. 
            // For example, if WebClient was run on a background thread, the event would be raised on the background thread. 
            // While WebClient can raise an event on the UI thread if called from the UI thread, a best practice is to always 
            // use the Dispatcher to update the UI. This keeps the UI thread free from heavy processing.
            //Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
                // Bind the list of SyndicationItems to our ListBox.
              //  feedListBox.ItemsSource = feed.Items; // ajoute l'item dans la listebox //
                //loadFeedButton.Content = "Refresh Feed";
            //});
        }
    }
}