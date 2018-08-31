using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Scintia_Thermocycler
{
    static class Program
    {
        /// <summary>
        /// Program Constants
        /// -------------------
        /// Steinhart Constants
        /// </summary>
        public static double c1 = 0.001125308852122, c2 = 0.000234711863267, c3 = 0.000000085663516;
        public static float ADC; //valor del ADC
        public static float voltajeres; //voltaje de la res10k
        public static float voltajetherm; // voltaje del termistor
        public static float Rt; //Valor de resistencia del termistor actual
        public static double Tc; //temperatura en Celsius
        public static double Tcelsius;
        public static string temperatura;
        /// <summary>
        /// Tolerance Constants
        /// </summary>
        public static double upperLimit = 2.0F;
        public static double lowerLimit = 1.0F;
        public static double topTempUpperLimit = 98.0F;
        public static double topTempLowerLimit = 91.0F;
        /// <summary>
        /// Thermal speed constants
        /// </summary>
        public static double tempRaiseConstant = 1.5F;
        public static double tempDropConstant = 2.5F;
        public static double tempNoFanDropConstant = 1.0F;
        public static double tempOnlyTopRaiseConstant = 1.0F;

        /// <summary>
        /// Global variables.
        /// -----------------
        /// Flags
        /// </summary>
        public static bool OKbtn = false;
        public static bool running = false;
        public static bool turn = false;
        public static bool reachedTopTemp = false;
        public static bool reachedTargetTempFirstTime = false;
        public static bool preheat = false;
        /// <summary>
        /// Components states
        /// </summary>
        public static bool readingPort = false;
        public static bool fansOn = false;
        public static bool topROn = false;
        public static bool botROn = false;
        /// <summary>
        /// GUI Helpers
        /// </summary>
        public static string stepTemperature = "0.0";
        public static string stepDuration = "0";
        public static string cycleName = "Main";
        public static string cycleReps = "0";
        /// <summary>
        /// GUI to Logic Helpers
        /// </summary>
        public static List<List<float>> cycleToPerform = new List<List<float>> { };
        /// <summary>
        /// Communication Helpers
        /// </summary>
        public static string aux;
        public static string nuevo;
        /// <summary>
        /// Step Helpers
        /// </summary>
        public static double topTemp;
        public static double botTemp;
        public static int currentStepDuration = 0;
        public static double currentTargetTemperature = 0;
        /// <summary>
        /// Timer Helpers
        /// </summary>
        public static double residualMilliseconds = 0;
        public static int timestamp = 0;
        
        /// <summary>
        /// Global functions.
        /// </summary>
        public static void TraverseTree(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                addToStep(node);
            }
        }

        public static void addToStep(TreeNode node)
        {
            if (node.Text.Contains("Cycle Name") && node.Name != "Root")
            {
                int reps = 0;
                while(reps < (int)node.Tag)
                {
                    TraverseTree(node.Nodes);
                    reps++;
                }
            }
            else if (node.Text.Contains("Temperature"))
            {
                cycleToPerform.Add((List<float>)node.Tag);
            }
        }

        public static bool tempInRange()
        {
            if (botTemp > currentTargetTemperature - lowerLimit)
            {
                if (botTemp < currentTargetTemperature + upperLimit)
                {
                    return true;
                }
            }
            return false;
        }

        public static double inDataToTemp()
        {
            nuevo = new String(aux.TakeWhile(Char.IsDigit).ToArray());
            ADC = Convert.ToInt32(nuevo);
            voltajeres = ((ADC * 5) / 1023);
            voltajetherm = 5 - voltajeres;
            Rt = (10000 * ((5 / voltajetherm) - 1));
            Tc = (1 / (c1 + (c2 * Math.Log(Rt)) + (c3 * (Math.Log(Rt) * Math.Log(Rt) * Math.Log(Rt))))); //Steinhart & Hart formula.
            Tcelsius = Tc - 273.15; //kelvin to celsius
            temperatura = Convert.ToString(Tcelsius);
            return Tcelsius;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
