using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.TaskBranches
{
    public class BranchManager : IBranchManager
    {
        private static readonly ILog Log = Logger.Create();

        private readonly Dictionary<String, ITaskBranch> _branches = new Dictionary<String, ITaskBranch>();
        private bool _initialized;

        public bool IsInitialized()
        {
            return _initialized;
        }

        public void Initialize()
        {
            try
            {
                _initialized = false;
                _branches.Clear();

                var boostrapXml = XmlUtilities.LoadXmlFromAssembly("Org.InCommon.InCert.Engine.Content.Bootstrap.xml");
                if (boostrapXml == null)
                {
                    Log.Warn("could not load bootstrap.xml from assembly resource");
                    return;
                }

                _initialized = ImportBranchesFromXml(boostrapXml);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public bool ImportBranchesFromXml(XElement node)
        {
            if (node == null)
            {
                Log.Warn("empty xml document passed to ImportBranchesFromXml; cannot import task branches");
                return false;
            }

            var branchesNode = node.Element("Branches");
            if (branchesNode == null)
                return false;

            foreach (var branchNode in branchesNode.Elements())
            {
                var branch = AbstractImportable.GetInstanceFromNode<AbstractBranch>(branchNode);
                if (branch == null || !branch.Initialized())
                    continue;

                AddBranch(branch);
            }

            return true;
        }

        public bool ImportBranches(List<ITaskBranch> branches)
        {
            foreach (var branch in branches)
                AddBranch(branch);

            return true;
        }

        public ITaskBranch GetBranch(string value)
        {
            return !_branches.ContainsKey(value) ? null : (_branches[value]);
        }

        public ITaskBranch GetBranchForRole(BranchRoles role, EngineModes mode, bool fallbackToAll)
        {
            var roleBranches = _branches.Values.Where(e => e as RoleBranch != null).Select(e => e as RoleBranch);
            
            var result = roleBranches.LastOrDefault(instance => 
                instance.BranchRole == role && 
                instance.RoleMode== mode) as ITaskBranch;

            if (result == null && fallbackToAll)
                result = GetBranchForRole(role, EngineModes.All, false) ;

            if (result == null)
                Log.DebugFormat("Cannot find branch for role {0} and mode {1}", role, mode);

            return result;
        }

        public void AddBranch(ITaskBranch branch)
        {
            if (branch == null)
                return;

            if (!branch.Initialized())
                return;

            _branches[branch.Name] = branch;
        }

       
    }
}
