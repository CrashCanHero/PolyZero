using Godot;
using System;

public partial class PolyWalljumpController : Node {
    [ExportGroup("Components")]
    [Export]
    public CharacterBody3D Body;

    [Export]
    public Node Move;

    [Export]
    public Resource Parameters;

    public override void _Process(double delta){
        base._Process(delta);
        if (!Input.IsActionJustPressed("pm_jump") || !Body.IsOnWall() || Body.IsOnFloor()) {
            return;
        }

        Move.Call("_airaccelerate", delta, Body.GetWallNormal(), float.MaxValue, float.MaxValue);
    }
}
