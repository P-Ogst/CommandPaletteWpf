using CommandPaletteLibrary;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;

namespace CommandPaletteSandbox
{
    public class MainWindowViewModel : IDisposable
    {
        private PaletteCommandService _commandService;
        public ObservableCollection<IPaletteCommand> CommandList => _commandService.CommandList;

        public enum TargetState
        {
            NotConnected,
            Connecting,
            Connected,
            Recording,
            StopRecording
        }

        public IReactiveProperty<TargetState> State { get; }

        internal CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveCommand CommandPaletteCommand { get; }

        public IReactiveProperty<bool> IsCommandPaletteOpen { get; }

        public ReactiveCollection<string> TargetList { get; }

        public ReactiveCommand<string> ConnectCommand { get; }
        public ReactiveCommand DisconnectCommand { get; }
        public IReactiveProperty<float> Volume { get; }

        public IReactiveProperty<string> RecordingDirectory { get; }
        public ReactiveCommand SelectRecordingDirectoryCommand { get; }
        public ReactiveCommand StartRecordingCommand { get; }
        public ReactiveCommand StopRecordingCommand { get; }

        internal ReactiveCommand<float> ChangeVolumeCommand { get; }

        public MainWindowViewModel()
        {
            _commandService = new PaletteCommandService();
            CommandPaletteCommand = new ReactiveCommand();
            CommandPaletteCommand.Subscribe(
                _ =>
                {
                    if (!IsCommandPaletteOpen.Value)
                    {
                        IsCommandPaletteOpen.Value = true;
                    }
                    else
                    {
                        IsCommandPaletteOpen.Value = false;
                    }
                });
            IsCommandPaletteOpen = new ReactiveProperty<bool>();

            State = new ReactiveProperty<TargetState>();

            TargetList = new ReactiveCollection<string>();
            TargetList.Add(new string("default"));
            TargetList.Add(new string("TargetA"));
            TargetList.Add(new string("TargetB"));
            TargetList.Add(new string("TargetC"));

            ConnectCommand = State
                .Select(x => x == TargetState.NotConnected)
                .ToReactiveCommand<string>()
                .AddTo(Disposable);
            ConnectCommand.Subscribe(
                (target) =>
                {
                    State.Value = TargetState.Connected;
                });

            var targetItemList = new List<IPaletteSearchItem>();
            foreach(var target in TargetList)
            {
                var targetItem = PaletteParameterFactory.CreateSearchItem(target, target.ToString());
                targetItemList.Add(targetItem);
            }
            var targetParameter = PaletteParameterFactory.CreateSearchParameter(targetItemList, "Target", "接続先を指定してください");
            _commandService.AddCommand(ConnectCommand,
                                       nameof(ConnectCommand),
                                       "ターゲットと接続します",
                                       (paramList) =>
                                       {
                                           return paramList.First();
                                       },
                                       targetParameter);

            DisconnectCommand = State
                .Select(x => x != TargetState.NotConnected)
                .ToReactiveCommand()
                .AddTo(Disposable);
            DisconnectCommand.Subscribe(
                _ =>
                {
                    State.Value = TargetState.NotConnected;
                });
            _commandService.AddCommand(DisconnectCommand, nameof(DisconnectCommand), "ターゲットとの接続を切断します");
            Volume = new ReactiveProperty<float>().AddTo(Disposable);
            Volume.Value = 0.5f;
            ChangeVolumeCommand = new ReactiveCommand<float>();
            ChangeVolumeCommand.Subscribe((volume) =>
            {
                Volume.Value = volume;
            });
            var volumeParameter = PaletteParameterFactory.CreateMinMaxParameter(0.0f, 1.0f, "Volume", "ボリュームを変更します (0.0 - 1.0)");
            _commandService.AddCommand(ChangeVolumeCommand,
                                       nameof(ChangeVolumeCommand),
                                       "プレイバックボリュームを変更します",
                                       (paramList) =>
                                       {
                                           return paramList.First();
                                       },
                                       volumeParameter);
            RecordingDirectory = new ReactiveProperty<string>().AddTo(Disposable);
            SelectRecordingDirectoryCommand = State
                .Select(x => x != TargetState.Recording)
                .ToReactiveCommand()
                .AddTo(Disposable);
            SelectRecordingDirectoryCommand.Subscribe(
                _ =>
                {
                    MessageBox.Show("SelectCommand");
                });
            _commandService.AddCommand(SelectRecordingDirectoryCommand, nameof(SelectRecordingDirectoryCommand), "録音の保存先を選択します");
            StartRecordingCommand = State
                .Select(x => x == TargetState.Connected)
                .ToReactiveCommand()
                .AddTo(Disposable);
            StartRecordingCommand.Subscribe(
                _ =>
                {
                    State.Value = TargetState.Recording;
                });
            _commandService.AddCommand(StartRecordingCommand, nameof(StartRecordingCommand), "録音を開始します");
            StopRecordingCommand = State
                .Select(x => x == TargetState.Recording)
                .ToReactiveCommand()
                .AddTo(Disposable);
            StopRecordingCommand.Subscribe(
                _ =>
                {
                    State.Value = TargetState.Connected;
                });
            _commandService.AddCommand(StopRecordingCommand, nameof(StopRecordingCommand), "録音を終了します");
        }

        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}
