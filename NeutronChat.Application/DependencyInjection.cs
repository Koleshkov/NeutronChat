using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using NeutronChat.Application.Common.Behaviors;
using NeutronChat.Application.Configurations;
using NeutronChat.Domain.Configurations;
using System.Reflection;

namespace NeutronChat.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssemblies(
                new[] { Assembly.GetExecutingAssembly() });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            MailKitConfiguration mailKitConfiguration = new();

            configuration.Bind("MailKitConfiguration", mailKitConfiguration);

            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    //get options from sercets.json
                    Server = mailKitConfiguration.Server,
                    Port = mailKitConfiguration.Port,
                    SenderName = mailKitConfiguration.SenderName,
                    SenderEmail = mailKitConfiguration.SenderEmail,


                    // can be optional with no authentication 
                    Account = mailKitConfiguration.SenderEmail,
                    Password = mailKitConfiguration.Password,
                    Security = true
                });
            });

            return services;
        }
    }
}
