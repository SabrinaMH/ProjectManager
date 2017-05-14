namespace ProjectManager.Domain
{
    public class TaskPriority : Enumeration<TaskPriority, int>
    {
        public static TaskPriority None = new TaskPriority(0, "none");
        public static TaskPriority Lowest = new TaskPriority(1, "lowest");
        public static TaskPriority Low = new TaskPriority(1, "low");
        public static TaskPriority Medium = new TaskPriority(2, "medium");
        public static TaskPriority High = new TaskPriority(3, "high");
        public static TaskPriority Highest = new TaskPriority(3, "highest");

        public TaskPriority(int value, string displayName) : base(value, displayName)
        {
        }
    }
}