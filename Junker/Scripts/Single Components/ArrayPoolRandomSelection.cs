using Godot;
using Junker.Tags;

namespace Junker.Components;

[GlobalClass]
public partial class ArrayPoolRandomSelection : TagSystem {
    [Export]
    public int PoolItemID;

    RandomNumberGenerator rng;

    public override void _Ready() {
        rng = new RandomNumberGenerator();
        AutoCreateTags = true;
        base._Ready();
    }

    public void CalculatePool() {
        float total = 0f;
        float rand = rng.RandfRange(0f, total);

        float tally = 0f;
        for (int i = 0; i < GetChildCount(); i++) {
            if (GetChild(i) is not JunkTag) {
                continue;
            }

            tally += ((JunkTag)GetChild(i)).Amount;

            if (tally >= rand) {
                PoolItemID = i;
                return;
            }
        }

        PoolItemID = GetChildCount() - 1;
    }

    public void Reset() {
        int offset = 0;
        while(GetChildCount() > 0) {
            if (GetChild(offset) is not JunkTag) {
                offset++;
                continue;
            }

            GetChild(offset).Free();
        }
    }
}
