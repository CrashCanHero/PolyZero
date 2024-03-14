using Godot;
using System;

[GlobalClass]
public partial class AutoPathFollower3D : PathFollow3D {
    [Export]
    public bool AutoStop;

    [Export]
    public bool Playing;

    [Export]
    public float Speed;
    [Export]
    public bool PerSecond;

    float ratioPrev;

    [Signal]
    public delegate void OnPathCompleteEventHandler();

    public override void _Process(double delta) {
        if (!Playing) {
            return;
        }
        base._Process(delta);

        float speed = Speed;

        if (PerSecond) {
            speed *= (float)delta;
        }

        Progress += speed;

        //The only time this is possible is if we've wrapped back around
        if (ratioPrev > ProgressRatio) {
            EmitSignal(SignalName.OnPathComplete);

            if (AutoStop) {
                ProgressRatio = 1f;
                Playing = false;
            }
        }

        ratioPrev = ProgressRatio;
    }
}
