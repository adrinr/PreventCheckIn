using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.TeamFoundation.VersionControl.Client;

using PreventCheckin.Configurations.Implementations;
using PreventCheckin.Services.Implementations;
using PreventCheckin.Services.Interfaces;

namespace PreventCheckinPolicies.PreventCheckin
{
    [Serializable]
    public class PreventCheckinPolicy : PolicyBase
    {
        private readonly IPreventCheckinService iPreventCheckinService;

        public PreventCheckinPolicy() : this(new PreventCheckinService(new PreventCheckinConfiguration())) // TODO: how to resolve IoC?
        {

        }

        public PreventCheckinPolicy(IPreventCheckinService iPreventCheckinService)
        {
            if (iPreventCheckinService == null)
            {
                throw new ArgumentNullException(nameof(iPreventCheckinService));
            }

            this.iPreventCheckinService = iPreventCheckinService;
        }


        public override string Type => "Prevent checkin";

        public override string TypeDescription => "Avoid to prevent a file which is protected from check-in";

        public override string Description => this.TypeDescription;

        public override PolicyFailure[] Evaluate()
        {
            var excluded = new List<string>();

            foreach (var change in this.PendingCheckin.PendingChanges.CheckedPendingChanges)
            {
                if (this.iPreventCheckinService.AreExcluded(change.LocalItem))
                {
                    excluded.Add(change.ServerItem);
                }
            }


            if (excluded.Any())
            {
                var sb = new StringBuilder();
                sb.AppendLine("The following items are protected from check-in:");
                excluded.ForEach(s => sb.AppendLine($"- {s}"));

                return new[] { new PolicyFailure(sb.ToString(), this) };
            }

            return new PolicyFailure[0];
        }

        public override bool Edit(Microsoft.TeamFoundation.VersionControl.Client.IPolicyEditArgs policyEditArgs)
        {
            return true;
        }
    }

    public interface IPolicyEditArgs
    {
    }
}