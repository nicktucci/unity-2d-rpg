using System;
using UnityEngine;

[Serializable]
public class AttributeInt : Attribute<int>
{
    public new Action<AttributeInt, int, int> OnChanged;

    protected override void InvokeOnChanged(int value, int oldValue)
    {
        OnChanged(this, value, oldValue);
    }

    protected override int EnforceMinMax(int val)
    {
        if (val < valueMin) val = valueMin;
        if (val > valueMax) val = valueMax;
        return val;
    }

    public AttributeInt(int value, int max)
    {
        this.valueMin = 0;
        this.valueMax = max;
        this.value = value;
    }
    public AttributeInt(int max)
    {
        this.valueMin = 0;
        this.valueMax = max;
        this.value = max;
    }

    public void Reset(int healthPoints)
    {
        this.valueMin = 0;
        this.valueMax = healthPoints;
        this.value = healthPoints;
    }
}