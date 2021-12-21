using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESH.Core
{
    public class Translit
    {

        Dictionary<string, string> dictionaryChar = new Dictionary<string, string>()
            {
                {"а","a"},
                {"б","b"},
                {"в","v"},
                {"г","g"},
                {"д","d"},
                {"е","e"},
                {"ё","yo"},
                {"ж","zh"},
                {"з","z"},
                {"и","i"},
                {"й","y"},
                {"к","k"},
                {"л","l"},
                {"м","m"},
                {"н","n"},
                {"о","o"},
                {"п","p"},
                {"р","r"},
                {"с","s"},
                {"т","t"},
                {"у","u"},
                {"ф","f"},
                {"х","h"},
                {"ц","ts"},
                {"ч","ch"},
                {"ш","sh"},
                {"щ","sch"},
                {"ъ","'"},
                {"ы","yi"},
                {"ь",""},
                {"э","e"},
                {"ю","yu"},
                {"я","ya"},
                {"А","a"},
                {"Б","b"},
                {"В","v"},
                {"Г","g"},
                {"Д","d"},
                {"Е","e"},
                {"Ё","yo"},
                {"Ж","zh"},
                {"З","z"},
                {"И","i"},
                {"Й","y"},
                {"К","k"},
                {"Л","l"},
                {"М","m"},
                {"Н","n"},
                {"О","o"},
                {"П","p"},
                {"Р","r"},
                {"С","s"},
                {"Т","t"},
                {"У","u"},
                {"Ф","f"},
                {"Х","h"},
                {"Ц","ts"},
                {"Ч","ch"},
                {"Ш","sh"},
                {"Щ","sch"},
                {"Ъ","'"},
                {"Ы","yi"},
                {"Ь",""},
                {"Э","e"},
                {"Ю","yu"},
                {"Я","ya"},
            {" ","_" }
            };

        public string TranslitFileName(string source)
        {
            var result = "";
           
            foreach (var ch in source)
            {
                var ss = "";

                if (dictionaryChar.TryGetValue(ch.ToString(), out ss))
                {
                    result += ss;
                }
      
                else result += ch;
            }
            return result;
        }
    }
}