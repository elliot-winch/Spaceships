using System.Linq;
using System.Collections.Generic;

public class UniqueList<T>
{
    private List<T> m_Values;

    public List<T> Values => m_Values.ToList();

    public UniqueList()
    {
        m_Values = new List<T>();
    }

    public void Add(T value)
    {
        if(m_Values.Contains(value) == false)
        {
            m_Values.Add(value);
        }
    }

    public void Remove(T value)
    {
        if (m_Values.Contains(value))
        {
            m_Values.Remove(value);
        }
    }
}
