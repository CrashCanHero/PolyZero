using Godot;
using System;

public partial class Comment : Node {
    [Export(PropertyHint.MultilineText)]
    public string Text;
}
