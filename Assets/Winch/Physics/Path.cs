using System;
using System.Linq;
using UnityEngine;

namespace Winch
{
    [Serializable]
    public struct SpaceTimePoint
    {
        public int Time; //measured in ticks
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
    }

    public struct Path
    {
        public SpaceTimePoint[] Points { get; private set; }

        public int StartTime { get; private set; }
        public int Time => Points.Last().Time;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="points">Time is relative to the start time</param>
        public Path(int startTime, SpaceTimePoint[] points)
        {
            StartTime = startTime;
            Points = points.OrderBy(point => point.Time).ToArray();
        }
    }
}