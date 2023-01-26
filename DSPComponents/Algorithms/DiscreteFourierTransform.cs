using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            int N = InputTimeDomainSignal.Samples.Count;
            List<float> Output = new List<float>();
            List<float> amplitude = new List<float>();
            List<float> phaseShift = new List<float>();
            float frequency = 2 * (float)Math.PI / (InputTimeDomainSignal.Samples.Count / InputSamplingFrequency);
            frequency = (float)Math.Round(frequency, 4);
            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++) {
                float real = 0, imaginary = 0;
                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++) {
                    
                    real += InputTimeDomainSignal.Samples[j] * (float)Math.Cos((2 * i * j * Math.PI)/N);
                    imaginary += (-1 * InputTimeDomainSignal.Samples[j] * (float)Math.Sin((2 * i * j * Math.PI) / N));
                }
                Output.Add((float)Math.Round((i * frequency), 1));
                Complex tmp = new Complex(real, imaginary);
                amplitude.Add((float)tmp.Magnitude);
                phaseShift.Add((float)tmp.Phase);

            }
            OutputFreqDomainSignal = new Signal(false,Output,amplitude,phaseShift);
        }
    }
}
