using Godot;
using System;

namespace Junker.Components;

public partial class BitFlagSystem : Node {
    [Export]
    public uint Flags { get; private set; }

    public void SetFlag(BitRegion32 region, uint value) => SetFlag((uint)region, value);

    public void SetFlag(uint region, uint value) {
        Flags &= ~region;
        Flags |= value & region;
    }

    public void SetIntFlag(BitRegion32 region, uint value) => SetIntFlag((uint)region, value);

    public void SetIntFlag(uint region, uint value) {
        int index = 0;
        while ((region & 1) == 0) {
            region >>= 1;
            index++;
        }

        while (index > 0) {
            region <<= 1;
            value <<= 1;
            index--;
        }

        SetFlag(region, value);
    }

    public void AddFlag(BitRegion32 region, int value) => AddFlag((uint)region, value);

    public void AddFlag(uint region, int value) {
        uint bitRegion = region;
        uint flagRegion = Flags & bitRegion;

        int index = 0;

        uint bits = bitRegion;
        uint flagBits = flagRegion;

        while((bits & (uint)BitRegion32.Bit1) == 0) {
            flagBits >>= 1;
            bits >>= 1;
            index++;
        }

        flagBits += (uint)value;
        flagBits &= bits;

        while (index > 0) {
            flagBits <<= 1;
            bits <<= 1;
            index--;
        }

        SetFlag(bits, flagBits);
    }

    public void FlipFlag(BitRegion32 region) => FlipFlag((uint)region);

    public void FlipFlag(uint region) {
        Flags ^= region;
    }

    public bool GetFlag(BitRegion32 region) => (Flags & (uint)region) > 0;

    public uint GetIntFlag(BitRegion32 region) {
        uint bitRegion = (uint)region;
        uint flags = Flags & bitRegion;

        while((bitRegion & (uint)BitRegion32.Bit1) == 0) {
            bitRegion >>= 1;
            flags >>= 1;
        }

        return flags;
    }
}

[Flags]
public enum BitRegion32 : uint {
    Bit1 = 1u,
    Bit2 = Bit1 << 1,
    Bit3 = Bit2 << 1,
    Bit4 = Bit3 << 1,

    Bit5 = Bit4 << 1,
    Bit6 = Bit5 << 1,
    Bit7 = Bit6 << 1,
    Bit8 = Bit7 << 1,

    Bit9 = Bit8 << 1,
    Bit10 = Bit9 << 1,
    Bit11 = Bit10 << 1,
    Bit12 = Bit11 << 1,

    Bit13 = Bit12 << 1,
    Bit14 = Bit13 << 1,
    Bit15 = Bit14 << 1,
    Bit16 = Bit15 << 1,

    Bit17 = Bit16 << 1,
    Bit18 = Bit17 << 1,
    Bit19 = Bit18 << 1,
    Bit20 = Bit19 << 1,

    Bit21 = Bit20 << 1,
    Bit22 = Bit21 << 1,
    Bit23 = Bit22 << 1,
    Bit24 = Bit23 << 1,

    Bit25 = Bit24 << 1,
    Bit26 = Bit25 << 1,
    Bit27 = Bit26 << 1,
    Bit28 = Bit27 << 1,

    Bit29 = Bit28 << 1,
    Bit30 = Bit29 << 1,
    Bit31 = Bit30 << 1,
    Bit32 = Bit31 << 1,

    Region1 = Bit1 | Bit2 | Bit3 | Bit4,
    Region2 = Bit5 | Bit6 | Bit7 | Bit8,
    Region3 = Bit9 | Bit10 | Bit11 | Bit12,
    Region4 = Bit13 | Bit14 | Bit15 | Bit16,
    Region5 = Bit17 | Bit18 | Bit19 | Bit20,
    Region6 = Bit21 | Bit22 | Bit23 | Bit24,
    Region7 = Bit25 | Bit26 | Bit27 | Bit28,
    Region8 = Bit29 | Bit30 | Bit31 | Bit32,

    BigRegion1 = Region1 | Region2,
    BigRegion2 = Region3 | Region4,
    BigRegion3 = Region5 | Region6,
    BigRegion4 = Region7 | Region8,

    FirstHalf = BigRegion1 | BigRegion2,
    LastHalf = BigRegion3 | BigRegion4,

    Whole = FirstHalf | LastHalf
}
