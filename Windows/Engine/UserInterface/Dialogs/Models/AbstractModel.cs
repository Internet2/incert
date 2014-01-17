using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models
{
    public abstract class AbstractModel : PropertyNotifyBase, IHasControlActions
    {
        private readonly List<AbstractControlAction> _actions = new List<AbstractControlAction>();
        private readonly Dictionary<string, AbstractModel> _childModels = new Dictionary<string, AbstractModel>();

        private Visibility _visibility;
        private bool _enabled;

        private Visibility? _originalVisibiltyValue;
        private bool? _originalEnabledValue;

        private DependencyObject _content;
        private Dock _dock;

        public string ControlKey { get; set; }
        public string SettingKey { get; protected set; }

        public DependencyObject Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(); }
        }

        public AbstractDialogModel RootDialogModel { get; private set; }

        protected AbstractModel(AbstractDialogModel rootDialogModel)
        {
            if (rootDialogModel == null)
                throw new InvalidOperationException("Root dialog model must be instantiated");

            RootDialogModel = rootDialogModel;
        }

        protected AbstractModel(AbstractModel parentModel): this(parentModel.RootDialogModel)
        {
            Parent = parentModel;
        }

        public AbstractModel Parent { get; private set; }

        public IAppearanceManager AppearanceManager { get { return RootDialogModel.AppearanceManager; } }

        public virtual Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                if (!_originalVisibiltyValue.HasValue)
                    _originalVisibiltyValue = value;

                OnPropertyChanged();
            }
        }

        public virtual bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (!_originalEnabledValue.HasValue)
                    _originalEnabledValue = value;
                OnPropertyChanged();
            }
        }

        protected internal virtual void RestoreOriginalValues()
        {
            if (_originalEnabledValue.HasValue)
                Enabled = _originalEnabledValue.Value;

            if (_originalVisibiltyValue.HasValue)
                Visibility = _originalVisibiltyValue.Value;

            foreach (var child in _childModels.Values)
            {
                child.RestoreOriginalValues();
            }
        }
        
        public Dock Dock
        {
            get { return _dock; }
            set { _dock = value; OnPropertyChanged(); }
        }

        public virtual T LoadContent<T>(AbstractContentWrapper wrapper) where T : DependencyObject
        {
            return default(T);
        }

        protected virtual void InitializeBindings(DependencyObject target)
        {
            InitializeVisiblityBinding(target);
            InitializeEnabledBinding(target);
            InitializeDockBinding(target);
        }

        protected void InitializeValues()
        {
            Visibility = Visibility.Visible;
            Enabled = true;
            Dock = Dock.Top;
        }

        protected void InitializeVisiblityBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, UIElement.VisibilityProperty, GetOneWayBinding(this, "Visibility"));
        }

        protected void InitializeEnabledBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, UIElement.IsEnabledProperty,GetOneWayBinding(this, "Enabled"));
        }

        protected void InitializeDockBinding(DependencyObject target)
        {
            BindingOperations.SetBinding(target, DockPanel.DockProperty,GetOneWayBinding(this, "Dock"));
        }

        public Dictionary<string, AbstractModel> ChildModels { get { return _childModels; } }

        protected void AddChildModel(string key, AbstractModel value)
        {
            if (value == null)
                return;

            if (string.IsNullOrWhiteSpace(key))
                key = Guid.NewGuid().ToString();

            value.ControlKey = key;
            ChildModels[key] = value;
        }

        public void ClearActions()
        {
            _actions.Clear();

            foreach (var childModel in _childModels.Values)
                childModel.ClearActions();

        }

        public virtual void AddAction(AbstractControlAction action)
        {
            if (action == null)
                return;

            if (!action.ControlKeys.Contains(ControlKey))
                return;
            
            _actions.Add(action);
        }

        public virtual void AddActions(List<AbstractControlAction> actions)
        {
            foreach (var action in actions)
                AddAction(action);

            foreach (var childModel in _childModels.Values)
                childModel.AddActions(actions);

        }

        public virtual void DoActions(bool includeOneTime)
        {
                foreach (var action in _actions)
                    action.DoAction(this, includeOneTime);
                
                foreach (var child in _childModels.Values)
                    child.DoActions(includeOneTime);
          
        }

        public T FindFirstParent<T>() where T : AbstractModel
        {
            if (Parent is T)
                return Parent as T;

            return Parent == null ? null : Parent.FindFirstParent<T>();
        }

        public void ClearChildModels()
        {
            ChildModels.Clear();
        }

        public void ListNamedModels(string name, ref List<AbstractModel> modelList)
        {
            if (_childModels.ContainsKey(name))
                modelList.Add(_childModels[name]);

            foreach (var childModel in _childModels.Values)
                childModel.ListNamedModels(name, ref modelList);
        }

        public void EnableDisableAllControls(List<string> excludeList, bool enabled)
        {
            foreach (var key in _childModels.Keys.Where(key => !excludeList.Contains(key)))
            {
                _childModels[key].Enabled = enabled;
                _childModels[key].EnableDisableAllControls(excludeList, enabled);
            }
        }

        public Style GetNamedStyle(string value)
        {
            if (RootDialogModel == null)
                return null;

            if (RootDialogModel.DialogInstance == null)
                return null;

            return RootDialogModel.DialogInstance.TryFindResource(value) as Style;
        }

        public T FindChildModel<T>(string controlKey) where T : AbstractModel
        {
            if (_childModels.ContainsKey(controlKey))
                return _childModels[controlKey] as T;

            return _childModels.Values.Select(
                childModel => childModel.FindChildModel<T>(controlKey)).FirstOrDefault(result => result != null);
        }


        public void ListKeyedModels(string key, ref List<AbstractModel> modelList)
        {
            modelList.AddRange(_childModels.Values.Where(child => !string.IsNullOrEmpty(child.SettingKey)).Where(child => child.SettingKey.Equals(key, StringComparison.InvariantCulture)));

            foreach (var child in _childModels.Values)
                child.ListKeyedModels(key, ref modelList);
        }
    }
}
