using Godot;
using System;

public partial class JunkerContextSensitiveCommand : JunkerConsoleCommand {
    public override string Execute(string[] args) {
        if (!JunkerDebugConsole.Instance.HasContext) {
			return $"{CommandKey} is a context sensitive command! Please set a node context using 'setcontext' first!";
		}

		Node context = JunkerDebugConsole.Instance.GetContext;

		args = ProcessArgs(context, args);
		return OnExecute(context, args);
    }

	public virtual string[] ProcessArgs(Node context, string[] args) => args;

	public virtual string OnExecute(Node context, string[] args) => "Flicking beans and smoking greens...";
}
