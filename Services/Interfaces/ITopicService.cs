using RazorBlog.Data.ViewModels;
using RazorBlog.Services.Communications;
using System.Threading.Tasks;

namespace RazorBlog.Services.Interfaces;

public interface ITopicService
{
    Task<Result> CreateTopic(TopicViewModel viewModel);

    Task<Result> UpdateTopic(TopicViewModel viewModel);

    /// <summary>
    /// Schedule removal (soft-delete) of a topic. Authors are informed.
    /// </summary>
    /// <returns></returns>
    Task<Result> ScheduleTopicRemoval();
}