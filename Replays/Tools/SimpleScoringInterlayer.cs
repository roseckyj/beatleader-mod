﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IPA.Utilities;
using BeatLeader.Replays.Interfaces;
using BeatLeader.Replays.Scoring;
using Zenject;

namespace BeatLeader.Replays.Tools
{
    public class SimpleScoringInterlayer : ISimpleScoringInterlayer
    {
        public class Pool : MemoryPool<SimpleScoringInterlayer>
        {
            protected override void Reinitialize(SimpleScoringInterlayer scoringInterlayer)
            {
                scoringInterlayer.Reinitialize();
            }
        }

        protected ScoringElement _scoringElement;
        protected CutScoreBuffer _cutScoreBuffer;
        protected bool _isFinished;

        public virtual ScoringElement scoringElement => _scoringElement;
        public virtual IReadonlyCutScoreBuffer cutScoreBuffer => _cutScoreBuffer;
        public bool isFinished => _isFinished;

        public virtual void Init(ScoringElement scoringElement)
        {
            SimpleCutScoringElement scoringElementWithInterlayer;
            if ((scoringElementWithInterlayer = (scoringElement as SimpleCutScoringElement)) != null)
            {
                ConvertScoringElement(scoringElementWithInterlayer, scoringElementWithInterlayer.cutScoreBuffer);
                _isFinished = true;
            }
        }
        public virtual void ConvertScoringElement(ScoringElement element, IReadonlyCutScoreBuffer buffer)
        {
            SimpleCutScoringElement scoringElementWithInterlayer;
            if ((scoringElementWithInterlayer = (element as SimpleCutScoringElement)) != null)
            {
                GoodCutScoringElement goodCutScoringElement = new GoodCutScoringElement();
                ConvertBuffer(scoringElementWithInterlayer.cutScoreBuffer);
                goodCutScoringElement.SetField("_multiplierEventType", scoringElementWithInterlayer.multiplierEventType);
                goodCutScoringElement.SetField("_wouldBeCorrectCutBestPossibleMultiplierEventType", scoringElementWithInterlayer.wouldBeCorrectCutBestPossibleMultiplierEventType);
                goodCutScoringElement.SetField("_cutScoreBuffer", _cutScoreBuffer);
                goodCutScoringElement.GetType().GetProperty("noteData").SetValue(goodCutScoringElement, scoringElementWithInterlayer.noteData);
                goodCutScoringElement.GetType().GetProperty("isFinished").SetValue(goodCutScoringElement, true);
                _scoringElement = goodCutScoringElement;
            }
        }
        public virtual void ConvertBuffer(IReadonlyCutScoreBuffer buffer)
        {
            CutScoreBuffer cutScoreBuffer = new CutScoreBuffer();
            cutScoreBuffer.SetField("_noteCutInfo", buffer.noteCutInfo); 
            cutScoreBuffer.SetField("_noteScoreDefinition", buffer.noteScoreDefinition); 
            cutScoreBuffer.SetField("_afterCutScore", buffer.afterCutScore); 
            cutScoreBuffer.SetField("_beforeCutScore", buffer.beforeCutScore); 
            cutScoreBuffer.SetField("_centerDistanceCutScore", buffer.centerDistanceCutScore); 
            cutScoreBuffer.SetField("_isFinished", buffer.isFinished); 
            _cutScoreBuffer = cutScoreBuffer;
        }
        protected void Reinitialize()
        {
            _isFinished = false;
            _scoringElement = null;
            _cutScoreBuffer = null;
        }
    }
}
