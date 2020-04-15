using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CommandPaletteLibrary
{
    public class TokenTextBox : RichTextBox
    {
        public DataTemplate TokenTemplate
        {
            get { return (DataTemplate)GetValue(TokenTemplateProperty); }
            set { SetValue(TokenTemplateProperty, value); }
        }

        public static readonly DependencyProperty TokenTemplateProperty =
            DependencyProperty.Register(nameof(TokenTemplate), typeof(DataTemplate), typeof(TokenTextBox), new PropertyMetadata(null));

        public TokenTextBox()
        {
        }

        public void ReplaceCurrentTextToToken(object token)
        {
            var targetParagraph = Document.Blocks.First() as Paragraph;
            if (targetParagraph == null)
            {
                return;
            }

            var lastRun = targetParagraph.Inlines.LastOrDefault() as Run;
            if (lastRun != null)
            {
                lastRun.Text = string.Empty;
            }

            var tokenUI = CreateTokenUi(token);
            targetParagraph.Inlines.InsertBefore(lastRun, tokenUI);
        }

        public void FocusToLast()
        {
            CaretPosition = CaretPosition.DocumentEnd;
        }

        public void Clear()
        {
            var targetParagraph = Document.Blocks.First() as Paragraph;
            var lastRun = targetParagraph.Inlines.LastOrDefault() as Run;
            targetParagraph.Inlines.Clear();
            if (lastRun != null)
            {
                lastRun.Text = string.Empty;
                targetParagraph.Inlines.Add(lastRun);
            }
        }

        private InlineUIContainer CreateTokenUi(object token)
        {
            var contentPresenter = new ContentPresenter()
            {
                Content = token,
                ContentTemplate = TokenTemplate
            };

            return new InlineUIContainer(contentPresenter) { BaselineAlignment = BaselineAlignment.TextBottom };
        }
    }
}
