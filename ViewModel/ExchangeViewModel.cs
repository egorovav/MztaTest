using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;

namespace MztaTest.ViewModel
{
    public class ExchangeViewModel : BaseViewModel
    {
        public ExchangeViewModel(SettingsViewModel aSettings, Dispatcher aDispatcher)
        {
            this.FSettings = aSettings;
            this.FDispatcher = aDispatcher;
        }

        static object PortLocker = new object();
        private SettingsViewModel FSettings;
        private Dispatcher FDispatcher;

        public string PortName
        {
            get { return this.FSettings.PortName; }
        }


        public static string IsInputDataInHexFormatPropertyName = "IsInputDataInHexFormat";
        private bool FIsInputDataInHexFormat = false;
        public bool? IsInputDataInHexFormat
        {
            get { return this.FIsInputDataInHexFormat; }
            set
            {
                this.FIsInputDataInHexFormat = value ?? false;
                NotifyPropertyChanged(IsInputDataInHexFormatPropertyName);
            }
        }

        public static string ExchangePropertyName = "Exchange";
        private ObservableCollection<string> FExchange = new ObservableCollection<string>();
        public ObservableCollection<string> Exchange
        {
            get { return this.FExchange; }
            set
            {
                this.FExchange = value;
                NotifyPropertyChanged(ExchangePropertyName);
            }
        }

        public static string MessagePropertyName = "Message";
        private string FMessage;
        public string Message
        {
            get { return this.FMessage; }
            set
            {
                this.FMessage = value;
                NotifyPropertyChanged(MessagePropertyName);
                this.FSendCommand.CanExecute(this);
            }
        }

        public static string IsRunPropertyName = "IsRun";
        private bool FIsRun = false;
        public bool IsRun
        {
            get { return this.FIsRun; }
            set
            {
                this.FIsRun = value;
                this.FReadCommand.CanExecute(this);
                this.FStopCommand.CanExecute(this);
            }
        }

        CancellationTokenSource FRunCancelationTokenSource;

        public async void Go()
        {
            if (this.IsRun)
                return;

            this.FRunCancelationTokenSource = new CancellationTokenSource();
            this.FRunCancelationTokenSource.Token.Register(() => this.IsRun = false);
            this.IsRun = true;
            this.IsRun = await Task.Run<bool>(() =>
            {
                try
                {
                    while (!this.FRunCancelationTokenSource.Token.IsCancellationRequested)
                    {
                        int _bufSize = 0;
                        byte[] _buf = null;

                        lock(PortLocker)
                        {
                            _bufSize = this.FSettings.Port.BytesToRead;
                            if (_bufSize > 0)
                            {
                                _buf = new byte[_bufSize];
                                this.FSettings.Port.Read(_buf, 0, _bufSize);
                            }
                        }
                        if (_bufSize > 0)
                        {
                            StringBuilder _sb = new StringBuilder(this.PortName);
                            _sb.Append("->");
                            for (int i = 0; i < _bufSize; i++)
                                _sb.Append(_buf[i].ToString("X"));
                            this.FDispatcher.InvokeAsync(() => this.Exchange.Add(_sb.ToString()));
                        }
                        Thread.Sleep(100);
                    }
                    return false;
                }
                catch (Exception exc)
                {
                    this.ErrorString = exc.Message;
                    return false;
                }

            }, this.FRunCancelationTokenSource.Token);
        }

        #region =Commands=

        private DelegateCommand FSendCommand = new DelegateCommand(ExecuteSend, CanExecuteSend);
        public DelegateCommand SendCommand
        {
            get { return this.FSendCommand; }
        }

        private static void ExecuteSend(object aCommandData)
        {
            var _model = (ExchangeViewModel)aCommandData;

            byte[] _bytes = null;
            int _bytesNum = 0;

            if (_model.FSettings.Port != null)
            {
                var _sb = new StringBuilder(_model.PortName);
                _sb.Append("<-");

                if (_model.IsInputDataInHexFormat != true)
                {
                    var _dc = ASCIIEncoding.ASCII.GetEncoder();
                    _bytes = new byte[_model.Message.Length];
                    int _charUsed = 0;
                    bool _completed = false;
                    _dc.Convert(_model.Message.ToArray(),
                        0,
                        _model.Message.Length,
                        _bytes,
                        0,
                        _model.Message.Length,
                        true,
                        out _charUsed, out _bytesNum, out _completed);

                    for (int i = 0; i < _bytesNum; i++)
                        _sb.Append(_bytes[i].ToString("X"));
                }
                else
                {
                    _bytesNum = _model.Message.Length / 2;
                    _bytes = new byte[_bytesNum];
                    for(int i = 0; i < _bytesNum; i++)
                    {
                        try 
                        {
                            _bytes[i] = Convert.ToByte(_model.Message.Substring(2 * i, 2), 16); 
                        }
                        catch(FormatException exc)
                        {
                            _model.ErrorString = String.Format("Invalid input - {0}.", _model.Message);
                            _bytesNum = 0;
                        }
                    }

                    _sb.Append(_model.Message);
                }

                if (!_model.FSettings.Port.IsOpen)                    
                    _model.FSettings.Port.Open();

                if (_bytesNum != 0)
                {
                    Task.Run(() =>
                        {
                            lock (PortLocker)
                            {
                                _model.FSettings.Port.Write(_bytes, 0, _bytesNum);
                                _model.FDispatcher.InvokeAsync(() => _model.Exchange.Add(_sb.ToString()));
                            }
                        });

                    
                }

                _model.Message = String.Empty;
            }
        }

        private static bool CanExecuteSend(object aCommandData)
        {
            var _model = (ExchangeViewModel)aCommandData;
            return _model != null 
                && !String.IsNullOrEmpty(_model.Message) 
                && !String.IsNullOrEmpty(_model.PortName);
        }

        private DelegateCommand FReadCommand = new DelegateCommand(ExecuteRead, CanExecuteRead);
        public DelegateCommand ReadCommand
        {
            get { return this.FReadCommand; }
        }

        private static void ExecuteRead(object aCommandData)
        {
            var _model = (ExchangeViewModel)aCommandData;
            _model.Go();
        }

        private static bool CanExecuteRead(object aCommandData)
        {
            var _model  = (ExchangeViewModel)aCommandData;
            if (_model == null)
                return true;
            return _model.FSettings.Port != null &&  !_model.IsRun;
        }

        private DelegateCommand FStopCommand = new DelegateCommand(ExecuteStop, CanExecuteStop);
        public DelegateCommand StopCommand
        {
            get { return this.FStopCommand; }
        }

        private static void ExecuteStop(object aCommandData)
        {
            var _model = (ExchangeViewModel)aCommandData;
            if (_model.FRunCancelationTokenSource != null)
            {
                _model.FRunCancelationTokenSource.Cancel();
            }
        }

        private static bool CanExecuteStop(object aCommandData)
        {
            var _model = (ExchangeViewModel)aCommandData;
            if (_model == null)
                return false;
            return _model.IsRun;
        }

        #endregion
    }
}
