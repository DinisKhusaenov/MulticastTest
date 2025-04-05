using System;
using System.Collections.Generic;

namespace GameLogic.Gameplay.GameLogic
{
    public class ClustersGenerator : IClustersGenerator
    {
        private readonly int _minLength;
        private readonly int _maxLength;
        private readonly Random _random;
        
        public ClustersGenerator(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            _random = new Random();
        }

        public List<string> GetClusterBy(Level data)
        {
            var allClusters = new List<string>();

            foreach (var word in data.Words)
            {
                if (string.IsNullOrEmpty(word) || word.Length < _minLength)
                    continue;

                var clusters = RandomSplit(word);
                allClusters.AddRange(clusters);
            }

            Shuffle(allClusters);

            return allClusters;
        }
        
        private List<string> RandomSplit(string word)
        {
            var result = new List<string>();
            int i = 0;

            while (i < word.Length)
            {
                int remaining = word.Length - i;

                // Если остаток меньше или равен максимальной длине, просто добиваем остаток
                if (remaining <= _maxLength)
                {
                    if (remaining >= _minLength)
                    {
                        result.Add(word.Substring(i, remaining));
                    }
                    else
                    {
                        // Присоединяем к предыдущему кластеру
                        if (result.Count > 0)
                            result[result.Count - 1] += word.Substring(i, remaining);
                        else
                            result.Add(word.Substring(i, remaining)); // на случай, если слово меньше minLength
                    }
                    break;
                }

                int maxPossible = Math.Min(_maxLength, remaining - _minLength);
                int clusterSize = _random.Next(_minLength, maxPossible + 1);

                result.Add(word.Substring(i, clusterSize));
                i += clusterSize;
            }

            return result;
        }
        
        private void Shuffle(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = _random.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}