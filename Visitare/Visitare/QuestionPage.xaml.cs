using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionPage : ContentPage
    {
        private Question question;

        public bool Result { get; set; }

        public QuestionPage(Question question)
        {
            InitializeComponent();
            BindingContext = question;
            this.question = question;

            Result = false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemIndex == question.GoodAnswer)
            {
                Result = true;
            }

            Navigation.PopModalAsync();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
          var question = new Question { Answers = new List<string> { "Tak", "Nie", "Może", "Lubię placki" }, GoodAnswer = 0, Text = "Czy kot ma ogon?" };
          var questionPage = new QuestionPage(question);
          questionPage.Disappearing += QuestionPageClosed;

          await Navigation.PushModalAsync(questionPage);
        }

       private async void QuestionPageClosed(object sender, EventArgs e)
       {
         var questionPage = sender as QuestionPage;
         await DisplayAlert("", "Człowiek odpowiedział: " + (questionPage.Result ? "dobrze" : "źle"), "OK");
       }
    }
}