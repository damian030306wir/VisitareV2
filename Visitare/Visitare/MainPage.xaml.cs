using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Visitare
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Position> positions = new ObservableCollection<Position>();
        public bool seeRoute = false; 
        public MainPage()
        {
            InitializeComponent();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(1.0)));
        }
        public MainPage(RoutePoints points)
        {
            InitializeComponent();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(2.0)));
            foreach(Points tmp in points.routePoints)
            {
                CustomPin pin = new CustomPin
                {
                    Type = PinType.SavedPin,
                    Position = new Position(tmp.X, tmp.Y),
                    Label = tmp.Name,
                    Address = tmp.Description,
                    Name = "Xamarin",
                    Url = "http://xamarin.com/about/",
                    Question = "",
                    Answer = ""
                };

                if(String.IsNullOrWhiteSpace(tmp.Name))
                    pin.Label = "Name";

                customMap.Pins.Add(pin);
            }
        }
        private async void OnLogOut(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        public async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(nazwaEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj nazwę punktu", "Ok");
                return;
            }
            
           
            CustomPin pin = new CustomPin
            {
                Type = PinType.SavedPin,
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                Label = nazwaEntry.Text,
                Address = opisEntry.Text,
                Name = "Xamarin",
                Url = "http://xamarin.com/about/",
                Question = zagadkaEntry.Text,
                Answer = odpowiedzEntry.Text
            };
          
            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                string pinName = ((CustomPin)s).Label;
               // string pytanie = ((CustomPin)s).Question;
                string opis = ((CustomPin)s).Address;
                // string odpowiedz = ((CustomPin)s).Answer;
                await DisplayAlert($"{pinName}", $"{opis}", "Quiz");
                // await DisplayAlert("Quiz", $"{pytanie}", "Przejdź do odpowiedzi");
                await Navigation.PushAsync(new QuestionPage(new Question()));
                
            };
            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);

            /*var json = JsonConvert.SerializeObject(new { X =  pin.Position.Latitude, Y = pin.Position.Longitude });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var result = await client.PostAsync("http://dearjean.ddns.net:44201/api/Points", content);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Komunikat", "Dodanie puntku przebiegło pomyślnie", "Anuluj");
            }
            */
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            customMap.Pins.Clear();
            customMap.MapElements.Clear();
            positions.Clear();
        }

        private async void OnRoutesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RoutesPage());
        }

        private void OnNewRoutesClicked(object sender, EventArgs e)
        {

        }
    }
}
