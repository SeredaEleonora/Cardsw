using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardsw
{
    using System;
    using System.IO;



    public class Game
    {
        private string logFile;
        private Player player;
        private PlayerHandler playerHandler;
        private Player dealer;
        private Deck deck;
        private string winner;

        // Конструктор класса Game, который инициализирует игру и создает файл лога
        public Game()
        {
            logFile = $"logs\\log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            player = new Player();
            playerHandler = new PlayerHandler(player, logFile);
            dealer = new Player();
            deck = new Deck();
            winner = null;
        }

        // Метод dealCardTo раздает карты игроку или дилеру
        private void DealCardTo(string name)
        {
            switch (name)
            {
                case "Player":
                    player.Hand.AddCard(deck.TakeCard());
                    break;
                case "Dealer":
                    dealer.Hand.AddCard(deck.TakeCard());
                    break;
            }
        }

        // Метод gameStart начинает новую игру, раздает карты и проверяет наличие победителя
        public void GameStart()
        {
            DealCardTo("Player");
            DealCardTo("Dealer");
            DealCardTo("Player");
            CheckForWinners();
            File.WriteAllText(logFile, ""); // Очистка файла лога перед записью.
            Log($"New game: {DateTime.Now:yyyy-MM-dd_HH-mm-ss}");
        }

        // Метод render выводит текущее состояние игры и записывает его в файл лога
        public void Render()
        {
            string message = new string('^', 60) + "\n";
            message += "Current cards:\n";
            message += $"Dealer: {dealer.Points()} points\n";
            message += $"{dealer.Hand.ArrayDisplay()}\n\n";
            message += $"Player: {player.Points()} points\n";
            message += $"{player.Hand.ArrayDisplay()}\n\n";
            message += $"Last player action: {player.Action}\n";
            message += new string('~', 60);
            Console.WriteLine(message);
            Log(message);
        }

        // Метод checkForWinners проверяет наличие победителя в игре
        private void CheckForWinners()
        {
            if (player.Points() == 21 && dealer.Points() != 21)
            {
                winner = "Player";
            }
            else if (dealer.Points() == 21 && player.Points() != 21)
            {
                winner = "Casino";
            }
            else if (player.Points() > 21 && dealer.Points() < 21)
            {
                winner = "Casino";
            }
            else if (dealer.Points() > 21 && player.Points() < 21)
            {
                winner = "Player";
            }
            else if (player.Points() == 21 && dealer.Points() == 21)
            {
                winner = "Draw";
            }
            else if (player.Points() >= 17 && dealer.Points() < player.Points() && dealer.Points() < 21)
            {
                winner = "Player";
            }
            else if (dealer.Points() > 17 && dealer.Points() == player.Points())
            {
                winner = "Draw";
            }
        }


        // Метод gameCycle выполняет один цикл игры
        public void GameCycle()
        {
            Render();
            playerHandler.SelectPlayerAction();
            if (player.Action == "Forfeit")
            {
                winner = "Casino";
                return;
            }

            if (player.Action == "Take")
            {
                DealCardTo("Player");
            }

            if (dealer.Points() <= 17)
            {
                DealCardTo("Dealer");
            }

            CheckForWinners();
        }

        // Метод gameSet управляет одной полной игрой
        public void GameSet()
        {
            GameStart();
            while (winner == null)
            {
                GameCycle();
            }
            Render();
            string winnerAnnouncement = $"And the winner is: {winner}";
            Console.WriteLine(winnerAnnouncement);
            Log(winnerAnnouncement);
        }

        // Метод gameProcess управляет процессом игры, включая запрос на новую игру
        public void GameProcess()
        {
            bool gameActive = true;
            while (gameActive)
            {
                GameSet();
                Console.Write("Another game? Y/N: ");
                string nextGame = Console.ReadLine();
                Log($"Another game? Y/N: {nextGame}");
                if (nextGame.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    Reset();
                }
                else
                {
                    gameActive = false;
                }
            }
        }

        // Метод reset сбрасывает состояние игры для новой партии
        private void Reset()
        {
            winner = null;
            player.Action = "";
            // Сброс карт игрока и дилера.
            deck.Shuffle();
        }

        // Вспомогательный метод для записи сообщений в файл лога
        private void Log(string message)
        {
            File.AppendAllText(logFile, message + Environment.NewLine);
        }
    }

}
