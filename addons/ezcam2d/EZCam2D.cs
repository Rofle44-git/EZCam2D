using Godot;

[GlobalClass]
public partial class EZCam2D : Camera2D
{
    public static EZCam2DCameraSettings ActiveEZCam2DCameraSettings;
    public static EZCam2D Inst;
    static Node2D CachedTarget;

    [Export]
    private EZCam2DCameraSettings DefaultEZCam2DCameraSettings = new();

    public EZCam2D()
    {
        Inst ??= this;
    }

    public override void _Ready()
    {
        base._Ready();
        ActiveEZCam2DCameraSettings ??= Inst.DefaultEZCam2DCameraSettings;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        switch (ActiveEZCam2DCameraSettings.MovementType)
        {
            case EZCam2DCameraSettings.EZCam2DMovementType.Autoscroll:
            {
                Position += ActiveEZCam2DCameraSettings.MovementAutoscrollSpeed * (float)delta;
                break;
            }
            case EZCam2DCameraSettings.EZCam2DMovementType.FollowTarget:
            {
                Position = CachedTarget.Position;
                break;
            }
            default:
                break;
        }
    }

    void SetCachedCamera(EZCam2DCameraSettings EZCam2DCameraSettings)
    {
        if (GetNode<Node2D>(EZCam2DCameraSettings.MovementTarget) is Node2D target)
        {
            CachedTarget = target;
        }
    }

    public static void SetEZCam2DCameraSettings(EZCam2DCameraSettings newEZCam2DCameraSettings)
    {
        if (newEZCam2DCameraSettings.Priority < ActiveEZCam2DCameraSettings.Priority)
            return;

        ActiveEZCam2DCameraSettings = newEZCam2DCameraSettings;
        Inst.InterpolateZoom(
            newEZCam2DCameraSettings.Zoom == Vector2.Zero
                ? Inst.Zoom
                : newEZCam2DCameraSettings.Zoom
        );
        Inst.InterpolatePosition(newEZCam2DCameraSettings.MovementStaticPosition);

        Inst.LimitTop = Inst.LimitTop == -1f ? -10000000 : Inst.LimitTop;
        Inst.LimitLeft = Inst.LimitLeft == -1f ? -10000000 : Inst.LimitLeft;
        Inst.LimitRight = Inst.LimitRight == -1f ? 10000000 : Inst.LimitRight;
        Inst.LimitBottom = Inst.LimitBottom == -1f ? 10000000 : Inst.LimitBottom;

        if (
            newEZCam2DCameraSettings.MovementType
            == EZCam2DCameraSettings.EZCam2DMovementType.FollowTarget
        )
            Inst.SetCachedCamera(newEZCam2DCameraSettings);
    }

    public static void RemoveEZCam2DCameraSettings(EZCam2DCameraSettings newEZCam2DCameraSettings)
    {
        if (newEZCam2DCameraSettings.Priority < ActiveEZCam2DCameraSettings.Priority)
            return;
        ActiveEZCam2DCameraSettings = Inst.DefaultEZCam2DCameraSettings;

        Inst.InterpolatePosition(Inst.DefaultEZCam2DCameraSettings.MovementStaticPosition);
        Inst.InterpolateZoom(Inst.DefaultEZCam2DCameraSettings.Zoom);
    }

    void InterpolateZoom(Vector2 to)
    {
        CreateTween()
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic)
            .TweenProperty(this, new("zoom"), to, 0.7d);
    }

    void InterpolatePosition(Vector2 to)
    {
        CreateTween()
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic)
            .TweenProperty(this, new("position"), to, 0.7d);
    }
}
