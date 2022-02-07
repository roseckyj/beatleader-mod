﻿using BeatLeader.Core.Managers.ReplayEnhancer;
using BeatLeader.Installers;
using HarmonyLib;
using JetBrains.Annotations;
using static PlayerSaveData;

namespace BeatLeader
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Init))]
    class LevelDataEnhancerPatch
    {
        static void Postfix(StandardLevelScenesTransitionSetupDataSO __instance, string gameMode, IDifficultyBeatmap difficultyBeatmap, IPreviewBeatmapLevel previewBeatmapLevel, OverrideEnvironmentSettings overrideEnvironmentSettings,
            GameplayModifiers gameplayModifiers, ColorScheme overrideColorScheme, PlayerSpecificSettings playerSpecificSettings, ref PracticeSettings practiceSettings, string backButtonText, bool useTestNoteCutSoundEffects)
        {
            EnvironmentInfoSO environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();
            if (overrideEnvironmentSettings is { overrideEnvironments: true })
            {
                environmentInfoSO = overrideEnvironmentSettings.GetOverrideEnvironmentInfoForType(environmentInfoSO.environmentType);
            }
            MapEnhancer.difficultyBeatmap = difficultyBeatmap;
            MapEnhancer.previewBeatmapLevel = previewBeatmapLevel;
            MapEnhancer.gameplayModifiers = gameplayModifiers;
            MapEnhancer.playerSpecificSettings = playerSpecificSettings;
            MapEnhancer.practiceSettings = practiceSettings;
            MapEnhancer.useTestNoteCutSoundEffects = useTestNoteCutSoundEffects;
            MapEnhancer.environmentInfo = environmentInfoSO;
            MapEnhancer.colorScheme = overrideColorScheme;
        }
    }
}
