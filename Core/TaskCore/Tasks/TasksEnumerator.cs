using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sisyphus.Common;

namespace Sisyphus.Tasks
{
    public class TasksEnumerator : IEnumerable<Type>
    {
        private readonly List<Type> _list = new List<Type>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        private TasksEnumerator()
        {
            CommonFunctions.LoadAllSolutionAssemblies();
            foreach (var asemblyName in AppDomain.CurrentDomain.GetAssemblies().OrderBy(t => t.FullName))
            {
                try
                {
                    var assembly = Assembly.Load(asemblyName.ToString());
                    var q = from t in assembly.GetTypes()
                        where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(TaskBase))
                        select t;
                    foreach (var type in q)
                        _list.Add(type);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static TasksEnumerator GetTaskEnumertor()
        {
            return new TasksEnumerator();
        }
    }
}