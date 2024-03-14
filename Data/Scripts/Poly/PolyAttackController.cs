using Godot;
using System;
using System.Collections.Generic;

public partial class PolyAttackController : Node {
    [Export]
    public ShapeCast3D EnemyDetector;

    [Export]
    public float AttackSpeed;

    [Export]
    public PackedScene HitFX;

    public float LeftTime, RightTime;



    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        LeftTime -= (float)delta;
        RightTime -= (float)delta;

        if (!EnemyDetector.IsColliding()) {
            return;
        }

        Vector3 pos = EnemyDetector.GetCollisionPoint(0);
        Node hitObj = ((Area3D)EnemyDetector.GetCollider(0)).GetParent();

        if (Input.IsActionJustPressed("LeftArmThrow") && LeftTime <= 0f && hitObj is Enemy enemy) {
            Attack(enemy, pos, EnemyDetector.GetCollisionNormal(0), out LeftTime);
        }

        if (Input.IsActionJustPressed("RightArmThrow") && RightTime <= 0f && hitObj is Enemy enmy) {
            Attack(enmy, pos, EnemyDetector.GetCollisionNormal(0), out RightTime);
        }
    }

    void Attack(Enemy enemy, Vector3 pos, Vector3 norm, out float timer) {
        enemy.Health--;

        Node3D hit = HitFX.Instantiate<Node3D>();
        hit.Visible = true;
        AddChild(hit);
        hit.GlobalPosition = pos + (norm * 0.1f);
        enemy.Velocity += -norm * 5f + (Vector3.Up * 5f);

        timer = AttackSpeed;
    }
}
