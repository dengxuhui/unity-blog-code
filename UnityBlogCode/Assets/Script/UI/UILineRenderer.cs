namespace UnityEngine.UI.Extensions
{
    /// <summary>
    /// canvas上画线
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UILineRenderer : UIPrimitiveBase
    {
        private enum SegmentType
        {
            Start,
            Middle,
            End,
            Full,
        }
    }
}