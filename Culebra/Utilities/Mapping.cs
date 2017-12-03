using System.Collections.Generic;

namespace CulebraData.Utilities
{
    /// <summary>
    /// Provides a set of static mapping Utilities
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// Remaps a value 
        /// </summary>
        /// <param name="value">value to map</param>
        /// <param name="istart">source min value</param>
        /// <param name="istop">source max value</param>
        /// <param name="ostart">target min value</param>
        /// <param name="ostop">target max value</param>
        /// <returns>the mapped float value</returns>
        public static float Map(float value, float istart, float istop, float ostart, float ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }
        /// <summary>
        /// Reparametrizes Values
        /// </summary>
        /// <param name="dataToMap">data to map</param> 
        /// <param name="sourceMax">source max value</param> 
        /// <returns>the list of mapped values</returns> 
        public static List<double> RemapValues(List<double> dataToMap, double sourceMax)
        {
            double oldMin = 0.0;
            double NewMax = 1.0;
            double NewMin = 0.0;
            for (int i = 0; i < dataToMap.Count; i++)
            {
                double oldValue = dataToMap[i];
                double oldRange = (sourceMax - oldMin);
                double newRange = (NewMax - NewMin);
                dataToMap[i] = (((oldValue - oldMin) * newRange) / oldRange) + NewMin;
            }
            return dataToMap;
        }
    }
}
