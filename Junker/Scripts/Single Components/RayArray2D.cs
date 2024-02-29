using Godot;

namespace Junker.Components;

[GlobalClass]
public partial class RayArray2D : Node {
    public int arrayNum;
    public Godot.Collections.Array<RayCast2D> arrays;

    public bool IsColliding() {
        for (int i = 0; i < arrayNum; i++){ 
            if (arrays[i].IsColliding()) return true;
        }
        return false;
    }
    
    public float getCollidingRatio() {
        int colls = 0;
        for (int i = 0; i < arrayNum; i++) {
            if (arrays[i].IsColliding()) colls++;
        }
        return colls / arrayNum;
    }

    public Vector2 GetCollisionPoint() {
        Vector2 result = Vector2.Zero;
        int colls = 0;
        for (int i = 0; i < arrayNum; i++) {
            if (arrays[i].IsColliding()) {
                colls++;
                result += arrays[i].GetCollisionPoint();
            }
        }
        return result / colls;
    }

    public Vector2 TargetPosition {
        get {
            Vector2 result = Vector2.Zero;
            for (int i = 0; i < arrayNum; i++) {
                result += arrays[i].TargetPosition;
            }
            return result / arrayNum;
        }
        set {
            for (int i = 0; i < arrayNum; i++) {
                arrays[i].TargetPosition = value;
            }
        }
    }

    public Vector2 GlobalOrigin{
        get {
            Vector2 result = Vector2.Zero;
            for (int i = 0; i < arrayNum; i++) {
                result += arrays[i].GlobalTransform.Origin;
            }
            return result / arrayNum;
        }
    }

    public override void _Ready() {
        base._Ready();
        arrayNum = GetChildCount();
        arrays = new Godot.Collections.Array<RayCast2D>();
        for (int i = 0; i < arrayNum; i++) {
            arrays.Add((RayCast2D)GetChild(i));
        }
    }
}
