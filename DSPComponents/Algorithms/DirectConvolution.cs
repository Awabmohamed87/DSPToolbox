using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //setting min, max, start 
            int min, max, k;
            if (InputSignal1.SamplesIndices.Count > 0)
            {
                min = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
                max = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();
                k = InputSignal1.SamplesIndices.Min() < InputSignal2.SamplesIndices.Min() ?
                InputSignal1.SamplesIndices.Min() : InputSignal2.SamplesIndices.Min();
            }
            else {
                min = 0;k = 0;
                max = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 2;
                InputSignal1.SamplesIndices = new List<int>();
                InputSignal2.SamplesIndices = new List<int>();
                for (int i = 0;i<InputSignal1.Samples.Count;i++) 
                    InputSignal1.SamplesIndices.Add(i);

                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    InputSignal2.SamplesIndices.Add(i);
            }
            //-----------------

            //Convolution
            List<float> results = new List<float>();
            List<int> indices = new List<int>();
            for (int i = min;i <= max ; i++) {
                float res = 0;
                int localK = k;
                while (localK != max) {
                    if (InputSignal1.SamplesIndices.IndexOf(localK) == -1 || InputSignal2.SamplesIndices.IndexOf(i - localK) == -1) {
                        localK++; continue;
                    }   
                    
                    float x = InputSignal1.Samples[InputSignal1.SamplesIndices.IndexOf(localK)];
                    float h = InputSignal2.Samples[InputSignal2.SamplesIndices.IndexOf(i - localK)];
                    res += (x * h);
                    localK++;
                }
                if (res == 0 && i == max) continue;
                results.Add(res);
                indices.Add(i);
            }
            
            OutputConvolvedSignal = new Signal(results, indices, false);
        }
    }
}
