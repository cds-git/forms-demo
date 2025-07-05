namespace Forms.Demo.Forms;

public abstract record FormValue
{
    public abstract object GetValue();
    public abstract T GetValue<T>();
}

public sealed record TextValue(string Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions
    public static implicit operator TextValue(string value) => new(value);
    public static implicit operator string(TextValue value) => value.Value;
}

public sealed record IntValue(int Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions for int and smaller types
    public static implicit operator IntValue(int value) => new(value);
    public static implicit operator IntValue(short value) => new(value);
    public static implicit operator IntValue(byte value) => new(value);

    public static implicit operator int(IntValue value) => value.Value;
    public static implicit operator short(IntValue value) => (short)value.Value;
    public static implicit operator byte(IntValue value) => (byte)value.Value;
    public static implicit operator long(IntValue value) => value.Value; // Safe upcast
}

public sealed record LongValue(long Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions for long
    public static implicit operator LongValue(long value) => new(value);

    public static implicit operator long(LongValue value) => value.Value;
    public static implicit operator int(LongValue value) => (int)value.Value; // May throw if out of range
}

public sealed record DecimalValue(decimal Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions for decimal
    public static implicit operator DecimalValue(decimal value) => new(value);

    public static implicit operator decimal(DecimalValue value) => value.Value;
}

public sealed record FloatValue(double Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions for floating point types
    public static implicit operator FloatValue(double value) => new(value);
    public static implicit operator FloatValue(float value) => new(value);

    public static implicit operator double(FloatValue value) => value.Value;
    public static implicit operator float(FloatValue value) => (float)value.Value;
}

public sealed record BooleanValue(bool Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions
    public static implicit operator BooleanValue(bool value) => new(value);
    public static implicit operator bool(BooleanValue value) => value.Value;
}

public sealed record DateValue(DateTime Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions
    public static implicit operator DateValue(DateTime value) => new(value);
    public static implicit operator DateTime(DateValue value) => value.Value;
}

public sealed record ArrayValue(FormValue[] Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions for arrays
    public static implicit operator ArrayValue(string[] value) => new(value.Select(s => (FormValue)new TextValue(s)).ToArray());
    public static implicit operator ArrayValue(int[] value) => new(value.Select(i => (FormValue)new IntValue(i)).ToArray());
    public static implicit operator ArrayValue(long[] value) => new(value.Select(l => (FormValue)new LongValue(l)).ToArray());
    public static implicit operator ArrayValue(decimal[] value) => new(value.Select(d => (FormValue)new DecimalValue(d)).ToArray());
    public static implicit operator ArrayValue(double[] value) => new(value.Select(d => (FormValue)new FloatValue(d)).ToArray());
    public static implicit operator ArrayValue(bool[] value) => new(value.Select(b => (FormValue)new BooleanValue(b)).ToArray());
    public static implicit operator ArrayValue(DateTime[] value) => new(value.Select(dt => (FormValue)new DateValue(dt)).ToArray());
    public static implicit operator ArrayValue(FormValue[] value) => new(value);

    // Conversion back to typed arrays
    public static implicit operator FormValue[](ArrayValue value) => value.Value;
    public static implicit operator string[](ArrayValue value) => value.Value.OfType<TextValue>().Select(t => t.Value).ToArray();
    public static implicit operator int[](ArrayValue value) => value.Value.OfType<IntValue>().Select(i => i.Value).ToArray();
    public static implicit operator long[](ArrayValue value) => value.Value.OfType<LongValue>().Select(l => l.Value).ToArray();
    public static implicit operator decimal[](ArrayValue value) => value.Value.OfType<DecimalValue>().Select(d => d.Value).ToArray();
    public static implicit operator double[](ArrayValue value) => value.Value.OfType<FloatValue>().Select(f => f.Value).ToArray();
    public static implicit operator bool[](ArrayValue value) => value.Value.OfType<BooleanValue>().Select(b => b.Value).ToArray();
    public static implicit operator DateTime[](ArrayValue value) => value.Value.OfType<DateValue>().Select(d => d.Value).ToArray();

    // Helper methods for mixed arrays
    public T[] GetTypedArray<T>() where T : FormValue => Value.OfType<T>().ToArray();
    public object[] GetMixedArray() => Value.Select(v => v.GetValue()).ToArray();
}

public sealed record ObjectValue(Dictionary<string, FormValue> Value) : FormValue
{
    public override object GetValue() => Value;
    public override T GetValue<T>() => (T)(object)Value;

    // Implicit conversions
    public static implicit operator ObjectValue(Dictionary<string, FormValue> value) => new(value);
    public static implicit operator Dictionary<string, FormValue>(ObjectValue value) => value.Value;

    // Indexer for easy access
    public FormValue this[string key] => Value[key];
    public bool TryGetValue(string key, out FormValue value) => Value.TryGetValue(key, out value);
}

public sealed record NullValue : FormValue
{
    public override object GetValue() => null!;
    public override T GetValue<T>() => default(T)!;

    // Static instance for convenience
    public static readonly NullValue Instance = new();
}
