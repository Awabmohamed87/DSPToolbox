using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        float getWindow(int n, int N) {
            if (InputStopBandAttenuation <= 21)
                return 1;
            else if (InputStopBandAttenuation <= 44)
                return (float)(0.5 + 0.5 * Math.Cos(2 * Math.PI * n / N));
            else if (InputStopBandAttenuation <= 53)
                return (float)(0.54 + 0.46 * Math.Cos(2 * Math.PI * n / N));
            else
                return (float)(0.42 + 0.5 * Math.Cos(2 * Math.PI * n / (N - 1)) + 0.08 * Math.Cos(4 * Math.PI * n / (N-1)));
        }

        public override void Run()
        {
            OutputHn = new Signal(new List<float>(),new List<int>(), InputTimeDomainSignal.Periodic);
            
            int N;
            if (InputStopBandAttenuation <= 21)
                N = (int)Math.Ceiling(0.9 / (InputTransitionBand / InputFS));
            else if(InputStopBandAttenuation <= 44)
                N = (int)Math.Ceiling(3.1 / (InputTransitionBand / InputFS));
            else if(InputStopBandAttenuation <= 53)
                N = (int)Math.Ceiling(3.3 / (InputTransitionBand / InputFS));
            else
                N = (int)Math.Ceiling(5.5 / (InputTransitionBand / InputFS));

            N = N % 2 == 0 ? N + 1 : N;

            if (InputFilterType == FILTER_TYPES.LOW) {
                float fc = (float)((InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS);
                for (int i = 0;i <= N / 2;i++)
                {
                    float h;
                    if (i == 0)
                        h = 2 * fc;
                    else
                    {
                        h = 2 * fc * (((float)Math.Sin(i * 2 * Math.PI * fc)) / (i * 2 * (float)Math.PI * fc));
                    }
                    float w = getWindow(i, N);
                    OutputHn.Samples.Add(w * h);
                    OutputHn.SamplesIndices.Add(i);
                    if(i > 0)
                    {
                        OutputHn.Samples.Insert(0, w * h);
                        OutputHn.SamplesIndices.Insert(0, i * -1);
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH) {
                float fc = (float)((InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS);
                for (int i = 0; i <= N / 2; i++)
                {
                    float h;
                    if (i == 0)
                        h = 1 - (2 * fc);
                    else
                    {
                        h = - 2 * fc * (((float)Math.Sin(i * 2 * Math.PI * fc)) / (i * 2 * (float)Math.PI * fc));
                    }
                    float w = getWindow(i, N);
                    OutputHn.Samples.Add((float)(w * h));
                    OutputHn.SamplesIndices.Add(i);
                    if (i > 0)
                    {
                        OutputHn.Samples.Insert(0, (float)(w * h));
                        OutputHn.SamplesIndices.Insert(0, i * -1);
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS) {
                float fc1 = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                float fc2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                for (int i = 0; i <= N / 2; i++)
                {
                    float h;
                    if (i == 0)
                        h = 2 * (fc2 - fc1);
                    else
                    {
                        h = 2 * fc2 * (((float)Math.Sin(i * 2 * Math.PI * fc2)) / (i * 2 * (float)Math.PI * fc2))
                         - 2 * fc1 * (((float)Math.Sin(i * 2 * Math.PI * fc1)) / (i * 2 * (float)Math.PI * fc1));
                    }
                    float w = getWindow(i, N);
                    OutputHn.Samples.Add((float)(w * h));
                    OutputHn.SamplesIndices.Add(i);
                    if (i > 0)
                    {
                        OutputHn.Samples.Insert(0, (float)(w * h));
                        OutputHn.SamplesIndices.Insert(0, i * -1);
                    }
                }

            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP) {
                float fc1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                float fc2 = (float)((InputF2 - (InputTransitionBand / 2)) / InputFS);
                for (int i = 0; i <= N / 2; i++)
                {
                    float h;
                    if (i == 0)
                        h = 1 - 2 * (fc2 - fc1);
                    else
                    {
                        h = 2 * fc1 * (((float)Math.Sin(i * 2 * Math.PI * fc1)) / (i * 2 * (float)Math.PI * fc1))
                         - 2 * fc2 * (((float)Math.Sin(i * 2 * Math.PI * fc2)) / (i * 2 * (float)Math.PI * fc2));
                    }
                    float w = getWindow(i, N);
                    OutputHn.Samples.Add((float)(w * h));
                    OutputHn.SamplesIndices.Add(i);
                    if (i > 0)
                    {
                        OutputHn.Samples.Insert(0, (float)(w * h));
                        OutputHn.SamplesIndices.Insert(0, i * -1);
                    }
                }
            }

            DirectConvolution directConvolution = new DirectConvolution();
            directConvolution.InputSignal1 = OutputHn;
            directConvolution.InputSignal2 = InputTimeDomainSignal;
            directConvolution.Run();
            OutputYn = directConvolution.OutputConvolvedSignal;
            
        }
    }
}
