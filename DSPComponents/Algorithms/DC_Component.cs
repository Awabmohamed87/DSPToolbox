using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        float calc_mean(List<float> Samples)
        {
            float sum = 0;
            for (int i = 0; i < Samples.Count; i++)
            {

                sum += Samples[i];
            }
            return sum/ Samples.Count;
        }
        public override void Run()
        {
            float mean = calc_mean(InputSignal.Samples);

            List<float> samples = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++) 
            samples.Add(InputSignal.Samples[i]-mean);
            
            OutputSignal = new Signal(samples, false);
        }
    }
}
