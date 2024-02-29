using Godot;
using System;

public partial class PolyArmController : Node {
    [ExportGroup("Components")]
    [Export]
    public Resource Parameters;

    [Export]
    public CharacterBody3D Body;

    [ExportGroup("Default")]
    [Export]
    public Sprite2D LeftArm, RightArm;

    [Export]
    public PolyArmMovementData LeftArmData, RightArmData;

    [Export]
    public float Speed;

    public Vector2 Scale = Vector2.One;
    public float SpeedScale = 1f;
    
    double t = 0d;
    float lastVelocity;
    float movementDelta;
    const float pi2 = Mathf.Pi * 2f;

    Vector2 GetMovementPosition(float offset) {
        float sin = Mathf.Sin((float)t * pi2 * Speed * SpeedScale + offset);
        float bounceSin = Mathf.Abs(sin);
        sin += 1f;
        sin /= 2f;

        return new Vector2(sin, bounceSin);
    }

    public override void _Ready() {
        base._Ready();

        LeftArmData.Cache = LeftArm.Position;
        RightArmData.Cache = RightArm.Position;
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (Body == null) {
            return;
        }

        #region Arm Speed Scaling
        float vel = Body.Velocity.Length();
        float max = Parameters.Get("MAX_SPEED").As<float>();

        movementDelta = Mathf.Abs(vel - lastVelocity);
        lastVelocity = vel;

        float percent = vel / max;
        SpeedScale = 1f + percent;

        #endregion

        #region Time Management
        t += delta;
        t %= 1f / (Speed * SpeedScale);
        #endregion

        #region Arm Position Calculation
        Vector2[] positions = LeftArmData.ConstructLimits();
        Vector2 pos = GetMovementPosition(LeftArmData.Offset) * Scale;
        
        LeftArm.Position = new Vector2(
            Mathf.Lerp(positions[0].X, positions[1].X, pos.X),
            Mathf.Lerp(positions[0].Y, positions[1].Y, pos.Y)
        );

        positions = RightArmData.ConstructLimits();
        pos = GetMovementPosition(RightArmData.Offset) * Scale;

        RightArm.Position = new Vector2(
            Mathf.Lerp(positions[0].X, positions[1].X, pos.X),
            Mathf.Lerp(positions[0].Y, positions[1].Y, pos.Y)
        );
        #endregion
    }   
}
