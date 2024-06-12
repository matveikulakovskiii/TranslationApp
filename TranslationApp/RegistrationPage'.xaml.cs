using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using TranslationApp.Services;

namespace TranslationApp
{
    public partial class RegistrationPage : ContentPage
    {
        Entry emailEntry;
        Entry passwordEntry;

        public RegistrationPage()
        {

            BuildUI();
        }

        private void BuildUI()
        {
            var stackLayout = new StackLayout
            {
                Padding = new Thickness(10)
            };

            var registrationLabel = new Label
            {
                Text = "Регистрация",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            stackLayout.Children.Add(registrationLabel);

            emailEntry = new Entry { Placeholder = "Email" };
            stackLayout.Children.Add(emailEntry);

            passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };
            stackLayout.Children.Add(passwordEntry);

            var registerButton = new Button { Text = "Зарегистрироваться" };
            registerButton.Clicked += OnRegisterButtonClicked;
            stackLayout.Children.Add(registerButton);

            Content = stackLayout;
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            await AnimateButton(button);

            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            UserRepository.CreateUser(email, password);

            await DisplayAlert("Успех", "Регистрация успешна", "ОК");

            await Navigation.PopAsync();
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
