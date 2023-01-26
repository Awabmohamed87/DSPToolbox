using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Interval {
    public float start { get; set; }
    public float end { get; set; }
    public float midpoint { get; set; }
    public string encoded { get; set; }
    }
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public string encode(int N,int b) {
            string binary = Convert.ToString(N, 2).PadLeft(b, '0');
            return binary;
        }
        public override void Run()
        {
            OutputSamplesError = new List<float>();
            OutputEncodedSignal = new List<string>();
            OutputIntervalIndices = new List<int>();

            //compute number of levels
            if (InputNumBits > 0) 
                InputLevel = (int)Math.Pow(2, InputNumBits);
            //compute number of bits
            else if(InputLevel>0)
                InputNumBits = (int)Math.Log(InputLevel, 2);

            //calculate intervals, midpoints, encoded index
            float delta = (float)Math.Round((InputSignal.Samples.Max() - InputSignal.Samples.Min()) / InputLevel,4);
            List<Interval> intervals = new List<Interval>();
            float start = InputSignal.Samples.Min();
            for (int i = 0; i < InputLevel; i++) {
                Interval interval = new Interval();
                interval.start = start;
                interval.end = start + delta;
                interval.midpoint = (float)Math.Round((interval.start + interval.end) / 2,4);
                interval.encoded = encode(i, InputNumBits);
                intervals.Add(interval);

                start = (float)Math.Round(interval.end,4);
            }

            //Quantize
            List<float> Output = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < intervals.Count; j++) {
                    if (InputSignal.Samples[i]>=intervals[j].start && InputSignal.Samples[i] <= intervals[j].end)
                    {
                        OutputIntervalIndices.Add(j+1);
                        Output.Add(intervals[j].midpoint);
                        OutputSamplesError.Add((float)Math.Round(intervals[j].midpoint - InputSignal.Samples[i],4));
                        OutputEncodedSignal.Add(intervals[j].encoded);
                        break;
                    }

                   }
            }
            
            OutputQuantizedSignal = new Signal(Output,false);
        }
    }
}
