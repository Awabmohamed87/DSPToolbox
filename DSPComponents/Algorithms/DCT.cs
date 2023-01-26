using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> outputs = new List<float>();
            float N = InputSignal.Samples.Count;
            for(int i = 0;i < InputSignal.Samples.Count;i++)
            {
                float res = 0;
                for (int j = 0;j < InputSignal.Samples.Count;j++) {
                    // DCT = sqrt(2 / N) * sum(x[i] * cos(pi / 4N )*(2j - 1)*(2k - 1))
                    res += InputSignal.Samples[j] * (float)Math.Cos(((float)Math.PI / (4 * N))*(2 * j - 1)*(2 * i - 1));
                }
                
                    outputs.Add((float)Math.Sqrt(2 / N) * res);
            }
            OutputSignal = new Signal(outputs, InputSignal.Periodic);
        }
    }
}
