namespace Forms.Demo.Forms;

public static class FormValueExtensions
{
    // Generic transform methods - automatically dispatch based on type
    public static TResult Transform<TResult>(this FormValue value, Func<string, TResult> transform)
        => value is TextValue text ? transform(text.Value) : throw new InvalidOperationException($"Expected TextValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<int, TResult> transform)
        => value is IntValue intVal ? transform(intVal.Value) : throw new InvalidOperationException($"Expected IntValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<long, TResult> transform)
        => value is LongValue longVal ? transform(longVal.Value) : throw new InvalidOperationException($"Expected LongValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<decimal, TResult> transform)
        => value is DecimalValue decimalVal ? transform(decimalVal.Value) : throw new InvalidOperationException($"Expected DecimalValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<double, TResult> transform)
        => value is FloatValue floatVal ? transform(floatVal.Value) : throw new InvalidOperationException($"Expected FloatValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<bool, TResult> transform)
        => value is BooleanValue boolean ? transform(boolean.Value) : throw new InvalidOperationException($"Expected BooleanValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<DateTime, TResult> transform)
        => value is DateValue date ? transform(date.Value) : throw new InvalidOperationException($"Expected DateValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<FormValue[], TResult> transform)
        => value is ArrayValue array ? transform(array.Value) : throw new InvalidOperationException($"Expected ArrayValue, got {value.GetType()}");
    
    public static TResult Transform<TResult>(this FormValue value, Func<Dictionary<string, FormValue>, TResult> transform)
        => value is ObjectValue obj ? transform(obj.Value) : throw new InvalidOperationException($"Expected ObjectValue, got {value.GetType()}");
    
    // Safe transform methods that return default if type doesn't match
    public static TResult? TryTransform<TResult>(this FormValue value, Func<string, TResult> transform)
        => value is TextValue text ? transform(text.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<int, TResult> transform)
        => value is IntValue intVal ? transform(intVal.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<long, TResult> transform)
        => value is LongValue longVal ? transform(longVal.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<decimal, TResult> transform)
        => value is DecimalValue decimalVal ? transform(decimalVal.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<double, TResult> transform)
        => value is FloatValue floatVal ? transform(floatVal.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<bool, TResult> transform)
        => value is BooleanValue boolean ? transform(boolean.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<DateTime, TResult> transform)
        => value is DateValue date ? transform(date.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<FormValue[], TResult> transform)
        => value is ArrayValue array ? transform(array.Value) : default;
    
    public static TResult? TryTransform<TResult>(this FormValue value, Func<Dictionary<string, FormValue>, TResult> transform)
        => value is ObjectValue obj ? transform(obj.Value) : default;
    
    // Action overloads for side effects
    public static void Transform(this FormValue value, Action<string> action)
    {
        if (value is TextValue text) action(text.Value);
        else throw new InvalidOperationException($"Expected TextValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<int> action)
    {
        if (value is IntValue intVal) action(intVal.Value);
        else throw new InvalidOperationException($"Expected IntValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<long> action)
    {
        if (value is LongValue longVal) action(longVal.Value);
        else throw new InvalidOperationException($"Expected LongValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<decimal> action)
    {
        if (value is DecimalValue decimalVal) action(decimalVal.Value);
        else throw new InvalidOperationException($"Expected DecimalValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<double> action)
    {
        if (value is FloatValue floatVal) action(floatVal.Value);
        else throw new InvalidOperationException($"Expected FloatValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<bool> action)
    {
        if (value is BooleanValue boolean) action(boolean.Value);
        else throw new InvalidOperationException($"Expected BooleanValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<DateTime> action)
    {
        if (value is DateValue date) action(date.Value);
        else throw new InvalidOperationException($"Expected DateValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<FormValue[]> action)
    {
        if (value is ArrayValue array) action(array.Value);
        else throw new InvalidOperationException($"Expected ArrayValue, got {value.GetType()}");
    }
    
    public static void Transform(this FormValue value, Action<Dictionary<string, FormValue>> action)
    {
        if (value is ObjectValue obj) action(obj.Value);
        else throw new InvalidOperationException($"Expected ObjectValue, got {value.GetType()}");
    }
    
    // Type checking helpers
    public static bool Is<T>(this FormValue value) => value switch
    {
        TextValue when typeof(T) == typeof(string) => true,
        IntValue when typeof(T) == typeof(int) => true,
        LongValue when typeof(T) == typeof(long) => true,
        DecimalValue when typeof(T) == typeof(decimal) => true,
        FloatValue when typeof(T) == typeof(double) || typeof(T) == typeof(float) => true,
        BooleanValue when typeof(T) == typeof(bool) => true,
        DateValue when typeof(T) == typeof(DateTime) => true,
        ArrayValue when typeof(T) == typeof(FormValue[]) => true,
        ObjectValue when typeof(T) == typeof(Dictionary<string, FormValue>) => true,
        NullValue => true,
        _ => false
    };
}
