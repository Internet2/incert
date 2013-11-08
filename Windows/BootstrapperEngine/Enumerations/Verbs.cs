namespace Org.InCommon.InCert.BootstrapperEngine.Enumerations
{
    public class Verbs
    {
        public static readonly Verbs Install = new Verbs("install", "installing", "installed", "installation");
        public static readonly Verbs Repair = new Verbs("repair", "repairing", "repaired", "repair");
        public static readonly Verbs Update = new Verbs("update", "updating", "updated", "update");
        public static readonly Verbs Remove = new Verbs("remove", "removing", "removed", "removal");
        public static  readonly Verbs Cancel = new Verbs("cancel", "cancelling", "cancelled", "cancellation");

        private Verbs(string infinitive, string presentParticiple, string pastParticiple, string gerundive)
        {
            Infinitive = infinitive;
            PresentParticiple = presentParticiple;
            PastParticiple = pastParticiple;
            Gerundive = gerundive;
        }

        public string Infinitive { get; private set; }
        public string PresentParticiple { get; private set; }
        public string PastParticiple { get; private set; }
        public string Gerundive { get; private set; }
    }
}
