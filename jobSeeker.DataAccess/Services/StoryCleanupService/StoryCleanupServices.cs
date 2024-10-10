using jobSeeker.DataAccess.Services.IStoryService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.StoryCleanupService
{
    public class StoryCleanupServices: BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public StoryCleanupServices(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var storyService = scope.ServiceProvider.GetRequiredService<IStoryServices>();
                    await storyService.MarkInactiveStoriesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
