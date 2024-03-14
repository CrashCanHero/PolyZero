using Godot;
using System;

public partial class SceneLoader : Node {
    public static SceneLoader Instance;

    [Export]
    public PackedScene firstScene;

    Node LoadedScene;

    public override void _Ready() {
        base._Ready();

        if (Instance != null && Instance != this) {
            QueueFree();
            return;
        }
        Instance = this;

        Node node = firstScene.Instantiate();

        AddChild(node);
        LoadedScene = node;
    }

    public void LoadScene(Node scene) {
        LoadedScene.QueueFree();
        AddChild(scene);
        LoadedScene = scene;
    }
}
