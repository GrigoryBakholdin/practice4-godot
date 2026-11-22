
using Godot;
using System;

// Книга, которую игрок может прочитать.
public partial class Book : StaticBody3D, IReadable
{
    [Export]
    public string TooltipText { get; set; } = "Press E to read";

    [Export(PropertyHint.MultilineText)]
    public string Text { get; set; }

    public bool Opened { get; set; }

    // Установка текста книги.
    public override void _Ready()
    {
        GetNode<Label>("CanvasLayer/Label").Text = Text;
    }

    // Открытие интерфейса чтения.
    public void Read(Node readingObject)
    {
        if (readingObject is Player player)
        {
            player.ShowReadable(this);
        }
    }

    // Переключение состояния книги.
    public void Interact(Node interactingObject)
    {
        if (Opened)
        {
            Close(interactingObject);
        }
        else
        {
            Read(interactingObject);
        }

        Opened = !Opened;
    }

    // Закрытие интерфейса чтения.
    public void Close(Node readingObject)
    {
        if (readingObject is Player player)
        {
            player.CloseReadable(this);
        }
    }
}
