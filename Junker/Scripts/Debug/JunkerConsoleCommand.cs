using Godot;
using System;

[GlobalClass]
public partial class JunkerConsoleCommand : Resource {
	[Export]
	public string CommandKey;

	[Export]
	public string HelpString;

	public virtual string Execute(string[] args) { 
		return "Flicking Beans and Smoking Greens";
	}

	public void Send(object msg) => JunkerDebugConsole.Instance.SendString(msg);
	public void SendVerbose(object msg) => JunkerDebugConsole.Instance.SendVerboseString(msg);
}
