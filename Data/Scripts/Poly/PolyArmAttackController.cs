using Godot;
using Junker.Components;
using System;

public partial class PolyArmAttackController : Node {
    [ExportGroup("Components")]
    [Export]
    public Node3D Camera;

    [ExportGroup("LeftArm")]
    [Export]
    public CanvasItem LeftArmUI;

    [Export]
    public Node3D LeftArmWorld;

    [Export]
    public Node3D LeftArmPositionController;

    [ExportGroup("RightArm")]
    [Export]
    public CanvasItem RightArmUI;

    [Export]
    public Node3D RightArmWorld;

    [Export]
    public Node3D RightArmPositionController;

    [ExportGroup("ArmAttackData")]
    [Export]
    public float ArmSpeed;

    [Export]
    public float MaxDistance;

    [Export]
    public float ArmReturnTime;

    BitFlagSystem flagSystem;

    Vector3 leftArmReturnPos, rightArmReturnPos;
    Vector3 leftArmReturnRot, rightArmReturnRot;
    Callable armState0, armState1, armState2;

    public override void _Ready() {
        base._Ready();

        flagSystem = GetNode<BitFlagSystem>("../FlagSystem");

        leftArmReturnPos = LeftArmPositionController.Position;
        leftArmReturnRot = LeftArmPositionController.Rotation;
        rightArmReturnPos = RightArmPositionController.Position;
        rightArmReturnRot = RightArmPositionController.Rotation;

        armState0 = Callable.From<Node3D, bool, double>(ArmState0);
        armState1 = Callable.From<Node3D, bool, double>(ArmState1);
        armState2 = Callable.From<Node3D, bool, double>(ArmState2);
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (LeftArmUI.Visible != !LeftArmWorld.Visible) {
            LeftArmUI.Visible = !LeftArmWorld.Visible;
        }

        if (RightArmUI.Visible != !RightArmWorld.Visible) {
            RightArmUI.Visible = !RightArmWorld.Visible;
        }

        if (Input.IsActionJustPressed("LeftArmThrow")) {

        }

        if (flagSystem.GetFlag(BitRegion32.Bit1)) {
            uint state = LeftArmWorld.GetMeta("state").As<uint>();

            (state switch {
                0 => armState0,
                1 => armState1,
                2 => armState2,
                _ => armState0,
            }).Call(LeftArmWorld, true, delta);
        }
    }

    public void ThrowArm(Vector3 direction, bool leftArm = false) {
        if ((leftArm && flagSystem.GetFlag(BitRegion32.Bit3)) || (!leftArm && flagSystem.GetFlag(BitRegion32.Bit4))) {
            return;
        }

        Node3D Pivot = leftArm ? LeftArmPositionController : RightArmPositionController;
        Node3D arm = leftArm ? LeftArmWorld : RightArmWorld;

        if ((leftArm && flagSystem.GetFlag(BitRegion32.Bit1)) || (!leftArm && flagSystem.GetFlag(BitRegion32.Bit2))) {
            ReturnArm(leftArm, arm);
            return;
        }

        arm.Visible = true;
        Pivot.LookAt(direction);

        flagSystem.SetFlag(BitRegion32.Bit1, (uint)leftArm.ConvertToInt());
        flagSystem.SetFlag(BitRegion32.Bit2, (uint)(!leftArm).ConvertToInt());

        arm.SetMeta("state", 0u);
        arm.SetMeta("direction", direction);
    }

    public void ReturnArm(bool leftArm, Node3D arm) {
        BitRegion32 region = (BitRegion32)(((uint)(!leftArm).ConvertToInt()) + (uint)BitRegion32.Bit3);
        Vector3 pos = leftArm ? leftArmReturnPos : rightArmReturnPos;
        Vector3 rot = leftArm ? leftArmReturnRot : rightArmReturnRot;

        flagSystem.SetFlag(region, 1u);
        flagSystem.SetFlag(region - 2u, 0u);

        Tween tween = GetTree().CreateTween().BindNode(arm);
        tween.TweenProperty(arm, "position", pos, ArmReturnTime);
        tween.TweenProperty(arm, "rotation", rot, ArmReturnTime);
        tween.TweenCallback(Callable.From(() => {
            flagSystem.SetFlag(region, 0u);
            arm.Visible = false;
        }));
    }

    //> Flying state, move forwards
    void ArmState0(Node3D arm, bool leftArm, double delta) {
        arm.Position += Camera.Transform.Basis.Z * ArmSpeed;
    }

    //> Returning, kinda nothing since the tween should handle it
    void ArmState1(Node3D arm, bool leftArm, double delta) {

    }

    //> Locked state, just stay there
    void ArmState2(Node3D arm, bool leftArm, double delta) {

    }
}
