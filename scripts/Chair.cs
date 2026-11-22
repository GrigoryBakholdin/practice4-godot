
using Godot;
using System;

// Стул, который можно уничтожить.
public partial class Chair : StaticBody3D, IDamageable
{
    [Export]
    public string TooltipText { get; set; } = "";

    [Export]
    public float Health { get; set; } = 10f;

    // Получение урона.
    public void ReceiveDamage(float damage)
    {
        GD.Print("get damage");
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    // Уничтожение стула.
    public void Die()
    {
        QueueFree();
    }
}
