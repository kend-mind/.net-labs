namespace VPBANK.RMD.Utils.AuditLog.Models
{
    public class ElasticSearchOptions
    {
        public string Uri { get; set; }
        public bool FixAlias { get; set; }
        public string NotiAlias { get; set; }
        public string AuditAlias { get; set; }
        public bool IndexPerYear { get; set; }
        public int AmountOfPreviousIndicesUsedInAlias { get; set; }

        public void UseSettings(string uri, bool fixAlias, string notiAlias, string auditAlias, bool indexPerYear, int amountOfPreviousIndicesUsedInAlias)
        {
            Uri = uri;
            FixAlias = fixAlias;
            NotiAlias = notiAlias;
            AuditAlias = auditAlias;
            IndexPerYear = indexPerYear;
            AmountOfPreviousIndicesUsedInAlias = amountOfPreviousIndicesUsedInAlias;
        }
    }
}
