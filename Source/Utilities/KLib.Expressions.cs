using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using KLib.KMath;
using KLib.Utilities.ExtensionMethods;

namespace KLib
{
    public class Expressions
    {
        private readonly string operatorSymbols = "+-*/^";
        private static string _lastError = "";
        private static System.Random _rng = null;

        public class PropVal
        {
            public string prop;
            public double value;
            public PropVal(string prop, double value)
            {
                this.prop = prop;
                this.value = value;
            }
            override public string ToString()
            {
                return prop + " = " + value;
            }
        }

        public static string LastError
        {
            get { return _lastError; }
        }

        public static string ToVectorString(double[] vals)
        {
            if (vals.Length == 1)
            {
                return vals[0].ToString();
            }

            string s = "[";
            foreach (var v in vals)
            {
                s += " " + v.ToString();
            }
            s += " ]";

            return s;
        }

        public static string SimpleRegEx(string expression, string pattern)
        {
            string result = "";

            Match m = Regex.Match(expression, pattern);
            if (m.Success)
            {
                result = m.Groups[1].Value;
            }
            return result;
        }

        public static int SimpleRegExInt(string expression, string pattern)
        {
            return int.Parse(SimpleRegEx(expression, pattern));
        }

        public static int[] EvaluateToInt(string expression)
        {
            double[] doubleVals = Evaluate(expression);
            int[] values = new int[doubleVals.Length];
            for (int k = 0; k < doubleVals.Length; k++) values[k] = (int)doubleVals[k];
            return values;
        }
        public static double[] Evaluate(string expression)
        {
            return Evaluate(expression);
        }

        public static int EvaluateToIntScalar(string expression)
        {
            int value = 0;
            bool result = true;
            try
            {
                double[] vals = Evaluate(expression);
                value = (int)vals[0];
            }
            catch (Exception ex)
            {
                result = false;
                _lastError = ex.Message;
            }

            return value;
        }

        public static double EvaluateToDoubleScalar(string expression)
        {
            double value = double.NaN;
            bool result = true;
            try
            {
                double[] vals = Evaluate(expression);
                int rn = MathUtils.NextRandomInt((int)0, (int)vals.Length);
                value = vals[rn];
            }
            catch (Exception ex)
            {
                result = false;
                _lastError = ex.Message;
            }

            return value;
        }

        public static bool TryEvaluate(string expression)
        {
            bool result = true;
            try
            {
                Evaluate(expression);
            }
            catch (Exception ex)
            {
                result = false;
                _lastError = ex.Message;
            }

            return result;
        }

        public static bool TryEvaluateScalar(string expression, out double value)
        {
            value = double.NaN;
            bool result = true;
            try
            {
                double[] vals = Evaluate(expression);
                value = vals[0];
            }
            catch (Exception ex)
            {
                result = false;
                _lastError = ex.Message;
            }

            return result;
        }

        public static bool EvaluateComparison(string expression)
        {
            var parts = expression.Split(new string[] { "<", ">", "==" }, StringSplitOptions.None);
            double lhs = EvaluateToDoubleScalar(parts[0]);

            if (parts.Length == 1)
                return lhs > 0;

            double rhs = EvaluateToDoubleScalar(parts[1]);

            if (expression.Contains("<"))
                return lhs < rhs;

            if (expression.Contains(">"))
                return lhs > rhs;

            if (expression.Contains("=="))
                return lhs == rhs;

            return false;
        }

        private double[] DoEvaluation(string expression, List<PropVal> propVals)
        {
            string vectorFunction = "";

            expression = ParseVectorFunction(expression, out vectorFunction);
            expression = expression.Trim();
            expression = SubstitutePropVals(expression, propVals);
            expression = SubstituteFunctions(expression);
            expression = ExpandRepeats(expression);
            expression = ParenthesizeVectorSpecs(expression);


            double[] values = EvaluateString(expression);

            if (!string.IsNullOrEmpty(vectorFunction))
            {
                values = EvaluateVectorFunction(vectorFunction, values);
            }

            return values;
        }

        private string ParseVectorFunction(string expression, out string func)
        {
            func = "";

            expression = expression.Trim();
            if (expression.ToLower().StartsWith("unique("))
            {
                func = "unique";
            }
            else if (expression.ToLower().StartsWith("max("))
            {
                func = "max";
            }
            else if (expression.ToLower().StartsWith("mean("))
            {
                func = "mean";
            }
            else if (expression.ToLower().StartsWith("median("))
            {
                func = "median";
            }
            else if (expression.ToLower().StartsWith("min("))
            {
                func = "min";
            }
            else if (expression.ToLower().StartsWith("perm("))
            {
                func = "perm";
            }

            if (!string.IsNullOrEmpty(func))
            {
                expression = expression.Remove(expression.Length - 1);
                expression = expression.Substring(func.Length + 1);
            }

            return expression;
        }

