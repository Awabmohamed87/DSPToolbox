using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            int isFolded = InputSignal.Periodic ?1:-1;
            List<int> indices = new List<int>();
            for (int i = 0;i<InputSignal.SamplesIndices.Count;i++)
                indices.Add(InputSignal.SamplesIndices[i] + (ShiftingValue * isFolded));
            
            OutputShiftedSignal = new Signal(InputSignal.Samples, indices,true);

        }
    }
}
