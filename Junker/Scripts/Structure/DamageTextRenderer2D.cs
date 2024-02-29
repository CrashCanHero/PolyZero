using System.Collections.Generic;
using Godot;

namespace Junker;

public partial class DamageTextRenderer2D : Node {
    public static DamageTextRenderer2D Instance;

    const string velocityMeta = "velocity";
    const string lifetimeMeta = "lifetime";

    [Export]
    public Theme LabelTheme;

    [Export]
    public float LabelLifeTime = 1f;

    [Export]
    public float GravityScale = 1f;

    [Export]
    public bool Combine;

    public float VelocityX, VelocityY;

    Dictionary<CanvasItem, Label> damageNumbers = new Dictionary<CanvasItem, Label>();
    List<Label> floatingLabels = new List<Label>();

    public override void _Ready() {
        base._Ready();

        if (Instance != null && Instance != this) {
            QueueFree();
            return;
        }
        Instance = this;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        
        Stack<CanvasItem> toRemove = new Stack<CanvasItem>();
        foreach (KeyValuePair<CanvasItem, Label> damageNum in damageNumbers) {
            Label label = damageNum.Value;
            Vector2 vel = label.GetMeta(velocityMeta).As<Vector2>();
            float life = label.GetMeta(lifetimeMeta).As<float>();

            life -= (float)delta;

            if (life <= 0f) {
                label.QueueFree();
                toRemove.Push(damageNum.Key);
            }

            vel += Vector2.Down * 9.8f * (float)delta * GravityScale;
            label.Position += vel;
            label.SetMeta(velocityMeta, vel);
            label.SetMeta(lifetimeMeta, life);
        }

        //!copied code
        for(int i = 0; i < floatingLabels.Count; i++) {
            Label label = floatingLabels[i];
            Vector2 vel = label.GetMeta(velocityMeta).As<Vector2>();
            float life = label.GetMeta(lifetimeMeta).As<float>();

            life -= (float)delta;

            if (life <= 0f) {
                label.QueueFree();
                floatingLabels.Remove(label);
            }

            vel += Vector2.Down * 9.8f * (float)delta * GravityScale;
            label.Position += vel;
            label.SetMeta(velocityMeta, vel);
            label.SetMeta(lifetimeMeta, life);
        }

        while (toRemove.Count > 0) {
            damageNumbers.Remove(toRemove.Pop());
        }
    }

    /// <summary>
    /// Request a damage number, will do different thing depending on DamageTextRenderer settings
    /// </summary>
    /// <param name="offset">
    /// The position to spawn at
    ///</param>
    /// <param name="velocity">
    /// How fast is the test moving
    ///</param>
    /// <param name="entity">
    /// The entity to attach to
    ///</param>
    /// <param name="number">
    /// The actual number to display
    ///</param>
    public void RequestDamageNumber(CanvasItem entity, Vector2 offset, float number) {
        if (entity == null) {
            GD.PushError("And entity is required for damage numbers!");
            return;
        }

        Label label = new Label() {
            Position = offset,
            Theme = LabelTheme,
            Text = number.ToString(),
        };
        label.SetMeta(velocityMeta, new Vector2(VelocityX, VelocityY));
        label.SetMeta(lifetimeMeta, LabelLifeTime);

        float total = 0f;
        if (damageNumbers.ContainsKey(entity)) {
            if (Combine) {
                Label ent = damageNumbers[entity];
                total = float.Parse(ent.Text);
                label.Text = (number + total).ToString();
                damageNumbers[entity].QueueFree();
            } else {
                floatingLabels.Add(damageNumbers[entity]);
            }

            damageNumbers.Remove(entity);
        }

        damageNumbers.Add(entity, label);

        entity.AddChild(label);
    }
}
