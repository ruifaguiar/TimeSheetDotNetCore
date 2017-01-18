using System;
using System.Collections.Generic;
using System.Linq;
using TimeSheet;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var files = new[] { "Post1_3h30_2_novembro.txt", "Post2_3h30_3_novembro.txt", "Post3_3h31_4_novembro.txt", "Post4_3h43_5_novembro.txt", "Post5_3h30_6_novembro.txt" };
            var passatempo = new IEnumerable<Pessoa>[files.Length];
            var postedDates = new DateTime[]
            {
                new DateTime(2016, 11, 2, 15, 30, 00), new DateTime(2016, 11, 3, 15, 30, 00) ,
                new DateTime(2016, 11, 4, 15, 31, 00), new DateTime(2016, 11, 5, 15, 43, 00),new DateTime(2016, 11, 6, 15, 30, 00)
            };
            var parser = new FileParser();
            var responses = new[] { "oris", "longines", "Tissot", "Gucci", "Swatch" };

            for (var i = 0; i < files.Length; i++)
            {
                passatempo[i] = parser.ReadFile(files[i], postedDates[i]).Where(a => a.Response.ToLowerInvariant().Contains(responses[i].ToLowerInvariant())).ToList();

            }

            var names = new List<string>();

            foreach (var pessoa in passatempo[0])
            {

                if (passatempo[1].Any(a => string.Equals(a.Name, pessoa.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                    passatempo[2].Any(a => string.Equals(a.Name, pessoa.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                    passatempo[3].Any(a => string.Equals(a.Name, pessoa.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                    passatempo[4].Any(a => string.Equals(a.Name, pessoa.Name, StringComparison.CurrentCultureIgnoreCase))
                    )
                {
                    names.Add(pessoa.Name);
                }
            }
            var finalList = new List<Pessoa>();


            foreach (string name in names)
            {
                var participacoes = passatempo.Select(a => a.FirstOrDefault(b => b.Name.ToLowerInvariant().Contains(name.ToLowerInvariant()))).ToList();
                var timeSpent = participacoes.ToList().Aggregate(new TimeSpan(0), (p, v) => p.Add(v.TimeSpent));
                var pessoa = new Pessoa
                {
                    Name = name,
                    TimeSpent = timeSpent
                };
                finalList.Add(pessoa);
                
            }
            foreach (var pessoa in finalList.OrderBy(a=>a.TimeSpent))
            {
                Console.WriteLine($"O participante {pessoa.Name} respondeu acertadamente todos os dias com um tempo total de {pessoa.TimeSpent}");
            }
            

            Console.ReadLine();
        }
    }
}
