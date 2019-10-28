using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorWpf;
using System.Windows.Input;

namespace CalculatorWpf.ViewsModel
{
    public class CalcyulatorVM : ObservableObjects
    {
        private enum operations
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide,
            Precent,
            Squared,
            SqareRoot,
            Sin,
            Cos,
            Tan,
            Equals,
            Pi
            

        }

        private Dictionary<string, operations> BianaryOperatons = new Dictionary<string, operations>()
        {
            {"+", operations.Add },
            {"-", operations.Subtract },
            {"*", operations.Multiply },
            {"/", operations.Divide },
            {"%", operations.Precent },
            {"=", operations.Equals }
            
        };

        private Dictionary<string, operations> UnaryOperations = new Dictionary<string, operations>()
        {
            {"Sin", operations.Sin },
            {"Cos", operations.Cos },
            {"Tan", operations.Tan },
            {"Sqr", operations.Squared },
            {"SqRt", operations.SqareRoot },
            {"Pi", operations.Pi },
        };

        private static string operandstring;
        private static double _operand1;
        private static double _operand2;
        private static operations _binaryOperator;

        public ICommand ButtonNumberCommand { get; set; }
        public ICommand ButtonOperationCommand { get; set; }

        private operations CurrentOperation { get; set; }

        private string _displayContent;

        public string DisplayContent
        {
            get { return _displayContent; }
            set
            {
                _displayContent = value;
                OnPropertyChanged("DisplayContent");
            }
        }

        public CalcyulatorVM()
        {
            InitializeViewsModel();
        }

        private void InitializeViewsModel()
        {
            _displayContent = "Please Enter your Number";
            ButtonNumberCommand = new RelayCommands(new Action<object>(UpdateOperandString));
            ButtonOperationCommand = new RelayCommands(new Action<object>(SetOperation));
        }

        private void UpdateOperandString(object obj)
        {
            if (obj.ToString() != "CE")
            {
                operandstring += obj.ToString();

            }
            else
            {
                operandstring = "";
            }
            DisplayContent = operandstring;
        }

        private operations CurrentOperator(string operationString)
        {
            if (BianaryOperatons.ContainsKey(operationString))
            {
                return BianaryOperatons[operationString];
            }
            else if (UnaryOperations.ContainsKey(operationString))
            {
                return (UnaryOperations[operationString]);
            }

            return operations.None;
        }

        private void SetOperation(object obj)
        {
            operations operation = CurrentOperator(obj.ToString());

            if (double.TryParse(operandstring, out double result))
            {
                if (BianaryOperatons.ContainsValue(operation))
                {
                    if (operation == operations.Equals)
                    {
                        _operand2 = result;
                        DisplayContent = ProcessBinaryOperation(_binaryOperator).ToString();
                        _binaryOperator = operations.None;
                    }
                    else if (operation == operations.Precent)
                    {
                        _operand2 = result;
                        _binaryOperator = operations.Precent;
                        DisplayContent = ProcessBinaryOperation(_binaryOperator).ToString();
                    }
                    else
                    {
                        _operand1 = result;
                        _binaryOperator = operation;
                        operandstring = "";
                        DisplayContent = "";
                    }
                }
                else if (UnaryOperations.ContainsValue(operation))
                {
                    _operand1 = result;
                    DisplayContent = ProcessUnaryOperation(operation).ToString();
                }
                else
                {
                    DisplayContent = "That is not a known Operation";
                }

                operandstring = DisplayContent;
            }
            else
            {
                DisplayContent = "Please enter a real number.";
            }
        }

        private double ProcessUnaryOperation(operations operation)
        {
            switch (operation)
            {
                case operations.Sin:
                    return Math.Sin(_operand1);
                case operations.Cos:
                    return Math.Cos(_operand1);
                case operations.Tan:
                    return Math.Tan(_operand1);
                case operations.Squared:
                    return Math.Pow(_operand1, 2);
                case operations.SqareRoot:
                    return Math.Sqrt(_operand1);
                case operations.Pi:
                    return 3.14159265359 * (_operand1);
                default:
                    DisplayContent = "That is not a known Operation";
                    return 0;
            }
        }

        private double ProcessBinaryOperation(operations operation)
        {
            switch (operation)
            {
                case operations.Add:
                    return _operand1 + _operand2;
                case operations.Subtract:
                    return _operand1 - _operand2;
                case operations.Multiply:
                    return _operand1 * _operand2;
                case operations.Divide:
                    return _operand1 / _operand2;
                case operations.Precent:
                    return _operand1 * (_operand2 / 100);
                
                default:
                    DisplayContent = "That is not a known Operation";
                    return 0;
            }




        }
    }
}

