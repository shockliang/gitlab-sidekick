using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gitlab.Sidekick.Application.Models.Groups;

public partial class Group
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("web_url")]
    public Uri WebUrl { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool LFSEnabled { get; set; }

    [JsonPropertyName("avatar_url")]
    public Uri AvatarUrl { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool RequestAccessEnabled { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("parent_id")]
    public int ParentId { get; set; }
}

public partial class Group
{
    public static Group FromJson(string json) => JsonSerializer.Deserialize<Group>(json);

    public static IEnumerable<Group> FromJsonToCollection(string json) =>
        JsonSerializer.Deserialize<IEnumerable<Group>>(json);
}

public static partial class Serialize
{
    public static string ToJson(this Group self) => JsonSerializer.Serialize(self);
}
