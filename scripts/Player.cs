
using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

// Управление игроком и взаимодействие с объектами.
[GlobalClass]
public partial class Player : CharacterBody3D
{
    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    [Export]
    private float _speed = 5.0f;

    [Export]
    private float _mouseSensitivity = 0.002f;

    [Export]
    private float _damage = 5.0f;

    [Export]
    private float _cameraRotationLimit = 70.0f;

    [Export]
    private Godot.Collections.Array<Node> _inventory = new Godot.Collections.Array<Node>();

    private bool _stopped = false;
    private IInteractable _currentInteractingObject;

    // Захват курсора при запуске.
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Движение игрока с учётом гравитации.
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        velocity.Y -= _gravity * (float)delta;

        Vector2 input = Input.GetVector("left", "right", "forward", "backward");
        Vector3 movementDirection = Transform.Basis * new Vector3(input.X, 0, input.Y);
        velocity.X = movementDirection.X * _speed;
        velocity.Z = movementDirection.Z * _speed;

        Velocity = velocity;

        if (!_stopped)
        {
            MoveAndSlide();
        }
    }

    // Поиск объекта и обработка взаимодействия.
    public override void _Process(double delta)
    {
        Label label = GetNode<Label>("CanvasLayer/CameraLabel");
        RayCast3D rayCast = GetNode<RayCast3D>("Camera3D/RayCast3D");
        rayCast.ForceRaycastUpdate();

        label.Text = rayCast.GetCollider() is IInteractable interactableCollider
            ? interactableCollider.TooltipText
            : "";

        if (rayCast.GetCollider() is IInteractable interactable && Input.IsActionJustPressed("interact"))
        {
            interactable.Interact(this);
        }

        if (rayCast.GetCollider() is IDamageable damageable && Input.IsActionJustPressed("attack"))
        {
            damageable.ReceiveDamage(_damage);
        }
    }

    // Управление камерой и выход из игры.
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && !_stopped)
        {
            RotateY(-mouseMotion.Relative.X * _mouseSensitivity);

            Camera3D camera = GetNode<Camera3D>("Camera3D");
            camera.RotateX(-mouseMotion.Relative.Y * _mouseSensitivity);

            Vector3 cameraRotation = camera.Rotation;
            cameraRotation.X = Mathf.Clamp(
                cameraRotation.X,
                Mathf.DegToRad(-_cameraRotationLimit),
                Mathf.DegToRad(_cameraRotationLimit)
            );

            camera.Rotation = cameraRotation;
        }

        if (@event.IsActionPressed("quit"))
        {
            GetTree().Quit();
        }
    }

    // Добавление предмета в инвентарь.
    public void AddToInventory(Node nodeToAdd)
    {
        _inventory.Add(nodeToAdd);
        GetNode<Label>("CanvasLayer/LabelItemsNumber").Text = _inventory.Count.ToString();
    }

    // Открытие интерфейса чтения.
    public void ShowReadable(IReadable readable)
    {
        GetNode<Control>("CanvasLayer/ReadMenu").Visible = true;
        GetNode<Label>("CanvasLayer/ReadMenu/PanelContainer/MarginContainer/ScrollContainer/Label").Text = readable.Text;

        Input.MouseMode = Input.MouseModeEnum.Visible;
        _stopped = true;
        _currentInteractingObject = readable;
    }

    // Закрытие интерфейса чтения.
    public void CloseReadable(IReadable readable)
    {
        GetNode<Control>("CanvasLayer/ReadMenu").Visible = false;

        Input.MouseMode = Input.MouseModeEnum.Captured;
        _stopped = false;
    }

    // Обработка нажатия кнопки закрытия.
    public void OnCloseButtonPressed()
    {
        GetNode<Control>("CanvasLayer/ReadMenu").Visible = false;

        Input.MouseMode = Input.MouseModeEnum.Captured;
        _stopped = false;

        if (_currentInteractingObject is IReadable readable)
        {
            readable.Close(this);
        }
    }
}
