using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MztaTest.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null && !String.IsNullOrEmpty(propertyName))
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static string ErrorStringPropertyName = "ErrorString";
        private string FErrorString;
        public string ErrorString
        {
            get { return this.FErrorString; }
            set
            {
                this.FErrorString = value;
                NotifyPropertyChanged(ErrorStringPropertyName);
            }
        }

        protected static StringBuilder GetErrorString(Exception exc)
        {
            StringBuilder _sb = new StringBuilder(exc.Message);
            Exception _temp = exc;
            while (_temp.InnerException != null)
            {
                _temp = _temp.InnerException;
                _sb.AppendLine(_temp.Message);
            }
            _sb.AppendLine(exc.StackTrace);
            return _sb;
        }
    }
}
