using System.Text.Json;
using Microsoft.Extensions.Options;
using NetHub.Application.Constants;
using NetHub.Application.Extensions;
using NetHub.Application.Interfaces;
using NetHub.Application.Models.Mezha;
using NetHub.Application.Options;
using NetHub.Core.DependencyInjection;

namespace NetHub.Infrastructure.Services;

[Inject]
internal class MezhaService : IMezhaService
{
	private readonly HttpClient _client;
	private readonly MezhaOptions _mezhaOptions;

	public MezhaService(IHttpClientFactory httpFactory, IOptions<MezhaOptions> mezhaOptionsAccessor)
	{
		_mezhaOptions = mezhaOptionsAccessor.Value;
		_client = httpFactory.CreateClient();
	}


	public async Task<PostModel[]> GetNews(PostsFilter filter)
	{
		return await GetPosts(MezhaArticleTypes.News.Id, filter);
	}

	private async Task<PostModel[]> GetPosts(int category, PostsFilter filter)
	{
		var uri = new Uri($"{_mezhaOptions.BaseUrl}posts");

		var parameters = new Dictionary<string, string> {{"categories", category.ToString()}};

		if (filter.Page is not null)
			parameters.Add("page", filter.Page.ToString()!);

		if (filter.PerPage is not null)
			parameters.Add("per_page", filter.PerPage.ToString()!);

		if (filter.Search is not null)
			parameters.Add("search", filter.Search);

		var requestUri = uri.AddQueryParameters(parameters);

		var response = await _client.GetAsync(requestUri);

		var message = await response.Content.ReadAsStringAsync();

		var posts = JsonSerializer.Deserialize<PostModel[]>(message)!;

		return posts;
	}
}