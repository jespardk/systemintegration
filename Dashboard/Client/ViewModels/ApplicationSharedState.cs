using Client.ViewModels;

namespace Client.State
{
    public static class ApplicationSharedState
    {
        public static List<LiveViewItem> ArrivedMessages { get; set; } = new List<LiveViewItem>();
        public static void AddMessage(LiveViewItem item)
        {
            ArrivedMessages.Add(item);
            ArrivedMessages = ArrivedMessages.OrderByDescending(x => x.DateTime).ToList();
        }
    }
}
