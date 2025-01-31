﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpellWork.Database;
using SpellWork.DBC.Structures;
using SpellWork.Extensions;

namespace SpellWork.Spell
{
    class SpellInfo
    {
        private readonly RichTextBox _rtb;
        private SpellInfoHelper _spell;

        private const string _line = "=================================================";

        public SpellInfo(RichTextBox rtb, SpellInfoHelper spell)
        {
            _rtb = rtb;
            _spell = spell;

            ProcInfo.SpellProc = spell;

            ViewSpellInfo();
        }

        private void ViewSpellInfo()
        {
            _rtb.Clear();
            _rtb.SetBold();
            _rtb.AppendFormatLine("ID - {0} {1}{2}", _spell.ID, _spell.SpellNameRank, _spell.ScalingText);
            _rtb.SetDefaultStyle();

            _rtb.AppendFormatLine(_line);

            _rtb.AppendLine(_spell.Description);
            _rtb.AppendFormatLineIfNotNull("ToolTip: {0}", _spell.ToolTip);
            _rtb.AppendFormatLineIfNotNull("Modal Next Spell: {0}", _spell.ModalNextSpell);
            if (_spell.Description != string.Empty && _spell.ToolTip != string.Empty && _spell.ModalNextSpell != 0)
                _rtb.AppendFormatLine(_line);

            _rtb.AppendFormatLine("Category = {0}, SpellIconID = {1}, activeIconID = {2}, SpellVisual = ({3},{4})",
                _spell.Category, _spell.SpellIconID, _spell.ActiveIconID, _spell.SpellVisual[0], _spell.SpellVisual[1]);

            _rtb.AppendFormatLine("Family {0} ({1}), flag [0] 0x{2:X8} [1] 0x{3:X8} [2] 0x{4:X8}",
                (SpellFamilyNames)_spell.SpellFamilyName, _spell.SpellFamilyName,
                _spell.SpellFamilyFlags[0], _spell.SpellFamilyFlags[1], _spell.SpellFamilyFlags[2]);

            _rtb.AppendLine();

            _rtb.AppendFormatLine("SpellSchoolMask = {0} ({1})", _spell.SchoolMask, _spell.School);
            _rtb.AppendFormatLine("DamageClass = {0} ({1})", _spell.DmgClass, (SpellDmgClass)_spell.DmgClass);
            _rtb.AppendFormatLine("PreventionType = {0} ({1})", _spell.PreventionType, (SpellPreventionType)_spell.PreventionType);

            if (_spell.Attributes != 0 || _spell.AttributesEx != 0 || _spell.AttributesEx2 != 0 || _spell.AttributesEx3 != 0 || _spell.AttributesEx4 != 0
                || _spell.AttributesEx5 != 0 || _spell.AttributesEx6 != 0 || _spell.AttributesEx7 != 0 || _spell.AttributesEx8 != 0)
                _rtb.AppendLine(_line);

            if (_spell.Attributes != 0)
                _rtb.AppendFormatLine("Attributes: 0x{0:X8} ({1})", _spell.Attributes, (SpellAtribute)_spell.Attributes);
            if (_spell.AttributesEx != 0)
                _rtb.AppendFormatLine("AttributesEx1: 0x{0:X8} ({1})", _spell.AttributesEx, (SpellAtributeEx)_spell.AttributesEx);
            if (_spell.AttributesEx2 != 0)
                _rtb.AppendFormatLine("AttributesEx2: 0x{0:X8} ({1})", _spell.AttributesEx2, (SpellAtributeEx2)_spell.AttributesEx2);
            if (_spell.AttributesEx3 != 0)
                _rtb.AppendFormatLine("AttributesEx3: 0x{0:X8} ({1})", _spell.AttributesEx3, (SpellAtributeEx3)_spell.AttributesEx3);
            if (_spell.AttributesEx4 != 0)
                _rtb.AppendFormatLine("AttributesEx4: 0x{0:X8} ({1})", _spell.AttributesEx4, (SpellAtributeEx4)_spell.AttributesEx4);
            if (_spell.AttributesEx5 != 0)
                _rtb.AppendFormatLine("AttributesEx5: 0x{0:X8} ({1})", _spell.AttributesEx5, (SpellAtributeEx5)_spell.AttributesEx5);
            if (_spell.AttributesEx6 != 0)
                _rtb.AppendFormatLine("AttributesEx6: 0x{0:X8} ({1})", _spell.AttributesEx6, (SpellAtributeEx6)_spell.AttributesEx6);
            if (_spell.AttributesEx7 != 0)
                _rtb.AppendFormatLine("AttributesEx7: 0x{0:X8} ({1})", _spell.AttributesEx7, (SpellAtributeEx7)_spell.AttributesEx7);
            if (_spell.AttributesEx8 != 0)
                _rtb.AppendFormatLine("AttributesEx8: 0x{0:X8} ({1})", _spell.AttributesEx8, (SpellAtributeEx8)_spell.AttributesEx8);
            if (_spell.AttributesEx9 != 0)
                _rtb.AppendFormatLine("AttributesEx9: 0x{0:X8} ({1})", _spell.AttributesEx9, (SpellAtributeEx9)_spell.AttributesEx9);
            if (_spell.AttributesEx10 != 0)
                _rtb.AppendFormatLine("AttributesEx10: 0x{0:X8} ({1})", _spell.AttributesEx10, (SpellAtributeEx10)_spell.AttributesEx10);

            _rtb.AppendLine(_line);

            if (_spell.Targets != 0)
                _rtb.AppendFormatLine("Targets Mask = 0x{0:X8} ({1})", _spell.Targets, (SpellCastTargetFlags)_spell.Targets);

            if (_spell.TargetCreatureType != 0)
                _rtb.AppendFormatLine("Creature Type Mask = 0x{0:X8} ({1})",
                    _spell.TargetCreatureType, (CreatureTypeMask)_spell.TargetCreatureType);

            if (_spell.Stances != 0)
                _rtb.AppendFormatLine("Stances: {0}", (ShapeshiftFormMask)_spell.Stances);

            if (_spell.StancesNot != 0)
                _rtb.AppendFormatLine("Stances Not: {0}", (ShapeshiftFormMask)_spell.StancesNot);

            AppendSkillLine();

            // reagents
            {
                var printedHeader = false;
                for (var i = 0; i < _spell.Reagent.Length; ++i)
                {
                    if (_spell.Reagent[i] == 0)
                        continue;

                    if (!printedHeader)
                    {
                        _rtb.AppendLine();
                        _rtb.Append("Reagents:");
                        printedHeader = true;
                    }

                    _rtb.AppendFormat("  {0} x{1}", _spell.Reagent[i], _spell.ReagentCount[i]);
                }

                if (printedHeader)
                    _rtb.AppendLine();
            }

            _rtb.AppendFormatLine($"SpellLevel = {_spell.SpellLevel}, BaseLevel = {_spell.BaseLevel}, MaxLevel =  {_spell.MaxLevel}, MaxTargetLevel = {_spell.MaxTargetLevel}, ConeAngle = {_spell.ConeAngle}");

            if (_spell.EquippedItemClass != 0)
            {
                _rtb.AppendFormatLine("EquippedItemClass = {0} ({1})", _spell.EquippedItemClass, (ItemClass)_spell.EquippedItemClass);

                if (_spell.EquippedItemSubClassMask != 0)
                {
                    switch ((ItemClass)_spell.EquippedItemClass)
                    {
                        case ItemClass.WEAPON:
                            _rtb.AppendFormatLine("    SubClass mask 0x{0:X8} ({1})",
                                _spell.EquippedItemSubClassMask, (ItemSubClassWeaponMask)_spell.EquippedItemSubClassMask);
                            break;
                        case ItemClass.ARMOR:
                            _rtb.AppendFormatLine("    SubClass mask 0x{0:X8} ({1})",
                                _spell.EquippedItemSubClassMask, (ItemSubClassArmorMask)_spell.EquippedItemSubClassMask);
                            break;
                        case ItemClass.MISC:
                            _rtb.AppendFormatLine("    SubClass mask 0x{0:X8} ({1})",
                                _spell.EquippedItemSubClassMask, (ItemSubClassMiscMask)_spell.EquippedItemSubClassMask);
                            break;
                    }
                }

                if (_spell.EquippedItemInventoryTypeMask != 0)
                    _rtb.AppendFormatLine("    InventoryType mask = 0x{0:X8} ({1})",
                        _spell.EquippedItemInventoryTypeMask, (InventoryTypeMask)_spell.EquippedItemInventoryTypeMask);
            }

            _rtb.AppendLine();
            _rtb.AppendFormatLine("Category = {0}", _spell.Category);
            _rtb.AppendFormatLine("DispelType = {0} ({1})", _spell.Dispel, (DispelType)_spell.Dispel);
            _rtb.AppendFormatLine("Mechanic = {0} ({1})", _spell.Mechanic, (Mechanics)_spell.Mechanic);

            _rtb.AppendLine(_spell.Range);

            _rtb.AppendFormatLineIfNotNull("Speed {0:F}", _spell.Speed);
            _rtb.AppendFormatLineIfNotNull("Stackable up to {0}", _spell.StackAmount);

            _rtb.AppendLine(_spell.CastTime);

            if (_spell.RecoveryTime != 0 || _spell.CategoryRecoveryTime != 0 || _spell.StartRecoveryCategory != 0)
            {
                _rtb.AppendFormatLine("RecoveryTime: {0} ms, CategoryRecoveryTime: {1} ms", _spell.RecoveryTime, _spell.CategoryRecoveryTime);
                _rtb.AppendFormatLine("StartRecoveryCategory = {0}, StartRecoveryTime = {1:F} ms", _spell.StartRecoveryCategory, _spell.StartRecoveryTime);
            }

            _rtb.AppendLine(_spell.Duration);

            if (_spell.ManaCost != 0 || _spell.ManaCostPercentage != 0 || _spell.PowerType != 0 ||
                _spell.ManaCostPerlevel != 0 || _spell.ManaPerSecond != 0)
            {
                _rtb.AppendFormat("Power {0}, Cost {1}",
                    (Powers)_spell.PowerType, _spell.ManaCost == 0 ? _spell.ManaCostPercentage + " %" : _spell.ManaCost.ToString());
                _rtb.AppendFormatIfNotNull(" + lvl * {0}", _spell.ManaCostPerlevel);
                _rtb.AppendFormatIfNotNull(" + {0} Per Second", _spell.ManaPerSecond);
                _rtb.AppendLine();
            }

            _rtb.AppendFormatLine("Interrupt Flags: 0x{0:X8} ({1})", _spell.InterruptFlags, (SpellInterruptFlags)_spell.InterruptFlags);
            _rtb.AppendFormatLine("AuraInterrupt Flags: 0x{0:X8} ({1}), 0x{2:X8} ({3})", _spell.AuraInterruptFlags[0], (SpellAuraInterruptFlags)_spell.AuraInterruptFlags[0], _spell.AuraInterruptFlags[1], (SpellAuraInterruptFlags2)_spell.AuraInterruptFlags[1]);
            _rtb.AppendFormatLine("ChannelInterrupt Flags: 0x{0:X8} ({1}), 0x{2:X8} ({3})", _spell.ChannelInterruptFlags[0], (SpellAuraInterruptFlags)_spell.ChannelInterruptFlags[0], _spell.ChannelInterruptFlags[1], (SpellAuraInterruptFlags2)_spell.ChannelInterruptFlags[1]);

            if (_spell.CasterAuraState != 0)
                _rtb.AppendFormatLine("CasterAuraState = {0} ({1})", _spell.CasterAuraState, (AuraState)_spell.CasterAuraState);

            if (_spell.TargetAuraState != 0)
                _rtb.AppendFormatLine("TargetAuraState = {0} ({1})", _spell.TargetAuraState, (AuraState)_spell.TargetAuraState);

            if (_spell.CasterAuraStateNot != 0)
                _rtb.AppendFormatLine("CasterAuraStateNot = {0} ({1})", _spell.CasterAuraStateNot, (AuraState)_spell.CasterAuraStateNot);

            if (_spell.TargetAuraStateNot != 0)
                _rtb.AppendFormatLine("TargetAuraStateNot = {0} ({1})", _spell.TargetAuraStateNot, (AuraState)_spell.TargetAuraStateNot);

            if (_spell.MaxAffectedTargets != 0)
                _rtb.AppendFormatLine("MaxAffectedTargets = {0}", _spell.MaxAffectedTargets);

            AppendSpellAura();

            AppendAreaInfo();

            _rtb.AppendFormatLineIfNotNull("Requires Spell Focus {0}", _spell.RequiresSpellFocus);

            if (_spell.ProcFlags != 0)
            {
                _rtb.SetBold();
                _rtb.AppendFormatLine("Proc flag 0x{0:X8}, chance = {1}, charges - {2}",
                _spell.ProcFlags, _spell.ProcChance, _spell.ProcCharges);
                _rtb.SetDefaultStyle();
                _rtb.AppendFormatLine(_line);
                _rtb.AppendText(_spell.ProcInfo);
            }
            else
            {
                _rtb.AppendFormatLine("Chance = {0}, charges - {1}", _spell.ProcChance, _spell.ProcCharges);
            }

            AppendSpellEffectInfo();
            AppendItemInfo();
            AppendDifficultyInfo();

            AppendSpellVisualInfo();
        }