        private double[] EvaluateVectorFunction(string func, double[] values)
        {
            double[] valuesOut = values;
            switch (func)
            {
                case "max":
                    valuesOut = new double[1];
                    valuesOut[0] = MathUtils.Max(values);
                    break;
                case "mean":
                    valuesOut = new double[1];
                    valuesOut[0] = MathUtils.Mean(values);
                    break;
                case "median":
                    valuesOut = new double[1];
                    valuesOut[0] = MathUtils.Median(values);
                    break;
                case "min":
                    valuesOut = new double[1];
                    valuesOut[0] = MathUtils.Min(values);
                    break;
                case "perm":
                    valuesOut = MathUtils.Permute(values);
                    break;
                case "unique":
                    valuesOut = MathUtils.Unique(values);
                    break;
            }
            return valuesOut;
        }

        private string SubstituteFunctions(string expression)
        {
            // exprndt(min, max, lambda)
            string pattern = @"(exprndt\(\s*([\d.-]+)\s*,\s*([\d.-]+),\s*([\d.-]+)\s*\))";
            Match m = Regex.Match(expression, pattern);

            if (m.Success)
            {
                double min = double.Parse(m.Groups[2].Value);
                double max = double.Parse(m.Groups[3].Value);
                double lambda = double.Parse(m.Groups[4].Value);
                expression = expression.Replace(m.Groups[1].Value, TruncatedExponential(min, max, lambda).ToString());
                return expression;
            }

            // mean([x1 x2 x3 ...])
            pattern = @"(mean\(\s*\[([\d.\-\s]+)\s*\]\s*\))";
            m = Regex.Match(expression, pattern);
            if (m.Success)
            {
                expression = expression.Replace(m.Groups[1].Value, MathUtils.Mean(ParseMatrixString("[" + m.Groups[2].Value + "]")).ToString());
                return expression;
            }

            // unifrnd(min, max)
            pattern = @"(unifrnd\(\s*([\d.-]+)\s*,\s*([\d.-]+)\s*\))";
            m = Regex.Match(expression, pattern);

            if (m.Success)
            {
                double min = double.Parse(m.Groups[2].Value);
                double max = double.Parse(m.Groups[3].Value);
                expression = expression.Replace(m.Groups[1].Value, UniformRandomNumber(min, max).ToString());
                return expression;
            }

            // tnormrnd(min, max, mu, sigma)
            pattern = @"(tnormrnd\(\s*([\d.-]+)\s*,\s*([\d.-]+)\s*,\s*([\d.-]+)\s*,\s*([\d.-]+)\s*\))";
            m = Regex.Match(expression, pattern);

            if (m.Success)
            {
                double min = double.Parse(m.Groups[2].Value);
                double max = double.Parse(m.Groups[3].Value);
                double mu = double.Parse(m.Groups[4].Value);
                double sigma = double.Parse(m.Groups[5].Value);
                var tnr = new TruncatedNormalRandom();
                expression = expression.Replace(m.Groups[1].Value, tnr.Next(min, max, mu, sigma).ToString());
                return expression;
            }

            // sqrt(min, max)
            pattern = @"(sqrt\(\s*([\d.]+)\s*\))";
            m = Regex.Match(expression, pattern);

            if (m.Success)
            {
                double op = double.Parse(m.Groups[2].Value);
                expression = expression.Replace(m.Groups[1].Value, System.Math.Sqrt(op).ToString());
                return expression;
            }

            return expression;
        }

        public static bool ContainsPDF(string expr)
        {
            return expr.Contains("tnormrnd") ||
                expr.Contains("unifrnd") ||
                expr.Contains("exprndt");
        }

        private string SubstitutePropVals(string expression, List<PropVal> propVals)
        {
            if (propVals == null) return expression;

            foreach (PropVal pv in propVals)
            {
                expression = expression.Replace(pv.prop, pv.value.ToString());
            }

            return expression;
        }

        private string ExpandRepeats(string expression)
        {
            string pattern = @"([\d]+)x([\d.-]+)";
            Match m = Regex.Match(expression, pattern);

            while (m.Success)
            {
                int n = int.Parse(m.Groups[1].Value);
                string s = "";
                for (int k = 0; k < n; k++) s += m.Groups[2].Value + " ";

                expression = expression.Replace(m.Groups[0].Value, s);

                m = m.NextMatch();
            }
            return expression;
        }

