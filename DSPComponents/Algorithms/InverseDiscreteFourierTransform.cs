using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        List<Complex> convertToComplex(List<float> amplitudes, List<float> phaseShifts) {
        List<Complex> convertedNumbers = new List<Complex>();
            for (int j = 0;j<amplitudes.Count;j++)
                convertedNumbers.Add(Complex.FromPolarCoordinates(amplitudes[j],phaseShifts[j]));

            return convertedNumbers;
        }

        public override void Run()
        {
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            List<Complex> inputsAsComplexNumbers = convertToComplex(InputFreqDomainSignal.FrequenciesAmplitudes,
                InputFreqDomainSignal.FrequenciesPhaseShifts);

            List<float> Output = new List<float>();
            for (int k = 0; k < N; k++)
            {
                float R = 0, I = 0;
                for (int j = 0; j < N; j++)
                {

                    float real = (float)Math.Cos((2 * k * j * Math.PI) / N);
                    float imaginary = (float)Math.Sin((2 * k * j * Math.PI) / N);
                    Complex tmp = new Complex(real, imaginary);

                    R += (float)(tmp * inputsAsComplexNumbers[j]).Real;
                }

                Output.Add((float) R / N);

            }
            OutputTimeDomainSignal = new Signal(Output, false);
        }
    }
}
