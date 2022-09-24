namespace Gitlab.Sidekick.Application.Models;

public class BaseCreation
{
    public Uri Host { get; set; }
    public ParentGroupTemplate ParentGroup { get; set; }
    public string DefaultVisibility { get; set; }
    public string DefaultBranch { get; set; }
    public string DefaultAuthor { get; set; }
    public string DefaultEmail { get; set; }
    public string DefaultRootPath { get; set; }
    public Dictionary<string, IEnumerable<string>> UserGroups { get; set; }
    public Dictionary<string, SlackTemplate> SlackServices { get; set; }
}
