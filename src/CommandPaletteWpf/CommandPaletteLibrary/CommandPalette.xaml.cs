using CommandPaletteLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private IPaletteCommand _paletteCommand = null;
        private IList<InputParameter> _inputParameterList = new List<InputParameter>();

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
        
        internal object FocusedItem
        {
            get { return (object)GetValue(FocusedItemProperty); }
            set { SetValue(FocusedItemProperty, value); }
        }

        internal static readonly DependencyProperty FocusedItemProperty =
            DependencyProperty.Register(nameof(FocusedItem), typeof(object), typeof(CommandPalette), new PropertyMetadata(null));

        public ObservableCollection<IPaletteCommand> CommandList
        {
            get { return (ObservableCollection<IPaletteCommand>)GetValue(CommandListProperty); }
            set { SetValue(CommandListProperty, value); }
        }

        public static readonly DependencyProperty CommandListProperty =
            DependencyProperty.Register(nameof(CommandList), typeof(ObservableCollection<IPaletteCommand>), typeof(CommandPalette), new PropertyMetadata(null, OnUpdateCommandList));

        internal bool IsCommandResultPopupOpen
        {
            get { return (bool)GetValue(IsCommandResultPopupOpenProperty); }
            set { SetValue(IsCommandResultPopupOpenProperty, value); }
        }

        internal static readonly DependencyProperty IsCommandResultPopupOpenProperty =
            DependencyProperty.Register(nameof(IsCommandResultPopupOpen),
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

        internal int SearchIndex
        {
            get { return (int)GetValue(SearchIndexProperty); }
            set { SetValue(SearchIndexProperty, value); }
        }

        internal static readonly DependencyProperty SearchIndexProperty =
            DependencyProperty.Register(nameof(SearchIndex), typeof(int), typeof(CommandPalette), new PropertyMetadata(-1));

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
            ExecuteCommand = new DelegateCommand((obj) =>
                {
                    
                    if (SearchIndex == -1)
                    {
                        _paletteCommand = GetCommandResultListView().SelectedItem as IPaletteCommand;
                    }

                    if (_paletteCommand == null)
                    {
                        SearchIndex = -1;
                        commandPaletteSearchPopup.IsOpen = false;
                        return;
                    }

                    if (SearchIndex == -1)
                    {
                        commandFindTextBox.ReplaceCurrentTextToToken(_paletteCommand);
                        commandFindTextBox.FocusToLast();
                    }
                    else if (SearchIndex != _paletteCommand.Parameters.Count())
                    {
                        var focusParameter = _paletteCommand.Parameters.ElementAt(SearchIndex);
                        if (!focusParameter.ValidateInput(SearchText))
                        {
                            return;
                        }
                        var inputParameter = new InputParameter(focusParameter.Name, focusParameter.CreateInput(SearchText), focusParameter.CreateInputExplanation(SearchText));

                        commandFindTextBox.ReplaceCurrentTextToToken(inputParameter);
                        commandFindTextBox.FocusToLast();
                        _inputParameterList.Add(inputParameter);
                    }

                    SearchIndex++;

                    if (_paletteCommand.Parameters.Count() != SearchIndex)
                    {
                        FocusedItem = _paletteCommand.Parameters.ElementAt(SearchIndex);
                        return;
                    }

                    var commandParameter = GenerateCommandParameter(_paletteCommand, _inputParameterList);

                    if (_paletteCommand.Command.CanExecute(commandParameter))
                    {
                        _paletteCommand.Command.Execute(commandParameter);
                    }
                    else
                    {
                    }

                    SearchIndex = -1;
                    commandFindTextBox.Clear();
                    _inputParameterList.Clear();
                    _paletteCommand = null;
                    IsCommandResultPopupOpen = false;
                    SearchText = string.Empty;
                    IsOpen = false;
                });
            SelectPrevItemCommand = new DelegateCommand(_ =>
            {
                var commandResultListView = GetCommandResultListView();
                if (commandResultListView == null)
                {
                    return;
                }
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
                var commandResultListView = GetCommandResultListView();
                if (commandResultListView == null)
                {
                    return;
                }
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

        private ListView GetCommandResultListView()
        {
            try
            {
                var contentPresenter = FindVisualChild<ContentPresenter>(resultView);
                var dataTemplate = (contentPresenter?.ContentTemplateSelector as ResultTemplateSelector)?.CommandSelector;

                return dataTemplate?.FindName("commandResultListView", contentPresenter) as ListView;
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }

        private object GenerateCommandParameter(IPaletteCommand paletteCommand, IList<InputParameter> inputParameterList)
        {
            var parameterList = new List<object>();
            foreach(var inputParameter in inputParameterList)
            {
                parameterList.Add(inputParameter.Input);
            }

            if (paletteCommand.CreateCommandParameter == null)
            {
                return null;
            }
            return paletteCommand.CreateCommandParameter(parameterList);
        }

        private void OnClose(object sender, EventArgs e)
        {
        }

        private void OnOpen(object sender, EventArgs e)
        {
            commandFindTextBox.Focus();
            commandFindTextBox.SelectAll();

            UpdateViewSource();

            var commandResultListView = GetCommandResultListView();
            if (commandResultListView != null 
                && commandResultListView.SelectedIndex == -1 
                && commandResultListView.Items.Count != 0)
            {
                commandResultListView.SelectedIndex = 0;
            }

            IsCommandResultPopupOpen = true;
            if (SearchIndex == -1)
            {
                FocusedItem = CommandList;
            }
            else
            {
                FocusedItem = CommandList.FirstOrDefault(x => x.Parameters.Count() > 0).Parameters.ElementAt(0);
            }
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

            var commandResultListView = commandPalette.GetCommandResultListView();
            if (commandResultListView != null && commandResultListView.SelectedIndex == -1 && commandResultListView.Items.Count != 0)
            {
                commandResultListView.SelectedIndex = 0;
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

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}
