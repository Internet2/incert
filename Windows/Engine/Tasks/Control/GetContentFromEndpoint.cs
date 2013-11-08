using System;
using System.Collections.Generic;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class GetContentFromEndpoint:AbstractTask
    {
        private readonly List<KeyedDynamicStringPropertyEntry> _contentEntries = new List<KeyedDynamicStringPropertyEntry>();
        
        private static readonly ILog Log = Logger.Create();
       
        [PropertyAllowedFromXml]
        public string ContentName  {
            set
            {
                _contentEntries.Add(
                    new KeyedDynamicStringPropertyEntry(Engine){Key = value});
            }
        }

        public GetContentFromEndpoint(
        IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var result in _contentEntries.Select(entry => ImportContent(entry.Key)).Where(result => !result.IsOk()))
                    return result;
                
                return new NextResult();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new ExceptionOccurred(e);
            }
        }

        private IResult ImportContent(string contentName)
        {
            if (String.IsNullOrWhiteSpace(contentName))
            {
                Log.Warn("Invalid tasklist name passed to task.");
                return new CouldNotImportContent { Issue = "No task list is specified." };
            }

            var request = EndpointManager.GetContract<AbstractUrlContract>(EndPointFunctions.GetContent);
            request.Url = contentName;

            var result = request.MakeRequest<ContentDataWrapper>();
            if (result == null)
                return request.GetErrorResult();

            if (!BranchManager.ImportBranches(result.Branches.ConvertAll(instance => (ITaskBranch)instance)))
                return new CouldNotImportContent { Issue = "Unable to import task branches from xml." };

            if (!BannerManager.ImportBanners(result.Banners))
                return new CouldNotImportContent { Issue = "Unable to import banners from xml." };

            if (!ErrorManager.ImportErrorEntries(result.Errors))
                return new CouldNotImportContent { Issue = "Unable to import error definitions from xml." };
            
            if (!AdvancedMenuManager.ImportItems(result.AdvancedMenuItems))
                return new CouldNotImportContent { Issue = "Unable to import advanced menu item definitions from xml." };
            
            if (!HelpManager.ImportTopics(result.Topics))
                return new CouldNotImportContent { Issue = "Unable to import help topic item definitions from xml." };

            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Get tasklist from endpoint";
        }
    }
}
