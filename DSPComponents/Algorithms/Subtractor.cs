using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
          /* 
            List<float> outSignals = new List<float>();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)

                outSignals.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);

            OutputSignal = new Signal(outSignals, false);
            */
            
            MultiplySignalByConstant multiplySignalByConstant = new MultiplySignalByConstant();
            multiplySignalByConstant.InputConstant = -1;
            multiplySignalByConstant.InputSignal = InputSignal2;
            multiplySignalByConstant.Run();
         
            Adder adder = new Adder();
            adder.InputSignals = new List<Signal>();
            adder.InputSignals.Add(InputSignal1);
            adder.InputSignals.Add(multiplySignalByConstant.OutputMultipliedSignal);
            adder.Run();
            
            OutputSignal = adder.OutputSignal;

            
        }
    }
}