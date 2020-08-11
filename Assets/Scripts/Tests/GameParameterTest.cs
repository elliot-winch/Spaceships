using UnityEngine;

public class TeamChangeEvent : Event
{
    public string NewName;

    public TeamChangeEvent(int tick, string receiverID, string newName) : base(tick, receiverID)
    {
        NewName = newName;
    }
}

public class TeamChanger : IEventReceiver
{
    public Parameter<string> TeamName = new Parameter<string>("Team 1");

    public string ID => "Test";

    public void ReceiveEvent(Event evt)
    {
        TeamName.Value = (evt as TeamChangeEvent).NewName;
    }
}

public class GameParameterTest : MonoBehaviour
{
    private void Start()
    {
        TeamChanger teamChanger = new TeamChanger();

        UniqueList<IEventReceiver> receivers = new UniqueList<IEventReceiver>();
        receivers.Add(teamChanger);

        GameRecorder recorder = new GameRecorder();
        GameReplayer replayer = new GameReplayer(receivers);

        teamChanger.TeamName.Subscribe((newName) => Debug.Log(newName));

        TeamChangeEvent evt = new TeamChangeEvent(40, teamChanger.ID, "New team name");
        recorder.AddEvent(evt);

        string gameLog = recorder.CreateGameLog();
        replayer.Load(gameLog);

        replayer.Update(new Winch.UpdateInfo()
        {
            Time = 40,
            TicksPerUpdate = 0.1f
        });
    }
}
