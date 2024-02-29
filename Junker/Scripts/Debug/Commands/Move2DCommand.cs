using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class Move2DCommand : JunkerContextSensitiveCommand {

    public override string[] ProcessArgs(Node context, string[] args) {
        Node2D node = (Node2D)context;

        for (int i = 0; i < args.Length; i++) {
            float positionContext = i switch {
                0 => node.Position.X,
                1 => node.Position.Y,
                _ => 0f
            };


            if (args[i].StartsWith('~')) {
                string offset = args[i].Remove(0, 1);

                float pos = positionContext;

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
            return "invalid! Syntax: move2d X Y";
        }

        try {
            Vector2 pos = new Vector2(
                float.Parse(args[0]),
                float.Parse(args[1])
            );
            
            ((Node2D)context).Position = pos;

            return "Moved \'" + context.Name + "\' to (" + args[0] + ", " + args[1] + ")";
        } catch {
            return $"Invalid position! [move2d {args[0]} {args[1]}] Syntax: move2d X Y";
        }
    }
}
