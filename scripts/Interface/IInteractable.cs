using Godot;
using System;
// Интерфейс для взаимодействия с объектами.
public interface IInteractable
{
    // Название объекта.
    string TooltipText { get; set; }

    // Взаимодействие с объектом.
    void Interact(Node interactingObject)
    {

    }
}
