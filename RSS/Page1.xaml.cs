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
    public partial class Page1 : PhoneApplicationPage
    {

        private string contentGet;

        public Page1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
            {
                this.contentGet = Parametres.currentContentParams; // l'élement a afficher, le content:encoded //
                this.webBrowser1.NavigateToString(this.contentGet);
                this.webBrowser1.IsScriptEnabled = true;
                //this.MyTBl.Text = Parametres.currentContentParams
            }
        }
    }
}