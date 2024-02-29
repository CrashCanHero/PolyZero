using Godot;

namespace Junker.Components;

[GlobalClass]
public partial class ValueSubValue : Node {
    [Export]
    public bool ResetSubValue;

    [Export]
    public int[] Values { get; private set; }

    int[] valuesCache;

    public override void _Ready() {
        base._Ready();

        valuesCache = Values;
    }

    public int this[int index] {
        get {
            if (Values == null) {
                Values = new int[1];
            }

            if (Values.Length <= index) {
                GD.PushError($"Value '{index}' does not exist!");
            }

            return Values[index];
        }
        set  {
            if (Values.Length <= index) {
                int[] newValues = new int[index];

                for (int i = 0; i < Values.Length; i++) {
                    newValues[i] = Values[i];
                }

                Values = newValues;
            }

            Values[index] = value;

            for (int i = index; i < Values.Length; i++) {
                Values[i] = 0;
            }
        }
    }

    public void ResetSubFValues() => Values = null;
}
