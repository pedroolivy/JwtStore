﻿using JwtStore.Core.Contexts.AccountContext.UserCases.Create;
using JwtStore.Core.Contexts.AccountContext.UserCases.Create.Contracts;
using JwtStore.Infra.Contexts.AccountContext.UseCases.Create;
using MediatR;

namespace JwtStore.Api.Extensions
{
    public static class AccountContextExtension
    {
        public static void AddAccountContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRepository, Repository>();
            builder.Services.AddTransient<IService, Service>();
        }

        public static void MapAccountEndpoints(this WebApplication app)
        {
            app.MapPost("api/v1/users", async (
                Request request,
                IRequestHandler<Request, Response> handler) =>
            {
                var result = await handler.Handle(request, new CancellationToken());
                return result.IsSuccess
                    ? Results.Created($"api/v1/users/{result.Data?.Id}", result)
                    : Results.Json(result, statusCode: result.Status);
            });
        }
    }
}
