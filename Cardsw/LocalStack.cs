using Cardsw;
using System;

// Класс StackItem представляет элемент стека
public class StackItem<T>
{
    public T Item { get; set; }
    public StackItem<T> Next { get; set; }

    public StackItem(T item)
    {
        Item = item;
        Next = null;
    }

    public override string ToString()
    {
        return Item.ToString();
    }
}

// Класс Stack представляет структуру данных стек
public class Stack<T>
{
    public int Length { get; private set; }
    public StackItem<T> FirstItem { get; private set; }

    public Stack()
    {
        Length = 0;
        FirstItem = null;
    }

    public override string ToString()
    {
        var totalString = "";
        var item = FirstItem;
        while (item != null)
        {
            totalString += $"{item}, ";
            item = item.Next;
        }
        return totalString.TrimEnd(',', ' ');
    }

    // Метод Push добавляет новый элемент в начало стека
    public void Push(StackItem<T> newItem)
    {
        if (FirstItem == null)
        {
            FirstItem = newItem;
        }
        else
        {
            newItem.Next = FirstItem;
            FirstItem = newItem;
        }
        Length++;
    }

    // Метод PushAt добавляет новый элемент в стек по указанному индексу
    public void PushAt(StackItem<T> newItem, int index)
    {
        if (index == 0)
        {
            Push(newItem);
            return;
        }
        if (index > Length)
        {
            Console.WriteLine("out_of_bounds push");
            return;
        }

        var current = FirstItem;
        for (int i = 0; i < index - 1; i++)
        {
            current = current.Next;
        }

        newItem.Next = current.Next;
        current.Next = newItem;

        Length++;
    }

    // Метод Pop удаляет и возвращает элемент из начала стека
    public StackItem<T> Pop()
    {
        if (FirstItem == null) throw new InvalidOperationException("Stack is empty");
        var returnedItem = FirstItem;
        FirstItem = returnedItem.Next;
        returnedItem.Next = null;
        Length--;
        return returnedItem;
    }

    // Метод PopAt удаляет и возвращает элемент из стека по указанному индексу
    public StackItem<T> PopAt(int index)
    {
        if (index == 0)
        {
            return Pop();
        }

        var previous = FirstItem;
        var current = FirstItem.Next;
        for (int i = 1; i < index; i++)
        {
            previous = current;
            current = current.Next;
        }

        previous.Next = current.Next;
        current.Next = null;
        Length--;
        return current;
    }

    // Метод IsEmpty проверяет, пуст ли стек
    public bool IsEmpty()
    {
        return Length == 0;
    }

    // Метод Shuffle перемешивает элементы стека в пределах заданного диапазона
    public void Shuffle(Random random, int min, int max)
    {
        int inserts = random.Next(min, max + 1);
        for (int i = 0; i < inserts; i++)
        {
            int randIndex = random.Next(1, Length);
            var item = Pop();
            PushAt(item, randIndex);
        }
    }

    internal void Push(StackItem<Card> stackItem)
    {
        throw new NotImplementedException();
    }
}
