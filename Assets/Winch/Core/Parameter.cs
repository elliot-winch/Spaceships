using System;

public class Parameter<T>
{
    private T m_Value;
    private Action<T> m_OnValueChanged;

    public T Value
    {
        get => m_Value;
        set
        {
            if (Equals(m_Value, value) == false)
            {
                m_Value = value;
                m_OnValueChanged?.Invoke(m_Value);
            }
        }
    }

    public Parameter() : this(default) { }

    public Parameter(T startingValue)
    {
        m_Value = startingValue;
    }

    public void Subscribe(Action<T> action)
    {
        action?.Invoke(m_Value);
        m_OnValueChanged += action;
    }

    public void Unsubscribe(Action<T> action)
    {
        m_OnValueChanged -= action;
    }
}