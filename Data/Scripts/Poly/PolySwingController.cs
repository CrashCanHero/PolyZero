using Godot;
using System;
using System.Runtime.Intrinsics.Arm;

public partial class PolySwingController : Node {
    [ExportCategory("Components")]
    [Export]
    public CharacterBody3D Body;

    [Export]
    public Node MovementHandler;

    [Export]
    public ShapeCast3D Raycaster;

    [Export]
    public Control LeftArmUI, RightArmUI;

    [ExportCategory("Pivots")]
    [Export]
    public Node3D LeftArmPivot;

    [Export]
    public Node3D RightArmPivot;

    [Export]
    public Node3D ActivePivot;

    [ExportCategory("Arms")]
    [Export]
    public PolyArmStateHandler LeftArm;

    [Export]
    public PolyArmStateHandler RightArm;

    [Export]
    public float RetractionSpeed;

    bool hasTarget => Raycaster.IsColliding();
    Vector3 getPosition => Raycaster.GetCollisionPoint(0);

    float leftArmDist, rightArmDist;

    [Signal]
    public delegate void OnArmAttachEventHandler(Vector3 pos);
    Vector3 posCache;
    Vector3 leftNormCache, rightNormCache;

    public bool RequestArmThrow(PolyArmStateHandler arm, Node3D pivot) {
        if (!hasTarget || arm.Active) {
            return false;
        }

        ThrowArm(arm);
        return true;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        LeftArmUI.Visible = !LeftArm.Visible;
        RightArmUI.Visible = !RightArm.Visible;

        if (Input.IsActionJustReleased("Retract")) {
            RetractArm(LeftArm, LeftArmPivot);
        }


        if (Input.IsActionJustPressed("LeftArmThrow") && RequestArmThrow(LeftArm, LeftArmPivot)) {
            leftArmDist = (LeftArm.GlobalPosition - Body.GlobalPosition).Length();
            leftNormCache = getPosition;
        }

        if (Input.IsActionJustReleased("Retract")) {
            RetractArm(RightArm, RightArmPivot);
        }

        if (Input.IsActionJustPressed("RightArmThrow") && RequestArmThrow(RightArm, RightArmPivot)) {
            rightArmDist = (RightArm.GlobalPosition - Body.GlobalPosition).Length();
            rightNormCache = getPosition;
        }

        if (Input.IsActionPressed("LeftArmThrow") && LeftArm.Active) {
            leftArmDist = (leftArmDist - (RetractionSpeed * (float)delta)).Clamp(0.1f, leftArmDist);
            posCache += Vector3.One;
        }

        if (Input.IsActionPressed("RightArmThrow") && RightArm.Active) {
            rightArmDist = (rightArmDist - (RetractionSpeed * (float)delta)).Clamp(0.1f, rightArmDist);
            posCache += Vector3.One;
        }

        if (posCache == Body.GlobalPosition) {
            return;
        }

        bool grounded = Body.IsOnFloor();

        //We don't wanna cap our movement speed here since we're simulating an outside force
        string funcName = "_airaccelerate";

        if (grounded) {
            MovementHandler.Call("_friction", delta, 1f);
        }

        if (LeftArm.Active) {
            DebugDraw3D.DrawArrow(Body.GlobalPosition, Body.GlobalPosition + (LeftArm.GlobalPosition - Body.GlobalPosition).Normalized(), new Color(1f, 1f, 0f, 0.1f), 0.1f);
            Vector3 dir = LeftArm.GlobalPosition - Body.GlobalPosition;
            float ang = dir.Normalized().AngleTo(leftNormCache);

            if (dir.Length() > leftArmDist) {
                MovementHandler.Call(funcName, delta, dir.Normalized(), Mathf.Sin(ang), float.MaxValue);
            }
        }

        if (RightArm.Active) {
            DebugDraw3D.DrawArrow(Body.GlobalPosition, Body.GlobalPosition + (RightArm.GlobalPosition - Body.GlobalPosition).Normalized(), new Color(0f, 1f, 1f, 0.1f), 0.1f);
            Vector3 dir = RightArm.GlobalPosition - Body.GlobalPosition;
            float ang = dir.Normalized().AngleTo(rightNormCache);

            if (dir.Length() > rightArmDist) {
                MovementHandler.Call(funcName, delta, dir.Normalized(), Mathf.Sin(ang), float.MaxValue);
            }
        }

        posCache = Body.GlobalPosition;
    }

    public void RetractArm(PolyArmStateHandler arm, Node3D pivot) {
        ResetArmPosition(arm, pivot);

        if (arm.Active) {
            arm.Active = false;
            return;
        }
    }

    static void ResetArmPosition(PolyArmStateHandler arm, Node3D pivot) {
        arm.GetParent().RemoveChild(arm);
        pivot.AddChild(arm);
        arm.Position = Vector3.Zero;
        arm.RotationDegrees = Vector3.Zero;
        arm.Visible = false;
    }

    void ThrowArm(PolyArmStateHandler arm) {
        Vector3 pos = getPosition;

        arm.GetParent().RemoveChild(arm);
        ActivePivot.AddChild(arm);

        arm.LookAt(pos - arm.GlobalPosition);
        arm.GlobalPosition = pos;
        arm.Visible = true;
        arm.Active = true;
        EmitSignal(SignalName.OnArmAttach, pos);
    }
}
