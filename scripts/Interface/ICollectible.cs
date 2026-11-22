
using Godot;
using System;

// Интерфейс для собираемых объектов.
public interface ICollectible : IInteractable
{
    // Подбор объекта.
    void Collect(Node collectObject)
    {

    }
}
