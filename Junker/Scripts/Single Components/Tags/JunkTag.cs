using Godot;

namespace Junker.Tags;

[GlobalClass]
public partial class JunkTag : Node{
    
    [Export]
    public float Amount {
        get {
            return amount;
        }
        private set {
            if (amount == value) {
                return;
            }

            OnValueChanged();
            amount = value;
        }
    }

    public TagSystem Container {
        get {
            if (GetParent() is not TagSystem) {
                return null;
            }
            
            if (container == null) {
                container = GetParent() as TagSystem;
            }

            return container;
        }
    }
    TagSystem container;

    public JunkTag(float amount) {
        Amount = amount;
    }

    public JunkTag() {
        Amount = 0f;
    }

    float amount;

    public override void _Ready() {
        OnCreate();
    }

    public virtual void OnCreate() { }
    public virtual void OnUpdate(double delta) { }
    public virtual void OnValueChanged() { }

    public void AddAmount(float amount) => Amount += amount;
    public void SetAmount(float amount) => Amount = amount;

#region Operators
    //operators for calculations
    public static JunkTag operator +(JunkTag lhs, JunkTag rhs) {
        lhs.AddAmount(rhs.Amount);
        return lhs;
    }
    
    public static JunkTag operator -(JunkTag lhs, JunkTag rhs) {
        lhs.AddAmount(-rhs.Amount);
        return lhs;
    }
    
    public static JunkTag operator +(JunkTag lhs, float rhs) {
        lhs.AddAmount(rhs);
        return lhs;
    }

    public static JunkTag operator +(float lhs, JunkTag rhs) {
        rhs.AddAmount(lhs);
        return rhs;
    }
    
    public static JunkTag operator -(JunkTag lhs, float rhs) {
        lhs.AddAmount(-rhs);
        return lhs;
    }
    
    public static JunkTag operator *(JunkTag lhs, float rhs) {
        lhs.SetAmount(lhs.Amount * rhs);
        return lhs;
    }

    public static JunkTag operator *(float rhs, JunkTag lhs) {
        lhs.SetAmount(lhs.Amount * rhs);
        return lhs;
    }

    public static JunkTag operator /(JunkTag lhs, float rhs) {
        lhs.SetAmount(lhs.Amount / rhs);
        return lhs;
    }

    public static bool operator >(JunkTag lhs, float rhs) => lhs.Amount > rhs;
    public static bool operator <(JunkTag lhs, float rhs) => lhs.Amount < rhs;
    public static bool operator >=(JunkTag lhs, float rhs) => lhs.Amount >= rhs;
    public static bool operator <=(JunkTag lhs, float rhs) => lhs.Amount <= rhs;
    public static bool operator !=(JunkTag lhs, float rhs) => lhs.Amount != rhs;
    public static bool operator ==(JunkTag lhs, float rhs) => lhs.Amount == rhs;
    public static bool operator >(float rhs, JunkTag lhs) => rhs > lhs.Amount;
    public static bool operator <(float rhs, JunkTag lhs) => rhs < lhs.Amount;
    public static bool operator >=(float rhs, JunkTag lhs) => rhs >= lhs.Amount;
    public static bool operator <=(float rhs, JunkTag lhs) => rhs <= lhs.Amount;
    public static bool operator !=(float rhs, JunkTag lhs) => rhs != lhs.Amount;
    public static bool operator ==(float rhs, JunkTag lhs) => rhs == lhs.Amount;

    public override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override string ToString() {
        return Amount.ToString();
    }

    public static implicit operator float(JunkTag tag) => tag.Amount;
#endregion
}
