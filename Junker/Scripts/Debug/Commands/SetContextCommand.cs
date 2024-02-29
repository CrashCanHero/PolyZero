using Godot;
using System;

[GlobalClass]
public partial class SetContextCommand : JunkerConsoleCommand {
    public override string Execute(string[] args) {
        if (args.Length <= 0) {
            return "Invalid syntax! Usage: setcontext Path/To/Node";
        }

        Node context = JunkerDebugConsole.Instance.GetNode<Node>(args[0]);

        if (context == null) {
            return "Invalid context! Please make sure that the path to the node is correct, account for the current context!";
        }

        JunkerDebugConsole.Instance.SetContext(context);

        return $"Set node context to {args[0]}";
    }
}
