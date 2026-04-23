namespace E_PANEL.ViewModels.Posts;

public class HomeIndexViewModel
{
    public string EtecNumber { get; set; } = string.Empty;
    public List<PostCardViewModel> FeedPosts { get; set; } = [];
    public List<PostCardViewModel> NoticePosts { get; set; } = [];
}
