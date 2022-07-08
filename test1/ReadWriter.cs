using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace test1
{
    class ReadWriter
    {
        private static readonly Regex regex = new Regex(
              @"( [^\W_\d]              # starting with a letter
                            # followed by a run of either...
      ( [^\W_\d] |          #   more letters or
        [-'\d](?=[^\W_\d])  #   ', -, or digit followed by a letter
      )*
      [^\W_\d]              # and finishing with a letter
    )",
              RegexOptions.IgnorePatternWhitespace);

        

        private ConcurrentDictionary<string, int> dictionary;
        private object locker = new object();
        private void ProcessLine(string line)
        {
            line = line.Trim();
            line = line.ToLower();
            lock(locker)
            {
                foreach (var word in from Match match in regex.Matches(line)
                                        let word = match.Groups[1].Value
                                        select word)
                {
                    if (dictionary.ContainsKey(word))
                        dictionary[word]++;
                    else
                        dictionary.TryAdd(word, 1);
                }
            }
                
        }


        public void ReadAsync(string path_read, string path_write)
        {
            dictionary = new ConcurrentDictionary<string, int>();

            File.ReadLines(path_read).AsParallel().WithDegreeOfParallelism(10).OfType<string>().ForAll(x => ProcessLine(x));


            Write(path_write);
        }
        private void ShowDictionary()
        {
            foreach (var item in dictionary.OrderByDescending(pair => pair.Value))
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
        }

        public void Read(string path_read, string path_write)
        {
            dictionary = new ConcurrentDictionary<string, int>();
            using (StreamReader reader = new StreamReader(path_read))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    ProcessLine(line);
                }
                
            }
            Write(path_write);
        }

        private void Write(string path_write)
        {
            using (StreamWriter writer = new StreamWriter(path_write, true))
            {
                foreach (var item in dictionary.OrderByDescending(pair => pair.Value))
                {
                    writer.WriteLine(item.Key + " " + item.Value);
                }
            }
        }

    }
}
