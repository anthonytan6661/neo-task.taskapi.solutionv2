

namespace neo_task.taskapi.Models
{
    public enum Status
    {
        STARTED = 0,
        IN_PROGRESS = 1,
        COMPLETED = 2,
        FAILED = 3
    }
    public class NeoTaskModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

      
        public Status Status { get; set; }

        public int CustomerId { get; set; }

    }
}
