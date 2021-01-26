using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bankrupt.Architecture;
using Bankrupt.Enums;
using Bankrupt.Models;

namespace Bankrupt
{
    class Program
    {
        private const int SimulationAmount = 300;
        private static readonly int SimulationShardAmount = 5;
        
        private static void Main(string[] args)
        {
            var results = new List<GameResult>();
            var shards = new List<Task>();
            Console.WriteLine("Simulation start");
            if (SimulationAmount % SimulationShardAmount != 0)
                throw new ArgumentOutOfRangeException(nameof(SimulationShardAmount), "Cannot divide simulation into shards, simulation amount must be divisible by shard amount");
            for (var i = 0; i < SimulationShardAmount; i++)
            {
                shards.Add(Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < SimulationAmount/SimulationShardAmount; j++)
                    {
                        var game = new GameLogic();
                        var result = game.RunGame();
                        results.Add(new GameResult{TurnCount = result.TurnCount, WinnerStrategy = result.Winner.Strategy});
                    }
                }));
            }
            Task.WaitAll(shards.ToArray());
            AnalyzeResults(results);
            Console.WriteLine("Simulation end");
        }

        private static void AnalyzeResults(IReadOnlyCollection<GameResult> results)
        {
            var mostWinStrategy = results.GroupBy(r => r.WinnerStrategy)
                                         .OrderByDescending(r => r.Count())
                                         .Take(2)
                                         .Select(r => r.Key)
                                         .First();
            Console.WriteLine($"Games ended by timeout: {results.Count(r => r.TurnCount > 999)}");
            Console.WriteLine($"Turn count average: {(int)results.Average(r => r.TurnCount)}");
            Console.WriteLine($"{PlayerStrategy.Cautious.ToString()} win percentage: {(double)results.Count(r => r.WinnerStrategy == PlayerStrategy.Cautious)/results.Count*100} %");
            Console.WriteLine($"{PlayerStrategy.Demanding.ToString()} win percentage: {(double)results.Count(r => r.WinnerStrategy == PlayerStrategy.Demanding)/results.Count*100} %");
            Console.WriteLine($"{PlayerStrategy.Impulsive.ToString()} win percentage: {(double)results.Count(r => r.WinnerStrategy == PlayerStrategy.Impulsive)/results.Count*100} %");
            Console.WriteLine($"{PlayerStrategy.Random.ToString()} win percentage: {(double)results.Count(r => r.WinnerStrategy == PlayerStrategy.Random)/results.Count*100} %");
            Console.WriteLine($"Strategy with most wins: {mostWinStrategy.ToString()}");
        }
    }
}