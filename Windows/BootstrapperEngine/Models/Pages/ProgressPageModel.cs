using System;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Org.InCommon.InCert.BootstrapperEngine.Views.Pages;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    public class ProgressPageModel : AbstractPageModel
    {
        private string _title;
        private string _subtitle;
        private string _note;
        private string _originalNoteText;
        private int _dotCount;
        private readonly DispatcherTimer _timer;

        public ProgressPageModel(PagedViewModel model)
            : base(new ProgressPage(), model)
        {
            Model.BaseModel.Bootstrapper.ExecutePackageBegin += ExecutePackageBegin;
            Model.BaseModel.Bootstrapper.ApplyBegin += ApplyBegin;
            Model.BaseModel.Bootstrapper.ExecutePackageComplete += ExecutePackageComplete;
            Model.BaseModel.Bootstrapper.ExecuteProgress += ExecuteProgressHandler;
            Foreground = model.Foreground;
            Background = model.Background;
            _timer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 500),
                    IsEnabled = true
                };
            _timer.Tick += TimerTickHandler;
        }

        void TimerTickHandler(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_originalNoteText))
                return;

            Model.Dispatcher.Invoke(
                new Action(() => Note = string.Format("{0}{1}", _originalNoteText, new string('.', _dotCount))));

            _dotCount++;
            if (_dotCount > 4) _dotCount = 0;
        }

       
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set { _subtitle = value; OnPropertyChanged("Subtitle"); }
        }

        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                if (string.IsNullOrWhiteSpace(_originalNoteText))
                    _originalNoteText = value;

                OnPropertyChanged("Note");
            }
        }

        private void ApplyBegin(object sender, ApplyBeginEventArgs e)
        {

            //SetPreparingMessages();
        }

        private void ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            e.Result = Result.Ok;

            lock (this)
            {
                var title = Model.BaseModel.GetPackageAlias(e.PackageId, "");
               
                if (!string.IsNullOrWhiteSpace(title))
                {
                    Title = Model.BaseModel.PlannedAction.GetProgressTitle(title);

                    var progressNote = Model.BaseModel.PlannedAction.GetProgressNote(title);
                    if (!_originalNoteText.Equals(progressNote, StringComparison.InvariantCulture))
                    {
                        _originalNoteText = progressNote;
                        Note = _originalNoteText;
                    }
                }
            }
        }

        private void ExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            Subtitle = "";
        }

        void ExecuteProgressHandler(object sender, ExecuteProgressEventArgs e)
        {
            lock (this)
            {
                Subtitle = string.Format("{0}% complete", e.ProgressPercentage);
                e.Result = Result.Ok;
            }
        }
    }
}