        private void AppendSpellVisualInfo()
        {
            SpellVisualEntry visualData;
            if (!DBC.DBC.SpellVisual.TryGetValue(_spell.SpellVisual[0], out visualData))
                return;

            SpellMissileEntry missileEntry;
            SpellMissileMotionEntry missileMotionEntry;
            var hasMissileEntry = DBC.DBC.SpellMissile.TryGetValue(visualData.MissileModel, out missileEntry);
            var hasMissileMotion = DBC.DBC.SpellMissileMotion.TryGetValue(visualData.MissileMotionId, out missileMotionEntry);

            if (!hasMissileEntry && !hasMissileMotion)
                return;

            _rtb.AppendLine(_line);
            _rtb.SetBold();
            _rtb.AppendLine("Missile data");
            _rtb.SetDefaultStyle();

            // Missile Model Data.
            if (hasMissileEntry)
            {
                _rtb.AppendFormatLine("Missile Model ID: {0}", visualData.MissileModel);
                _rtb.AppendFormatLine("Missile attachment: {0}", visualData.MissileAttachment);
                _rtb.AppendFormatLine("Missile cast offset: X:{0} Y:{1} Z:{2}", visualData.MissileCastOffsetX, visualData.MissileCastOffsetY, visualData.MissileCastOffsetZ);
                _rtb.AppendFormatLine("Missile impact offset: X:{0} Y:{1} Z:{2}", visualData.MissileImpactOffsetX, visualData.MissileImpactOffsetY, visualData.MissileImpactOffsetZ);
                _rtb.AppendFormatLine("MissileEntry ID: {0}", missileEntry.Id);
                _rtb.AppendFormatLine("Collision Radius: {0}", missileEntry.CollisionRadius);
                _rtb.AppendFormatLine("Default Pitch: {0} - {1}", missileEntry.DefaultPitchMin, missileEntry.DefaultPitchMax);
                _rtb.AppendFormatLine("Random Pitch: {0} - {1}", missileEntry.RandomizePitchMax, missileEntry.RandomizePitchMax);
                _rtb.AppendFormatLine("Default Speed: {0} - {1}", missileEntry.DefaultSpeedMin, missileEntry.DefaultSpeedMax);
                _rtb.AppendFormatLine("Randomize Speed: {0} - {1}", missileEntry.RandomizeSpeedMin, missileEntry.RandomizeSpeedMax);
                _rtb.AppendFormatLine("Gravity: {0}", missileEntry.Gravity);
                _rtb.AppendFormatLine("Maximum duration:", missileEntry.MaxDuration);
                _rtb.AppendLine("");
            }

            // Missile Motion Data.
            if (hasMissileMotion)
            {
                _rtb.AppendFormatLine("Missile motion: {0}", missileMotionEntry.Name);
                _rtb.AppendFormatLine("Missile count: {0}", missileMotionEntry.MissileCount);
                _rtb.AppendLine("Missile Script body:");
                _rtb.AppendText(missileMotionEntry.Script);
            }
        }

