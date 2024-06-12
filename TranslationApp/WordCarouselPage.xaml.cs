using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TranslationApp
{
    public partial class WordCarouselPage : CarouselPage
    {
        private Dictionary<string, string> translations;

        public WordCarouselPage(List<string> words, Dictionary<string, string> translations)
        {
            InitializeComponent();

            this.translations = translations;

            foreach (var word in words)
            {
                var contentPage = new ContentPage();
                var label = new Label
                {
                    Text = word,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                label.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => TranslateWord(label)),
                    NumberOfTapsRequired = 1
                });
                contentPage.Content = label;
                Children.Add(contentPage);
            }
        }

        private void TranslateWord(Label label)
        {
            string word = label.Text;
            if (translations != null && translations.ContainsKey(word))
            {
                label.Text = translations[word];
            }
            else
            {
                label.Text = $"Перевод для слова '{word}' не найден";
            }
        }
    }
}
