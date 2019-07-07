using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Tokens;
using MathParser.Expressions;
using MathParser.Expressions.Extensions;

namespace MathParser.Expressions.Builder
{
    public class ExpressionBuilder
    {
        private const int MinOperatorPrecedence = 1;

        // Data source.
        private List<Token> tokenList;
        int tokenCount;

        // Building variables.
        int currentPos;
        Token currentToken;

        #region Constructor
        public ExpressionBuilder(List<Token> tokenList)
        {
            if (tokenList.Count == 0)
            {
                throw new ArgumentException("Token list is empty.", nameof(tokenList));
            }

            this.tokenList = tokenList;
            tokenCount = tokenList.Count;

            currentToken = tokenList[0];
        } 
        #endregion

        public ExpressionizeResult GenerateExpression()
        {
            ExpressionizeResult result = CreateExpression(MinOperatorPrecedence);

            if (currentToken != null && currentToken.Type == TokenType.ParenRight)
            {
                return ExpressionizeResult.NewError(ErrorInfo.UnexpectRightParen);
            }

            return result;
        }

        ExpressionizeResult CreateExpression(int basePrecedence)
        {
            Expression single = null;

            // Make an atom.
            ExpressionizeResult atomResult = CreateAtom();

            if (!atomResult.Success)
            {
                return atomResult;
            }

            // Atomizing succeeded.
            single = atomResult.CalculatedExpression;

            while (true)
            {
                // After an atom might be an operator, an argument or another expression.

                // End of the token list.
                if (currentToken == null)
                {
                    break;
                }

                // Operator after an atom.
                if (TokenType.Operator.HasFlag(currentToken.Type))
                {
                    BinaryType binaryType = currentToken.Type.ToBinaryType();

                    int opPrecedence = binaryType.GetPrecedence();

                    if (opPrecedence < basePrecedence)
                    {
                        break;
                    }

                    int nextPrecedence = binaryType.IsLeftAssociative() ? (opPrecedence + 1) : opPrecedence;

                    GetNextToken();
                    ExpressionizeResult rightResult = CreateExpression(nextPrecedence);

                    if (!rightResult.Success)
                    {
                        return rightResult;
                    }

                    Expression left = single;
                    Expression right = rightResult.CalculatedExpression;

                    single = new BinaryExpression(left, right, binaryType);
                }
                else
                {
                    // Variable or left parenthesis after an atom,
                    // connect them with implied multiplication.
                    if (currentToken.Type == TokenType.Variable || currentToken.Type == TokenType.ParenLeft)
                    {
                        BinaryType binaryType = BinaryType.ImpliedMultiply;

                        int opPrecedence = binaryType.GetPrecedence();

                        if (opPrecedence < basePrecedence)
                        {
                            break;
                        }

                        int nextPrecedence = binaryType.IsLeftAssociative() ? (opPrecedence + 1) : opPrecedence;

                        // No need to consume the operator token.

                        ExpressionizeResult rightResult = CreateExpression(nextPrecedence);

                        if (!rightResult.Success)
                        {
                            return rightResult;
                        }

                        Expression right = rightResult.CalculatedExpression;

                        single = new BinaryExpression(single, right, binaryType);
                    }
                    else
                    {
                        if (currentToken.Type == TokenType.Constant)
                        {
                            return ExpressionizeResult.NewError(ErrorInfo.ExpectOperator);
                        }

                        // The current expression ends if we encounter anything else.
                        break;
                    }
                }
            }

            return ExpressionizeResult.NewSuccess(single);
        }

        ExpressionizeResult CreateAtom()
        {
            if (currentToken == null)
            {
                return ExpressionizeResult.NewError(ErrorInfo.ExpectExpression);
            }

            if (currentToken.Type == TokenType.Constant)
            {
                Constant constant = (Constant)currentToken;
                GetNextToken();

                // Create a new constant expression.
                ConstantExpression constantExpression = new ConstantExpression(constant.Value);
                return ExpressionizeResult.NewSuccess(constantExpression);
            }
            else if (currentToken.Type == TokenType.Variable)
            {
                Variable variable = (Variable)currentToken;
                GetNextToken();

                // Create a new argument expression.
                VariableExpression argumentExpression = new VariableExpression(variable.Name);
                return ExpressionizeResult.NewSuccess(argumentExpression);
            }
            else if (TokenType.UnaryOperator.HasFlag(currentToken.Type))
            {
                UnaryType unaryType = currentToken.Type.ToUnaryType();

                GetNextToken();
                // Since unary operator is left associative, we add 1 to the base precedence.
                ExpressionizeResult childResult = CreateExpression(unaryType.GetPrecedence() + 1);

                if (!childResult.Success)
                {
                    return childResult;
                }

                Expression childExpression = childResult.CalculatedExpression;

                // Create a new unary expression.
                UnaryExpression unaryExpression = new UnaryExpression(childExpression, unaryType);
                return ExpressionizeResult.NewSuccess(unaryExpression);
            }
            else if (currentToken.Type == TokenType.ParenLeft)
            {
                GetNextToken();
                // Try to make a sub-expression enclosed by this left parenthesis.
                ExpressionizeResult subResult = CreateExpression(MinOperatorPrecedence);

                if (!subResult.Success)
                {
                    return subResult;
                }

                Expression subExpression = subResult.CalculatedExpression;

                // Expect a matching right parenthesis.
                if (currentToken != null && currentToken.Type == TokenType.ParenRight)
                {
                    GetNextToken();
                    return ExpressionizeResult.NewSuccess(subExpression);
                }
                else
                {
                    return ExpressionizeResult.NewError(ErrorInfo.ExpectRightParen);
                }
            }
            else if (currentToken.Type == TokenType.Function)
            {
                // A function has two parts: 
                // - An identifier(name) 
                // - Its arguments enclosed in a pair of parens.

                FunctionHeader functionHeader = (FunctionHeader)currentToken;
                List<Expression> argumentsList = new List<Expression>();

                GetNextToken();

                if (currentToken != null && currentToken.Type == TokenType.ParenLeft)
                {
                    GetNextToken();

                    // Only create expressions for each argument if the function requires any.
                    if (functionHeader.ArgumentCount > 0)
                    {
                        // NOTE: Handle multiple args here.
                        ExpressionizeResult argumentResult = CreateExpression(MinOperatorPrecedence);

                        if (!argumentResult.Success)
                        {
                            return argumentResult;
                        }

                        argumentsList.Add(argumentResult.CalculatedExpression);
                    }

                    if (currentToken != null && currentToken.Type == TokenType.ParenRight)
                    {
                        GetNextToken();

                        // Create a function expression.
                        FunctionExpression functionExpression = 
                            new FunctionExpression(
                                functionHeader.Name,
                                functionHeader.Target,
                                argumentsList);

                        return ExpressionizeResult.NewSuccess(functionExpression);
                    }
                    else
                    {
                        return ExpressionizeResult.NewError(ErrorInfo.ExpectRightParen);
                    }
                }
                else
                {
                    return ExpressionizeResult.NewError(ErrorInfo.ExpectLeftParen);
                }
                
            }

            return ExpressionizeResult.NewError(ErrorInfo.ExpectExpression);
        }


        void GetNextToken()
        {
            if (++currentPos < tokenCount)
            {
                currentToken = tokenList[currentPos];
            }
            else
            {
                currentToken = null;
            }
        }
    }
}
