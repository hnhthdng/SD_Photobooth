using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;

namespace PhotoBooth_App.Handler
{
    public static class LanguageHandler
    {
        public static event EventHandler LanguageChanged;

        public static void SwitchLanguage(string language)
        {
            ResourceDictionary newLangDict = new ResourceDictionary();
            switch (language)
            {
                case "en":
                    newLangDict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
                default:
                    newLangDict.Source = new Uri("Resources/Strings.vi.xaml", UriKind.Relative);
                    break;
            }

            var dictionaries = Application.Current.Resources.MergedDictionaries;
            for (int i = dictionaries.Count - 1; i >= 0; i--)
            {
                var md = dictionaries[i];
                if (md.Source != null && md.Source.OriginalString.Contains("Strings"))
                {
                    dictionaries.RemoveAt(i);
                }
            }
            dictionaries.Add(newLangDict);

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Language = XmlLanguage.GetLanguage(language == "en" ? "en-US" : "vi-VN");
            }
            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
