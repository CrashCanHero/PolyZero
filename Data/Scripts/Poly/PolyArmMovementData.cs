using Godot;
using System;

[GlobalClass]
public partial class PolyArmMovementData : Resource {
    [Export]
    public Vector2 xLimits, yLimits;

    [Export]
    public float Offset;

    public Vector2[] ConstructLimits() => new Vector2[] {
        new Vector2(xLimits.X, yLimits.X),
        new Vector2(xLimits.Y, yLimits.Y)
    };

    public Vector2 Cache;
}
