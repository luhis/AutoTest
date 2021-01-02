using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Models
{
    public class EntrantTimes
    {
        public EntrantTimes(Entrant entrant, int totalTime, IEnumerable<TestTime> times, int position, int classPosition)
        {
            Entrant = entrant;
            TotalTime = totalTime;
            Times = times;
            Position = position;
            ClassPosition = classPosition;
        }

        public Entrant Entrant { get; }

        public IEnumerable<TestTime> Times { get; }

        public int TotalTime { get; }

        public int Position { get; }

        public int ClassPosition { get; }
    }
}
