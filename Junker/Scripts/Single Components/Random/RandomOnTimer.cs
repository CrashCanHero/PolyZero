using Godot;
using System;

public partial class RandomOnTimer : Node {
    [Export]
    public bool Playing;

    [Export]
    public float Value;

    [ExportGroup("Values")]
    [Export]
    public float Minimum;

    [Export]
    public float Maximum;

    [ExportGroup("Time")]
    [Export]
    public double Time;


    [Signal]
    public delegate void OnRandomizeEventHandler(float rand);


    double time;
    RandomNumberGenerator rng;

    public override void _Ready() {
        base._Ready();

        rng = new RandomNumberGenerator();
        rng.Randomize();
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (!Playing) {
            return;
        }

        time += delta;

        if (time < Time) {
            return;
        }

        Value = rng.RandfRange(Minimum, Maximum);
        EmitSignal(SignalName.OnRandomize, Value);

        //More accurate way to measure time, I should do this more
        //While loop ensures that if you start at a time so far that it would trigger twice, it will still only trigger once
        while(time > Time) {
            time -= Time;
        }
    }

    public void Play() => Playing = true;
    public void Start() {
        time = 0f;
        Play();
    }

    public void Pause() => Playing = false;
    public void Stop() {
        Pause();
        time = 0f;
    }

    public void StartAtTime(float time) {
        this.time = time;
        Play();
    }
}
