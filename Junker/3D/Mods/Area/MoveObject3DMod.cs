using Godot;
using System;

public partial class MoveObject3DMod : Area3DMod {
    [Export]
    public Node3D ReturnPosition;

    protected override void OnBodyEntered(CharacterBody3D body) {
        base.OnBodyEntered(body);

        body.GlobalPosition = ReturnPosition.GlobalPosition;
    }
}
