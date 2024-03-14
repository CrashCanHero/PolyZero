using Godot;
using System.Collections.Generic;

public partial class WindZone : Area3D {
    [Export]
    public bool OnEnterOnly;

    [Export]
    public float Strength;

    [Export]
    public Curve SpeedCurve;

    Dictionary<CharacterBody3D, Vector3> velocities = new Dictionary<CharacterBody3D, Vector3>();

    public override void _Ready() {
        base._Ready();

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        if (OnEnterOnly || !HasOverlappingBodies()) {
            return;
        }

        foreach(KeyValuePair<CharacterBody3D, Vector3> mov in velocities) {
            Boost(mov.Key);
        }
    }

    public void Boost(CharacterBody3D body) {
        if (!velocities.ContainsKey(body)) {
            velocities.Add(body, body.Velocity);
            return;
        }

        Vector3 velPrev = velocities[body];
        Vector3 vel = body.Get("velocity").As<Vector3>();

        float velDelta = (vel - velPrev).Length();
        Vector3 dir = body.GlobalPosition - GlobalPosition;

        float dot = dir.Normalized().Dot(vel.Normalized());
        dot++;
        dot /= 2f;

        float strength = SpeedCurve.Sample(dot);

        if (strength == 0f) {
            return;
        }

        velDelta *= strength;
        velocities[body] = body.Velocity;

        GD.Print(dot, ", ", velDelta);

        body.Set("velocity", vel + (vel.Normalized() * velDelta));
    }

    void OnBodyEntered(Node3D body) {
        if (body is not CharacterBody3D) {
            return;
        }

        CharacterBody3D character = (CharacterBody3D)body;

        velocities.Add(character, character.Velocity);

        if (OnEnterOnly) {
            Boost(character);
            velocities.Remove(character);
        }
    }

    void OnBodyExited(Node3D body) {
        if (!velocities.ContainsKey((CharacterBody3D)body)) {
            return;
        }

        velocities.Remove((CharacterBody3D)body);
    }
}
