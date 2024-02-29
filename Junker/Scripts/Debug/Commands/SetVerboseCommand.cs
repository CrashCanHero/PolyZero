using Godot;
using System;

[GlobalClass]
public partial class SetVerboseCommand : JunkerConsoleCommand {
    public override string Execute(string[] args) {
        try {
            float mode = float.Parse(args[0]);
            mode = Mathf.Round(mode);
            JunkerDebugConsole.Instance.Tags["VerboseMode"].SetAmount(mode);
        } catch {
            JunkerDebugConsole.Instance.SendString("dumbass! \'setverbose [0/1]\'");
        }
        return $"Set verbose mode to {args[0] == "1"}";
    }
}
