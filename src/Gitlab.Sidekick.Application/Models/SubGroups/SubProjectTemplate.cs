namespace Gitlab.Sidekick.Application.Models.SubGroups;

public class SubProjectTemplate
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }
    public string DefaultBranch { get; set; } = "";
    public string DefaultVisibility { get; set; } = "";
    public string AvatarFile { get; set; } = "";
    public string SlackService { get; set; } = "";
    public IEnumerable<string> MemberAssociateGroups { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
}
