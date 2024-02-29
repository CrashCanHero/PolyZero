using Godot;

namespace Junker.Tags;

[GlobalClass]
public partial class TagSystem : Node {
    [Export]
    public bool AutoCreateTags;

    public JunkTag this[string ID] {
        get {
            JunkTag tag = null;
            if (AutoCreateTags && !HasNode(ID)) {
                tag = new JunkTag();
                AddChild(tag);
                ID = tag.Name;
            }

            return GetNode<JunkTag>(ID);
        }
        set {
            if (HasNode(ID)) {
                GetNode(ID).QueueFree();
            }

            JunkTag tag = value;

            AddChild(tag);
            tag.Name = ID;
        }
    }

    public override void _Process(double delta) {
        foreach(JunkTag tag in GetChildren()) {
            tag.OnUpdate(delta);
        }
    }
}
