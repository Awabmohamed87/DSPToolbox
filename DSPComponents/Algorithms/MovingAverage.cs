using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            Console.WriteLine( "in: "+ InputSignal.Samples.Count);
            List<float> outputs = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count - InputWindowSize+1; i++) {
                float res = 0;
                for (int j = 0; j < InputWindowSize; j++) {
                    
                res += InputSignal.Samples[i + j];
                }
                outputs.Add((float)Math.Round(res / InputWindowSize,4));
            }
            OutputAverageSignal = new Signal(outputs,false);
        }
    }
}
