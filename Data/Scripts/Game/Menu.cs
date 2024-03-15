using Godot;
using System;

public partial class Menu : Control {
    [Export]
    public Node3D SecretButton;


    public override void _Ready() {
        base._Ready();

        Input.MouseMode = Input.MouseModeEnum.Visible;
        if (GetTree().Root.HasMeta("Secret")) {
            SecretButton.Visible = true;
        }
    }

    public void PlayClicked() => SceneLoader.Instance.LoadSceneIndex(1);

    public void QuitClicked() => GetTree().Quit();

    public void SecretClicked() => SceneLoader.Instance.LoadSceneIndex(2);
}
