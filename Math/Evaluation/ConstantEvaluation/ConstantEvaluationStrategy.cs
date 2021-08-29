using System;
using System.Collections.Generic;
using LSharp.Math.Symbols;

namespace LSharp.Math.Evaluation.ConstantEvaluation
{
    public class ConstantEvaluationStrategy : IEvaluationStrategy
    {
        public Expression expression { get; set; }
        public ConstantEvaluationStrategy(Expression expression)
        {
            this.expression = expression;
        }
        public Expression Evaluate()
        {
            ChainEvaluation(expression.GetRoot());

            return expression;
        }
        public void ChainEvaluation(int index)
        {
            foreach (int child in expression.GetChildren(index))
            {
                ChainEvaluation(child);
            }
            EvaluationResult evaluation = EvaluateConstants(index);

            if (evaluation.change)
            {
                expression.ReplaceNode(index, evaluation.result);
            }
        }
        public EvaluationResult EvaluateConstants(int index)
        {
            int? total = null;

            List<Expression> duplicated = new List<Expression>();

            foreach (int child in expression.GetChildren(index))
            {
                int? value = expression.GetNode(child).GetNumericValue();

                if (value != null)
                {
                    if (expression.IsSummation(index))
                    {
                        total = total == null ? (int) value : total + value;
                    }
                    else if (expression.IsMultiplication(index))
                    {
                        total = total == null ? (int) value : total * (int) value;
                    }
                    else if (expression.IsDivision(index))
                    {
                        total = total == null ? (int) value : (total % (int) value == 0 ? total / (int) value : total = null);
                    }
                    else if (expression.IsExponent(index))
                    {
                        total = total == null ? (int) value : (int) (System.Math.Pow((double) total, (double) value));
                    }
                    else if (expression.IsRadical(index))
                    {

                    }
                    else
                    {
                        throw new Exception("Symbol type not recognised");
                    }
                }
                else
                {
                    duplicated.Add(expression.CopySubTree(child));
                }
            }
            if (total == null)
            {
                return new EvaluationResult(){ result = expression, change = false };
            }
            else
            {
                Expression result = new Expression();
                
                if (duplicated.Count == 0)
                {
                    result.SetRoot(new Constant(true, (int) total));
                }
                else
                {
                    Symbol newParent = expression.GetNode(index).Copy();

                    result.SetRoot(newParent);

                    result.AppendNode(0, new Constant(true, (int) total));

                    result.AppendBulkNodes(0, duplicated);
                }
                return new EvaluationResult(){ result = result, change = true };
            }
        }
    }
}