using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecorder
{
    private GameLog m_Log = new GameLog();

    public void AddEvent(Event evt)
    {
        m_Log.EventStrings.Add(JsonUtility.ToJson(evt));
    }

    public string CreateGameLog()
    {
        return JsonUtility.ToJson(m_Log);
    }
}