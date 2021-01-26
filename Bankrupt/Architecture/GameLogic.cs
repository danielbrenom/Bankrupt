using System;
using System.Linq;
using Bankrupt.Enums;
using Bankrupt.Extensions;
using Bankrupt.Models;

namespace Bankrupt.Architecture
{
    public class GameLogic
    {
        private const int MaxTurns = 1000;
        private GameSession Session { get; set; }
        private int CurrentPlayer { get; set; } = 0;
        private bool GameEnded { get; set; }
        public GameLogic()
        {
            Session = new GameSession();
        }

        public GameSession RunGame()
        {
            while (Session.TurnCount < MaxTurns && !GameEnded)
            {
                var diceResult = DiceRoller.Roll();
                Session.Players[CurrentPlayer].SetNewPosition(diceResult);
                BuyPayProperty(Session.Players[CurrentPlayer], Session.Properties[Session.Players[CurrentPlayer].PlayerPosition]);
                CheckBankruptcy(Session.Players[CurrentPlayer]);
                CheckGameEnd();
                Session.TurnCount++;
                if (CurrentPlayer >= Session.Players.Count - 1)
                    CurrentPlayer = 0;
                else
                    CurrentPlayer++;
            }

            return Session;
        }

        private void BuyPayProperty(Player player, Property property)
        {
            if (property.OwnerId.IsEmptyNullWhitespace())
            {
                if (!PlayerDecision(player, property)) return;
                Session.Properties[property.Order].OwnerId = player.Id;
                Session.Players[Session.Players.IndexOf(player)].Coins -= property.BuyValue;
            }
            else
            {
                var owner = Session.Players.First(p => p.Id == property.OwnerId);
                if(owner.Id == player.Id) return;
                var canPayRent = Session.Players[Session.Players.IndexOf(player)].Coins - property.RentValue >= 0;
                if (canPayRent)
                {
                    Session.Players[Session.Players.IndexOf(player)].Coins -= property.RentValue;
                    Session.Players[Session.Players.IndexOf(owner)].Coins += property.RentValue;
                }
                else
                {
                    Session.Players[Session.Players.IndexOf(owner)].Coins += Session.Players[Session.Players.IndexOf(player)].Coins;
                    Session.Players[Session.Players.IndexOf(player)].Coins = 0;
                }
            }
        }

        private void CheckBankruptcy(Player player)
        {
            if (player.Coins > 0) return;
            Session.Properties.Where(p => p.OwnerId == player.Id).ToList().ForEach(property =>
            {
                Session.Properties[Session.Properties.IndexOf(property)].OwnerId = string.Empty; 
            });
            Session.Players.Remove(player);
        }

        private void CheckGameEnd()
        {
            if (Session.Players.Count != 1)
            {
                if (Session.TurnCount + 1 < MaxTurns) return;
                var highestAmount = Session.Players.Max(player => player.Coins);
                var possibleWinners = Session.Players.Where(player => player.Coins == highestAmount);
                Session.Winner = possibleWinners.Last();
            }
            else
            {
                Session.Winner = Session.Players.First();
                GameEnded = true;
            }
        }

        private bool PlayerDecision(Player player, Property property)
        {
            return player.Strategy switch
            {
                PlayerStrategy.Impulsive => player.Coins >= property.BuyValue,
                PlayerStrategy.Demanding => player.Coins >= property.BuyValue && property.RentValue >= 50,
                PlayerStrategy.Cautious => player.Coins - property.BuyValue >= 80,
                PlayerStrategy.Random => ThreadSafeRandom.ThisThreadsRandom.Next(0,2) > 0,
                _ => false
            };
        }

        private int SetPosition(Player player, int adder)
        {
            return player.PlayerPosition + adder > 19 ? player.PlayerPosition - adder : player.PlayerPosition + adder;
        }
    }
}