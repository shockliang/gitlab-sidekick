using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gitlab.Sidekick.Application.Models.Namespaces;

public partial class Namespace
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    [JsonPropertyName("parent_id")]
    public int? ParentId { get; set; }

    [JsonPropertyName("members_count_with_descendants")]
    public int MembersCountWithDescendants { get; set; }
}

public partial class Namespace
{
    public static Namespace FromJson(string json) => JsonSerializer.Deserialize<Namespace>(json);

    public static IEnumerable<Namespace> FromJsonToCollection(string json) =>
        JsonSerializer.Deserialize<IEnumerable<Namespace>>(json);
}
