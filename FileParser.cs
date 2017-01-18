using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TimeSheet
{
    public class FileParser
    {
        public IEnumerable<Pessoa> ReadFile(string filepath, DateTime postedDate)
        {
            string prefix="Files";
            List<Pessoa> pessoas = new List<Pessoa>();
            FileStream fileStream = new FileStream(Path.Combine(prefix,filepath), FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                int i = 1;
                Pessoa pessoa = null;
                while ((line = reader.ReadLine()) != null)
                {
                    switch (i)
                    {
                        case 1:
                            {
                                pessoa = new Pessoa { Name = ParseName(line) };
                                break;
                            }
                        case 2:
                            {
                                pessoa.Response = PaserAnswer(line, pessoa);
                                break;
                            }
                        case 3:
                            {
                                pessoa.TimeSpent = ParseDate(line) - postedDate;
                                pessoas.Add(pessoa);
                                i = 0;
                                break;
                            }
                    }
                    i++;
                }
            }
            return pessoas;
        }

        private string ParseName(string line)
        {
            return line;
        }

        private string PaserAnswer(string line, Pessoa pessoa)
        {
            var resposta = line.Replace(pessoa.Name, "");
            return resposta.Trim();

        }

        private DateTime ParseDate(string line)
        {
            line = line.Replace("at", "");
            var dateParts = line.Split(' ');

            int month = DateTime.ParseExact(dateParts[0], "MMMM", CultureInfo.CurrentCulture).Month;
            int day = int.Parse(dateParts[1]);
            int hour = DateTime.ParseExact(dateParts[3], "h:mmtt", CultureInfo.CurrentCulture).Hour;
            int minute = DateTime.ParseExact(dateParts[3], "h:mmtt", CultureInfo.CurrentCulture).Minute;

            return new DateTime(2016, month, day, hour, minute, 0);
        }
    }
}