        private void AppendSkillLine()
        {
            var query = from skillLineAbility in DBC.DBC.SkillLineAbility.Records
                        join skillLine in DBC.DBC.SkillLine.Records
                        on skillLineAbility.SkillId equals skillLine.Id
                        where skillLineAbility.SpellId == _spell.ID
                        select new
                        {
                            skillLineAbility,
                            skillLine
                        };

            if (query.Count() == 0)
                return;

            var skill = query.First().skillLineAbility;
            var line =  query.First().skillLine;

            _rtb.AppendFormatLine("Skill (Id {0}) \"{1}\"", skill.SkillId, line.Name);
            _rtb.AppendFormat("    ReqSkillValue {0}", skill.ReqSkillValue);

            _rtb.AppendFormat(", Forward Spell = {0}, MinMaxValue ({1}, {2})", skill.ForwardSpellid, skill.MinValue, skill.MaxValue);
            _rtb.AppendFormat(", CharacterPoints ({0}, {1})", skill.CharacterPoints[0], skill.CharacterPoints[1]);
        }

        private void AppendSpellEffectInfo()
        {
            _rtb.AppendLine(_line);

            foreach (var effect in _spell.Effects)
            {
                if (effect == null)
                    continue;

                _rtb.SetBold();
                _rtb.AppendFormatLine("Effect {0}: Id {1} ({2})", effect.EffectIndex, effect.Effect, (SpellEffects)effect.Effect);
                _rtb.SetDefaultStyle();

                if (effect.ScalingMultiplier != 0.0f && _spell.Scaling != null && _spell.Scaling.PlayerClass != 0)
                {
                    var gtEntry = (uint)((_spell.Scaling.PlayerClass != -1 ? _spell.Scaling.PlayerClass - 1 : 11) * 100) + DBC.DBC.SelectedLevel - 1;
                    var gtMultiplier = DBC.DBC.gtSpellScaling[gtEntry].Multiplier;
                    if (_spell.Scaling.MaxCastTime > 0 && _spell.Scaling.MaxCastTimeLevel > DBC.DBC.SelectedLevel)
                        gtMultiplier *= (float)(_spell.Scaling.MinCastTime + (DBC.DBC.SelectedLevel - 1) * (_spell.Scaling.MaxCastTime - _spell.Scaling.MinCastTime) / (_spell.Scaling.MaxCastTimeLevel - 1)) / (float)_spell.Scaling.MaxCastTime;
                    if (_spell.Scaling.CoefLevelBase > DBC.DBC.SelectedLevel)
                        gtMultiplier *= (1.0f - _spell.Scaling.CoefBase) * (float)(DBC.DBC.SelectedLevel - 1) / (float)(_spell.Scaling.CoefLevelBase - 1) + _spell.Scaling.CoefBase;

                    if (effect.RandomPointsScalingMultiplier != 0.0f)
                    {
                        var avg = effect.ScalingMultiplier * gtMultiplier;
                        var delta = effect.RandomPointsScalingMultiplier * effect.ScalingMultiplier * gtMultiplier * 0.5;
                        _rtb.AppendFormat("BasePoints = {0:F} to {1:F}", avg - delta, avg + delta);
                    }
                    else
                        _rtb.AppendFormat("AveragePoints = {0:F}", effect.ScalingMultiplier * gtMultiplier);

                    if (effect.ComboPointsScalingMultiplier != 0.0f)
                        _rtb.AppendFormatIfNotNull(" + combo * {0:F}", effect.ComboPointsScalingMultiplier * gtMultiplier);
                    else
                        _rtb.AppendFormatIfNotNull(" + combo * {0:F}", effect.EffectPointsPerResource);
                }
                else
                {
                    _rtb.AppendFormat("BasePoints = {0}", effect.EffectBasePoints + ((effect.EffectDieSides == 0) ? 0 : 1));

                    if (effect.EffectRealPointsPerLevel != 0)
                        _rtb.AppendFormat(" + Level * {0:F}", effect.EffectRealPointsPerLevel);

                    if (effect.EffectDieSides > 1)
                    {
                        if (effect.EffectRealPointsPerLevel != 0)
                            _rtb.AppendFormat(" to {0} + lvl * {1:F}",
                                effect.EffectBasePoints + effect.EffectDieSides, effect.EffectRealPointsPerLevel);
                        else
                            _rtb.AppendFormat(" to {0}", effect.EffectBasePoints + effect.EffectDieSides);
                    }

                    _rtb.AppendFormatIfNotNull(" + combo * {0:F}", effect.EffectPointsPerResource);
                }

                if (effect.EffectChainAmplitude != 1.0f)
                    _rtb.AppendFormat(" x {0:F}", effect.EffectChainAmplitude);

                _rtb.AppendFormatIfNotNull("  Multiple = {0:F}", effect.EffectAmplitude);
                _rtb.AppendLine();

                _rtb.AppendFormatLine("Targets ({0}, {1}) ({2}, {3})",
                    effect.EffectImplicitTargetA, effect.EffectImplicitTargetB,
                    (Targets)effect.EffectImplicitTargetA, (Targets)effect.EffectImplicitTargetB);

                AuraModTypeName(effect);

                var classMask = effect.EffectSpellClassMask;

                if (classMask[0] != 0 || classMask[1] != 0 || classMask[2] != 0)
                {
                    _rtb.AppendFormatLine("SpellClassMask = {0:X8} {1:X8} {2:X8}", classMask[0], classMask[1], classMask[2]);

                    var query = from spell in DBC.DBC.SpellInfoStore.Values
                                where spell.SpellFamilyName == _spell.SpellFamilyName && spell.SpellFamilyFlags.ContainsElement(classMask)
                                join sk in DBC.DBC.SkillLineAbility.Values on spell.ID equals sk.SpellId into temp
                                from skill in temp.DefaultIfEmpty(new SkillLineAbilityEntry())
                                select new
                                {
                                    SpellID = spell.ID,
                                    SpellName = spell.SpellNameRank,
                                    SkillId = skill.SkillId
                                };

                    foreach (var row in query)
                    {
                        if (row.SkillId > 0)
                        {
                            _rtb.SelectionColor = Color.Blue;
                            _rtb.AppendFormatLine("\t+ {0} - {1}",  row.SpellID, row.SpellName);
                        }
                        else
                        {
                            _rtb.SelectionColor = Color.Red;
                            _rtb.AppendFormatLine("\t- {0} - {1}", row.SpellID, row.SpellName);
                        }
                        _rtb.SelectionColor = Color.Black;
                    }
                }

                _rtb.AppendFormatLineIfNotNull("{0}", effect.Radius);
                _rtb.AppendFormatLineIfNotNull("{0}", effect.MaxRadius);

                // append trigger spell
                var trigger = effect.EffectTriggerSpell;
                if (trigger != 0)
                {
                    if (DBC.DBC.SpellInfoStore.ContainsKey(trigger))
                    {
                        var triggerSpell = DBC.DBC.SpellInfoStore[trigger];
                        _rtb.SetStyle(Color.Blue, FontStyle.Bold);
                        _rtb.AppendFormatLine("   Trigger spell ({0}) {1}. Chance = {2}", trigger, triggerSpell.SpellNameRank, _spell.ProcChance);
                        _rtb.AppendFormatLineIfNotNull("   Description: {0}", triggerSpell.Description);
                        _rtb.AppendFormatLineIfNotNull("   ToolTip: {0}", triggerSpell.ToolTip);
                        _rtb.SetDefaultStyle();
                        if (triggerSpell.ProcFlags != 0)
                        {
                            _rtb.AppendFormatLine("Charges - {0}", triggerSpell.ProcCharges);
                            _rtb.AppendLine(_line);
                            _rtb.AppendLine(triggerSpell.ProcInfo);
                            _rtb.AppendLine(_line);
                        }
                    }
                    else
                        _rtb.AppendFormatLine("Trigger spell ({0}) Not found, Chance = {1}", trigger, _spell.ProcChance);
                }

                _rtb.AppendFormatLineIfNotNull("EffectChainTarget = {0}", effect.EffectChainTargets);
                _rtb.AppendFormatLineIfNotNull("EffectItemType = {0}", effect.EffectItemType);

                if ((Mechanics)effect.EffectMechanic != Mechanics.MECHANIC_NONE)
                    _rtb.AppendFormatLine("Effect Mechanic = {0} ({1})", effect.EffectMechanic, (Mechanics)effect.EffectMechanic);

                _rtb.AppendLine();
            }
        }

