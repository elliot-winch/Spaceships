using System;
using UnityEngine;

[Serializable]
public class Event
{
    [SerializeField]
    private string m_TypeString;
    [SerializeField]
    private int m_Tick;
    [SerializeField]
    private string m_ReceiverID;

    public string TypeName => m_TypeString;
    public int Tick => m_Tick;
    public string ReceiverID => m_ReceiverID;

    public Event(int tick, string receiverID)
    {
        m_TypeString = GetType().ToString();
        m_Tick = tick;
        m_ReceiverID = receiverID;
    }
}
