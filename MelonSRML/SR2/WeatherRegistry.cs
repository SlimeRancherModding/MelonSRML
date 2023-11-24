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

        public static void RegisterWeatherState(WeatherStateDefinition weatherState)
        {
            weatherStatesToPatch.AddIfDoesNotContain(weatherState);
            SRLookup.Get<WeatherStateCollection>("All Weather States")?.items?.AddIfDoesNotContain(weatherState);
        }

        public static void RegisterWeatherPattern(WeatherPatternDefinition weatherPattern)
        {
            weatherPatternsToPatch.AddIfDoesNotContain(weatherPattern);
            SRLookup.Get<WeatherPatternCollection>("All Weather Patterns")?.items?.AddIfDoesNotContain(weatherPattern);
        }

        public static WeatherStateDefinition.ActivityIntensityMapping CreateStateActivity(float intensity, AbstractActivity activity)
        {
            return new WeatherStateDefinition.ActivityIntensityMapping()
            {
                Activity = activity,
                Intensity = intensity
            };
        }

        public static WeatherPatternDefinition.Transition CreatePatternTransition(float chancePerHour, WeatherStateDefinition toState, AbstractWeatherCondition[] conditions)
        {
            return new WeatherPatternDefinition.Transition()
            {
                ToState = toState,
                Conditions = conditions,
                ChancePerHour = chancePerHour
            };
        }

        public static void AddPatternToZone(WeatherPatternDefinition weatherPattern, ZoneDefinition zoneDefinition)
        {
            var weatherConfigs = SceneContext.Instance.WeatherRegistry.ZoneConfigList;
            var zoneConfig = weatherConfigs?.ToArray().FirstOrDefault(x => x.Zone == zoneDefinition);

            if (zoneConfig == null)
                return;

            zoneConfig.Patterns.Add(weatherPattern);
        }

        public static void RemovePatternFromZone(WeatherPatternDefinition weatherPattern, ZoneDefinition zoneDefinition)
        {
            var weatherConfigs = SceneContext.Instance.WeatherRegistry.ZoneConfigList;
            var zoneConfig = weatherConfigs?.ToArray().FirstOrDefault(x => x.Zone == zoneDefinition);

            if (zoneConfig == null)
                return;

            zoneConfig.Patterns.Remove(weatherPattern);
        }

        public static WeatherTypeMetadata CreateWeatherMetadata(string metadataName, Sprite weatherIcon, PediaEntry weatherPediaEntry)
        {
            WeatherTypeMetadata weatherMetadata = ScriptableObject.CreateInstance<WeatherTypeMetadata>();
            weatherMetadata.hideFlags |= HideFlags.HideAndDontSave;
            weatherMetadata.name = metadataName;
            weatherMetadata.AnalyticsName = metadataName.Replace(" ", "").Replace("Metadata", "");
            weatherMetadata.Icon = weatherIcon;
            weatherMetadata.PediaEntry = weatherPediaEntry;
            return weatherMetadata;
        }

        public static WeatherStateDefinition CreateWeatherState(string stateName, Il2CppSystem.Collections.Generic.List<WeatherStateDefinition.ActivityIntensityMapping> stateActivities, int stateTier = 0, float minDurationHours = 3)
        {
            WeatherStateDefinition weatherStateDefinition = ScriptableObject.CreateInstance<WeatherStateDefinition>();
            weatherStateDefinition.hideFlags |= HideFlags.HideAndDontSave;
            weatherStateDefinition.name = stateName;
            weatherStateDefinition.Guid = "WeatherStateDefinition." + stateName.Replace(" ", "");
            weatherStateDefinition.StateName = stateName.Replace("State", "");

            weatherStateDefinition.MapTier = stateTier;
            weatherStateDefinition.Activities = stateActivities;
            weatherStateDefinition.MinDurationHours = minDurationHours;

            RegisterWeatherState(weatherStateDefinition);
            return weatherStateDefinition;
        }

        public static WeatherPatternDefinition CreateWeatherPattern(string patternName, WeatherTypeMetadata weatherMetadata, Il2CppSystem.Collections.Generic.List<WeatherPatternDefinition.TransitionList> runningTransitions, Il2CppSystem.Collections.Generic.List<WeatherPatternDefinition.Transition> startingTransitions, float cooldownHours = 16)
        {
            WeatherPatternDefinition weatherPatternDefinition = ScriptableObject.CreateInstance<WeatherPatternDefinition>();
            weatherPatternDefinition.hideFlags |= HideFlags.HideAndDontSave;
            weatherPatternDefinition.name = patternName;
            weatherPatternDefinition.Guid = "WeatherPatternDefinition." + patternName.Replace(" ", "");
            weatherPatternDefinition.Metadata = weatherMetadata;

            weatherPatternDefinition.CooldownHours = cooldownHours;
            weatherPatternDefinition.RunningTransitions = runningTransitions;
            weatherPatternDefinition.StartingTransitions = startingTransitions;

            RegisterWeatherPattern(weatherPatternDefinition);
            return weatherPatternDefinition;
        }
    }
}
