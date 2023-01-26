using System;
using DSPAlgorithms.DataStructures;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DSPAlgorithms.Algorithms
{
    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency = 4;
        public Signal OutputFreqDomainSignal { get; set; }

        private List<Complex> calcFFT(List<Complex> list1, List<Complex> list2) {
            
            List<Complex> fft = new List<Complex>();
            int N = InputTimeDomainSignal.Samples.Count;
            int n = 0;

            for (int i = 0;i < list1.Count;i++) {
                // W = e^ -2 * pi * n / k
                Complex W = new Complex(Math.Cos(2 * Math.PI * n / N), -1 * Math.Sin(2 * Math.PI * n / N));
                fft.Add(list1[i] + list2[i] * W);
                n += N / (list1.Count * 2);
            }

            for (int i = 0; i < list1.Count; i++)
            {
                Complex W = new Complex(Math.Cos(2 * Math.PI * n / N), -1 * Math.Sin(2 * Math.PI * n / N));
                fft.Add(list1[i] + list2[i] * W);
                n += N / (list1.Count * 2);
            }
            return fft;
        }

        private List<Complex> FFT(List<Complex> list) {
            
            if (list.Count == 1) return list;

            List<Complex> evenList = new List<Complex>();
            List<Complex> oddList = new List<Complex>();

            for (int i = 0; i < list.Count; i++) {
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

            foreach (var i in InputTimeDomainSignal.Samples)
                samples.Add(new Complex(i, 0));
            
            List<Complex> outPut = FFT(samples);

            List<float> amplitudes = new List<float>();
            List<float> phases = new List<float>();
            List<float> samps = new List<float>();
            float frequency = 2 * (float)Math.PI / (InputTimeDomainSignal.Samples.Count / InputSamplingFrequency);

            for (int i = 0; i < outPut.Count; i++) {
                amplitudes.Add((float)outPut[i].Magnitude);
                phases.Add((float)outPut[i].Phase);
                samps.Add(i * frequency);
            }

            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Periodic, samps, amplitudes, phases);

        }
    }
}
