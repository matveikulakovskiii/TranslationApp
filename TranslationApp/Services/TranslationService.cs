using System.Threading.Tasks;

namespace TranslationApp.Services
{
    public static class TranslationService
    {
        public static async Task<string> TranslateText(string text, string fromLanguage = "et", string toLanguage = "ru")
        {
            // Здесь добавьте код для вызова API перевода, например, Google Translate API или Azure Cognitive Services
            // В данном примере это просто заглушка

            await Task.Delay(500); // Имитация задержки сети

            return text switch
            {
                "Tere" => "Здравствуйте",
                "Aitäh" => "Спасибо",
                "Vesi" => "Вода",
                "Maja" => "Дом",
                "Auto" => "Машина",
                _ => "Неизвестное слово"
            };
        }
    }
}
