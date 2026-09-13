using Bunit;
using Blog.Blazor.Components;
using Blog.Blazor.Pages;
using Xunit;

namespace Blog.Blazor.Test.Components;

public class PostHeaderTests : TestContext
{
    [Fact]
    public void RendersAuthorNameAndCurrentCity()
    {
        var post = new PostDetail.GetPostByIdDtoRes
        {
            UserProfile = new PostDetail.UserProfile
            {
                BasicInfo = new PostDetail.BasicInfo
                {
                    FirstName = "Ada",
                    LastName = "Lovelace",
                    CurrentCity = "London"
                }
            }
        };

        var cut = RenderComponent<PostHeader>(parameters => parameters
            .Add(p => p.Post, post));

        Assert.Contains("Ada Lovelace", cut.Markup);
        Assert.Contains("London", cut.Markup);
    }
}
