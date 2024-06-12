// Подключение необходимого пространства имен для работы с Hand
namespace Cardsw
{
    

    // Класс Player представляет игрока
    public class Player
    {
        public Hand Hand { get; private set; }
        public string Action { get; set; }

        public Player()
        {
            Hand = new Hand();
            Action = string.Empty;
        }

        // Метод Points возвращает общую стоимость карт в руке игрока
        public int Points()
        {
            return Hand.TotalCardValue;
        }
    }

    // Класс PlayerHandler управляет действиями игрока
    public class PlayerHandler
    {
        // Допустимые действия игрока.
        private static readonly string[] ValidActions = {
        "Take", "Q", "T", "q", "t", "+",
        "Pass", "E", "P", "e", "p", "-",
        "Forfeit", "F", "L", "f", "l", "*"
    };

        public Player Player { get; private set; }
        public string LogFile { get; private set; }

        public PlayerHandler(Player player, string logFile)
        {
            Player = player;
            LogFile = logFile;
        }

        // Метод SelectPlayerAction запрашивает у игрока выбор действия
        public void SelectPlayerAction()
        {
            string inputMessage = "Select your action: \n \"Take (Q/T/+)\" \n \"Pass (E/P/-)\"\n \"Forfeit (F/L/*)\"\n";
            Console.Write(inputMessage);
            string action = Console.ReadLine();
            System.IO.File.AppendAllText(LogFile, "\n" + inputMessage + action);

            while (!ValidActions.Contains(action))
            {
                Console.Write("Invalid action. " + inputMessage);
                action = Console.ReadLine();
                System.IO.File.AppendAllText(LogFile, "\n" + "Invalid action. " + inputMessage + action);
            }

            switch (action)
            {
                case "Q":
                case "q":
                case "T":
                case "t":
                case "+":
                    action = "Take";
                    break;
                case "E":
                case "e":
                case "P":
                case "p":
                case "-":
                    action = "Pass";
                    break;
                case "F":
                case "L":
                case "f":
                case "l":
                case "*":
                    action = "Forfeit";
                    break;
            }

            Player.Action = action;
        }
    }

}
