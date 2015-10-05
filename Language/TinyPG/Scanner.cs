// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TinyPG
{
    #region Scanner

    public partial class Scanner
    {
        public string Input;
        public int StartPos = 0;
        public int EndPos = 0;
        public int CurrentLine;
        public int CurrentColumn;
        public int CurrentPosition;
        public List<Token> Skipped; // tokens that were skipped
        public Dictionary<TokenType, Regex> Patterns;

        private Token LookAheadToken;
        private List<TokenType> Tokens;
        private List<TokenType> SkipList; // tokens to be skipped

        public Scanner()
        {
            Regex regex;
            Patterns = new Dictionary<TokenType, Regex>();
            Tokens = new List<TokenType>();
            LookAheadToken = null;
            Skipped = new List<Token>();

            SkipList = new List<TokenType>();
            SkipList.Add(TokenType.WHITESPACE);
            SkipList.Add(TokenType.COMMENT);

            regex = new Regex(@"global", RegexOptions.Compiled);
            Patterns.Add(TokenType.GLOBAL, regex);
            Tokens.Add(TokenType.GLOBAL);

            regex = new Regex(@"end", RegexOptions.Compiled);
            Patterns.Add(TokenType.END, regex);
            Tokens.Add(TokenType.END);

            regex = new Regex(@"return", RegexOptions.Compiled);
            Patterns.Add(TokenType.RETURN, regex);
            Tokens.Add(TokenType.RETURN);

            regex = new Regex(@"=>", RegexOptions.Compiled);
            Patterns.Add(TokenType.ARROW, regex);
            Tokens.Add(TokenType.ARROW);

            regex = new Regex(@"if", RegexOptions.Compiled);
            Patterns.Add(TokenType.IF, regex);
            Tokens.Add(TokenType.IF);

            regex = new Regex(@"else", RegexOptions.Compiled);
            Patterns.Add(TokenType.ELSE, regex);
            Tokens.Add(TokenType.ELSE);

            regex = new Regex(@"for", RegexOptions.Compiled);
            Patterns.Add(TokenType.FOR, regex);
            Tokens.Add(TokenType.FOR);

            regex = new Regex(@"to", RegexOptions.Compiled);
            Patterns.Add(TokenType.TO, regex);
            Tokens.Add(TokenType.TO);

            regex = new Regex(@"incby", RegexOptions.Compiled);
            Patterns.Add(TokenType.INCBY, regex);
            Tokens.Add(TokenType.INCBY);

            regex = new Regex(@"while", RegexOptions.Compiled);
            Patterns.Add(TokenType.WHILE, regex);
            Tokens.Add(TokenType.WHILE);

            regex = new Regex(@"do", RegexOptions.Compiled);
            Patterns.Add(TokenType.DO, regex);
            Tokens.Add(TokenType.DO);

            regex = new Regex(@"or", RegexOptions.Compiled);
            Patterns.Add(TokenType.OR, regex);
            Tokens.Add(TokenType.OR);

            regex = new Regex(@"and", RegexOptions.Compiled);
            Patterns.Add(TokenType.AND, regex);
            Tokens.Add(TokenType.AND);

            regex = new Regex(@"not", RegexOptions.Compiled);
            Patterns.Add(TokenType.NOT, regex);
            Tokens.Add(TokenType.NOT);

            regex = new Regex(@"\+", RegexOptions.Compiled);
            Patterns.Add(TokenType.PLUS, regex);
            Tokens.Add(TokenType.PLUS);

            regex = new Regex(@"-", RegexOptions.Compiled);
            Patterns.Add(TokenType.MINUS, regex);
            Tokens.Add(TokenType.MINUS);

            regex = new Regex(@"\*", RegexOptions.Compiled);
            Patterns.Add(TokenType.MULT, regex);
            Tokens.Add(TokenType.MULT);

            regex = new Regex(@"/", RegexOptions.Compiled);
            Patterns.Add(TokenType.DIV, regex);
            Tokens.Add(TokenType.DIV);

            regex = new Regex(@"=|\!=|\<\=|\<|\>=|\>", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMP, regex);
            Tokens.Add(TokenType.COMP);

            regex = new Regex(@"\%", RegexOptions.Compiled);
            Patterns.Add(TokenType.MOD, regex);
            Tokens.Add(TokenType.MOD);

            regex = new Regex(@"//", RegexOptions.Compiled);
            Patterns.Add(TokenType.INTDIV, regex);
            Tokens.Add(TokenType.INTDIV);

            regex = new Regex(@"\^", RegexOptions.Compiled);
            Patterns.Add(TokenType.POW, regex);
            Tokens.Add(TokenType.POW);

            regex = new Regex(@"\+\+", RegexOptions.Compiled);
            Patterns.Add(TokenType.INC, regex);
            Tokens.Add(TokenType.INC);

            regex = new Regex(@"--", RegexOptions.Compiled);
            Patterns.Add(TokenType.DEC, regex);
            Tokens.Add(TokenType.DEC);

            regex = new Regex(@":", RegexOptions.Compiled);
            Patterns.Add(TokenType.COLON, regex);
            Tokens.Add(TokenType.COLON);

            regex = new Regex(@"\?", RegexOptions.Compiled);
            Patterns.Add(TokenType.QUESTION, regex);
            Tokens.Add(TokenType.QUESTION);

            regex = new Regex(@",", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMA, regex);
            Tokens.Add(TokenType.COMMA);

            regex = new Regex(@"\=", RegexOptions.Compiled);
            Patterns.Add(TokenType.ASSIGN, regex);
            Tokens.Add(TokenType.ASSIGN);

            regex = new Regex(@"\(", RegexOptions.Compiled);
            Patterns.Add(TokenType.BROPEN, regex);
            Tokens.Add(TokenType.BROPEN);

            regex = new Regex(@"\)", RegexOptions.Compiled);
            Patterns.Add(TokenType.BRCLOSE, regex);
            Tokens.Add(TokenType.BRCLOSE);

            regex = new Regex(@"\[", RegexOptions.Compiled);
            Patterns.Add(TokenType.SQOPEN, regex);
            Tokens.Add(TokenType.SQOPEN);

            regex = new Regex(@"\]", RegexOptions.Compiled);
            Patterns.Add(TokenType.SQCLOSE, regex);
            Tokens.Add(TokenType.SQCLOSE);

            regex = new Regex(@"@?\""(\""\""|[^\""])*\""", RegexOptions.Compiled);
            Patterns.Add(TokenType.STRING, regex);
            Tokens.Add(TokenType.STRING);

            regex = new Regex(@"[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.INTEGER, regex);
            Tokens.Add(TokenType.INTEGER);

            regex = new Regex(@"[0-9]*\.[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOUBLE, regex);
            Tokens.Add(TokenType.DOUBLE);

            regex = new Regex(@"true|false", RegexOptions.Compiled);
            Patterns.Add(TokenType.BOOL, regex);
            Tokens.Add(TokenType.BOOL);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*(?<!(^)(end|else|do|while|for|true|false|return|to|incby|global|or|and|not))(?!\w)", RegexOptions.Compiled);
            Patterns.Add(TokenType.IDENTIFIER, regex);
            Tokens.Add(TokenType.IDENTIFIER);

            regex = new Regex(@"\s+", RegexOptions.Compiled);
            Patterns.Add(TokenType.NEWLINE, regex);
            Tokens.Add(TokenType.NEWLINE);

            regex = new Regex(@"^$", RegexOptions.Compiled);
            Patterns.Add(TokenType.EOF, regex);
            Tokens.Add(TokenType.EOF);

            regex = new Regex(@"\s+", RegexOptions.Compiled);
            Patterns.Add(TokenType.WHITESPACE, regex);
            Tokens.Add(TokenType.WHITESPACE);

            regex = new Regex(@"//[^\n]*\n?", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMENT, regex);
            Tokens.Add(TokenType.COMMENT);


        }

        public void Init(string input)
        {
            this.Input = input;
            StartPos = 0;
            EndPos = 0;
            CurrentLine = 0;
            CurrentColumn = 0;
            CurrentPosition = 0;
            LookAheadToken = null;
        }

        public Token GetToken(TokenType type)
        {
            Token t = new Token(this.StartPos, this.EndPos);
            t.Type = type;
            return t;
        }

         /// <summary>
        /// executes a lookahead of the next token
        /// and will advance the scan on the input string
        /// </summary>
        /// <returns></returns>
        public Token Scan(params TokenType[] expectedtokens)
        {
            Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
            LookAheadToken = null; // reset lookahead token, so scanning will continue
            StartPos = tok.EndPos;
            EndPos = tok.EndPos; // set the tokenizer to the new scan position
            return tok;
        }

        /// <summary>
        /// returns token with longest best match
        /// </summary>
        /// <returns></returns>
        public Token LookAhead(params TokenType[] expectedtokens)
        {
            int i;
            int startpos = StartPos;
            Token tok = null;
            List<TokenType> scantokens;


            // this prevents double scanning and matching
            // increased performance
            if (LookAheadToken != null 
                && LookAheadToken.Type != TokenType._UNDETERMINED_ 
                && LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;

            // if no scantokens specified, then scan for all of them (= backward compatible)
            if (expectedtokens.Length == 0)
                scantokens = Tokens;
            else
            {
                scantokens = new List<TokenType>(expectedtokens);
                scantokens.AddRange(SkipList);
            }

            do
            {

                int len = -1;
                TokenType index = (TokenType)int.MaxValue;
                string input = Input.Substring(startpos);

                tok = new Token(startpos, EndPos);

                for (i = 0; i < scantokens.Count; i++)
                {
                    Regex r = Patterns[scantokens[i]];
                    Match m = r.Match(input);
                    if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len )))
                    {
                        len = m.Length;
                        index = scantokens[i];  
                    }
                }

                if (index >= 0 && len >= 0)
                {
                    tok.EndPos = startpos + len;
                    tok.Text = Input.Substring(tok.StartPos, len);
                    tok.Type = index;
                }
                else if (tok.StartPos < tok.EndPos - 1)
                {
                    tok.Text = Input.Substring(tok.StartPos, 1);
                }

                if (SkipList.Contains(tok.Type))
                {
                    startpos = tok.EndPos;
                    Skipped.Add(tok);
                }
                else
                {
                    // only assign to non-skipped tokens
                    tok.Skipped = Skipped; // assign prior skips to this token
                    Skipped = new List<Token>(); //reset skips
                }
            }
            while (SkipList.Contains(tok.Type));

            LookAheadToken = tok;
            return tok;
        }
    }

    #endregion

    #region Token

    public enum TokenType
    {

            //Non terminal tokens:
            _NONE_  = 0,
            _UNDETERMINED_= 1,

            //Non terminal tokens:
            Start   = 2,
            Program = 3,
            Member  = 4,
            Globalvar= 5,
            Function= 6,
            Parameters= 7,
            Statements= 8,
            Statement= 9,
            IfStm   = 10,
            WhileStm= 11,
            DoStm   = 12,
            ForStm  = 13,
            ReturnStm= 14,
            CallOrAssign= 15,
            Assign  = 16,
            Variable= 17,
            Array   = 18,
            Call    = 19,
            Arguments= 20,
            Literal = 21,
            Expr    = 22,
            OrExpr  = 23,
            AndExpr = 24,
            NotExpr = 25,
            CompExpr= 26,
            AddExpr = 27,
            MultExpr= 28,
            PowExpr = 29,
            UnaryExpr= 30,
            Atom    = 31,

            //Terminal tokens:
            GLOBAL  = 32,
            END     = 33,
            RETURN  = 34,
            ARROW   = 35,
            IF      = 36,
            ELSE    = 37,
            FOR     = 38,
            TO      = 39,
            INCBY   = 40,
            WHILE   = 41,
            DO      = 42,
            OR      = 43,
            AND     = 44,
            NOT     = 45,
            PLUS    = 46,
            MINUS   = 47,
            MULT    = 48,
            DIV     = 49,
            COMP    = 50,
            MOD     = 51,
            INTDIV  = 52,
            POW     = 53,
            INC     = 54,
            DEC     = 55,
            COLON   = 56,
            QUESTION= 57,
            COMMA   = 58,
            ASSIGN  = 59,
            BROPEN  = 60,
            BRCLOSE = 61,
            SQOPEN  = 62,
            SQCLOSE = 63,
            STRING  = 64,
            INTEGER = 65,
            DOUBLE  = 66,
            BOOL    = 67,
            IDENTIFIER= 68,
            NEWLINE = 69,
            EOF     = 70,
            WHITESPACE= 71,
            COMMENT = 72
    }

    public class Token
    {
        private int startpos;
        private int endpos;
        private string text;
        private object value;

        // contains all prior skipped symbols
        private List<Token> skipped;

        public int StartPos { 
            get { return startpos;} 
            set { startpos = value; }
        }

        public int Length { 
            get { return endpos - startpos;} 
        }

        public int EndPos { 
            get { return endpos;} 
            set { endpos = value; }
        }

        public string Text { 
            get { return text;} 
            set { text = value; }
        }

        public List<Token> Skipped { 
            get { return skipped;} 
            set { skipped = value; }
        }
        public object Value { 
            get { return value;} 
            set { this.value = value; }
        }

        [XmlAttribute]
        public TokenType Type;

        public Token()
            : this(0, 0)
        {
        }

        public Token(int start, int end)
        {
            Type = TokenType._UNDETERMINED_;
            startpos = start;
            endpos = end;
            Text = ""; // must initialize with empty string, may cause null reference exceptions otherwise
            Value = null;
        }

        public void UpdateRange(Token token)
        {
            if (token.StartPos < startpos) startpos = token.StartPos;
            if (token.EndPos > endpos) endpos = token.EndPos;
        }

        public override string ToString()
        {
            if (Text != null)
                return Type.ToString() + " '" + Text + "'";
            else
                return Type.ToString();
        }
    }

    #endregion
}
