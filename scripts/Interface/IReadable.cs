using Godot;
using System;

// Интерфейс для чтения текста игровых объектов.
public interface IReadable : IInteractable
{
    // Текст объекта.
    [Export]
    string Text { get; set; }

    // Открытие интерфейса чтения.
    void Read(Node readingObject)
    {

    }

    // Закрытие интерфейса чтения.
    void Close(Node readingObject)
    {

    }
}

