using CefSharp;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.KeyboardHandlers
{
    public class KeyboardHandler:IKeyboardHandler
    {
        private readonly IScriptingModel _model;


        public KeyboardHandler(IScriptingModel model)
        {
            _model = model;
        }

        public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey)
        {
            if (code == 13)
            {
                _model.RaiseEvent("engine_enter_key_pressed",new EnterKeyPressedEventData());
                return true;
            }

            if (code == 27)
            {
                _model.RaiseEvent("engine_escape_key_pressed", new EnterKeyPressedEventData());
                return true;
            }

            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, bool isKeyboardShortcut)
        {
            return false;
        }
    }
}
