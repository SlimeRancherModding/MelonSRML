using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppMonomiPark.SlimeRancher.Weather;
using Il2CppMonomiPark.SlimeRancher.Weather.Activity;
using Il2CppMonomiPark.SlimeRancher.Weather.Conditions;
using Il2CppMonomiPark.SlimeRancher.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelonSRML.SR2
{
    public static class WeatherRegistry
    {
        internal static HashSet<WeatherStateDefinition> weatherStatesToPatch = new HashSet<WeatherStateDefinition>();
        internal static HashSet<WeatherPatternDefinition> weatherPatternsToPatch = new HashSet<WeatherPatternDefinition>();

        public static void RegisterWeatherState(WeatherStateDefinition weatherStateDefinition)
        {
            weatherStatesToPatch.AddIfDoesNotContain(weatherStateDefinition);
            SRLookup.Get<WeatherStateCollection>("All Weather States")?.items?.AddIfDoesNotContain(weatherStateDefinition);
        }

        public static void RegisterWeatherPattern(WeatherPatternDefinition weatherPatternDefinition)
        {
            weatherPatternsToPatch.AddIfDoesNotContain(weatherPatternDefinition);
            SRLookup.Get<WeatherPatternCollection>("All Weather Patterns")?.items?.AddIfDoesNotContain(weatherPatternDefinition);
        }

        public static WeatherStateDefinition.ActivityIntensityMapping CreateStateActivity(AbstractActivity activity, float intensity)
        {
            return new WeatherStateDefinition.ActivityIntensityMapping()
            {
                Activity = activity,
                Intensity = intensity
            };
        }

        public static WeatherPatternDefinition.Transition CreatePatternTransition(WeatherStateDefinition toState, float chancePerHour, AbstractWeatherCondition[] conditions)
        {
            return new WeatherPatternDefinition.Transition()
            {
                ToState = toState,
                Conditions = conditions,
                ChancePerHour = chancePerHour
            };
        }

        public static void AddPatternToZone(ZoneDefinition zoneDefinition, WeatherPatternDefinition weatherPatternDefinition)
        {
            var weatherConfigs = SceneContext.Instance.WeatherRegistry.ZoneConfigList;
            var zoneConfig = weatherConfigs?.ToArray().FirstOrDefault(x => x.Zone == zoneDefinition);

            if (zoneConfig == null)
                return;

            zoneConfig.Patterns.Add(weatherPatternDefinition);
        }

        public static void RemovePatternFromZone(ZoneDefinition zoneDefinition, WeatherPatternDefinition weatherPatternDefinition)
        {
            var weatherConfigs = SceneContext.Instance.WeatherRegistry.ZoneConfigList;
            var zoneConfig = weatherConfigs?.ToArray().FirstOrDefault(x => x.Zone == zoneDefinition);

            if (zoneConfig == null)
                return;

            zoneConfig.Patterns.Remove(weatherPatternDefinition);
        }

        public static WeatherTypeMetadata CreateWeatherMetadata(Sprite icon, string name, PediaEntry pediaEntry)
        {
            WeatherTypeMetadata weatherMetadata = ScriptableObject.CreateInstance<WeatherTypeMetadata>();
            weatherMetadata.hideFlags |= HideFlags.HideAndDontSave;
            weatherMetadata.name = name;
            
            weatherMetadata.Icon = icon;
            weatherMetadata.PediaEntry = pediaEntry;
            weatherMetadata.AnalyticsName = name.Replace(" ", "").Replace("Metadata", "");
            return weatherMetadata;
        }

        public static WeatherStateDefinition CreateWeatherState(string name, Il2CppSystem.Collections.Generic.List<WeatherStateDefinition.ActivityIntensityMapping> activities, int mapTier = 0, float minDurationHours = 3)
        {
            WeatherStateDefinition weatherStateDefinition = ScriptableObject.CreateInstance<WeatherStateDefinition>();
            weatherStateDefinition.hideFlags |= HideFlags.HideAndDontSave;
            weatherStateDefinition.name = name;

            weatherStateDefinition.Guid = "WeatherStateDefinition." + name.Replace(" ", "");
            weatherStateDefinition.StateName = name.Replace("State", "");

            weatherStateDefinition.MapTier = mapTier;
            weatherStateDefinition.Activities = activities;
            weatherStateDefinition.MinDurationHours = minDurationHours;

            RegisterWeatherState(weatherStateDefinition);
            return weatherStateDefinition;
        }

        public static WeatherPatternDefinition CreateWeatherPattern(WeatherTypeMetadata metadata, string name, Il2CppSystem.Collections.Generic.List<WeatherPatternDefinition.TransitionList> runningTransitions, Il2CppSystem.Collections.Generic.List<WeatherPatternDefinition.Transition> startingTransitions, float cooldownHours = 16)
        {
            WeatherPatternDefinition weatherPatternDefinition = ScriptableObject.CreateInstance<WeatherPatternDefinition>();
            weatherPatternDefinition.hideFlags |= HideFlags.HideAndDontSave;
            weatherPatternDefinition.name = name;

            weatherPatternDefinition.Guid = "WeatherPatternDefinition." + name.Replace(" ", "");
            weatherPatternDefinition.Metadata = metadata;

            weatherPatternDefinition.CooldownHours = cooldownHours;
            weatherPatternDefinition.RunningTransitions = runningTransitions;
            weatherPatternDefinition.StartingTransitions = startingTransitions;

            RegisterWeatherPattern(weatherPatternDefinition);
            return weatherPatternDefinition;
        }
    }
}
