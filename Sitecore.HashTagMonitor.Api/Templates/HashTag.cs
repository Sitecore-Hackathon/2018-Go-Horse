namespace Sitecore.HashTagMonitor.Api.Templates
{
    public partial class HashTag
    {
        public PatternCard PatternCard => PatternCardReference.TargetItem == null
            ? null
            : new PatternCard(PatternCardReference.TargetItem);
    }
}
