using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Winch;

public class GameReplayer : IUpdateable
{
    private UniqueList<IEventReceiver> m_Receivers;
    private Queue<Event> m_Events;

    public GameReplayer(UniqueList<IEventReceiver> receivers)
    {
        m_Receivers = receivers;
    }

    public void Load(string gameLogString)
    {
        GameLog gameLog = JsonUtility.FromJson<GameLog>(gameLogString);

        List<Event> events = new List<Event>();

        foreach (string eventString in gameLog.EventStrings)
        {
            Event evt = JsonUtility.FromJson<Event>(eventString);

            Type type = Type.GetType(evt.TypeName);

            events.Add(JsonUtility.FromJson(eventString, type) as Event);
        }

        m_Events = new Queue<Event>(events.OrderBy(evt => evt.Tick));
    }

    public void Update(UpdateInfo info)
    {
        while (m_Events.Count > 0 && m_Events.Peek().Tick <= info.Tick)
        {
            Event evt = m_Events.Dequeue();

            IEventReceiver evtRec = m_Receivers.Values.FirstOrDefault(receiver => receiver.ID == evt.ReceiverID);
            evtRec.ReceiveEvent(evt);
        }
    }
}
