using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        }
    }
}
