
using Godot;
using System;

// Интерфейс для объектов, получающих урон.
public interface IDamageable : IInteractable
{
    // Текущее здоровье объекта.
    float Health { get; set; }

    // Получение урона.
    void ReceiveDamage(float damage)
    {

    }

    // Уничтожение объекта.
    void Die()
    {

    }
}
