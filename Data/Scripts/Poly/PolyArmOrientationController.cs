using Godot;
using System;

public partial class PolyArmOrientationController : Node3D {
    Node3D visual;

    Vector3 lastPos;

    public override void _Ready() {
        base._Ready();

        visual = GetNode<Node3D>("Visual");
        lastPos = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        Vector3 posDelta = GlobalPosition - lastPos;
        posDelta = posDelta.Normalized();

        if (posDelta.Length() <= 0f) {
            return;
        }
        visual.RotationDegrees = Vector3.Zero;
        visual.LookAt(posDelta);

        lastPos = GlobalPosition;
    }
}
