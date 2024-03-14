using Godot;
using System;

public partial class Enemy : CharacterBody3D {
    [Export]
    public long MaxHealth = 3;
    public long Health {
        get => health;
        set {
            health = value;

            if (health <= 0) {
                EmitSignal(SignalName.OnDeath, Math.Abs(health));
            }
        }
    }
    long health;
    
    [Signal]
    public delegate void OnDeathEventHandler(int extraDamage);

    public override void _Ready() {
        base._Ready();

        health = MaxHealth;
    }

    public override void _PhysicsProcess(double delta) {
        base._Process(delta);

        Velocity += -UpDirection;

        MoveAndSlide();
        Velocity *= Vector3.Up + ((Vector3.Right + Vector3.Back) * 0.95f);
    }
}
