using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections.ObjectModel;

namespace MztaTest.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            this.BaudRate = 9600;
        }

        const int DataBits = 8; 
        SerialPort FPort;
        public SerialPort Port
        {
            get { return this.FPort; }
        }

        public static string PortListPropertyName = "PortList";
        private List<string> FPortList;
        public List<string> PortList
        {
            get { return this.FPortList; }
            set
            {
                this.FPortList = value;
                NotifyPropertyChanged(PortListPropertyName);
            }
        }

        public static string BaudRatePropertyName = "BaudRate";
        private int FBaudRate;
        public int BaudRate
        {
            get { return this.FBaudRate; }
            set
            {
                this.FBaudRate = value;
                if(this.FPort != null)
                    this.FPort.BaudRate = value;
                NotifyPropertyChanged(BaudRatePropertyName);
            }
        }

        public static string PortNamePropertyName = "PortName";
        private string FPortName;
        public string PortName
        {
            get { return this.FPortName; }
            set
            {
                this.FPortName = value;
                if (!String.IsNullOrEmpty(value))
                {
                    if (this.FPort != null && this.FPort.IsOpen)
                    {
                        this.FPort.Close();
                        this.FPort = null;
                    }
                    this.FPort = new SerialPort(value, this.BaudRate, Parity.None, DataBits, StopBits.One);
                    this.FPort.Open();
                }
                NotifyPropertyChanged(PortNamePropertyName);
            }
        }

        #region =Commands=

        private DelegateCommand FGetPortListCommand = new DelegateCommand(ExecuteGetPortList, CanExecuteGetPortList);
        public DelegateCommand GetPortListCommand
        {
            get { return this.FGetPortListCommand; }
        }

        private static void ExecuteGetPortList(object aCommandData)
        {
            var _model = (SettingsViewModel)aCommandData;
            _model.GetPortList();
        }

        private static bool CanExecuteGetPortList(object aCommandData)
        {
            return true;
        }

        private void GetPortList()
        {
            string[] _portNames = SerialPort.GetPortNames();
            this.PortList = _portNames.ToList();
        }

        #endregion
    }
}
