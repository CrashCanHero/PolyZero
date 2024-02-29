using Godot;

namespace Junker.Components;

[GlobalClass]
public partial class AngleDirectionVector2D : Node2D {
    [Export]
    public float Angle;

    public Vector2 GetDirection {
        get {
            return Angle.ConvertToVector();
        }
    }   
}
