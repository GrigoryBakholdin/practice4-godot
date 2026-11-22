
using Godot;
using System;

// Дверь, которую можно открыть, закрыть и уничтожить.
[GlobalClass]
public partial class Door : CharacterBody3D, IDamageable, IInteractable
{
    [Export]
    public float Health { get; set; } = 50f;

    [Export]
    public string TooltipText { get; set; } = "Press E to open";

    public bool Opened = false;

    [Export]
    private Node _parent;

    // Открытие и закрытие двери.
    public void Interact(Node interactingObject)
    {
        if (Opened)
        {
            TooltipText = "Press E to open";
            _parent.GetNode<AnimationPlayer>("AnimationPlayer").Play("Close");
        }
        else
        {
            TooltipText = "Press E to close";
            _parent.GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
        }

        Opened = !Opened;
    }

    // Получение урона.
    public void ReceiveDamage(float damage)
    {
        GD.Print("door get damage");
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    // Уничтожение двери.
    public void Die()
    {
        _parent.QueueFree();
    }
}
