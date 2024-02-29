using Godot;
using System.Collections.Generic;

namespace Junker.Random;

public partial class MegaBool : Node {
    [Export]
    public MegaBoolState State;
    
    [ExportGroup("Setup")]
    [Export(PropertyHint.Range,"0.01,100f,or_greater")]
    public float OscillatingSpeed = 1f;

    [Export(PropertyHint.Range,"0.01,100f,or_greater")]
    public float MaybeChance = 0.5f;

    [Export]
    public Vector2 AskAgainLaterTimeMinMax;

    RandomNumberGenerator rng;
    bool lastValue;
    double time;
    int complexID;
    Queue<bool> overrides = new Queue<bool>();

    public bool GetValue {
        get {
            complexID++;
            complexID = complexID.Wrap(0, 6);

            if (overrides.Count > 0) {
                lastValue = overrides.Dequeue();
                return lastValue;
            }

            bool val = State switch {
                MegaBoolState.True => true,
                MegaBoolState.False => false,
                MegaBoolState.Maybe => MaybeCalc(),
                MegaBoolState.Trueish => IshChanceCalc(true),
                MegaBoolState.Falsish => IshChanceCalc(false),
                MegaBoolState.ItDepends => IshChanceCalc(true) && !IshChanceCalc(false),
                MegaBoolState.Oscillating => OscillatingCalc(),
                MegaBoolState.ItsComplicated => complexID switch {0 => true, 1 => false, 2 => MaybeCalc(), 3 => IshChanceCalc(true), 4 => IshChanceCalc(false), 5 => IshChanceCalc(true) && !IshChanceCalc(false), 6 => OscillatingCalc(), _ => true},
                MegaBoolState.AskAgainLater => AskAgainLaterCalc(),
                _ => false
            };
            lastValue = val;

            return val;
        }
    }

    public bool GetValueType(MegaBoolState state) {
        MegaBoolState cache = State;
        State = state;
        
        bool val = GetValue;

        State = cache;

        return val;
    }

    bool MaybeCalc() => rng.Randf() >= MaybeChance;
    bool IshChanceCalc(bool input) {
        if (MaybeCalc() && MaybeCalc() && MaybeCalc()) {
            return !input;
        }

        return input;
    }

    bool OscillatingCalc() {
        double currentTime = Time.GetUnixTimeFromSystem();

        double delta = currentTime - time;

        bool val = lastValue;

        if (delta >= OscillatingSpeed) {
            val = !val;
            time = currentTime;
        }

        return val;
    }

    bool AskAgainLaterCalc() {
        Timer timer = new Timer {
            WaitTime = rng.RandfRange(AskAgainLaterTimeMinMax.X, AskAgainLaterTimeMinMax.Y)
        };

        timer.Timeout += () => {GetValueType(MegaBoolState.ItsComplicated);};

        AddChild(timer);
        timer.Start();

        return false;
    }

    public override void _Ready() {
        base._Ready();

        rng = new RandomNumberGenerator();
        rng.Randomize();
    }
}

public enum MegaBoolState {
    True,
    False,
    Maybe,
    Trueish,
    Falsish,
    ItDepends,
    Oscillating,
    ItsComplicated,
    AskAgainLater
}
