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
            private readonly IHttpClientFactory HttpClientFactory;
            private readonly ILogReaderFactory LogReaderFactory;
            private readonly IParser<AdminListModel> Parser;

            public Handler(IParser<AdminListModel> parser,
                ILogReaderFactory logReaderFactory,
                IHttpClientFactory httpClientFactory)
            {
                Parser = parser;
                LogReaderFactory = logReaderFactory;
                HttpClientFactory = httpClientFactory;
            }

            public async Task<AdminListModel> Handle(Request request, CancellationToken cancellationToken)
            {
                AdminListModel adminListModel = new()
                {
                    Admins = [],
                    Groups = []
                };

                foreach ((string source, LogReaderType type) in request.AdminSources)
                {
                    string data = string.Empty;

                    data = type switch
                    {
                        LogReaderType.Ftp => await ((LogReaderFactory.Create(type) as FtpLogReader)!)
                            .DownloadFileAsync(source),
                        LogReaderType.Sftp => await ((LogReaderFactory.Create(type) as SftpLogReader)!)
                            .DownloadFileAsync(source),
                        LogReaderType.Tail => await ((LogReaderFactory.Create(type) as TailLogReader)!)
                            .ReadFileAsync(source),
                        _ => await FetchFromOtherSourcesAsync(source)
                    };

                    AdminListModel parsedList = Parser.Parse(data);
                    adminListModel.Admins.AddRange(parsedList.Admins);
                    adminListModel.Groups.AddRange(parsedList.Groups);
                }

                return adminListModel;
            }

            private async Task<string> FetchFromOtherSourcesAsync(string source)
            {
                if (source.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    using HttpClient httpClient = HttpClientFactory.CreateClient();
                    return await httpClient.GetStringAsync(source);
                }

                throw new Exception($"Unsupported source type: {source}");
            }
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