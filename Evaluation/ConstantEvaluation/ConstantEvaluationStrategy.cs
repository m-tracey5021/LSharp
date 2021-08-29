using System;
using System.Collections.Generic;
using LSharp.Symbols;

namespace LSharp.Evaluation.ConstantEvaluation
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

            Symbol parent = expression.GetNode(index);

            List<Expression> duplicated = new List<Expression>();

            foreach (int child in expression.GetChildren(index))
            {
                int? value = expression.GetNode(child).GetNumericValue();

                if (value != null)
                {
                    if (parent.IsSummation())
                    {
                        total = total == null ? (int) value : total + value;
                    }
                    else if (parent.IsMultiplication())
                    {
                        total = total == null ? (int) value : total * (int) value;
                    }
                    else if (parent.IsDivision())
                    {
                        total = total == null ? (int) value : (total % (int) value == 0 ? total / (int) value : total = null);
                    }
                    else if (parent.IsExponent())
                    {
                        total = total == null ? (int) value : (int) (Math.Pow((double) total, (double) value));
                    }
                    else if (parent.IsRadical())
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
                    Symbol newParent = parent.Copy();

                    result.SetRoot(newParent);

                    result.AppendNode(0, new Constant(true, (int) total));

                    result.AppendBulkNodes(0, duplicated);
                }
                return new EvaluationResult(){ result = result, change = true };
            }
        }
    }
}