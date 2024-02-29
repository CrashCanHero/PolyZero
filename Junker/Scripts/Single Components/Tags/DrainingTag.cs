using Godot;

namespace Junker.Tags;

[GlobalClass]
public partial class DrainingTag : JunkTag {
    [Signal]
    public delegate void OnDrainedEventHandler();

    [Export]
    public float Scale = 1f;

    bool triggered;
    float lastAmount;

    public override void OnUpdate(double delta) {
        if (Amount >= 0 && Amount > lastAmount) {
            triggered = false;
        }

        AddAmount(-((float)delta * Scale));

        if (Amount <= 0f && !triggered) {
            EmitSignal(SignalName.OnDrained);
            lastAmount = Amount;
            triggered = true;
        }

        SetAmount(Mathf.Clamp(Amount, 0f, float.MaxValue));
    }
}
