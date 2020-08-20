using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using NookRemind.Services;

namespace NookRemind.Modules
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {

        private InfoManipulateService manipulateService { get; set; }
        /// <summary>
        /// Command nook.events will return events for the current year on a Northern/Southern
        /// Hemisphere island.
        /// </summary>
        /// <returns>List of seasonal recurring/annual events and dates of said events</returns>
        [Command("events")]
        public async Task EventsCommand()
        {

        }
        /// <summary>
        /// Command nook.omniseasonal will retrn the fish and bugs that are present all
        /// year round for both the Northern Hemisphere and Southern Hemisphere. Command will
        /// also list location of species, value, and time of day they can be captured.
        /// </summary>
        /// <returns>List of fish and bugs present all season</returns>
        [Command("omniseasonal")]
        public async Task OmniSeasonalCommand()
        {
            var list = manipulateService.GetOmniseasonalSpecies();
            await ReplyAsync(list);
        }
        /// <summary>
        /// Command nook.thismonth will return the fish and bugs that will be leaving
        /// this month for both the Northern Hemisphere and Southern Hemisphere. Command
        /// will also list location of species, value, and time of day they can be captured.
        /// </summary>
        /// <returns>List of fish and bugs leaving on the current month</returns>
        [Command("thismonth")]
        public async Task ThisMonthCommand()
        {

        }
        /// <summary>
        /// Command nook.nextmonth will return the fish and bugs coming to the island next month
        /// for the Northern and Southern Hemispheres. Command will also list location of species,
        /// value, and time of day they can be captured.
        /// </summary>
        /// <returns>List of fish and bugs coming to island next month</returns>
        [Command("nextmonth")]
        public async Task NextMonthCommand()
        {

        }
    }
}
