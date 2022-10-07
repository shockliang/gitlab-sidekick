using System.Text.Json.Serialization;

namespace Gitlab.Sidekick.Application.Models;

public partial class Properties
{
    [JsonPropertyName("webhook")] public Uri Webhook { get; set; }

    [JsonPropertyName("username")] public string Username { get; set; }

    [JsonPropertyName("notify_only_broken_pipelines")]
    public string NotifyOnlyBrokenPipelines { get; set; }

    [JsonPropertyName("notify_only_default_branch")]
    public string NotifyOnlyDefaultBranch { get; set; }

    [JsonPropertyName("push_channel")] public string PushChannel { get; set; }

    [JsonPropertyName("issue_channel")] public string IssueChannel { get; set; }

    [JsonPropertyName("confidential_issue_channel")]
    public string ConfidentialIssueChannel { get; set; }

    [JsonPropertyName("merge_request_channel")]
    public string MergeRequestChannel { get; set; }

    [JsonPropertyName("note_channel")] public string NoteChannel { get; set; }

    [JsonPropertyName("confidential_note_channel")]
    public string ConfidentialNoteChannel { get; set; }

    [JsonPropertyName("tag_push_channel")] public string TagPushChannel { get; set; }

    [JsonPropertyName("pipeline_channel")] public string PipelineChannel { get; set; }

    [JsonPropertyName("wiki_page_channel")] public string WikiPageChannel { get; set; }
}
