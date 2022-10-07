using System.Text.Json.Serialization;

namespace Gitlab.Sidekick.Application.Models;

public partial class Slack
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")] public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("active")] public bool Active { get; set; }

    [JsonPropertyName("push_events")] public bool PushEvents { get; set; }

    [JsonPropertyName("issues_events")] public bool IssuesEvents { get; set; }

    [JsonPropertyName("confidential_issues_events")]
    public bool ConfidentialIssuesEvents { get; set; }

    [JsonPropertyName("merge_requests_events")]
    public bool MergeRequestsEvents { get; set; }

    [JsonPropertyName("tag_push_events")] public bool TagPushEvents { get; set; }

    [JsonPropertyName("note_events")] public bool NoteEvents { get; set; }

    [JsonPropertyName("confidential_note_events")]
    public bool ConfidentialNoteEvents { get; set; }

    [JsonPropertyName("pipeline_events")] public bool PipelineEvents { get; set; }

    [JsonPropertyName("wiki_page_events")] public bool WikiPageEvents { get; set; }

    [JsonPropertyName("job_events")] public bool JobEvents { get; set; }

    [JsonPropertyName("properties")] public Properties Properties { get; set; }
}

