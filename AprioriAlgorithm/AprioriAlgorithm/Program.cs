using System;
using System.Collections.Generic;
using System.Linq;

namespace Apriori
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sample dataset
            var transactions = new List<List<string>>()
            {
                new List<string> { "A", "B", "C", "D", "E", "F" },
                new List<string> { "A", "C", "D", "E" },
                new List<string> { "C", "D", "E" },
                new List<string> { "B", "F" },
                new List<string> { "A", "B", "C", "D" },
                new List<string> { "A", "B", "E" },
                new List<string> { "A", "B", "C" },
                new List<string> { "A", "B", "E" },
                new List<string> { "A", "C" },
                new List<string> { "A", "C", "D", "E" }
            };

            // Minimum support
            double minSupport = 0.3;

            // Generate frequent itemsets
            var frequentItemsets = Apriori(transactions, minSupport);

            // Print results
            Console.WriteLine("Frequent Itemsets:");
            foreach (var itemset in frequentItemsets)
            {
                Console.WriteLine(string.Join(", ", itemset));
            }
        }

        static List<List<string>> Apriori(List<List<string>> transactions, double minSupport)
        {
            // Get all items
            var allItems = transactions.SelectMany(x => x).Distinct().ToList();

            // Initialize frequent itemsets
            var frequentItemsets = new List<List<string>>();

            // Generate 1-frequent itemsets
            var oneFrequentItemsets = allItems.Where(item => transactions.Count(t => t.Contains(item)) >= minSupport * transactions.Count).Select(item => new List<string> { item }).ToList();
            frequentItemsets.AddRange(oneFrequentItemsets);

            // Generate k-frequent itemsets
            for (int k = 2; k <= 4; k++)
            {
                var candidateItemsets = GenerateCandidateItemsets(frequentItemsets, k);
                var kFrequentItemsets = candidateItemsets.Where(itemset => transactions.Count(t => itemset.All(i => t.Contains(i))) >= minSupport * transactions.Count).ToList();
                frequentItemsets.AddRange(kFrequentItemsets);
            }

            return frequentItemsets;
        }

        static List<List<string>> GenerateCandidateItemsets(List<List<string>> frequentItemsets, int k)
        {
            var candidateItemsets = new List<List<string>>();

            for (int i = 0; i < frequentItemsets.Count; i++)
            {
                for (int j = i + 1; j < frequentItemsets.Count; j++)
                {
                    var itemset1 = frequentItemsets[i];
                    var itemset2 = frequentItemsets[j];

                    if (itemset1.Take(k - 2).SequenceEqual(itemset2.Take(k - 2)))
                    {
                        var candidateItemset = itemset1.Union(itemset2).ToList();
                        candidateItemset.Sort();
                        candidateItemsets.Add(candidateItemset);
                    }
                }
            }

            return candidateItemsets;
        }
    }
}
