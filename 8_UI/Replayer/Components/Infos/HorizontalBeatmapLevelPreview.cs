﻿using BeatSaberMarkupLanguage.Attributes;
using UnityEngine.UI;
using System.Threading;
using BeatLeader.Models;

namespace BeatLeader.Components {
    internal class HorizontalBeatmapLevelPreview : EditableElement {
        #region Components

        [UIComponent("song-preview-image")] private readonly Image _songPreviewImage;

        #endregion

        #region Name, Author

        [UIValue("song-name")]
        public string SongName {
            get => _songName;
            private set {
                _songName = value;
                NotifyPropertyChanged(nameof(SongName));
            }
        }
        [UIValue("song-author")]
        public string SongAuthor {
            get => _songAuthor;
            private set {
                _songAuthor = value;
                NotifyPropertyChanged(nameof(SongAuthor));
            }
        }

        #endregion

        #region Editable

        public override string Name => "Beatmap Preview";
        public override LayoutMapData DefaultLayoutMap { get; protected set; } = new() {
            enabled = true,
            position = new(0, 0.8f)
        };

        #endregion

        #region Setup

        private string _songName;
        private string _songAuthor;

        public void SetBeatmapLevel(IPreviewBeatmapLevel level) {
            SongName = level.songName;
            SongAuthor = level.levelAuthorName;
            LoadAndAssignImage(level);
        }
        private async void LoadAndAssignImage(IPreviewBeatmapLevel level) {
            var token = new CancellationTokenSource().Token;
            _songPreviewImage.sprite = await level.GetCoverImageAsync(token);
        }

        #endregion
    }
}
