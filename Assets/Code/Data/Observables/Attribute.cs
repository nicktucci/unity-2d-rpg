using System;

[Serializable]
public abstract class Attribute<T> : Observable<T> where T : IComparable
{
    protected T valueMin;
    protected T valueMax;

    public new Action<Attribute<T>, T, T> OnChanged;


    public Attribute()
    {
    }

    public Attribute(T value, T max, T min)
    {
        this.valueMin = min;
        this.valueMax = max;
        this.value = value;
    }
    public Attribute(T value, T max)
    {
        this.valueMin = default;
        this.valueMax = max;
        this.value = value;
    }

    public override T Value
    {
        get { return value; }
        set
        {
            var oldValue = this.value;
            this.value = value;
            this.value = EnforceMinMax(this.value);
            if(this.value.CompareTo(oldValue) != 0)
            {
                InvokeOnChanged(this.value, oldValue);
            }
        }
    }

    protected virtual void InvokeOnChanged(T value, T oldValue)
    {
        OnChanged?.Invoke(this, oldValue, value);
    }

    public T ValueMax
    {
        get { return valueMax; }
        set
        {
            this.valueMax = value;
            this.value = EnforceMinMax(this.value);

            InvokeOnChanged(this.value, this.value);
        }
    }
    public T ValueMin
    {
        get { return valueMin; }
        set
        {
            this.valueMin = value;
            this.value = EnforceMinMax(this.value);

            InvokeOnChanged(this.value, this.value);
        }
    }
     protected abstract T EnforceMinMax(T val);
    //private T EnforceMinMax(T val)
    //{
        //if (val<valueMin) val = valueMin;
        //if (val > valueMax) val = valueMax;
        //return val;
    //}
}