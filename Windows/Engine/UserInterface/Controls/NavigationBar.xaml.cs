using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions;

namespace Org.InCommon.InCert.Engine.UserInterface.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : IHasResult, IHasControlActions
    {
        private readonly List<AbstractControlAction> _actions = new List<AbstractControlAction>();
        
        public NavigationBar()
        {
            InitializeComponent();
        }

        public IResult Result { get; set; }
        
        public void ClearActions()
        {
            _actions.Clear();
        }

        public void AddActions(List<AbstractControlAction> actions)
        {
           /* var toAdd = (from action in actions 
                         let target = FindName(action.Key) as Button 
                         where target != null select action).ToList();
            _actions.AddRange(toAdd);*/
        }

        public void AddAction(AbstractControlAction action)
        {
            /*if (action == null)
                return;
            
            var target = FindName(action.Key) as Button;
            if (target == null)
                return;

            _actions.Add(action);*/
        }

        public void DoActions(bool includeOneTime)
        {
          
        }

        public void RemoveOneTimeActions()
        {
            _actions.RemoveAll(c => c.OneTime);
        }
    }
}
