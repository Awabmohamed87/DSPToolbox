﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public Signal InputSignal;
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        private FIR applyFilter(Signal InputSignal)
        {
            FIR fir = new FIR();
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.InputTimeDomainSignal = InputSignal;

            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;

            fir.InputFS = Fs;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;

            fir.Run();
            return fir;
        }
        private Sampling applySampling(Signal InputSignal)
        {
            Sampling sampling = new Sampling();
            sampling.InputSignal = InputSignal;
            sampling.L = L;
            sampling.M = M;
            sampling.Run();

            return sampling;
        }
        private DC_Component applyDC(Signal InputSignal)
        {
            DC_Component dc = new DC_Component();
            dc.InputSignal = InputSignal;
            dc.Run();
            return dc;
        }

        private Normalizer applyNormalization(Signal InputSignal)
        {
            Normalizer normalizer = new Normalizer();
            normalizer.InputMaxRange = 1;
            normalizer.InputMinRange = -1;
            normalizer.InputSignal = InputSignal;
            normalizer.Run();
            return normalizer;
        }
        private DiscreteFourierTransform applyDFT(Signal InputSignal)
        {
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = InputSignal;
            dft.InputSamplingFrequency = (int)Fs;
            dft.Run();
            return dft;
        }
        public override void Run()
        {
            FIR BandPassFilter = applyFilter(InputSignal);
            
            Sampling sampling = applySampling(BandPassFilter.OutputYn);
            
            DC_Component dc = applyDC(newFs >= 2 * maxF? sampling.OutputSignal: BandPassFilter.OutputYn);
            
            Normalizer normalizer = applyNormalization(dc.OutputSignal);
            
            DiscreteFourierTransform dft = applyDFT(normalizer.OutputNormalizedSignal);
            
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;

            string s = Directory.GetCurrentDirectory();
            string path = s.Remove(s.Length - 31, 31) + "OutputSignals";

            ExportSignal(path + "\\InputSignal.ds", InputSignal);
            ExportSignal(path + "\\FilteredSignal.ds", BandPassFilter.OutputYn);
            ExportSignal(path + "\\DCedSignal.ds", dc.OutputSignal);
            ExportSignal(path + "\\NormalizedSignal.ds", normalizer.OutputNormalizedSignal);
            ExportSignal(path + "\\FinalSignal.ds", OutputFreqDomainSignal);

        }
        public void ExportSignal(String SignalPath, Signal signal)
        {
            using (StreamWriter writer = File.CreateText(SignalPath))
            {
                if (signal.Frequencies == null || signal.Frequencies.Count == 0)
                    writer.WriteLine(0);
                else
                    writer.WriteLine(1);

                if (signal.Periodic == false)
                    writer.WriteLine(0);
                else
                    writer.WriteLine(1);

                if(signal.Samples != null)
                    writer.WriteLine(signal.Samples.Count);
                else
                    writer.WriteLine(signal.Frequencies.Count);

                if (signal.Frequencies == null || signal.Frequencies.Count == 0)
                {
                    for (int i = 0; i < signal.Samples.Count; i++)
                    {
                        writer.Write(signal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(signal.Samples[i]);

                    }
                }
                else
                {
                    for (int i = 0; i < signal.Frequencies.Count; i++)
                    {
                        writer.Write(signal.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(signal.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(signal.FrequenciesPhaseShifts[i]);
                    }
                }

                writer.Flush();
                writer.Close();
            }
        }

    }
}
