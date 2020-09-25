using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Yml_catalog catalog;
        private Button startButton;
        private ListView listView;
        private const string url = "http://partner.market.yandex.ru/pages/help/YML.xml";
        private const string startButtonNormalText = "Fetch data";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            startButton = FindViewById<Button>(Resource.Id.StartButton);
            startButton.Click += StartButton_Click;
            startButton.Text = startButtonNormalText;

            listView = FindViewById<ListView>(Resource.Id.ListIDs);
            listView.ItemClick += ListView_ItemClick;
        }

        private async void StartButton_Click(object sender, System.EventArgs e)
        {
            string[] ids = null;
            try
            {
                startButton.Text = "Please wait";
                startButton.Enabled = false;
                listView.Visibility = Android.Views.ViewStates.Invisible;
                catalog = await GetCatalog();
                startButton.Text = startButtonNormalText;
                startButton.Enabled = true;
                ids = catalog.Shop.Offers.Offer.Select(l => l.Id.ToString()).ToArray();
            }
            catch (Exception ex)
            {
                // TODO: error handling
            }
            if (ids != null && ids.Length > 0)
            {
                listView.Adapter = new ArrayAdapter<string>(this, Resource.Layout.simple_list_item_1, ids);
                listView.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                // TODO: error handling
            }
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (int.TryParse(((TextView)e.View).Text, out int id))
            {
                Offer offer = catalog.Shop.Offers.Offer.FirstOrDefault(l => l.Id == id);
                if (offer != null)
                {
                    Data.JsonData = JsonConvert.SerializeObject(offer, Formatting.Indented);
                    Intent intent = new Intent(this, typeof(JsonActivity));
                    StartActivity(intent);
                }
                else
                {
                    // TODO: error handling
                }
            }
            else
            {
                // TODO: error handling
            }
        }

        private async Task<Yml_catalog> GetCatalog()
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] xml = await webClient.DownloadDataTaskAsync(url);
                using (MemoryStream stream = new MemoryStream(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Yml_catalog));
                    return (Yml_catalog)serializer.Deserialize(stream);
                }
            }
        }
    }
}