        private double TruncatedExponential(double min, double max, double lambda)
        {
            System.Random r = new System.Random();
            double rn = (double)r.NextDouble();
            //   double rn = Rand.Range(0f, 1f);

            double a = 1 - Math.Exp(-(max - min) / lambda);
            double b = Math.Log(1 - a * rn);
            return min - b * lambda;
        }

        public static double UniformRandomNumber(double min, double max)
        {
            if (_rng == null)
            {
                _rng = new System.Random();
            }
            double rn = (double)_rng.NextDouble();
            return (max - min) * rn + min;
        }

        private string ParenthesizeVectorSpecs(string s)
        {
            // Put parentheses around vector specification (A:B:C) so it parses correctly.

            int icolon = s.IndexOf(':');
            int ibracket = s.IndexOf('[');

            if (icolon < 0) return s; // No colon? No vector specification
            if (ibracket >= 0 && ibracket < icolon) return s; // vector expression already contained in brackets

            // Start at the colon and work backward to find the beginning of "A"
            int idx = icolon;
            int startIndex = 0;
            while (true)
            {
                if (s[idx] == ')') // contains an expression in parentheses...
                {
                    startIndex = FindParentheticalOpen(s, idx); // ...find the beginning
                    break;
                }
                if (operatorSymbols.ContainsChar(s[idx]) && s[idx] != '-') // operator does not belong to vector spec...
                {
                    startIndex = idx + 1; // ..."A" begins the character after the operator
                    break;
                }
                if (--idx == -1) break; // "A" begins the string
            }

            if (s.TrimStart()[0] == '(') // "A" beginning with (...
            {
                // ... can mean the vector spec is already in parentheses
                if (FindParentheticalClose(s, '(', startIndex) > icolon) // closing parenthesis is beyond the first colon
                    return s;
            }

            // Start at the colon and work forward
            idx = icolon;
            int endIndex = s.Length - 1;
            while (true)
            {
                if (s[idx] == '(')
                {
                    idx = FindParentheticalClose(s, '(', idx);
                    break;
                }

                if (operatorSymbols.ContainsChar(s[idx]))
                {
                    endIndex = idx - 1;
                    break;
                }
                if (++idx == s.Length) break;
            }

            s = s.Insert(startIndex, "(");
            s = s.Insert(endIndex + 2, ")");

            return s;
        }

        private double[] EvaluateString(string s)
        {
            List<double[]> operands = new List<double[]>();
            List<char> ops = new List<char>();

            while (s.Length > 0)
            {
                string[] split = SplitString(s);
                operands.Add(EvaluateOperand(split[0]));

                if (split.Length == 3)
                {
                    ops.Add(split[1][0]);
                    s = split[2];
                }
                else
                {
                    s = "";
                }
            }

            while (ops.Count > 0)
            {
                int index = ops.FindIndex(o => o == '^');
                if (index < 0)
                    index = ops.FindIndex(o => o == '*' || o == '/');
                if (index < 0)
                    index = ops.FindIndex(o => o == '+' || o == '-');

                if (operands[index].Length != operands[index + 1].Length)
                {
                    if (operands[index].Length == 1)
                    {
                        double v = operands[index][0];
                        int n = operands[index + 1].Length;
                        operands[index] = new double[n];
                        for (int k = 0; k < n; k++) operands[index][k] = v;
                    }
                    else if (operands[index + 1].Length == 1)
                    {
                        double v = operands[index + 1][0];
                        int n = operands[index].Length;
                        operands[index + 1] = new double[n];
                        for (int k = 0; k < n; k++) operands[index + 1][k] = v;
                    }
                    else
                        throw new Exception("Invalid expression: inconsistent vector lengths");
                }

                for (int k = 0; k < operands[index].Length; k++)
                {
                    switch (ops[index])
                    {
                        case '^':
                            operands[index][k] = Math.Pow(operands[index][k], operands[index + 1][k]);
                            break;
                        case '*':
                            operands[index][k] *= operands[index + 1][k];
                            break;
                        case '/':
                            operands[index][k] /= operands[index + 1][k];
                            break;
                        case '+':
                            operands[index][k] += operands[index + 1][k];
                            break;
                        case '-':
                            operands[index][k] -= operands[index + 1][k];
                            break;
                    }
                }

                operands.RemoveAt(index + 1);
                ops.RemoveAt(index);
            }

            return operands[0];
        }

