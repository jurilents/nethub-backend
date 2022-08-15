﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using NetHub.Application.Features.Public.Articles.Localizations;
using NetHub.Application.Features.Public.Articles.Ratings.Rate;
using NetHub.Application.Tools;
using NetHub.Data.SqlServer.Entities.ArticleEntities;

namespace NetHub.Application.Features.Public.Articles.Ratings.Get;

public class GetArticleRateHandler : AuthorizedHandler<GetArticleRateRequest, RatingModel>
{
	public GetArticleRateHandler(IServiceProvider serviceProvider) : base(serviceProvider)
	{
	}

	protected override async Task<RatingModel> Handle(GetArticleRateRequest request)
	{
		var rating = await Database.Set<ArticleRating>()
			.Include(ar => ar.Article)
			.FirstOrDefaultAsync(ar => ar.ArticleId == request.ArticleId);

		return rating is null ? new RatingModel(RateModel.None) : new RatingModel(rating.Rating.Adapt<RateModel>());
	}
}