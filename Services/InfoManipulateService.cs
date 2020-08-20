using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NookRemind.Services
{
    public class InfoManipulateService
    {
        private InfoAccessService accessService { get; set; }

        private object RetrieveFishJSON()
        {
            return accessService.GetFishInfo();
        }
        private object RetrieveBugJSON()
        {
            return accessService.GetBugsInfo();
        }
        private string JSONBugToString()
        {
            return RetrieveBugJSON().ToString();
        }
        private string JSONFishToString()
        {
            return RetrieveFishJSON().ToString();
        }
        private string JSONMonthPrettify(string json)
        {
            //TODO: Make props modularized/decoupled
            var nhStartProp = "Northern Hemisphere Season Start";
            var nhEndProp = "Northern Hemisphere Season End";
            var shStartProp = "Southern Hemisphere Season Start";
            var shEndProp = "Southern Hemisphere Season End";

            var speciesArray = JArray.Parse(json);

            foreach(JObject species in speciesArray)
            {
                species[nhStartProp] = MonthsModifier(species, nhStartProp);
                species[nhEndProp] = MonthsModifier(species, nhEndProp);
                species[shStartProp] = MonthsModifier(species, shStartProp);
                species[shEndProp] = MonthsModifier(species, shEndProp);
            }
            return speciesArray.ToString();
        }

        private JObject MonthsModifier(JObject species, string prop)
        {
            var monthVal = species.GetValue(prop).ToString();
            if (!monthVal.Equals("-1") && !monthVal.Contains(","))
            {
                var month = DateTimeFormatInfo.CurrentInfo.GetMonthName(Int32.Parse(monthVal));
                species.Property(prop).Value.Replace(month);
            }
            else if (monthVal.Equals("-1"))
            {
                species.Property(prop).Remove();
            }
            else if (monthVal.Contains(","))
            {
                var months = monthVal.Split(',');
                for (int i = 0; i < months.Length; i++)
                {
                    var stringifiedMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(Int32.Parse(months[i]));
                    months[i] = stringifiedMonth;
                }
                var rejoin = string.Join(", ", months);
                species.Property(prop).Value.Replace(rejoin);
            }
            return species;
        }

        private JArray FindOmniSeasonalSpecies()
        {
            var omniseasonalSpecies = new JArray();
            var prettyBugInfo = JArray.Parse(JSONMonthPrettify(JSONBugToString()));
            var prettyFishInfo = JArray.Parse(JSONMonthPrettify(JSONFishToString()));

            foreach(JObject bug in prettyBugInfo)
            {
                if(bug.GetValue("omniseasonal").ToString().Equals("1"))
                {
                    omniseasonalSpecies.Add(bug);
                }
            }

            foreach(JObject fish in prettyFishInfo)
            {
                if (fish.GetValue("omniseasonal").ToString().Equals("1"))
                {
                    omniseasonalSpecies.Add(fish);
                }
            }
            return omniseasonalSpecies;

        }

        public JArray FindMonthSpecies(int month)
        {
            var monthSpecies = new JArray();
            var prettyBugInfo = JArray.Parse(JSONMonthPrettify(JSONBugToString()));
            var prettyFishInfo = JArray.Parse(JSONMonthPrettify(JSONFishToString()));
            var monthToString = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

            foreach (JObject bug in prettyBugInfo)
            {
                if(bug.GetValue("Northern Hemisphere Season Start").Contains(monthToString))
                {
                    monthSpecies.Add(bug);
                }
                if(bug.GetValue("Southern Hemisphere Season Start").Contains(monthToString))
                {
                    monthSpecies.Add(bug);
                }
            }

            foreach (JObject fish in prettyFishInfo)
            {
                if(fish.GetValue("Northern Hemisphere Season Start").Contains(monthToString))
                {
                    monthSpecies.Add(fish);
                }
                if(fish.GetValue("Southern Hemisphere Season Start").Contains(monthToString))
                {
                    monthSpecies.Add(fish);
                }
            }

            return monthSpecies;
        }

        public string ListPrettify(JArray list)
        {
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public string GetOmniseasonalSpecies()
        {
            var omniseasonalSpeciesArr = FindOmniSeasonalSpecies();

            return ListPrettify(omniseasonalSpeciesArr); 

        }
    }
}
