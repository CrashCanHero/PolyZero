using Godot;
using System;

public partial class PolyIntroHandler : Node {
    [ExportGroup("Components")]
    [Export]
    public AutoPathFollower3D PathFollower;

    [Export]
    public Control UI;

    [Export]
    public Button PlayButton, QuitButton;

    public override void _Ready() {
        base._Ready();
        PathFollower.OnPathComplete += IntroDone;
    }

    void PlayClicked() {

    }

    void QuitClicked() => GetTree().Quit();

    void IntroDone() => UI.Visible = true;
}
