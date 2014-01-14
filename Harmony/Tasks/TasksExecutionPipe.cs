using System;
using Harmony.Sdk.Reactive;

namespace Harmony.Sdk.Tasks
{
    public class TasksExecutionPipe
    {
        public AbstractObservable<TaskBase> Planned { get; private set; }

        public AbstractObservable<TaskBase> BeforeRun { get; private set; }

        public AbstractObservable<TaskBase> AfterRun { get; private set; }

        public TasksExecutionPipe()
        {
        }
    }
}

