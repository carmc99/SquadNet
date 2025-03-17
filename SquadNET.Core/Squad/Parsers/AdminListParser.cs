// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;
using System.Text.RegularExpressions;

namespace SquadNET.Core.Squad.Parsers
{
    internal class AdminListParser : IParser<AdminListModel>
    {
        public AdminListModel Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new AdminListModel { Admins = new List<AdminModel>(), Groups = new List<GroupModel>() };

            input = input.SanitizeInput();
            string[] lines = input.Split('\n');

            var adminListModel = new AdminListModel
            {
                Admins = new List<AdminModel>(),
                Groups = new List<GroupModel>()
            };

            foreach (string line in lines.Select(l => l.Trim()))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                if (TryParseGroup(line, out var groupModel))
                {
                    adminListModel.Groups.Add(groupModel);
                    continue;
                }

                if (TryParseAdmin(line, adminListModel.Groups, out var adminModel))
                {
                    adminListModel.Admins.Add(adminModel);
                }
            }

            return adminListModel;
        }

        private bool TryParseAdmin(string line, List<GroupModel> groups, out AdminModel adminModel)
        {
            adminModel = null;
            Match match = RegexPatternHelper.GetRegex<AdminModel>().Match(line);

            if (!match.Success)
                return false;

            string groupID = match.Groups["groupID"].Value.Trim();
            if (!groups.Any(g => g.GroupID == groupID))
                return false;

            adminModel = new AdminModel
            {
                AdminID = match.Groups["adminID"].Value.Trim(),
                GroupID = groupID
            };

            return true;
        }

        private bool TryParseGroup(string line, out GroupModel groupModel)
        {
            groupModel = null;
            string regexPattern = RegexPatternHelper.GetRegex<GroupModel>().ToString();
            Match match = RegexPatternHelper.GetRegex<GroupModel>().Match(line);

            if (!match.Success)
                return false;

            groupModel = new GroupModel
            {
                GroupID = match.Groups["groupID"].Value.Trim(),
                GroupPerms = match.Groups["groupPerms"].Value.Trim()
            };

            return true;
        }
    }
}