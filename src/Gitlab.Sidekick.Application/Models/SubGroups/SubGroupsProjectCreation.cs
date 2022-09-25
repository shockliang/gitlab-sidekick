using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gitlab.Sidekick.Application.Models.SubGroups;

public partial class SubGroupsProjectCreation : BaseCreation
{
    public IEnumerable<string> GroupNames { get; set; }
    public IEnumerable<SubProjectTemplate> SubProjectTemplates { get; set; }
}

public partial class SubGroupsProjectCreation
{
    public static SubGroupsProjectCreation FromJson(string json) => JsonSerializer.Deserialize<SubGroupsProjectCreation>(json);
}
