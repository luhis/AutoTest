//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AutoTest.Domain.StorageModels;
//using AutoTest.Service.Models;

//namespace AutoTest.Service.ResultCalculation
//{
//    public static class EntrantTimeHelpers
//    {
//        public static EntrantTimes ToEntrantTimes(this (Entrant entrant, IEnumerable<TestRun> runs, int totalTime) entrantTimes)
//        {
//            return new EntrantTimes(entrantTimes.entrant, entrantTimes.totalTime, entrantTimes.runs.GroupBy(a => a.Ordinal).Select(r =>
//                    new TestTime(testsDict[r.Key].Ordinal, r)), Array.IndexOf(entrantsAndRuns, x), index))
//        }
//    }
//}
