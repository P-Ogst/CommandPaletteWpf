﻿using CommandPaletteLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommandPaletteLibrary
{
    /// <summary>
    /// CommandPalette.xaml の相互作用ロジック
    /// </summary>
    public partial class CommandPalette : UserControl
    {
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen),
                                        typeof(bool),
                                        typeof(CommandPalette),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<IPaletteCommand> CommandList
        {
            get { return (ObservableCollection<IPaletteCommand>)GetValue(CommandListProperty); }
            set { SetValue(CommandListProperty, value); }
        }

        public static readonly DependencyProperty CommandListProperty =
            DependencyProperty.Register(nameof(CommandList), typeof(ObservableCollection<IPaletteCommand>), typeof(CommandPalette), new PropertyMetadata(null, OnUpdateCommandList));

        internal bool IsResultPopupOpen
        {
            get { return (bool)GetValue(IsResultPopupOpenProperty); }
            set { SetValue(IsResultPopupOpenProperty, value); }
        }

        internal static readonly DependencyProperty IsResultPopupOpenProperty =
            DependencyProperty.Register(nameof(IsResultPopupOpen),
                                        typeof(bool),
                                        typeof(CommandPalette),
                                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        internal ICommand SelectNextItemCommand
        {
            get { return (ICommand)GetValue(SelectNextItemCommandProperty); }
            set { SetValue(SelectNextItemCommandProperty, value); }
        }

        internal static readonly DependencyProperty SelectNextItemCommandProperty =
            DependencyProperty.Register(nameof(SelectNextItemCommand), typeof(ICommand), typeof(CommandPalette), new PropertyMetadata(null));

        internal ICommand SelectPrevItemCommand
        {
            get { return (ICommand)GetValue(SelectPrevItemCommandProperty); }
            set { SetValue(SelectPrevItemCommandProperty, value); }
        }

        internal static readonly DependencyProperty SelectPrevItemCommandProperty =
            DependencyProperty.Register(nameof(SelectPrevItemCommand), typeof(ICommand), typeof(CommandPalette), new PropertyMetadata(null));

        internal ICommand ExecuteCommand
        {
            get { return (ICommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }

        internal static readonly DependencyProperty ExecuteCommandProperty =
            DependencyProperty.Register(nameof(ExecuteCommand), typeof(ICommand), typeof(CommandPalette), new PropertyMetadata(null));

        internal string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        internal static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(CommandPalette), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSearchTextChanged));

        public CommandPalette()
        {
            InitializeComponent();
            ExecuteCommand = new DelegateCommand((command) =>
                {
                    var paletteCommand = (command as IPaletteCommand);
                    if (paletteCommand != null &&
                        paletteCommand.Command.CanExecute(null))
                    {
                        (command as IPaletteCommand)?.Command?.Execute(null);
                    }
                    commandPaletteSearchPopup.IsOpen = false;
                });
            SelectPrevItemCommand = new DelegateCommand(_ =>
            {
                var count = commandResultListView.Items.Count;
                if (count == 0)
                {
                    return;
                }
                if (commandResultListView.SelectedIndex == -1 || commandResultListView.SelectedIndex == 0)
                {
                    commandResultListView.SelectedIndex = count - 1;
                    return;
                }
                commandResultListView.SelectedIndex--;
            });
            SelectNextItemCommand = new DelegateCommand(_ =>
            {
                var count = commandResultListView.Items.Count;
                if (count == 0)
                {
                    return;
                }
                if (commandResultListView.SelectedIndex == -1 || commandResultListView.SelectedIndex == count - 1)
                {
                    commandResultListView.SelectedIndex = 0;
                    return;
                }
                commandResultListView.SelectedIndex++;
            });
            commandPaletteSearchPopup.Opened += OnOpen;
            commandPaletteSearchPopup.Closed += OnClose;
        }

        private void OnClose(object sender, EventArgs e)
        {
        }

        private void OnOpen(object sender, EventArgs e)
        {
            commandFindTextBox.Focus();
            commandFindTextBox.SelectAll();

            UpdateViewSource();

            if (commandResultListView.SelectedIndex == -1 && commandResultListView.Items.Count != 0)
            {
                commandResultListView.SelectedIndex = 0;
            }

            IsResultPopupOpen = true;
        }

        private bool Contains(string src, string value)
        {
            return src.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        private void UpdateViewSource()
        {
            var viewSource = CollectionViewSource.GetDefaultView(CommandList);
            viewSource.Refresh();
        }

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var commandPalette = (d as CommandPalette);
            commandPalette.UpdateViewSource();

            if (commandPalette.commandResultListView.SelectedIndex == -1 && commandPalette.commandResultListView.Items.Count != 0)
            {
                commandPalette.commandResultListView.SelectedIndex = 0;
            }
        }

        private static void OnUpdateCommandList(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var commandPalette = d as CommandPalette;
            var viewSource = CollectionViewSource.GetDefaultView(commandPalette.CommandList);
            if (viewSource == null)
            {
                return;
            }
            viewSource.Filter += x =>
            {
                var command = x as IPaletteCommand;
                if (command == null)
                {
                    return false;
                }
                // TODO: パラメーターを扱うときにどのようにするか決める
                if (!command.Command.CanExecute(null))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(commandPalette.SearchText))
                {
                    return true;
                }
                if (commandPalette.Contains(command.Name, commandPalette.SearchText))
                {
                    return true;
                }
                if (commandPalette.Contains(command.Explanation, commandPalette.SearchText))
                {
                    return true;
                }
                return false;
            };
            viewSource.Refresh();
        }
    }
}
