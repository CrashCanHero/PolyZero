using Godot;

public partial class PolyUIPointerControl : Node {
    [Export]
    public Sprite3D Pointer;

    [Export]
    public ShapeCast3D SwingDetector;

    [Export]
    public ShapeCast3D EnemyDetector;

    Vector3 pos;

    public override void _Ready() {
        base._Ready();

        pos = Pointer.Position;
        GD.Print(pos);
    }

    public override void _Process(double delta) {
        base._Process(delta);

        bool swing = SwingDetector.IsColliding();
        bool enemy = EnemyDetector.IsColliding();

        Vector3 position = pos;

        if (!swing && !enemy) {
            Pointer.Position = pos;
            Pointer.Modulate = new Color(Colors.White, 1f);
            return;
        }

        if (swing) {
            position = SwingDetector.GetCollisionPoint(0);
            Pointer.Modulate = new Color(Colors.White, 1f);
        }

        if (enemy) {
            position = EnemyDetector.GetCollisionPoint(0);
            Pointer.Modulate = new Color(Colors.Red, 1f);
        }

        Pointer.GlobalPosition = position;
    }
}
