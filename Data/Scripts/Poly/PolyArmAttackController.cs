using Godot;
using System;

public partial class PolyArmAttackController : Node {
    [ExportGroup("LeftArm")]
    [Export]
    public CanvasItem LeftArmUI;

    [Export]
    public Node3D LeftArmWorld;

    [Export]
    public Node3D LeftArmPositionController;

    [Export]
    public Node3D LeftArmSpawnPivot;

    [ExportGroup("RightArm")]
    [Export]
    public CanvasItem RightArmUI;

    [Export]
    public Node3D RightArmWorld;

    [Export]
    public Node3D RightArmPositionController;

    [Export]
    public Node3D RightArmSpawnPivot;

    public override void _Process(double delta) {
        base._Process(delta);

        LeftArmUI.Visible = !LeftArmWorld.Visible;
        RightArmUI.Visible = !RightArmWorld.Visible;
    }
}
