using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct TimeSpacePoint
{
    public int Time; //measured in ticks
    public Vector3 Position;
    public bool UseVelocity;
    public Vector3 Velocity;
}

public struct Path
{
    public TimeSpacePoint[] Points { get; private set; }

    public int StartTime { get; private set; }
    public int Time => Points.Last().Time;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="points">Time is relative to the start time</param>
    public Path(int startTime, TimeSpacePoint[] points)
    {
        StartTime = startTime;
        Points = points.OrderBy(point => point.Time).ToArray();
    }
}