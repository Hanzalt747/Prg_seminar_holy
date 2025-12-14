using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PraceSTextovymiSoubory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            #region Prakticka cast
            // Nasledujici ukoly vyreste pomoci programovani. Metodu "kouknu a vidim" si nechte pro kontrolu.
            // Vysledek si nechte vypsat do konzole a uvedte odpoved v komentari.
            // Vyreste vzdy i vsechny podotazky.

            // Slozku vstupni_soubory si presunte do ...\bin\Debug\netX.Y\. 
            // Dale si ji otevrete ve VS Code a sledujte obsah souboru. 
            // Doporuceni: pro dukladnejsi patrani si stahnete extension do VS Code Inspector Hex nebo Hex Editor.



            //
            File.WriteAllText("vstupni_soubory/2.txt", "Ahoj \tsvěte!\n", Encoding.UTF8);
            // (10b) 1. Jaky je pocet znaku v souboru 1.txt a jaky v 2.txt?
            // Zkontrolujte s VS Code a vysvetlete rozdily.
            // Tip: Pri Debugovani uvidite vschny ctene znaky.
            string text1 = File.ReadAllText("vstupni_soubory/1.txt", Encoding.UTF8);
            string text2 = File.ReadAllText("vstupni_soubory/2.txt", Encoding.UTF8);
            Console.WriteLine($"1) 1.txt má {text1.Length} znaků, 2.txt má {text2.Length} znaků.");
            // 1.txt obsahuje navic vic mezer misto tabulatoru, 2.txt ma tab a dva newline znaky navic.


            // (10b) 2. Jaky je pocet znaku v souboru 1.txt, kdyz pomineme bile znaky?
            // Tip: Struktura Char ma statickou funkci IsWhiteSpace().            
            int nonWhite = text1.Count(c => !char.IsWhiteSpace(c));
            Console.WriteLine($"2) 1.txt bez bílých znaků: {nonWhite}");


            //
            using (StreamWriter sw = new StreamWriter("vstupni_soubory/4.txt", false, Encoding.UTF8))
            {
                sw.WriteLine("1");
                sw.WriteLine("2");
                sw.WriteLine("3");
            }
            using (StreamWriter sw = new StreamWriter("vstupni_soubory/5.txt", false, Encoding.UTF8))
            {
                sw.Write("1\n2\n3");
            }
            // (5b) 3. Jake znaky (vypiste jako integery) jsou pouzity pro oddeleni radku v souboru 3.txt?
            // Porovnejte s 4.txt a 5.txt.
            // Jakym znakum odpovidaji v ASCII tabulce? https://www.ascii-code.com/
            // Zde se staci podivat do VS Code a napsat sem odpoved, neni potreba nic programovat.
            PrintLineSeparators("vstupni_soubory/3.txt", "3.txt");
            PrintLineSeparators("vstupni_soubory/4.txt", "4.txt");
            PrintLineSeparators("vstupni_soubory/5.txt", "5.txt");



            // (10b) 4. Kolik slov ma soubor 6.txt?
            // Za slovo ted povazujme neprazdnou souvislou posloupnost nebilich znaku oddelene bilymi.
            // Tip: Split defaultne oddeluje na zaklade libovolnych bilych znaku, ale je tam jeden hacek.. jaky?
            // V souboru je videt 52 slov.
            string text6 = File.ReadAllText("vstupni_soubory/6.txt", Encoding.UTF8);
            var words = text6.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"4) Počet slov v 6.txt: {words.Length}");


            // (15b) 5. Zapiste do souboru 7.txt slovo "rericha". Povedlo se? 
            // Vypiste obsah souboru do konzole. V cem je u konzole problem a jak ho spravit?
            // Jake kodovani pouziva C#? Kolik bytu na znak?
            string slovo = "řeřicha";
            File.WriteAllText("vstupni_soubory/7.txt", slovo, Encoding.UTF8);
            string text7 = File.ReadAllText(@"vstupni_soubory/7.txt", Encoding.UTF8);
            Console.WriteLine($"5) Obsah 7.txt: {text7}");
            Console.WriteLine("5) Konzole musí používat UTF-8, .NET defaultně ukládá UTF-8 (diakritika 2 byty).");



            // (25b) 6. Vypiste cetnosti jednotlivych slov v souboru 8.txt do souboru 9.txt ve formatu slovo:cetnost na samostatny radek.
            // Tentokrat vsak slova nejprve ocistete od diakritiky a vsechna pismena berte jako mala (tak je i ukladejte do slovniku).
            // Tip: Vyuzijte slovnik: Dictionary<string, int> slova = new Dictionary<string, int>();
            string text8 = File.ReadAllText("vstupni_soubory/8.txt", Encoding.UTF8);
            string cleaned = RemoveDiacritics(text8).ToLowerInvariant();
            string[] rawWords = cleaned.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> slova = new Dictionary<string, int>();
            foreach (string w in rawWords)
            {
                string word = w.Trim(',', '.', ';', ':', '!', '?');
                if (string.IsNullOrWhiteSpace(word))
                {
                    continue;
                }
                if (slova.ContainsKey(word))
                {
                    slova[word]++;
                }
                else
                {
                    slova[word] = 1;
                }
            }
            string[] output = slova.Select(kv => $"{kv.Key}:{kv.Value}").ToArray();
            File.WriteAllLines("vstupni_soubory/9.txt", output, Encoding.UTF8);
            Console.WriteLine("6) Zapsáno do 9.txt");


            // (+15b) Bonus: Vypiste cetnosti jednotlivych znaku abecedy (mala a velka pismena) v souboru 8.txt do konzole.
            Dictionary<char, int> znaky = new Dictionary<char, int>();
            foreach (char ch in text8)
            {
                if (char.IsLetter(ch))
                {
                    if (znaky.ContainsKey(ch))
                    {
                        znaky[ch]++;
                    }
                    else
                    {
                        znaky[ch] = 1;
                    }
                }
            }
            Console.WriteLine("Bonus) Četnosti písmen:");
            foreach (var pair in znaky.OrderBy(p => p.Key))
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            #endregion
        }

        private static void PrintLineSeparators(string path, string fileName)
        {
            byte[] bytes = File.ReadAllBytes(path);
            List<byte> found = new List<byte>();
            foreach (byte b in bytes)
            {
                if (b == '\n' || b == '\r')
                {
                    if (!found.Contains(b))
                    {
                        found.Add(b);
                    }
                }
            }
            Console.WriteLine($"3) Oddělovače v {fileName}: {string.Join(", ", found.Select(f => ((int)f).ToString()))}");
        }

        private static string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
