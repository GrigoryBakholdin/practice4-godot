
using Godot;
using System;

// Записка, которую можно прочитать и подобрать.
[GlobalClass]
public partial class Note : StaticBody3D, IReadable, ICollectible
{
    [Export]
    public string TooltipText { get; set; } = "Press E to read";

    [Export(PropertyHint.MultilineText)]
    public string Text { get; set; }

    public bool Opened { get; set; }

    // Установка текста записки.
    public override void _Ready()
    {
        GetNode<Label>("CanvasLayer/Label").Text = Text;
    }

    // Открытие интерфейса чтения.
    public void Read(Node readingObject)
    {
        TooltipText = "Press E to pick";

        if (readingObject is Player player)
        {
            player.ShowReadable(this);
        }
    }

    // Переключение состояния записки.
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

    // Закрытие чтения и подбор записки.
    public void Close(Node readingObject)
    {
        if (readingObject is Player player)
        {
            player.CloseReadable(this);
        }

        Collect(readingObject);
    }

    // Добавление записки в инвентарь.
    public void Collect(Node collectObject)
    {
        if (GetTree().GetFirstNodeInGroup("Player") is Player player)
        {
            player.AddToInventory(this);
        }

        GetParent().RemoveChild(this);
    }
}
