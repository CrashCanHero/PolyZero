using Godot;
using System;

[GlobalClass]
public partial class Area3DMod : Node {
    public override void _Ready() {
        base._Ready();

        if (GetParent() is not Area3D) {
            GD.PushError("Area 3D mods require to be children of Area3Ds!");
            return;
        }

        ((Area3D)GetParent()).BodyEntered += CharacterEntered;
        ((Area3D)GetParent()).BodyExited += CharacterExited;
    }

    void CharacterEntered(Node3D node) {
        if (node is not CharacterBody3D) {
            return;
        }

        OnBodyEntered((CharacterBody3D)node);
    }

    void CharacterExited(Node3D node) {
        if (node is not CharacterBody3D) {
            return;
        }

        OnBodyExited((CharacterBody3D)node);
    }

    protected virtual void OnBodyEntered(CharacterBody3D body) { }
    protected virtual void OnBodyExited(CharacterBody3D body) { }
}
