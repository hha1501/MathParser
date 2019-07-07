using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MathParser.Tokens;
using MathParser.Tokens.Configuration;


namespace MathParser.Tokens.Tokenizing
{
    public class Tokenizer
    {

        /// <summary>
        /// The current <see cref="TokenizerConfig"/> 
        /// associated with this instance of <see cref="Tokenizer"/>
        /// </summary>
        public TokenizerConfig Config { get; private set; }

        private Dictionary<char, List<TokenInfo>> tokenMap;

        private string inputSequence;
        private int parsingPosition;

        #region Constructor
        public Tokenizer(TokenizerConfig config)
        {
            Config = config;
        }
        public Tokenizer()
        {
            Config = TokenizerConfig.DefaultConfig;
        }
        #endregion

        public TokenizeResult TokenizeString(string data)
        {
            data = EnsureValidExpressionString(data);

            // Get a new token map from config.
            tokenMap = Config.TokenMap;

            return BeginTokenize(data);
        }

        string EnsureValidExpressionString(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("Expression string is empty, null, or contains only white spaces.");
            }

            // Trim off all white spaces.
            return data.Trim(' ');
        }


        TokenizeResult BeginTokenize(string data)
        {
            inputSequence = data;
            parsingPosition = 0;

            List<Token> tokenList = new List<Token>();

            while (parsingPosition < inputSequence.Length)
            {
                ParseResult parseResult = ParseNext();

                // Process parse result.
                if (parseResult.Success)
                {
                    tokenList.Add(parseResult.ParsedToken);

                    // Advance current positon by the number of characters consumed.
                    parsingPosition += parseResult.TokenSize;
                }
                else
                {
                    return TokenizeResult.NewError(parseResult.AdditionalInfo);
                }
            }

            return TokenizeResult.NewSuccess(tokenList);
        }

        ParseResult ParseNext()
        {
            char currentChar = inputSequence[parsingPosition];

            if (currentChar.IsArabicDigit())
            {
                return ParseNumber();
            }
            else
            {
                return ParseGeneric();
            }
        }
        ParseResult ParseNumber()
        {
            string tokenString = inputSequence.Substring(parsingPosition);

            int currentPos = 0;
            int characterRead = 0;
            bool hasDecimalPoint = false;

            char c;

            // Check subsequent characters.
            while (currentPos < tokenString.Length)
            {
                c = tokenString[currentPos];

                // Check for decimal point.
                if (c == '.')
                {
                    // Already has a decimal point, asset a syntax error.
                    if (hasDecimalPoint)
                    {
                        return ParseResult.NewError(ParseResult.DecimalPoint);
                    }
                    else
                    {
                        hasDecimalPoint = true;
                        characterRead++;
                    }
                }
                else if (c.IsArabicDigit())
                {
                    characterRead++;
                }
                else
                {
                    // Current character is not part of a number.
                    break;
                }

                currentPos++;
            }

            string numberString = tokenString.Substring(0, characterRead);

            // Try parsing the string as a number.
            if (double.TryParse(numberString, out double number))
            {
                return ParseResult.NewSuccess(Constant.New(number), numberString.Length);
            }

            return ParseResult.NewError(ParseResult.UnknownError);
        }
        ParseResult ParseGeneric()
        {
            char currentChar = inputSequence[parsingPosition];

            if (tokenMap.ContainsKey(currentChar))
            {
                List<TokenInfo> possibleTokenMatches = tokenMap[currentChar];

                foreach (TokenInfo info in possibleTokenMatches)
                {
                    if (parsingPosition + info.Pattern.Length <= inputSequence.Length)
                    {
                        string matchSequence = inputSequence.Substring(parsingPosition, info.Pattern.Length);
                        if (matchSequence == info.Pattern)
                        {
                            Token parsedToken = InflateToken(info);
                            return ParseResult.NewSuccess(parsedToken, matchSequence.Length);
                        }
                    }
                }
            }

            return ParseResult.NewError(ParseResult.UnknownCharacter);
        }

        Token InflateToken(TokenInfo tokenInfo)
        {
            string match = tokenInfo.Pattern;
            TokenType tokenType = tokenInfo.Type;

            switch (tokenType)
            {
                case TokenType.Variable:
                    return Variable.New(match);
                case TokenType.Constant:
                    return InflateConstantToken(match);
                case TokenType.ParenLeft:
                    return Parenthesis.NewLeft();
                case TokenType.ParenRight:
                    return Parenthesis.NewRight();
                case TokenType.OpAdd:
                    return Operator.NewAddition();
                case TokenType.OpSubtract:
                    return Operator.NewSubtraction();
                case TokenType.OpMultiply:
                    return Operator.NewMultiplication();
                case TokenType.OpDivide:
                    return Operator.NewDivision();
                case TokenType.OpPower:
                    return Operator.NewPower();
                case TokenType.Function:
                    return FunctionHeader.New(match, match.Substring(0, 1).ToUpper() + match.Substring(1), 1);
                default:
                    return null;
            }
        }

        Token InflateConstantToken(string constantName)
        {
            switch (constantName)
            {
                case "pi":
                    return Constant.New("pi", Math.PI);
                case "e":
                    return Constant.New("e", Math.E);
                default:
                    return null;
            }
        }
    }
}
