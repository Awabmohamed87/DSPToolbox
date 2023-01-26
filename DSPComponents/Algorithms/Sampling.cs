﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        private FIR setupFilter()
        {
            FIR fir = new FIR();
            fir.InputFilterType = FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;
            return fir;
        }

        private List<float> UpSample(List<float> list)
        {
            List<float> results = new List<float>();
            for (int i = 0; i < list.Count; i++)
            {
                results.Add(InputSignal.Samples[i]);
                if (i == InputSignal.Samples.Count - 1)
                {
                    results.Add(0); continue;
                }
                for (int j = 0; j < L - 1; j++)
                    results.Add(0);
            }

            return results;
        }
        private List<float> DownSample(List<float> list)
        {
            List<float> results = new List<float>();
            for (int i = 0; i < list.Count; i += M)
                results.Add(list[i]);

            return results;
        }
        public override void Run()
        {
            FIR LowPassFilter = setupFilter();
            // Up sample 
            if (M == 0 && L != 0) 
            {
                LowPassFilter.InputTimeDomainSignal = new Signal(UpSample(InputSignal.Samples), InputSignal.Periodic);
                LowPassFilter.Run();
                OutputSignal = LowPassFilter.OutputYn;
            }
            // Down sample
            else if (M != 0 && L == 0) 
            {
                LowPassFilter.InputTimeDomainSignal = InputSignal;
                LowPassFilter.Run();
                OutputSignal = new Signal(DownSample(LowPassFilter.OutputYn.Samples), InputSignal.Periodic);
            }
            // Up then Down
            else if(M != 0 && L != 0)
            {
                LowPassFilter.InputTimeDomainSignal = new Signal(UpSample(InputSignal.Samples), InputSignal.Periodic);
                LowPassFilter.Run();
                OutputSignal = new Signal(DownSample(LowPassFilter.OutputYn.Samples), InputSignal.Periodic);
            }

        }
    }

}