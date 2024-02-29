using Godot;
using System;

[GlobalClass]
public partial class Rotate2DCommand : JunkerContextSensitiveCommand{
    //This was copied from the move2D command so anything that references position actually references scale
	public override string[] ProcessArgs(Node context, string[] args) {
        Node2D node = (Node2D)context;

        for (int i = 0; i < args.Length; i++) {
            float rot = node.Rotation;

            if (args[i].StartsWith('~')) {
                string offset = args[i].Remove(0, 1);

                float pos = rot;

                try {
                    float off = float.Parse(offset);
                    pos += off;
                } catch {
                    //fuckin nothin who cares lol
                }

                args[i] = pos.ToString();
            }
        }


        return args;
    }

    public override string OnExecute(Node context, string[] args) {
        if (args.Length < 1) {
			return "Invalid arguments! Syntax: rot2d rotation";
		}

        float rot = 0f;

        try {
            rot = float.Parse(args[0]);
        } catch {
            return "Invalid argument! Syntax: rot2d rotation";
        }

        ((Node2D)context).Rotation = rot;

        return $"Set {context.Name}'s rotation to {rot}";
    }
}
