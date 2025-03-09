using Godot;

[Tool]
[GlobalClass]
public partial class EZCam2DArea : Area2D
{
    public enum EZCam2DAreaBehavior
    {
        Activate,
        Deactivate,
        DoNothing,
    }

    [ExportToolButton("Copy position from shape", Icon = "ActionCopy")]
    public Callable CopyPositionFromShapeButton => Callable.From(CopyPositionFromShape);

    [ExportToolButton("Copy limits from shape", Icon = "ActionCopy")]
    public Callable CopyLimitsFromShapeButton => Callable.From(CopyLimitsFromShape);

    [ExportToolButton("Fit zoom to width", Icon = "MirrorX")]
    public Callable FitZoomToWidthButton => Callable.From(FitZoomToWidth);

    [ExportToolButton("Fit zoom to height", Icon = "MirrorY")]
    public Callable FitZoomToHeightButton => Callable.From(FitZoomToHeight);

    [Export]
    public EZCam2DCameraSettings AreaEZCam2DCameraSettings = new();

    [Export]
    EZCam2DAreaBehavior EnterBehavior = EZCam2DAreaBehavior.Activate;

    [Export]
    EZCam2DAreaBehavior ExitBehavior = EZCam2DAreaBehavior.Deactivate;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    void OnBodyEntered(Node2D body)
    {
        switch (EnterBehavior)
        {
            case EZCam2DAreaBehavior.Activate:
            {
                EZCam2D.SetEZCam2DCameraSettings(AreaEZCam2DCameraSettings);
                break;
            }
            case EZCam2DAreaBehavior.Deactivate:
            {
                EZCam2D.RemoveEZCam2DCameraSettings(AreaEZCam2DCameraSettings);
                break;
            }
            default:
                break;
        }
    }

    void OnBodyExited(Node2D body)
    {
        switch (ExitBehavior)
        {
            case EZCam2DAreaBehavior.Activate:
            {
                EZCam2D.SetEZCam2DCameraSettings(AreaEZCam2DCameraSettings);
                break;
            }
            case EZCam2DAreaBehavior.Deactivate:
            {
                EZCam2D.RemoveEZCam2DCameraSettings(AreaEZCam2DCameraSettings);
                break;
            }
            default:
                break;
        }
    }

    public void CopyLimitsFromShape()
    {
        CollisionShape2D collisionShape2D = GetChild<CollisionShape2D>(0);
        Shape2D shape2D = collisionShape2D.Shape;
        Rect2 rect2 = shape2D.GetRect();
        Vector2 offset = collisionShape2D.GlobalPosition;

        AreaEZCam2DCameraSettings.LimitLeft = offset.X + rect2.Position.X;
        AreaEZCam2DCameraSettings.LimitUp = offset.Y + rect2.Position.Y;
        AreaEZCam2DCameraSettings.LimitRight = offset.X + rect2.Size.X + rect2.Position.X;
        AreaEZCam2DCameraSettings.LimitDown = offset.Y + rect2.Size.Y + rect2.Position.Y;
    }

    public void CopyPositionFromShape()
    {
        CollisionShape2D collisionShape2D = GetChild<CollisionShape2D>(0);
        Vector2 offset = collisionShape2D.GlobalPosition;

        AreaEZCam2DCameraSettings.MovementStaticPosition = offset;
    }

    public void FitZoomToWidth()
    {
        CollisionShape2D collisionShape2D = GetChild<CollisionShape2D>(0);
        Shape2D shape2D = collisionShape2D.Shape;
        Rect2 rect2 = shape2D.GetRect();

        float newZoom = GetViewportRect().Size.X / rect2.Size.X;
        AreaEZCam2DCameraSettings.Zoom = new(newZoom, newZoom);
    }

    public void FitZoomToHeight()
    {
        CollisionShape2D collisionShape2D = GetChild<CollisionShape2D>(0);
        Shape2D shape2D = collisionShape2D.Shape;
        Rect2 rect2 = shape2D.GetRect();

        float newZoom = GetViewportRect().Size.Y / rect2.Size.Y;
        AreaEZCam2DCameraSettings.Zoom = new(newZoom, newZoom);
    }
}
