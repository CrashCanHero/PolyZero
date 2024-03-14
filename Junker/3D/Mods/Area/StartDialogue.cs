using Godot;
using System;

public partial class StartDialogue : Area3DMod {
    [Export]
    public Label TextBox;

    [Export]
    public Node AravoxController;
    
    string[] lines;
    int index;
    bool running = false;
    Node character;

    [Signal]
    public delegate void OnDialogueEndEventHandler();

    public override void _Ready() {
        base._Ready();

        GD.Print(Name);
        AravoxController.Set("visible", false);
        AravoxController.Connect("script_generation_finished", Callable.From<Godot.Collections.Dictionary>(GenDone));
        AravoxController.Call("generate");
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (!running) {
            return;
        }

        TextBox.Text = lines[index];

        if (Input.IsActionJustPressed("Retract")) {
            index++;

            //Done, cleanup time
            if (index >= lines.Length) {
                EmitSignal(SignalName.OnDialogueEnd);
                running = false;
                character.SetDeferred("CanControl", true);
                QueueFree();
            }
        }
    }

    protected override void OnBodyEntered(CharacterBody3D body) {
        base.OnBodyEntered(body);

        //We can only hit the player so this is fine
        character = body.GetParent();
        character.Set("CanControl", false);
        AravoxController.Set("visible", true);
        running = true;
        GD.Print("Starting Run");

        //Parent area will want to destroy itself so we should move off of it
        Node root = GetTree().Root;
        GetParent().RemoveChild(this);
        root.AddChild(this);
    }

    void GenDone(Godot.Collections.Dictionary dict) {
        GD.Print(Name);
        GD.Print("Generated Scirpt");

        lines = dict["script"].As<string[]>();
        index = 0;
    }
}
