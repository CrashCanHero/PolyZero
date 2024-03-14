using Godot;
using System;

public partial class ResetVelocity3DMod : Area3DMod {
    protected override void OnBodyEntered(CharacterBody3D body) {
        base.OnBodyEntered(body);

        body.Velocity = Vector3.Zero;
    }
}
