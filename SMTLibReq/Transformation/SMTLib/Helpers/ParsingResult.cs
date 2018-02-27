using System;
using System.Collections.Generic;
using System.Text;

namespace SMTLibReq.Transformation.SMTLib.Helpers
{
    public class ParsingResult
    {
        private int intervalDuration;
        private string smtExpression;

        public int IntervalDuration { get => intervalDuration; set => intervalDuration = value; }
        public string SmtExpression { get => smtExpression; set => smtExpression = value; }

        public ParsingResult(string _smtResult, int _intervalDuration) {
            SmtExpression = _smtResult;
            intervalDuration = 0;
            IntervalDuration = _intervalDuration;
        }

        public ParsingResult(string _smtResult)
        {
            SmtExpression = _smtResult;
            intervalDuration = 0;
        }
        public ParsingResult()
        {
            SmtExpression = "";
            intervalDuration = 0;
        }
    }
}