        private void AppendSpellAura()
        {
            if (_spell.CasterAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(_spell.CasterAuraSpell))
                    _rtb.AppendFormatLine("  Caster Aura Spell ({0}) {1}", _spell.CasterAuraSpell, DBC.DBC.SpellInfoStore[_spell.CasterAuraSpell].SpellName);
                else
                    _rtb.AppendFormatLine("  Caster Aura Spell ({0}) ?????", _spell.CasterAuraSpell);
            }

            if (_spell.TargetAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(_spell.TargetAuraSpell))
                    _rtb.AppendFormatLine("  Target Aura Spell ({0}) {1}", _spell.TargetAuraSpell, DBC.DBC.SpellInfoStore[_spell.TargetAuraSpell].SpellName);
                else
                    _rtb.AppendFormatLine("  Target Aura Spell ({0}) ?????", _spell.TargetAuraSpell);
            }

            if (_spell.ExcludeCasterAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(_spell.ExcludeCasterAuraSpell))
                    _rtb.AppendFormatLine("  Ex Caster Aura Spell ({0}) {1}", _spell.ExcludeCasterAuraSpell, DBC.DBC.SpellInfoStore[_spell.ExcludeCasterAuraSpell].SpellName);
                else
                    _rtb.AppendFormatLine("  Ex Caster Aura Spell ({0}) ?????", _spell.ExcludeCasterAuraSpell);
            }

            // ReSharper disable InvertIf
            if (_spell.ExcludeTargetAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(_spell.ExcludeTargetAuraSpell))
                    _rtb.AppendFormatLine("  Ex Target Aura Spell ({0}) {1}", _spell.ExcludeTargetAuraSpell, DBC.DBC.SpellInfoStore[_spell.ExcludeTargetAuraSpell].SpellName);
                else
                    _rtb.AppendFormatLine("  Ex Target Aura Spell ({0}) ?????", _spell.ExcludeTargetAuraSpell);
            }
            // ReSharper enable InvertIf
        }

        private void AuraModTypeName(SpellEffectEntry effect)
        {
            var aura = (AuraType)effect.EffectAura;
            var misc = effect.EffectMiscValue;

            if (effect.EffectAura == 0)
            {
                _rtb.AppendFormatLineIfNotNull("EffectMiscValueA = {0}", effect.EffectMiscValue);
                _rtb.AppendFormatLineIfNotNull("EffectMiscValueB = {0}", effect.EffectMiscValueB);
                _rtb.AppendFormatLineIfNotNull("EffectAuraPeriod = {0}", effect.EffectAuraPeriod);

                return;
            }

            _rtb.AppendFormat("Aura Id {0:D} ({0})", aura);
            _rtb.AppendFormat(", value = {0}", effect.EffectBasePoints);
            _rtb.AppendFormat(", misc = {0} (", misc);

            switch (aura)
            {
                case AuraType.SPELL_AURA_MOD_STAT:
                    _rtb.Append((UnitMods)misc);
                    break;
                case AuraType.SPELL_AURA_MOD_RATING:
                    _rtb.Append((CombatRating)misc);
                    break;
                case AuraType.SPELL_AURA_ADD_FLAT_MODIFIER:
                case AuraType.SPELL_AURA_ADD_PCT_MODIFIER:
                    _rtb.Append((SpellModOp)misc);
                    break;
                // todo: more case
                default:
                    _rtb.Append(misc);
                    break;
            }

            _rtb.AppendFormat("), miscB = {0}", effect.EffectMiscValueB);
            _rtb.AppendFormatLine(", periodic = {0}", effect.EffectAuraPeriod);

            switch (aura)
            {
                case AuraType.SPELL_AURA_OVERRIDE_SPELLS:
                    if (!DBC.DBC.OverrideSpellData.ContainsKey((uint)misc))
                    {
                        _rtb.SetStyle(Color.Red, FontStyle.Bold);
                        _rtb.AppendFormatLine("Cannot find key {0} in OverrideSpellData.dbc", (uint)misc);
                    }
                    else
                    {
                        _rtb.AppendLine();
                        _rtb.SetStyle(Color.DarkRed, FontStyle.Bold);
                        var @override = DBC.DBC.OverrideSpellData[(uint)misc];
                        _rtb.AppendFormatLine("Overriding Spells{0}:", !String.IsNullOrEmpty(@override.Name) ? String.Format(" ({0})", @override.Name) : String.Empty);
                        for (var i = 0; i < 10; ++i)
                        {
                            if (@override.Spells[i] == 0)
                                continue;

                            _rtb.SetStyle(Color.DarkBlue, FontStyle.Regular);
                            _rtb.AppendFormatLine("\t - #{0} ({1}) {2}", i + 1, @override.Spells[i],
                                DBC.DBC.SpellInfoStore.ContainsKey(@override.Spells[i]) ? DBC.DBC.SpellInfoStore[@override.Spells[i]].SpellName : "?????");
                        }
                        _rtb.AppendLine();
                    }
                    break;
                case AuraType.SPELL_AURA_SCREEN_EFFECT:
                    _rtb.SetStyle(Color.DarkBlue, FontStyle.Bold);
                    _rtb.AppendFormatLine("ScreenEffect: {0}",
                        DBC.DBC.ScreenEffect.ContainsKey((uint)misc) ? DBC.DBC.ScreenEffect[(uint)misc].Name : "?????");
                    break;
                default:
                    break;
            }
        }

        private readonly string[] modeNames = new[]
        {
            "Normal 10",
            "Normal 25",
            "Heroic 10",
            "Heroic 25",
        };

        private void AppendDifficultyInfo()
        {
            var difficultyId = _spell.SpellDifficultyId;
            if (difficultyId == 0)
                return;

            if (!DBC.DBC.SpellDifficulty.ContainsKey(difficultyId))
            {
                _rtb.AppendFormatLine("Cannot find difficulty overrides for id {0} in SpellDifficulty.dbc!", difficultyId);
                return;
            }

            _rtb.SetBold();
            _rtb.AppendLine("Spell difficulty Ids:");

            var difficulty = DBC.DBC.SpellDifficulty[difficultyId];
            for (var i = 0; i < 4; ++i)
            {
                if (difficulty.SpellId[i] <= 0)
                    continue;

                _rtb.AppendFormatLine("{0}: {1}", modeNames[i], difficulty.SpellId[i]);
            }
        }

        private void AppendAreaInfo()
        {
            if (_spell.AreaGroupId <= 0)
                return;

            var areaGroupId = (uint)_spell.AreaGroupId;
            if (!DBC.DBC.AreaGroup.ContainsKey(areaGroupId))
            {
                _rtb.AppendFormatLine("Cannot find area group id {0} in AreaGroup.dbc!", _spell.AreaGroupId);
                return;
            }

            _rtb.AppendLine();
            _rtb.SetBold();
            _rtb.AppendLine("Allowed areas:");
            while (DBC.DBC.AreaGroup.ContainsKey(areaGroupId))
            {
                var groupEntry = DBC.DBC.AreaGroup[areaGroupId];
                for (var i = 0; i < 6; ++i)
                {
                    var areaId = groupEntry.AreaId[i];
                    if (DBC.DBC.AreaTable.ContainsKey(areaId))
                    {
                        var areaEntry = DBC.DBC.AreaTable[areaId];
                        _rtb.AppendFormatLine("{0} - {1} (Map: {2})", areaId, areaEntry.Name, areaEntry.MapId);
                    }
                }


                if (groupEntry.NextGroup == 0)
                    break;

                // Try search in next group
                areaGroupId = groupEntry.NextGroup;
            }

            _rtb.AppendLine();
        }

        private void AppendItemInfo()
        {
            var items = from item in DBC.DBC.ItemTemplate
                        where  item.SpellId.ContainsElement((int)_spell.ID)
                        select item;

            if (items.Count() == 0)
                return;

            _rtb.AppendLine(_line);
            _rtb.SetStyle(Color.Blue, FontStyle.Bold);
            _rtb.AppendLine("Items using this spell:");
            _rtb.SetDefaultStyle();

            foreach (var item in items)
            {
                var name = string.IsNullOrEmpty(item.LocalesName) ? item.Name : item.LocalesName;
                var desc = string.IsNullOrEmpty(item.LocalesDescription) ? item.Description : item.LocalesDescription;

                desc = string.IsNullOrEmpty(desc) ? string.Empty : string.Format(" - \"{0}\"", desc);

                _rtb.AppendFormatLine(@"   {0}: {1} {2}", item.Entry, name, desc);
            }
        }
    }
}
