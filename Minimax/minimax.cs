using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniMax_Nim_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialPiles = new List<int> { 2, 2};
            bool botStarts = true;

            var game = new NimGame(initialPiles, botStarts);
            GameState state;

            do
            {
                state = game.PlayTurn();
            } while (state == GameState.Ongoing);


            if (state == GameState.BotWon)
                Console.WriteLine("Vyhrál počítač!");
            else
                Console.WriteLine("Gratulujeme! Vyhráli jste!");
        }
    }

    public enum GameState
    {
        Ongoing,
        BotWon,
        HumanWon
    }

    public class NimGameState
    {
        public List<int> Piles { get; private set; }
        public int MatchesInGame { get; private set; }

        public NimGameState(IEnumerable<int> initialPiles)
        {
            Piles = new List<int>(initialPiles);
            MatchesInGame = Piles.Sum();
        }

        public void MakeMove(int pileIndex, byte matchesToRemove)
        {
            if (IsValidMove(pileIndex, matchesToRemove))
            {
                Piles[pileIndex] -= matchesToRemove;
                MatchesInGame -= matchesToRemove;
            }
            else
            {
                throw new ArgumentException("Neplatný tah!");
            }
        }

        private bool IsValidMove(int pileIndex, byte matchesToRemove)
        {
            return pileIndex >= 0 &&
                   pileIndex < Piles.Count &&
                   Piles[pileIndex] >= matchesToRemove &&
                   matchesToRemove > 0;
        }
    }

    public class NimGame
    {
        private NimGameState _state; // pro privátní datové položky používáme podtržítko na začátku jména
        private bool _botStarts;
        private bool _isBotTurn;

        public NimGame(List<int> initialPiles, bool botStarts)
        {
            _state = new NimGameState(initialPiles);
            _botStarts = botStarts;
            _isBotTurn = botStarts;
        }

        public GameState PlayTurn()
        {
            PrintGameState();

            if (_isBotTurn)
            {
                var botMove = GetBestBotMove();
                MakeAndPrintBotMove(botMove);
            }
            else
            {
                var humanMove = GetHumanInput();
                _state.MakeMove(humanMove.Item1, humanMove.Item2);
            }

            _isBotTurn = !_isBotTurn;

            if (_state.MatchesInGame == 0)
            	if (_isBotTurn)
								return GameState.BotWon;
							else
								return GameState.HumanWon;
						else 
            	return GameState.Ongoing;
        }

        // Vybere nejlepsi tah pro pocitac podle minimaxu.
        private Tuple<int, byte> GetBestBotMove()
        {
            int bestPileIndex = -1;
            byte bestMatchesToRemove = 1;
            int bestScore = int.MinValue;

            // Projdeme vsechny mozne tahy (pro kazdou hromadku lze odebrat 1 az N sirek).
            for (int i = 0; i < _state.Piles.Count; i++)
            {
                int pileSize = _state.Piles[i];
                if (pileSize == 0)
                    continue;

                for (int matches = 1; matches <= pileSize; matches++)
                {
                    var newPiles = new List<int>(_state.Piles);
                    newPiles[i] -= matches;

                    // Po tahu nasleduje hrac proto volame minimax s minimizing hracem.
                    int score = Minimax(newPiles, isBotTurn: false);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPileIndex = i;
                        bestMatchesToRemove = (byte)matches;
                    }
                }
            }

            if (bestPileIndex == -1)
                throw new InvalidOperationException("Bot nema zadny platny tah.");

            return new Tuple<int, byte>(bestPileIndex, bestMatchesToRemove);
        }

        /// Minimax algoritmus nad stavem hry. Rekurzivne uvazuje vsechny mozne tahy.
        /// Kladne skore znamena vyhodu pro pocitac, zaporne pro hrace.
        private int Minimax(List<int> piles, bool isBotTurn)
        {
            bool noMatchesLeft = piles.All(p => p == 0);
            if (noMatchesLeft)
            {
                // Souper prave vzal posledni sirku, takye aktualni hrac vyhral.
                return isBotTurn ? 1 : -1;
            }

            if (isBotTurn)
            {
                int maxEval = int.MinValue;
                for (int i = 0; i < piles.Count; i++)
                {
                    if (piles[i] == 0)
                        continue;

                    for (int matches = 1; matches <= piles[i]; matches++)
                    {
                        var newPiles = new List<int>(piles);
                        newPiles[i] -= matches;
                        maxEval = Math.Max(maxEval, Minimax(newPiles, isBotTurn: false));
                    }
                }

                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                for (int i = 0; i < piles.Count; i++)
                {
                    if (piles[i] == 0)
                        continue;

                    for (int matches = 1; matches <= piles[i]; matches++)
                    {
                        var newPiles = new List<int>(piles);
                        newPiles[i] -= matches;
                        minEval = Math.Min(minEval, Minimax(newPiles, isBotTurn: true));
                    }
                }

                return minEval;
            }
        }

        private void PrintGameState()
        {
            Console.WriteLine("Aktuální stav hry:");
            foreach (var pile in _state.Piles)
                Console.Write(pile + " ");
            Console.WriteLine();
        }

        private void MakeAndPrintBotMove(Tuple<int, byte> move)
        {
            _state.MakeMove(move.Item1, move.Item2);
            Console.WriteLine($"Počítač bere {move.Item2} sirky z hromádky {move.Item1}");
        }

        private Tuple<int, byte> GetHumanInput()
        {

            Console.Write("Z které hromádky chcete brát? (");
            for (int i = 0; i < _state.Piles.Count; i++)
            {
                if (_state.Piles[i] > 0)
                    Console.Write($"{i} ");
            }   
            Console.Write(")");

            int pileIndex = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Kolik sirek chcete vzít? (1-{_state.Piles[pileIndex]})");
            byte matches = Convert.ToByte(Console.ReadLine());

            return new Tuple<int, byte>(pileIndex, matches);
        }
    }

    
}
