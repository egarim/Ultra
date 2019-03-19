using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using System.Drawing;

namespace Ultra.Scripting.Core.Win
{
    /// <summary>
    ///  This class provides colors to highlight the tokens.
    /// </summary>
    public class SyntaxColors
    {
        private static Color DefaultCommentColor
        { get { return Color.Green; } }

        private static Color DefaultKeywordColor
        { get { return Color.Blue; } }

        private static Color DefaultStringColor
        { get { return Color.Brown; } }

        private static Color DefaultXmlCommentColor
        { get { return Color.Gray; } }

        private static Color DefaultTextColor
        { get { return Color.Black; } }

        private UserLookAndFeel lookAndFeel;

        public Color CommentColor { get { return GetCommonColorByName(CommonSkins.SkinInformationColor, DefaultCommentColor); } }
        public Color KeywordColor { get { return GetCommonColorByName(CommonSkins.SkinQuestionColor, DefaultKeywordColor); } }
        public Color TextColor { get { return GetCommonColorByName(CommonColors.WindowText, DefaultTextColor); } }
        public Color XmlCommentColor { get { return GetCommonColorByName(CommonColors.DisabledText, DefaultXmlCommentColor); } }
        public Color StringColor { get { return GetCommonColorByName(CommonSkins.SkinWarningColor, DefaultStringColor); } }

        public SyntaxColors(UserLookAndFeel lookAndFeel)
        {
            this.lookAndFeel = lookAndFeel;
        }

        private Color GetCommonColorByName(string colorName, Color defaultColor)
        {
            Skin skin = CommonSkins.GetSkin(lookAndFeel);
            if (skin == null)
                return defaultColor;
            return skin.Colors[colorName];
        }
    }
}