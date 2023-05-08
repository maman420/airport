using System.Timers;
using Timer = System.Timers.Timer;

namespace server.Models
{
    public abstract class Leg
    {
        public DateTime startTime { get; set; }
        public DateTime ExitTime { get; set; }
        public Leg()
        {
            this.startTime = DateTime.Now;                
            // Create a timer to check for the exit time
            Timer timer = new Timer(1000); // 1000 milliseconds = 1 second
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        public virtual void OnTimerElapsed(object source, ElapsedEventArgs e)
        {

        }
    }
}