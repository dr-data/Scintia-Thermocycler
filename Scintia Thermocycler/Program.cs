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
        /// </summary>
        /// 
        /// Steinhart Constants
        public static double c1 = 0.001125308852122, c2 = 0.000234711863267, c3 = 0.000000085663516;
        public static float upperLimit = 2.0F;
        public static float lowerLimit = 1.0F;
        public static float tempRaiseConstant = 1.5F;
        public static float tempDropConstant = 2.5F;
        public static float tempNoFanDropConstant = 1.0F;
        public static float tempOnlyTopRaiseConstant = 1.0F;
        public static float topTempUpperLimit = 94.0F;
        public static float topTempLowerLimit = 91.0F;

        /// <summary>
        /// Global variables.
        /// -----------------
        /// Flags
        /// </summary>
        public static bool OKbtn = false;
        public static bool running = false;
        public static bool readingPort = false;
        public static bool readingBottomTemp = false;
        public static bool reachedTargetTempFirstTime = false;
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
        public static float topTemp;
        public static float botTemp;
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
            foreach (TreeNode child in nodes)
            {
                addToStep(child);
                TraverseTree(child.Nodes);
            }
        }

        public static void addToStep(TreeNode node)
        {
            if (node.Name == "Root")
            {
                return;
            }
            else if (node.Text.Contains("Cycle Name"))
            {
                int reps = 0;
                while(reps < (int)node.Tag){
                    foreach (TreeNode child in node.Nodes)
                    {
                        addToStep(child);
                    }
                    reps++;
                }
            }
            else if (node.Text.Contains("Temperature") && node.Parent.Name == "Root")
            {
                cycleToPerform.Add((List<float>)node.Tag);
            }
        }

        public static List<double> inDataToTemp(String inputData)
        {
            List<double> result = new List<double> { };
            string testData = Regex.Match(inputData, @"\d+").Value;
            int ADC = Int32.Parse(testData);
            double voltres = ((ADC * 5) / 512);
            double volttherm = 5 - voltres;
            double Rt = (10000 * ((5 / volttherm) - 1));
            double Tkelvin = (1 / (c1 + (c2 * Math.Log(Rt)) + (c3 * (Math.Log(Rt) * Math.Log(Rt) * Math.Log(Rt)))));
            double Tcelsius = Tkelvin - 273.15;

            result.Add(Tcelsius);
            result.Add(Tkelvin);
            result.Add(Rt);
            result.Add(volttherm);
            result.Add(voltres);
            result.Add((double)ADC);
            return result;
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
