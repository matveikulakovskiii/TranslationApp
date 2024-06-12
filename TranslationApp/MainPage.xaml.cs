using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using TranslationApp.Services;

namespace TranslationApp
{
    public partial class MainPage : ContentPage
    {
        Entry emailEntry;
        Entry passwordEntry;

        public MainPage()
        {
            
            BuildUI();
        }

        private void BuildUI()
        {
            var stackLayout = new StackLayout
            {
                Padding = new Thickness(10)
            };

            var loginLabel = new Label
            {
                Text = "Вход",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            stackLayout.Children.Add(loginLabel);

            emailEntry = new Entry { Placeholder = "Email" };
            stackLayout.Children.Add(emailEntry);

            passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };
            stackLayout.Children.Add(passwordEntry);

            var loginButton = new Button { Text = "Войти" };
            loginButton.Clicked += OnLoginButtonClicked;
            stackLayout.Children.Add(loginButton);

            var registerButton = new Button { Text = "Регистрация" };
            registerButton.Clicked += OnRegisterButtonClicked;
            stackLayout.Children.Add(registerButton);

            Content = stackLayout;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            await AnimateButton(button);

            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (UserRepository.UserExists(email, password))
            {
                await Navigation.PushAsync(new TranslatorPage());
            }
            else
            {
                await DisplayAlert("Ошибка", "Неверный email или пароль", "ОК");
            }
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            await AnimateButton(button);

            await Navigation.PushAsync(new RegistrationPage());
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
