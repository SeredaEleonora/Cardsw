using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardsw

{
    public class Card
    {
        // Конструктор класса Card, который создает карту с заданной мастью и номиналом
        public Card(string suit, string name)
        {
            Suit = suit;
            Name = name;

            // Определяем базовое значение карты в зависимости от ее номинала.
            switch (name)
            {
                case "Jack":
                case "Queen":
                case "King":
                    BaseValue = 10;
                    break;
                case "Ace":
                    BaseValue = 11;
                    break;
                default:
                    BaseValue = int.Parse(name);
                    break;
            }
        }

        // Метод ToString возвращает строковое представление карты, например "Ace of Hearts"
        public override string ToString()
        {
            return $"{Name} of {Suit}";
        }

        // Свойства для хранения масти, номинала и базового значения карты
        public string Suit { get; set; }
        public string Name { get; set; }
        public int BaseValue { get; set; }
    }

    public class Deck
    {
        private Card[] cards = new Card[0]; // Инициализация пустого массива для избежания NullReferenceException

        public Deck(bool newDeck = true, bool toShuffle = true)
        {
            if (newDeck)
            {
                cards = CreateDeck();
            }

            if (toShuffle)
            {
                Shuffle();
            }
        }

        private Card[] CreateDeck()
        {
            var deckList = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Spades", "Clubs" };
            foreach (var suit in suits)
            {
                for (int i = 1; i <= 13; i++)
                {
                    string cardName = i switch
                    {
                        1 => "Ace",
                        11 => "Jack",
                        12 => "Queen",
                        13 => "King",
                        _ => i.ToString(),
                    };
                    deckList.Add(new Card(suit, cardName));
                }
            }
            return deckList.ToArray();
        }

        public void Shuffle()
        {
            var random = new Random();
            for (int i = cards.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public void AddCard(Card card)
        {
            Array.Resize(ref cards, cards.Length + 1);
            cards[cards.Length - 1] = card;
        }

        public Card TakeCard()
        {
            if (cards.Length == 0)
            {
                throw new InvalidOperationException("Нет карт для изъятия.");
            }
            var cardToTake = cards[cards.Length - 1];
            Array.Resize(ref cards, cards.Length - 1);
            return cardToTake;
        }

        public override string ToString()
        {
            return string.Join(", ", cards.Select(c => c.ToString()));
        }

        public string ArrayDisplay()
        {
            return string.Join("\n", cards.Select(c => c.ToString()));
        }
    }

    public class Hand : Deck
    {
        // Конструктор класса Hand, который создает пустую руку без новой колоды и перемешивания
        public Hand() : base(newDeck: false, toShuffle: false)
        {
            TotalCardValue = 0;
        }

        // Метод AddCard добавляет карту в руку и обновляет общую стоимость карт
        public new void AddCard(Card card)
        {
            if (card.Name == "Ace" && TotalCardValue + card.BaseValue > 21)
            {
                TotalCardValue += 1;
            }
            else
            {
                TotalCardValue += card.BaseValue;
            }
            base.AddCard(card);
        }

        // Метод TakeCard извлекает карту из руки и обновляет общую стоимость карт
        public new Card TakeCard()
        {
            Card card = base.TakeCard();
            TotalCardValue -= card.BaseValue;
            if (TotalCardValue < 0)
            {
                TotalCardValue = 0;
            }
            return card;
        }

        // Свойство для хранения общей стоимости карт в руке
        public int TotalCardValue { get; private set; }
    }
}
