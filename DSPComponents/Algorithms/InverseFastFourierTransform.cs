using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseFastFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        private List<Complex> calcFFT(List<Complex> list1, List<Complex> list2)
        {

            List<Complex> fft = new List<Complex>();
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            int n = 0;

            for (int i = 0; i < list1.Count; i++)
            {

                Complex W = new Complex(Math.Cos(2 * Math.PI * n / N), Math.Sin(2 * Math.PI * n / N));
                fft.Add(list1[i] + list2[i] * W);
                n += N / (list1.Count * 2);
            }

            for (int i = 0; i < list1.Count; i++)
            {
                Complex W = new Complex(Math.Cos(2 * Math.PI * n / N), Math.Sin(2 * Math.PI * n / N));
                fft.Add(list1[i] + list2[i] * W);
                n += N / (list1.Count * 2);
            }
            return fft;
        }

        private List<Complex> FFT(List<Complex> list)
        {

            if (list.Count == 1) return list;

            List<Complex> evenList = new List<Complex>();
            List<Complex> oddList = new List<Complex>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                    evenList.Add(list[i]);

                else
                    oddList.Add(list[i]);
            }

            return calcFFT(FFT(evenList), FFT(oddList));
        }

        public override void Run()
        {

            List<Complex> samples = new List<Complex>();

            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count;i++)
                samples.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[i],
                    InputFreqDomainSignal.FrequenciesPhaseShifts[i]));

            List<Complex> outPut = FFT(samples);

            List<float> samps = new List<float>();

            for (int i = 0; i < outPut.Count; i++)
            {
                samps.Add((float)outPut[i].Real / InputFreqDomainSignal.FrequenciesAmplitudes.Count);
            }

            OutputTimeDomainSignal = new Signal(samps,InputFreqDomainSignal.Periodic);

        }
    }
}
