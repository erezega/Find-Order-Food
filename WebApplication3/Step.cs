using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3
{
    public class Step
    {
        public int StepNum { get; set; }
        public string StepName { get; set; }
        public int StepTime { get; set; }
        public string StepType { get; set; }
        public bool IsNeedAttension { get; set; }
        public List<int> DependOnSteps { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsStepFinished { get; set; }

        public Step(int stepNum, string stepName, int stepTime, string stepType, bool isNeedAttension, List<int> dependOnSteps, bool isInProgress, bool isStepFinished)
        {
            StepNum = stepNum;
            StepName = stepName;
            StepTime = stepTime;
            StepType = stepType;
            IsNeedAttension = isNeedAttension;
            DependOnSteps = dependOnSteps;
            IsInProgress = isInProgress;
            IsStepFinished = isStepFinished;
        }
    }
}