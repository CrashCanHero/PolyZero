using Godot;
using System;

[GlobalClass]
public partial class Scale2DCommand : JunkerContextSensitiveCommand {

	//This was copied from the move2D command so anything that references position actually references scale
	public override string[] ProcessArgs(Node context, string[] args) {
        Node2D node = (Node2D)context;

        for (int i = 0; i < args.Length; i++) {
            float scaleContext = i switch {
                0 => node.Scale.X,
                1 => node.Scale.Y,
                _ => 0f
            };


            if (args[i].StartsWith('~')) {
                string offset = args[i].Remove(0, 1);

                float pos = scaleContext;

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
		if (args.Length < 2) {
			return "Invalid arguments! Syntax: scale2d scaleX scaleY";
		}

		Vector2 scale = Vector2.Zero;

		try {
			scale = new Vector2(
				float.Parse(args[0]),
				float.Parse(args[1])
			);
		} catch {
			return "Invalid scale! Syntax scale2d scaleX scaleY";
		}

		((Node2D)context).Scale = scale;

        return $"Set {context.Name}'s scale to ({args[0]}, {args[1]})";
    }
}
