public interface ISpatulaGrabbableReceiver
{
    public string[] AllowedGrabbableNames { get; }

    public void Pass(SpatulaGrabbable grabbable);
    void ShowHighlight();
    void HideHighlight();
}
