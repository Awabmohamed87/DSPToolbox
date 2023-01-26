using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            FastFourierTransform fft = new FastFourierTransform();
            FastFourierTransform fft2 = new FastFourierTransform();
            InverseFastFourierTransform IFFT = new InverseFastFourierTransform();

            int N = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            while(InputSignal1.Samples.Count < N)
                InputSignal1.Samples.Add(0);
            while (InputSignal2.Samples.Count < N)
                InputSignal2.Samples.Add(0);

            fft.InputTimeDomainSignal = InputSignal1;
            fft2.InputTimeDomainSignal = InputSignal2;
            fft.Run();
            fft2.Run();

            IFFT.InputFreqDomainSignal = new Signal(InputSignal1.Periodic, new List<float>(), new List<float>(), new List<float>());

            for (int i = 0;i<fft.OutputFreqDomainSignal.FrequenciesAmplitudes.Count;i++) {
                Complex c1 = Complex.FromPolarCoordinates(fft.OutputFreqDomainSignal.FrequenciesAmplitudes[i], fft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                Complex c2 = Complex.FromPolarCoordinates(fft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i], fft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                
                IFFT.InputFreqDomainSignal.FrequenciesAmplitudes.Add((float)(c1 * c2).Magnitude);
                IFFT.InputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)(c1 * c2).Phase);
            }

            IFFT.Run();

            OutputConvolvedSignal = IFFT.OutputTimeDomainSignal;

        }
    }
}
