using DevExpress.XtraRichEdit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.CodeParser;
using DevExpress.LookAndFeel;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Drawing;
using System.Diagnostics;
using Ultra.Scripting.Core.BusinessObjects;
using Ultra.Scripting.Core.Win;

namespace Ultra.Scripting.Core.Win
{
    /// <summary>
    ///  This class implements the Execute method of the ISyntaxHighlightService interface to parse and colorize the text.
    /// </summary>
    public class MySyntaxHighlightService : ISyntaxHighlightService
    {
        private readonly RichEditControl syntaxEditor;
        private SyntaxColors syntaxColors;
        private SyntaxHighlightProperties commentProperties;
        private SyntaxHighlightProperties keywordProperties;
        private SyntaxHighlightProperties stringProperties;
        private SyntaxHighlightProperties xmlCommentProperties;
        private SyntaxHighlightProperties textProperties;
        private Script _CurrentScript;

        public MySyntaxHighlightService(RichEditControl syntaxEditor, Script CurrentScript)
        {
            _CurrentScript = CurrentScript;
            this.syntaxEditor = syntaxEditor;
            syntaxColors = new SyntaxColors(UserLookAndFeel.Default);
        }

        private void HighlightSyntax(TokenCollection tokens)
        {
            commentProperties = new SyntaxHighlightProperties();
            commentProperties.ForeColor = syntaxColors.CommentColor;

            keywordProperties = new SyntaxHighlightProperties();
            keywordProperties.ForeColor = syntaxColors.KeywordColor;

            stringProperties = new SyntaxHighlightProperties();
            stringProperties.ForeColor = syntaxColors.StringColor;

            xmlCommentProperties = new SyntaxHighlightProperties();
            xmlCommentProperties.ForeColor = syntaxColors.XmlCommentColor;

            textProperties = new SyntaxHighlightProperties();
            textProperties.ForeColor = syntaxColors.TextColor;

            if (tokens == null || tokens.Count == 0)
                return;

            Document document = syntaxEditor.Document;
            //CharacterProperties cp = document.BeginUpdateCharacters(0, 1);
            List<SyntaxHighlightToken> syntaxTokens = new List<SyntaxHighlightToken>(tokens.Count);
            foreach (DevExpress.CodeParser.Token token in tokens)
            {
                HighlightCategorizedToken((CategorizedToken)token, syntaxTokens);
            }
            document.ApplySyntaxHighlight(syntaxTokens);
            //document.EndUpdateCharacters(cp);
        }

        private void HighlightCategorizedToken(CategorizedToken token, List<SyntaxHighlightToken> syntaxTokens)
        {
            Color backColor = syntaxEditor.ActiveView.BackColor;
            TokenCategory category = token.Category;
            if (category == TokenCategory.Comment)
                syntaxTokens.Add(SetTokenColor(token, commentProperties, backColor));
            else if (category == TokenCategory.Keyword)
                syntaxTokens.Add(SetTokenColor(token, keywordProperties, backColor));
            else if (category == TokenCategory.String)
                syntaxTokens.Add(SetTokenColor(token, stringProperties, backColor));
            else if (category == TokenCategory.XmlComment)
                syntaxTokens.Add(SetTokenColor(token, xmlCommentProperties, backColor));
            else
                syntaxTokens.Add(SetTokenColor(token, textProperties, backColor));
        }

        private SyntaxHighlightToken SetTokenColor(DevExpress.CodeParser.Token token, SyntaxHighlightProperties foreColor, Color backColor)
        {
            if (syntaxEditor.Document.Paragraphs.Count < token.Range.Start.Line)
                return null;
            int paragraphStart = DocumentHelper.GetParagraphStart(syntaxEditor.Document.Paragraphs[token.Range.Start.Line - 1]);
            int tokenStart = paragraphStart + token.Range.Start.Offset - 1;
            if (token.Range.End.Line != token.Range.Start.Line)
                paragraphStart = DocumentHelper.GetParagraphStart(syntaxEditor.Document.Paragraphs[token.Range.End.Line - 1]);

            int tokenEnd = paragraphStart + token.Range.End.Offset - 1;
            Debug.Assert(tokenEnd > tokenStart);
            return new SyntaxHighlightToken(tokenStart, tokenEnd - tokenStart, foreColor);
        }

        #region #ISyntaxHighlightServiceMembers

        public void Execute()
        {
            string newText = syntaxEditor.Text;
            // Determine language by file extension.
            ParserLanguageID lang_ID;
            //string ext = System.IO.Path.GetExtension(syntaxEditor.Options.DocumentSaveOptions.CurrentFileName);
            if (_CurrentScript.Language == ScriptLanguage.CSharp)
            {
                lang_ID = ParserLanguage.FromFileExtension(".cs");
            }
            else
            {
                lang_ID = ParserLanguage.FromFileExtension(".vb");
            }

            // Do not parse HTML or XML.
            if (lang_ID == ParserLanguageID.Html ||
                lang_ID == ParserLanguageID.Xml ||
                lang_ID == ParserLanguageID.None) return;
            // Use DevExpress.CodeParser to parse text into tokens.
            ITokenCategoryHelper tokenHelper = TokenCategoryHelperFactory.CreateHelper(lang_ID);
            TokenCollection highlightTokens;
            highlightTokens = tokenHelper.GetTokens(newText);
            HighlightSyntax(highlightTokens);
        }

        public void ForceExecute()
        {
            Execute();
        }

        #endregion #ISyntaxHighlightServiceMembers
    }
}