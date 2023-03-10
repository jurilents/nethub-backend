using NetHub.Application.Interfaces;
using NetHub.Application.Models.Mezha;
using NetHub.Application.Tools;

namespace NetHub.Application.Features.Public.News;

public class GetMezhaNewsHandler : DbHandler<GetMezhaNewsRequest, PostModel[]>
{
	private readonly IMezhaService _mezhaService;

	public GetMezhaNewsHandler(IServiceProvider serviceProvider, IMezhaService mezhaService) : base(serviceProvider)
	{
		_mezhaService = mezhaService;
	}

	public override async Task<PostModel[]> Handle(GetMezhaNewsRequest request, CancellationToken cancel)
	{
		var result = await _mezhaService.GetNews(request);

		return result;
	}
}