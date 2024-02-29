using Godot;
using System;

public partial class GenericValueSetter : Node {
    [Export]
    public NodePath FromPath, ToPath;

    [Export]
    public string FromProperty, ToProperty;

    GenericPropertyHandler getter, setter;

    NodePath fromPath, toPath;

    public override void _Ready() {
        base._Ready();

        getter = GetNode<GenericPropertyHandler>("Getter");
        setter = GetNode<GenericPropertyHandler>("Setter");

        fromPath = FromPath + ":" + FromProperty;
        toPath = ToPath + ":" + ToProperty;

        GD.Print(fromPath, ", ", toPath);

        getter.PropertyPath = fromPath;
        setter.PropertyPath = toPath;

        GetTree().Quit();
    }

    public override void _Process(double delta) {
        base._Process(delta);

        Variant get = getter.Value;

        GD.Print(get.Obj);
        GD.Print(get.VariantType);
    }
}
