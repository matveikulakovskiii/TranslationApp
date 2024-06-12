using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TranslationApp
{
    public partial class TranslatorPage : ContentPage
    {
        ListView wordsListView;
        ObservableCollection<WordCard> wordCards;
        private int currentWordIndex = 0;
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string wordsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Words");

        public TranslatorPage()
        {
            InitializeWordsFolder();
            wordCards = new ObservableCollection<WordCard>(); // Инициализация wordCards
            BuildUI();
        }

        private void InitializeWordsFolder()
        {
            if (!Directory.Exists(wordsFolderPath))
                Directory.CreateDirectory(wordsFolderPath);
        }

        private void BuildUI()
        {
            var stackLayout = new StackLayout
            {
                Padding = new Thickness(10)
            };

            var translatorLabel = new Label
            {
                Text = "Переводчик",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            stackLayout.Children.Add(translatorLabel);

            wordsListView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    var cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, "Word");
                    return cell;
                })
            };

            var translateButton = new Button
            {
                Text = "Перевести",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(10),
            };
            translateButton.Clicked += async (sender, e) => await TranslateWord();

            stackLayout.Children.Add(wordsListView);
            stackLayout.Children.Add(translateButton);

            var resetButton = new Button
            {
                Text = "Начать заново",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromHex("#007AFF"),
                TextColor = Color.White
            };
            resetButton.Clicked += async (sender, e) => await ResetTranslation();
            stackLayout.Children.Add(resetButton);

            var addButton = new Button
            {
                Text = "Добавить слово",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromHex("#007AFF"),
                TextColor = Color.White
            };
            addButton.Clicked += async (sender, e) => await AddWord();
            stackLayout.Children.Add(addButton);
            var clearButton = new Button
            {
                Text = "Очистить",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromHex("#007AFF"),
                TextColor = Color.White
            };
            clearButton.Clicked += (sender, e) => ClearWords();
            stackLayout.Children.Add(clearButton);
            var removeButton = new Button
            {
                Text = "Удалить слово",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromHex("#007AFF"),
                TextColor = Color.White
            };
            removeButton.Clicked += async (sender, e) => await RemoveWord();
            stackLayout.Children.Add(removeButton);


            var showWordsButton = new Button
            {
                Text = "Показать все слова",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromHex("#007AFF"),
                TextColor = Color.White
            };
            showWordsButton.Clicked += async (sender, e) => await ShowAllWords();
            stackLayout.Children.Add(showWordsButton);


            Content = stackLayout;
        }

        private async Task ShowAllWords()
        {
            List<string> allWords = new List<string>();

            foreach (var wordCard in wordCards)
            {
                allWords.Add(wordCard.Word);
            }

            // Создаем словарь переводов для передачи в WordCarouselPage
            Dictionary<string, string> translations = new Dictionary<string, string>();
            foreach (var wordCard in wordCards)
            {
                translations[wordCard.Word] = wordCard.TranslatedWord;
            }

            await Navigation.PushAsync(new WordCarouselPage(allWords, translations));
        }


        private async Task RemoveWord()
        {
            if (wordsListView.SelectedItem == null)
            {
                await DisplayAlert("Ошибка", "Не выбрано слово для удаления", "OK");
                return;
            }

            WordCard selectedWord = (WordCard)wordsListView.SelectedItem;
            string wordFilePath = Path.Combine(wordsFolderPath, $"{selectedWord.Word}.txt");
            string translationFilePath = Path.Combine(wordsFolderPath, $"{selectedWord.Word}_translated.txt");

            if (File.Exists(wordFilePath))
            {
                File.Delete(wordFilePath); // Удаляем файл со словом
            }

            if (File.Exists(translationFilePath))
            {
                File.Delete(translationFilePath); // Удаляем файл с переводом слова
            }

            // Удаляем слово из списка
            wordCards.Remove(selectedWord);

            // Обновляем ListView
            wordsListView.ItemsSource = null;
            wordsListView.ItemsSource = wordCards;
        }
        private void ClearWords()
        {
            wordCards.Clear();
            wordsListView.ItemsSource = null; // Очистка источника данных
        }

        private async Task ResetTranslation()
        {
            var confirmation = await DisplayAlert("Начать заново", "Вы уверены, что хотите начать переводить слова заново?", "Да", "Нет");
            if (confirmation)
            {
                currentWordIndex = 0; // Сброс текущего индекса
                LoadWords();
                await DisplayAlert("Начать заново", "Теперь вы можете начать переводить слова заново", "ОК");
            }
        }

        private async Task AddWord()
        {
            if (wordCards == null)
                wordCards = new ObservableCollection<WordCard>();

            string newWord = await DisplayPromptAsync("Добавить слово", "Введите новое слово", "Добавить", "Отмена", keyboard: Keyboard.Text);
            if (string.IsNullOrEmpty(newWord))
                return;

            string translatedWord = await DisplayPromptAsync("Добавить слово", $"Введите перевод слова '{newWord}'", "Добавить", "Отмена", keyboard: Keyboard.Text);

            if (string.IsNullOrEmpty(translatedWord))
                return;

            // Записываем слово и его перевод в соответствующие файлы
            string wordFilePath = Path.Combine(wordsFolderPath, $"{newWord}.txt");
            string translationFilePath = Path.Combine(wordsFolderPath, $"{newWord}_translated.txt");

            File.WriteAllText(wordFilePath, newWord);
            File.WriteAllText(translationFilePath, translatedWord);

            // Добавляем слово в список
            wordCards.Add(new WordCard { Word = newWord, TranslatedWord = translatedWord });

            // После добавления нового слова обновляем ListView
            LoadWords();
        }



        private void LoadWords()
        {
            wordCards.Clear(); // Очищаем список слов перед загрузкой
            foreach (string wordFile in Directory.GetFiles(wordsFolderPath, "*.txt"))
            {
                if (!wordFile.EndsWith("_translated.txt")) // Проверяем, что это не файл с переводом
                {
                    string word = File.ReadAllText(wordFile);
                    string translationFileName = Path.GetFileNameWithoutExtension(wordFile) + "_translated.txt";
                    string translationFilePath = Path.Combine(wordsFolderPath, translationFileName);

                    if (File.Exists(translationFilePath))
                    {
                        string translatedWord = File.ReadAllText(translationFilePath);
                        wordCards.Add(new WordCard { Word = word, TranslatedWord = translatedWord });
                    }
                    else
                    {
                        wordCards.Add(new WordCard { Word = word, TranslatedWord = "Перевод не найден" });
                    }
                }
            }

            wordsListView.ItemsSource = wordCards;
        }



        private async Task TranslateWord()
        {
            if (wordsListView.SelectedItem == null)
            {
                await DisplayAlert("Ошибка", "Не выбрано слово для перевода", "OK");
                return;
            }

            WordCard selectedWord = (WordCard)wordsListView.SelectedItem;
            string translationFilePath = Path.Combine(wordsFolderPath, $"{selectedWord.Word}_translated.txt");

            if (File.Exists(translationFilePath))
            {
                string translatedWord = await Task.Run(() => File.ReadAllText(translationFilePath));
                selectedWord.Word = translatedWord; // Заменяем оригинальное слово на его перевод

                // Уведомляем ListView о изменениях в источнике данных
                wordsListView.ItemsSource = null;
                wordsListView.ItemsSource = wordCards;
            }
            else
            {
                await DisplayAlert("Ошибка", "Перевод не найден", "OK");
            }
        }





        public class WordCard
        {
            public string Word { get; set; }
            public string TranslatedWord { get; set; }
        }
    }
}