// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    public sealed class AdminListModel
    {
        public List<AdminModel> Admins { get; set; }
        public List<GroupModel> Groups { get; set; }
    }

    [RegexPattern(@"^Admin=(?<adminID>\d+|[a-f0-9]{32}):(?<groupID>[^\s:]+)$")]
    public class AdminModel
    {
        public string AdminID { get; set; }
        public string GroupID { get; set; }
    }

    [RegexPattern(@"^Group=(?<groupID>[^\s:]+):(?<groupPerms>.+)$")]
    public class GroupModel
    {
        public string GroupID { get; set; }
        public string GroupPerms { get; set; }
    }
}