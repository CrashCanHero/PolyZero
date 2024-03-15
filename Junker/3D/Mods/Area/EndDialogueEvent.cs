using Godot;
using System;

public partial class EndDialogueEvent : Node {

    [Export]
    public StartDialogue Dialogue;

    public override void _Ready() {
        base._Ready();

        Dialogue.OnDialogueEnd += End;  
    }

    void End() {
        //Load title
        SceneLoader.Instance.LoadSceneIndex(0);
        GetTree().Root.SetMeta("Secret", 1);
    }
}
