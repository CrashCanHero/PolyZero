using Godot;
using System;

public partial class AutoDestroy : Area3DMod {
    protected override void OnBodyEntered(CharacterBody3D body) {
        base.OnBodyEntered(body);

        GetParent().QueueFree();
    }
}
