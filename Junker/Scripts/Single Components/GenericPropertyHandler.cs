using Godot;
using System;

public partial class GenericPropertyHandler : Node {
    [Export]
    public bool CanGet = true, CanSet = true;

    public string PropertyPath;

    public T GetValue<[MustBeVariant]T>() => Value.As<T>();

    public Variant Value {
        get {
            if (!Cached && CanGet) {
                Variant v = GetParent().Get(PropertyPath);
                obj = v;
                Cached = true;
                SetDeferred(PropertyName.Cached, false);

                GD.Print(v.As<float>());
            }

            return obj;
        }
        set {
            if (!CanSet) {
                return;
            }

            GetParent().Set(PropertyPath, Variant.From(value));
            obj = value;
            Cached = true;
            SetDeferred(PropertyName.Cached, false);
        }
    }
    protected Variant obj;
    protected bool Cached;

    public void ClearCache() => Cached = false;
}
