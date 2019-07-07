using System.Collections.Generic;
using System.Linq;

using MathParser.Tokens;

namespace MathParser.Tokens.Configuration
{
    public class TokenizerConfig
    {
        public static TokenizerConfig DefaultConfig => new TokenizerConfig();


        public Dictionary<char, List<TokenInfo>> TokenMap => GetTokenMap();

        private Dictionary<char, List<TokenInfo>> tokenMap;
        private bool tokenMapInvalid;

        private List<TokenInfo> tokenInfoList;

        #region Constructor
        /// <summary>
        /// Initialize a new instance of <see cref="TokenizerConfig"/> with specified list of <see cref="TokenInfo"/>
        /// </summary>
        public TokenizerConfig(List<TokenInfo> tokenInfoList)
        {
            this.tokenInfoList = tokenInfoList;

            // Token map is not initialized, so it is invalid.
            tokenMapInvalid = true;
        }
        /// <summary>
        /// Initialize a new instance of <see cref="TokenizerConfig"/> with default configuration.
        /// </summary>
        public TokenizerConfig()
        {
            // Should load this from a config file.
            tokenInfoList = new List<TokenInfo>
            {
                new TokenInfo {Type = TokenType.ParenLeft, Pattern = "("},
                new TokenInfo {Type = TokenType.ParenRight, Pattern = ")"},
                new TokenInfo {Type = TokenType.OpAdd, Pattern = "+"},
                new TokenInfo {Type = TokenType.OpSubtract, Pattern = "-"},
                new TokenInfo {Type = TokenType.OpMultiply, Pattern = "*"},
                new TokenInfo {Type = TokenType.OpDivide, Pattern = "/"},
                new TokenInfo {Type = TokenType.OpPower, Pattern = "^"},
                new TokenInfo {Type = TokenType.Variable, Pattern = "x"},
                new TokenInfo {Type = TokenType.Variable, Pattern = "y"},
                new TokenInfo {Type = TokenType.Variable, Pattern = "z"},
                new TokenInfo {Type = TokenType.Constant, Pattern = "pi"},
                new TokenInfo {Type = TokenType.Constant, Pattern = "e"},
                new TokenInfo {Type = TokenType.Function, Pattern = "sin"},
                new TokenInfo {Type = TokenType.Function, Pattern = "cos"},
                new TokenInfo {Type = TokenType.Function, Pattern = "tan"},
                new TokenInfo {Type = TokenType.Function, Pattern = "acos"},
                new TokenInfo {Type = TokenType.Function, Pattern = "asin"},
                new TokenInfo {Type = TokenType.Function, Pattern = "atan"},
                new TokenInfo {Type = TokenType.Function, Pattern = "ln"},
                new TokenInfo {Type = TokenType.Function, Pattern = "log"},
                new TokenInfo {Type = TokenType.Function, Pattern = "abs"}
            };

            // Token map is not initialized, so it is invalid.
            tokenMapInvalid = true;
        }
        #endregion

        public void Add(TokenInfo newInfo)
        {
            tokenInfoList.Add(newInfo);

            tokenMapInvalid = true;
        }

        private Dictionary<char, List<TokenInfo>> GetTokenMap()
        {
            if (tokenMapInvalid)
            {
                tokenMap = GenerateTokenMap();

                tokenMapInvalid = false;
            }

            return tokenMap;
        }

        private Dictionary<char, List<TokenInfo>> GenerateTokenMap()
        {
            // Group TokenInfos by their initial character.
            var tokenInfoGroups = tokenInfoList.GroupBy(item => item.Pattern.First());

            // Sort each group in descending order of TokenInfo's pattern length.
            var sortedGroups = from tokenInfoGroup in tokenInfoGroups
                               select new
                               {
                                   Key = tokenInfoGroup.Key,
                                   InfoGroup =
                                      from tokenInfo in tokenInfoGroup
                                      orderby tokenInfo.Pattern.Length descending
                                      select tokenInfo
                               };

            // Create a dictionary from sortedGroups.
            Dictionary<char, List<TokenInfo>> tokenInfoDict =
                sortedGroups.ToDictionary(group => group.Key, group => group.InfoGroup.ToList());

            return tokenInfoDict;
        }
    }
}
