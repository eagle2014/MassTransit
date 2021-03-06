namespace MassTransit
{
    using System;
    using Conductor;
    using JobService;
    using JobService.Components.StateMachines;
    using JobService.Configuration;
    using Registration;
    using Saga;


    public static class JobServiceConfigurationExtensions
    {
        /// <summary>
        /// Configures support for job consumers on the service instance, which supports executing long-running jobs without blocking the consumer pipeline.
        /// Job consumers use multiple state machines to track jobs, each of which runs on its own dedicated receive endpoint. Multiple service
        /// instances will use the competing consumer pattern, so a shared saga repository should be configured.
        /// </summary>
        /// <typeparam name="T">The transport receive endpoint configurator type</typeparam>
        /// <param name="configurator">The Conductor service instance</param>
        /// <param name="configure"></param>
        public static IServiceInstanceConfigurator<T> ConfigureJobServiceEndpoints<T>(this IServiceInstanceConfigurator<T> configurator,
            Action<IJobServiceConfigurator> configure = default)
            where T : IReceiveEndpointConfigurator
        {
            var jobServiceConfigurator = new JobServiceConfigurator<T>(configurator);

            configure?.Invoke(jobServiceConfigurator);

            jobServiceConfigurator.ConfigureJobServiceEndpoints();

            return configurator;
        }

        /// <summary>
        /// Configures support for job consumers on the service instance, which supports executing long-running jobs without blocking the consumer pipeline.
        /// Job consumers use multiple state machines to track jobs, each of which runs on its own dedicated receive endpoint. Multiple service
        /// instances will use the competing consumer pattern, so a shared saga repository should be configured.
        /// This method does not configure the state machine endpoints required to use the job service, and should only be used for services where another
        /// service has the job service endpoints configured.
        /// </summary>
        /// <typeparam name="T">The transport receive endpoint configurator type</typeparam>
        /// <param name="configurator">The Conductor service instance</param>
        /// <param name="configure"></param>
        public static IServiceInstanceConfigurator<T> ConfigureJobService<T>(this IServiceInstanceConfigurator<T> configurator,
            Action<IJobServiceConfigurator> configure = default)
            where T : IReceiveEndpointConfigurator
        {
            var jobServiceConfigurator = new JobServiceConfigurator<T>(configurator);

            configure?.Invoke(jobServiceConfigurator);

            return configurator;
        }

        /// <summary>
        /// Configures support for job consumers on the service instance, which supports executing long-running jobs without blocking the consumer pipeline.
        /// Job consumers use multiple state machines to track jobs, each of which runs on its own dedicated receive endpoint. Multiple service
        /// instances will use the competing consumer pattern, so a shared saga repository should be configured.
        /// </summary>
        /// <typeparam name="T">The transport receive endpoint configurator type</typeparam>
        /// <param name="configurator">The Conductor service instance</param>
        /// <param name="options"></param>
        internal static IServiceInstanceConfigurator<T> ConfigureJobServiceEndpoints<T>(this IServiceInstanceConfigurator<T> configurator,
            JobServiceOptions options)
            where T : IReceiveEndpointConfigurator
        {
            var jobServiceConfigurator = new JobServiceConfigurator<T>(configurator);

            jobServiceConfigurator.ApplyJobServiceOptions(options);

            jobServiceConfigurator.ConfigureJobServiceEndpoints();

            return configurator;
        }

        /// <summary>
        /// Configures support for job consumers on the service instance, which supports executing long-running jobs without blocking the consumer pipeline.
        /// Job consumers use multiple state machines to track jobs, each of which runs on its own dedicated receive endpoint. Multiple service
        /// instances will use the competing consumer pattern, so a shared saga repository should be configured.
        /// This method does not configure the state machine endpoints required to use the job service, and should only be used for services where another
        /// service has the job service endpoints configured.
        /// </summary>
        /// <typeparam name="T">The transport receive endpoint configurator type</typeparam>
        /// <param name="configurator">The Conductor service instance</param>
        /// <param name="options"></param>
        internal static IServiceInstanceConfigurator<T> ConfigureJobService<T>(this IServiceInstanceConfigurator<T> configurator,
            JobServiceOptions options)
            where T : IReceiveEndpointConfigurator
        {
            var jobServiceConfigurator = new JobServiceConfigurator<T>(configurator);

            jobServiceConfigurator.ApplyJobServiceOptions(options);

            return configurator;
        }

        /// <summary>
        /// Configure the job server saga repositories to resolve from the container.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="provider">The bus registration context provided during configuration</param>
        /// <returns></returns>
        public static IJobServiceConfigurator ConfigureSagaRepositories(this IJobServiceConfigurator configurator, IConfigurationServiceProvider provider)
        {
            configurator.Repository = provider.GetRequiredService<ISagaRepository<JobTypeSaga>>();
            configurator.JobRepository = provider.GetRequiredService<ISagaRepository<JobSaga>>();
            configurator.JobAttemptRepository = provider.GetRequiredService<ISagaRepository<JobAttemptSaga>>();

            return configurator;
        }
    }
}
