using System;
using System.Diagnostics;
using Grasshopper.Kernel;

namespace Culebra_GH.Utilities
{
    /// <summary>
    /// Custom Stopwatch Class handles component computing time 
    /// </summary>
    public class Timer : Stopwatch
    {
        /// <summary>
        /// Getter Setter for the start time 
        /// </summary>
        public DateTime? StartAt { get; private set; }
        /// <summary>
        /// Getter Setter for the end time
        /// </summary>
        public DateTime? EndAt { get; private set; }
        /// <summary>
        /// Starts the stop watch
        /// </summary>
        public void Start()
        {
            StartAt = DateTime.Now;

            base.Start();
        }
        /// <summary>
        /// Stops the stop watch
        /// </summary>
        public void Stop()
        {
            EndAt = DateTime.Now;

            base.Stop();
        }
        /// <summary>
        /// Resets the stop watch
        /// </summary>
        public void Reset()
        {
            StartAt = null;
            EndAt = null;

            base.Reset();
        }
        /// <summary>
        /// Restarts the stop watch
        /// </summary>
        public void Restart()
        {
            StartAt = DateTime.Now;
            EndAt = null;

            base.Restart();
        }
        /// <summary>
        /// Sets the message at the bottom of the Grasshopper component with run time information
        /// </summary>
        /// <param name="Component">The component to display</param>
        /// <param name="engineType">Which engine are we running</param>
        public void DisplayMessage(IGH_Component Component, string engineType, int cycles, bool display)
        {
            GH_Component thisobj = Component as GH_Component;
            if (display)
            {
                if (engineType == "Single")
                {
                    thisobj.Message = "Creeper Engine | Cycles : " + cycles;
                }
                else if (engineType == "Double")
                {
                    thisobj.Message = "Multi Creeper Engine | Cycles : " + cycles;
                }else if(engineType == "Bundling")
                {
                    thisobj.Message = "Bundling Engine | Cycles : " + cycles;
                }
            }
            else
            {
                thisobj.Message = "";
            }
        }
    }
}
