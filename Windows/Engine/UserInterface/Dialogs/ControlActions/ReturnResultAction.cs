using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    class ReturnResultAction:AbstractControlAction
    {
        public ReturnResultAction(IEngine engine) : base(engine)
        {
        }

        public AbstractTaskResult Result { get; set; }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);

            var typeName = XmlUtilities.GetTextFromAttribute(node, "result");
            if (string.IsNullOrWhiteSpace(typeName))
                return;

            Result = ReflectionUtilities.LoadFromAssembly<AbstractTaskResult>(typeName);

        }
       
        public override void DoAction(AbstractModel model, bool includeOneTime)
        {
            if (!IsOneTimeOk(includeOneTime))
                return;
            
            if (model == null)
                return;

            if (model.RootDialogModel == null)
                return;

            var conditionResult = EvaluateConditions();
            if (!conditionResult.Result)
                return;

            model.RootDialogModel.Result = Result;
        }

        public override bool Initialized()
        {
            return Result != null;
        }
    }
}
