using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public class HtmlDialogModel:AbstractDialogModel
    {
        public HtmlDialogModel(IHasEngineFields engine) : base(engine, new HtmlWindow())
        {
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.SingleBorderWindow; 
        }

        private Uri Uri
        {
            get
            {
                var browserInstance = ((HtmlWindow) DialogInstance).BrowserControl;
                return browserInstance.Source;
            }
            set
            {
                var browserInstance = ((HtmlWindow)DialogInstance).BrowserControl;
                browserInstance.Source = value;
                OnPropertyChanged();
            }
        }

        public IResult ShowPage(string url)
        {
            Uri = new Uri(url);
            return WaitForResult();
        }
    }
}