        private string[] SplitString(string s)
        {
            // Find operand
            int start = 0;
            int len = -1;
            int idx = 0;

            s = s.TrimStart();

            if (s[0] == '(')
            {
                start = 1;
                len = FindParentheticalClose(s, '(') - 1;
                idx = len;
            }
            if (s[0] == '[')
            {
                start = 0;
                idx = FindParentheticalClose(s, '[');
                len = idx + 1;
            }

            while (true)
            {
                if (operatorSymbols.ContainsChar(s[idx]) && idx > 0)
                    break;
                if (++idx == s.Length)
                    break;
            }

            if (len < 0) len = idx;

            List<string> split = new List<string>();
            split.Add(s.Substring(start, len));

            if (idx == s.Length - 1)
                throw new Exception("Invalid expression: dangling operator " + s[idx]);

            if (idx < s.Length)
            {
                split.Add(s.Substring(idx, 1));
                split.Add(s.Substring(idx + 1));
            }

            return split.ToArray();
        }

        private int FindParentheticalClose(string s, char opener, int startIndex = 0)
        {
            char closer;
            switch (opener)
            {
                case '(':
                    closer = ')';
                    break;
                case '[':
                    closer = ']';
                    break;
                default:
                    throw new Exception("Invalid character");
            }

            int nopen = 1;
            int idx = startIndex + 1;
            int end = -1;
            while (true)
            {
                if (s[idx] == opener) ++nopen;
                if (s[idx] == closer) --nopen;

                if (nopen > 1 && opener == '[')
                    throw new Exception("Invalid expression: can't handle nested []'s");

                if (nopen == 0 && end < 0) end = idx;
                if (nopen <= 0 && operatorSymbols.ContainsChar(s[idx])) break;
                if (++idx == s.Length) break;
            }

            if (nopen > 0)
                throw new Exception("Invalid expression: unbalanced " + opener);
            if (nopen < 0)
                throw new Exception("Invalid expression: unbalanced " + closer);

            return end;
        }

        private int FindParentheticalOpen(string s, int startIndex)
        {
            char opener = '(';
            char closer = ')';

            int nclose = 1;
            int idx = 1;
            while (true)
            {
                if (s[idx] == opener) --nclose;
                if (s[idx] == closer) ++nclose;

                if (nclose == 0) break;
                if (--idx == -1) break;
            }

            if (nclose > 0)
                throw new Exception("Invalid expression: unbalanced " + closer);

            return idx;
        }

        private double[] EvaluateOperand(string operand)
        {
            double[] result = null;
            if (operand[0] == '[')
            {
                result = ParseMatrixString(operand);
            }
            else if (operand.ContainsChar(':'))
            {
                result = ParseVectorString(operand);
            }
            else if (ContainsOperator(operand))
            {
                result = EvaluateString(operand);
            }
            else if (operand.ToLower().StartsWith("inf"))
            {
                result = new double[1] { double.PositiveInfinity };
            }
            else if (operand.ToLower().StartsWith("-inf"))
            {
                result = new double[1] { double.NegativeInfinity };
            }
            else
            {
                result = new double[1] { double.Parse(operand) };
            }
            return result;
        }

        private bool ContainsOperator(string s)
        {
            foreach (char op in operatorSymbols)
            {
                if (s.IndexOf(op) > 0)
                    return true;
            }
            return false;
        }

        private double[] ParseMatrixString(string matrixString)
        {
            List<double[]> vals = new List<double[]>();

            string[] split = matrixString.Substring(1, matrixString.Length - 2).Trim().Split(';', ' ');
            foreach (string s in split)
            {
                if (!string.IsNullOrEmpty(s)) vals.Add(EvaluateOperand(s));
            }

            int n = vals.Sum(v => v.Length);
            double[] result = new double[n];
            int idx = 0;
            foreach (double[] v in vals)
            {
                for (int k = 0; k < v.Length; k++) result[idx++] = v[k];
            }

            return result;
        }

        private double[] ParseVectorString(string vectorString)
        {
            //if (vectorString[0] == '(')
            //{
            //    vectorString = vectorString.Substring(1, vectorString.Length - 2);
            //}

            string[] split = vectorString.Split(':');

            if (split.Length > 3)
                throw new Exception("Invalid vector expression: '" + vectorString + "'");

            double[] components = new double[split.Length];
            for (int k = 0; k < split.Length; k++)
            {
                double[] v = EvaluateOperand(split[k]);
                if (v.Length > 1)
                    throw new Exception("Invalid vector expression");

                components[k] = v[0];
            }

            double start = components[0];
            double step = (components.Length == 3) ? components[1] : 1;
            double stop = (components.Length == 3) ? components[2] : components[1];

            if (step == 0)
                throw new Exception("Invalid vector expression: step size = 0");

            int n = (int)Math.Floor((stop - start) / step) + 1;

            if (n <= 0)
                throw new Exception("Invalid vector expression:" + vectorString);

            double[] result = new double[n];
            for (int k = 0; k < n; k++)
            {
                result[k] = start + k * step;
            }

            return result;
        }

    }
}