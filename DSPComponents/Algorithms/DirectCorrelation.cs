using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {

            if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples.ToList(), InputSignal1.Periodic);

            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            float sum1 = 0;
            float sum2 = 0;
            
            for(int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                sum1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                sum2 += InputSignal2.Samples[i] * InputSignal2.Samples[i];
            }
            float mqam = (float)Math.Sqrt(sum1 * sum2) / InputSignal1.Samples.Count;
           
         for(int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                float res = 0;
                for (int j = 0; j < InputSignal2.Samples.Count; j++){
                    res += ((InputSignal1.Samples[j] * InputSignal2.Samples[j]) / InputSignal1.Samples.Count);
                }
                OutputNonNormalizedCorrelation.Add(res);
                if (InputSignal1.Periodic)
                {
                    InputSignal2.Samples.Add(InputSignal2.Samples.ElementAt(0));
                    InputSignal2.Samples.RemoveAt(0);
                }
                else {
                    InputSignal2.Samples.RemoveAt(0);
                    InputSignal2.Samples.Add(0);
                }
                OutputNormalizedCorrelation.Add(res/mqam);
            }
            
         
        }
    }
}