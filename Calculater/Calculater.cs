using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;

namespace Calculater
{
    /// <summary>
    /// A headless wrapper for a Windows calculator. Only standard mode is used.
    /// </summary>
    public class Calculater
    {
        private AutomationElement _mainWindow; // Main Calculator window
        private AutomationElement _coreWindow;
        private AutomationElement _mainGroup;

        private IDictionary<char, Action> _controlsMap;

        private AutomationElement StandardOperators => _mainGroup.FindFirstChildrenById("StandardOperators");
        private AutomationElement NumberPad => _mainGroup.FindFirstChildrenById("NumberPad");
        private AutomationElement DisplayControls => _mainGroup.FindFirstChildrenById("DisplayControls");

        public Calculater()
        {
            var calcProcess = System.Diagnostics.Process.Start("calc.exe");
            if (calcProcess == null) throw new Exception("Cannot start 'calc.exe'");

            calcProcess.WaitForInputIdle();
            Thread.Sleep(500); // wait a bit to be sure that Calculator is started

            Init();
            ValidateMode();
        }

        public virtual double Calculate(string expression)
        {
            Input('C'); // Clear previous operation

            foreach (var character in expression)
            {
                Input(character);
            }

            Input('=');

            return GetCurrentResults();
        }

        public void Close()
        {
            var titleBar = _mainWindow?.FindFirstChildrenById("TitleBar");
            var closeButton = titleBar?.FindFirstChildrenById("Close");
            closeButton?.Click();
        }

        private void Init()
        {
            // TODO: It's better to find by PID or smth
            _mainWindow = AutomationElement.RootElement.FindLastChildren(
                new PropertyCondition(AutomationElement.NameProperty, "Calculator")
            );
            if (_mainWindow == null) throw new Exception("Cannot find Calculator window.");

            _coreWindow = _mainWindow.FindFirstChildrenByClassName("Windows.UI.Core.CoreWindow");
            if (_coreWindow == null) throw new Exception("Cannot find CoreWindow class.");

            _mainGroup = _coreWindow.FindFirstChildrenByClassName("LandmarkTarget");

            _controlsMap = new Dictionary<char, Action>
            {
                {'N', _mainGroup.FindFirstChildrenById("negateButton").Click},
                {'C', DisplayControls.FindFirstChildrenById("clearButton").Click},

                {'/', StandardOperators.FindFirstChildrenById("divideButton").Click},
                {'*', StandardOperators.FindFirstChildrenById("multiplyButton").Click},
                {'-', StandardOperators.FindFirstChildrenById("minusButton").Click},
                {'+', StandardOperators.FindFirstChildrenById("plusButton").Click},

                {'=', StandardOperators.FindFirstChildrenById("equalButton").Click},

                {'0', NumberPad.FindFirstChildrenById("num0Button").Click},
                {'1', NumberPad.FindFirstChildrenById("num1Button").Click},
                {'2', NumberPad.FindFirstChildrenById("num2Button").Click},
                {'3', NumberPad.FindFirstChildrenById("num3Button").Click},
                {'4', NumberPad.FindFirstChildrenById("num4Button").Click},
                {'5', NumberPad.FindFirstChildrenById("num5Button").Click},
                {'6', NumberPad.FindFirstChildrenById("num6Button").Click},
                {'7', NumberPad.FindFirstChildrenById("num7Button").Click},
                {'8', NumberPad.FindFirstChildrenById("num8Button").Click},
                {'9', NumberPad.FindFirstChildrenById("num9Button").Click},

                {'.', NumberPad.FindFirstChildrenById("decimalSeparatorButton").Click},
                {',', NumberPad.FindFirstChildrenById("decimalSeparatorButton").Click},
            };
        }

        private void ValidateMode()
        {
            var mode = _coreWindow.FindFirstChildrenById("Header").Current.Name;

            // TODO: Set Standard mode automatically
            // TODO: Check if this value is localizable
            if (!mode.Contains("Standard")) throw new Exception("Mode must be 'Standard'");
        }

        private void Input(char character)
        {
            if (!_controlsMap.ContainsKey(character)) throw new NotImplementedException();

            _controlsMap[character]();
        }

        private double GetCurrentResults()
        {
            var calcResults = _mainGroup.FindFirstChildrenById("CalculatorResults");
            return double.Parse(calcResults.Current.Name.Substring("Display is ".Length));
        }
    }
}