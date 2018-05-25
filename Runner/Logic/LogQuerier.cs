using System;
using System.Collections.Generic;
using MSUtil;

namespace HorseSpeed.Runner.Logic
{
    public class LogQuerier
    {
        public static IReadOnlyList<int> QueryLogs(string fromClause, string whereClause)
        {
            var query = $"SELECT time-taken AS TimeTaken FROM {fromClause} WHERE {whereClause} ORDER BY TimeTaken";

            var timesTaken = new List<int>();

            var logParser = new LogQueryClassClass();
            var iisLog = new COMIISW3CInputContextClassClass();

            var recordSet = logParser.Execute(query, iisLog);

            while (!recordSet.atEnd())
            {
                var recordRow = recordSet.getRecord();
                timesTaken.Add(Convert.ToInt32(recordRow.getValue(0)));

                recordSet.moveNext();
            }

            return timesTaken;
        }
    }
}