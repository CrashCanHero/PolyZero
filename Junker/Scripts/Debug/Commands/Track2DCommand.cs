using Godot;
using System;

[GlobalClass]
public partial class Track2DCommand : JunkerContextSensitiveCommand {
    public override string OnExecute(Node context, string[] args) {
        JunkerDebugConsole.Instance.SetTracking2D(args[0] != "0");

        return $"Set Context tracking to {args[0] != "0"}";
    }
}
