// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.LogManagement;
using SquadNET.LogManagement.LogReaders;

namespace SquadNET.Application.Squad.Admin.Queries
{
    public static class ListAdminsQuery
    {
        public class Handler : IRequestHandler<Request, AdminListModel>
        {
            private readonly HttpClient HttpClient;
            private readonly ILogReaderFactory LogReaderFactory;
            private readonly IParser<AdminListModel> Parser;

            public Handler(IParser<AdminListModel> parser, ILogReaderFactory logReaderFactory, HttpClient httpClient)
            {
                Parser = parser;
                LogReaderFactory = logReaderFactory;
                HttpClient = httpClient;
            }

            public async Task<AdminListModel> Handle(Request request, CancellationToken cancellationToken)
            {
                var adminListModel = new AdminListModel
                {
                    Admins = [],
                    Groups = []
                };

                foreach ((string source, LogReaderType type) in request.AdminSources)
                {
                    string data = string.Empty;

                    try
                    {
                        data = type switch
                        {
                            LogReaderType.Ftp => await ((LogReaderFactory.Create(type) as FtpLogReader)!).DownloadFileAsync(source),
                            LogReaderType.Sftp => await ((LogReaderFactory.Create(type) as SftpLogReader)!).DownloadFileAsync(source),
                            _ => await FetchFromOtherSourcesAsync(source, type)
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching {type} admin list from {source}: {ex.Message}");
                        continue;
                    }

                    AdminListModel parsedList = Parser.Parse(data);
                    adminListModel.Admins.AddRange(parsedList.Admins);
                    adminListModel.Groups.AddRange(parsedList.Groups);
                }

                return adminListModel;
            }

            private async Task<string> FetchFromOtherSourcesAsync(string source, LogReaderType type)
            {
                return type switch
                {
                    _ when source.StartsWith("http") => await HttpClient.GetStringAsync(source),
                    _ when File.Exists(source) => await File.ReadAllTextAsync(source),
                    _ => throw new Exception($"Unsupported source type: {source}")
                };
            }
        }

        public class Request : IRequest<AdminListModel>
        {
            public List<(string source, LogReaderType type)> AdminSources { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.AdminSources).NotEmpty();
            }
        }
    }
}