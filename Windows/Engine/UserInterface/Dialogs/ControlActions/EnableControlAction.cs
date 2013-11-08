using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    class EnableControlAction : AbstractControlAction
    {
        private static readonly ILog Log = Logger.Create();


        public EnableControlAction(IEngine engine) : base(engine)
        {
        }

        public override void DoAction(AbstractModel model, bool includeOneTime)
        {
            try
            {
                if (!IsOneTimeOk(includeOneTime))
                    return;
                
                if (model == null)
                    return;

                var result = EvaluateConditions();
                if (!result.Result)
                    return;

                model.Enabled = true;
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

    }
}
