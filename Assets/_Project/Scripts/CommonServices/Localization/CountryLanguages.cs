using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs.Localization;

namespace _Project.Scripts.CommonServices.Localization
{
    public static class CountryLanguages
    {
        private static readonly Language DefaultLanguage = Language.English;
        
        private static Dictionary<string[], Language> Map = new()
        {
            { new [] { "ru", "be", "kz", "uk", "kk" }, Language.Russian },
            { new [] { "en" }, Language.English }
        };

        public static Language GetLanguageByCountry(string country)
        {
            string countryCode = country.ToLower();

            foreach (var pair in Map)
            {
                if (pair.Key.Contains(countryCode))
                {
                    return pair.Value;
                }
            }

            return DefaultLanguage; 
        }
    }
}