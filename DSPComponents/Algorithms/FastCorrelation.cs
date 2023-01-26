using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public float calcNorm(List<float> lst1, List<float> lst2) {
            float sum1 = 0, sum2 = 0;
            for (int i = 0;i<lst1.Count;i++) {
            sum1+= lst1[i] * lst1[i]; 
            sum2+= lst2[i] * lst2[i];
            }
            return (float)Math.Sqrt(sum1 * sum2) / lst1.Count;
        }

        public override void Run()
        {
            // Auto correlation
            if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples.ToList(), InputSignal1.Periodic);

            float norm = calcNorm(InputSignal1.Samples, InputSignal2.Samples);

            FastFourierTransform FFT = new FastFourierTransform();
            FFT.InputTimeDomainSignal = InputSignal1;
            FFT.Run();
            
            FastFourierTransform FFT2 = new FastFourierTransform();
            FFT2.InputTimeDomainSignal = InputSignal2;
            FFT2.Run();

            InverseFastFourierTransform IFFT = new InverseFastFourierTransform();
            IFFT.InputFreqDomainSignal = new Signal(InputSignal1.Periodic, new List<float>(), new List<float>(), new List<float>());

            for (int i = 0; i < FFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts.Count; i++)
            {
                Complex c1 = Complex.FromPolarCoordinates(FFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i], FFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                Complex c2 = Complex.FromPolarCoordinates(FFT2.OutputFreqDomainSignal.FrequenciesAmplitudes[i], FFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);


                IFFT.InputFreqDomainSignal.FrequenciesAmplitudes.Add((float)(Complex.Conjugate(c1) * c2).Magnitude);
                IFFT.InputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)(Complex.Conjugate(c1) * c2).Phase);
            }

            IFFT.Run();

            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            
            foreach(var i in IFFT.OutputTimeDomainSignal.Samples)
            {
                OutputNonNormalizedCorrelation.Add(i / InputSignal1.Samples.Count);
                OutputNormalizedCorrelation.Add((i / InputSignal1.Samples.Count) / norm);
            }
            
        }
    }
}