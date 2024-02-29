using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ShowNamesCommand : JunkerConsoleCommand {
    [Export]
    public Theme labelTheme;

    int nameVerbosity;

    public override string Execute(string[] args) {

        try {
            nameVerbosity = int.Parse(args[0]);
        } catch {
            return "Invalid name index. Valid=[0,1,2]";
        }

        Label3D label3D = new Label3D{
            Name = "DEBUG LABEL NAME",
            Visible = false
        };

        Label label2D = new Label {
            Name = "DEBUG LABEL NAME",
            Visible = false
        };


        Send(nameVerbosity switch {
            0 => "Hiding Names...",
            1 => "Showing Names...",
            2 => "Showing Paths...",
            _ => "Setting Names..."
        });
        
        Node[] nodes = GetAllNodes();

        for (int i = 0; i < nodes.Length; i++) {
            string name = nameVerbosity switch {
                0 => "",
                1 => nodes[i].Name,
                2 => nodes[i].GetPath().ToString(),
                _ => nodes[i].Name
            };
            
            JunkerDebugConsole.Instance.SendVerboseString("Show " + name);


            if (nodes[i] is CanvasItem) {
                if (nodes[i].FindChild("DEBUG LABEL NAME", owned: false) != null) {
                    JunkerDebugConsole.Instance.SendVerboseString("Found debug label, using instead...");
                    Label text = nodes[i].GetNode<Label>("DEBUG LABEL NAME");
                    text.Text = name;
                    continue;
                }

                Label label = (Label)label2D.Duplicate();
                label.Text = name;
                nodes[i].AddChild(label);
                label.Position = Vector2.Zero;
                label.Visible = true;
            }

            if (nodes[i] is Node3D) {
                if (nodes[i].FindChild("DEBUG LABEL NAME", owned: false) != null) {
                    JunkerDebugConsole.Instance.SendVerboseString("Found debug label, using instead...");
                    Label3D text = nodes[i].GetNode<Label3D>("DEBUG LABEL NAME");
                    text.Text = name;
                    continue;
                }

                Label3D label = (Label3D)label3D.Duplicate();
                label.Text = name;
                nodes[i].AddChild(label);
                label.Position = Vector3.Zero;
                label.Visible = true;
            }
        }

        return "Finished! :)";
    }

    public Node[] GetAllNodes() => GetNodesRecursive(JunkerDebugConsole.Instance.GetTree().Root);

    public Node[] GetNodesRecursive(Node node) {
        List<Node> output = new List<Node>();

        Godot.Collections.Array<Node> children = node.GetChildren();
        Node[] recursionOutput = null;
        foreach(Node child in children) {
            recursionOutput = GetNodesRecursive(child);
        }

        foreach(Node check in children) {
            if (check is not Node2D && check is not Node3D) {
                continue;
            }

            output.Add(check);
        }

        if (recursionOutput != null) {
            foreach(Node check in recursionOutput) {           
                output.Add(check);
            }
        }

        return output.ToArray();
    }
}
