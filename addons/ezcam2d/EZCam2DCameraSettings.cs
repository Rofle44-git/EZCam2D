using Godot;

[GlobalClass]
public partial class EZCam2DCameraSettings : Resource
{
    public enum EZCam2DMovementType
    {
        Autoscroll,
        FollowTarget,
        Static,
    }

    [Export]
    public Vector2 Zoom = Vector2.Zero;

    [Export]
    public int Priority = 0;

    [Export]
    public EZCam2DMovementType MovementType = EZCam2DMovementType.Static;

    [ExportSubgroup("Movement", "Movement")]
    [Export]
    public Vector2 MovementStaticPosition = Vector2.Zero;

    [Export]
    public NodePath MovementTarget;

    [Export]
    public Vector2 MovementAutoscrollSpeed = Vector2.Zero;

    [ExportSubgroup("Limit", "Limit")]
    [Export]
    public float LimitLeft = -1f;

    [Export]
    public float LimitUp = -1f;

    [Export]
    public float LimitRight = -1f;

    [Export]
    public float LimitDown = -1f;
}
