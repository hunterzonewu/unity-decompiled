// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerChannelStripView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerChannelStripView
  {
    public static float kVolumeScaleMouseDrag = 1f;
    public static float kEffectScaleMouseDrag = 0.3f;
    private static Color kMoveColorHighlight = new Color(0.3f, 0.6f, 1f, 0.4f);
    private static Color kMoveSlotColHiAllowed = new Color(1f, 1f, 1f, 0.7f);
    private static Color kMoveSlotColLoAllowed = new Color(1f, 1f, 1f, 0.0f);
    private static Color kMoveSlotColBorderAllowed = new Color(1f, 1f, 1f, 1f);
    private static Color kMoveSlotColHiDisallowed = new Color(1f, 0.0f, 0.0f, 0.7f);
    private static Color kMoveSlotColLoDisallowed = new Color(0.8f, 0.0f, 0.0f, 0.0f);
    private static Color kMoveSlotColBorderDisallowed = new Color(1f, 0.0f, 0.0f, 1f);
    private static int kRectSelectionHashCode = "RectSelection".GetHashCode();
    private static int kEffectDraggingHashCode = "EffectDragging".GetHashCode();
    private static int kVerticalFaderHash = "VerticalFader".GetHashCode();
    private static readonly Color kGridColorDark = new Color(0.0f, 0.0f, 0.0f, 0.18f);
    private static readonly Color kGridColorLight = new Color(0.0f, 0.0f, 0.0f, 0.1f);
    private static Color hfaderCol1 = new Color(0.2f, 0.2f, 0.2f, 1f);
    private static Color hfaderCol2 = new Color(0.4f, 0.4f, 0.4f, 1f);
    public int m_FocusIndex = -1;
    public Vector2 m_RectSelectionStartPos = new Vector2(0.0f, 0.0f);
    public Rect m_RectSelectionRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private MixerGroupControllerCompareByName m_GroupComparer = new MixerGroupControllerCompareByName();
    private int m_ChangingWetMixIndex = -1;
    private int m_MovingEffectSrcIndex = -1;
    private int m_MovingEffectDstIndex = -1;
    private Rect m_MovingSrcRect = new Rect(-1f, -1f, 0.0f, 0.0f);
    private Rect m_MovingDstRect = new Rect(-1f, -1f, 0.0f, 0.0f);
    private List<int> m_LastNumChannels = new List<int>();
    private readonly Vector2 channelStripsOffset = new Vector2(15f, 10f);
    public GUIStyle sharedGuiStyle = new GUIStyle();
    private GUIContent bypassButtonContent = new GUIContent(string.Empty, "Toggle bypass on this effect");
    private GUIContent headerGUIContent = new GUIContent();
    private GUIContent addText = new GUIContent("Add..");
    [NonSerialized]
    private GUIStyle developerInfoStyle = AudioMixerDrawUtils.BuildGUIStyleForLabel(new Color(1f, 0.0f, 0.0f, 0.5f), 20, false, FontStyle.Bold, TextAnchor.MiddleLeft);
    [NonSerialized]
    private Vector3[] cablepoints = new Vector3[20];
    private const float k_MinVULevel = -80f;
    private const float headerHeight = 22f;
    private const float vuHeight = 170f;
    private const float dbHeight = 17f;
    private const float soloMuteBypassHeight = 30f;
    private const float effectHeight = 16f;
    private const float spaceBetween = 0.0f;
    private const int channelStripSpacing = 4;
    private const float channelStripBaseWidth = 90f;
    private const float spaceBetweenMainGroupsAndReferenced = 50f;
    private const float kGridTileWidth = 12f;
    public int m_IndexCounter;
    public int m_EffectInteractionControlID;
    public int m_RectSelectionControlID;
    public float m_MouseDragStartX;
    public float m_MouseDragStartY;
    public float m_MouseDragStartValue;
    private AudioMixerChannelStripView.State m_State;
    private AudioMixerController m_Controller;
    private bool m_WaitingForDragEvent;
    private bool m_MovingEffectAllowed;
    private AudioMixerGroupController m_MovingSrcGroup;
    private AudioMixerGroupController m_MovingDstGroup;
    private bool m_RequiresRepaint;
    private static Texture2D m_GridTexture;
    [NonSerialized]
    private int FrameCounter;

    public bool requiresRepaint
    {
      get
      {
        if (!this.m_RequiresRepaint)
          return false;
        this.m_RequiresRepaint = false;
        return true;
      }
    }

    private static Color gridColor
    {
      get
      {
        if (EditorGUIUtility.isProSkin)
          return AudioMixerChannelStripView.kGridColorDark;
        return AudioMixerChannelStripView.kGridColorLight;
      }
    }

    private AudioMixerDrawUtils.Styles styles
    {
      get
      {
        return AudioMixerDrawUtils.styles;
      }
    }

    private Texture2D gridTextureTilable
    {
      get
      {
        if ((UnityEngine.Object) AudioMixerChannelStripView.m_GridTexture == (UnityEngine.Object) null)
          AudioMixerChannelStripView.m_GridTexture = AudioMixerChannelStripView.CreateTilableGridTexture(12, 12, new Color(0.0f, 0.0f, 0.0f, 0.0f), AudioMixerChannelStripView.gridColor);
        return AudioMixerChannelStripView.m_GridTexture;
      }
    }

    public AudioMixerChannelStripView(AudioMixerChannelStripView.State state)
    {
      this.m_State = state;
    }

    private static Texture2D CreateTilableGridTexture(int width, int height, Color backgroundColor, Color lineColor)
    {
      Color[] colors = new Color[width * height];
      for (int index = 0; index < height * width; ++index)
        colors[index] = backgroundColor;
      for (int index = 0; index < height; ++index)
        colors[index * width + (width - 1)] = lineColor;
      for (int index = 0; index < width; ++index)
        colors[(height - 1) * width + index] = lineColor;
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.SetPixels(colors);
      texture2D.Apply();
      return texture2D;
    }

    private void DrawAreaBackground(Rect rect)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      GUI.color = new Color(1f, 1f, 1f, !EditorGUIUtility.isProSkin ? 0.2f : 0.6f);
      AudioMixerDrawUtils.styles.channelStripAreaBackground.Draw(rect, false, false, false, false);
      GUI.color = color;
    }

    private void SetFocus()
    {
      this.m_FocusIndex = this.m_IndexCounter;
    }

    private void ClearFocus()
    {
      this.m_FocusIndex = -1;
    }

    private bool HasFocus()
    {
      return this.m_FocusIndex == this.m_IndexCounter;
    }

    private bool IsFocusActive()
    {
      return this.m_FocusIndex != -1;
    }

    public static void InsertEffectPopupCallback(object obj)
    {
      AudioMixerChannelStripView.EffectContext effectContext = (AudioMixerChannelStripView.EffectContext) obj;
      foreach (AudioMixerGroupController group in effectContext.groups)
      {
        Undo.RecordObject((UnityEngine.Object) group, "Add effect");
        AudioMixerEffectController effect = new AudioMixerEffectController(effectContext.name);
        int index = effectContext.index == -1 || effectContext.index > group.effects.Length ? group.effects.Length : effectContext.index;
        group.InsertEffect(effect, index);
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) effect, (UnityEngine.Object) effectContext.controller);
        effect.PreallocateGUIDs();
      }
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public void RemoveEffectPopupCallback(object obj)
    {
      AudioMixerChannelStripView.EffectContext effectContext = (AudioMixerChannelStripView.EffectContext) obj;
      foreach (AudioMixerGroupController group in effectContext.groups)
      {
        if (effectContext.index < group.effects.Length)
        {
          AudioMixerEffectController effect = group.effects[effectContext.index];
          effectContext.controller.ClearSendConnectionsTo(effect);
          effectContext.controller.RemoveEffect(effect, group);
        }
      }
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public static void ConnectSendPopupCallback(object obj)
    {
      AudioMixerChannelStripView.ConnectSendContext connectSendContext = (AudioMixerChannelStripView.ConnectSendContext) obj;
      Undo.RecordObject((UnityEngine.Object) connectSendContext.sendEffect, "Change Send Target");
      connectSendContext.sendEffect.sendTarget = connectSendContext.targetEffect;
    }

    private bool ClipRect(Rect r, Rect clipRect, ref Rect overlap)
    {
      overlap.x = Mathf.Max(r.x, clipRect.x);
      overlap.y = Mathf.Max(r.y, clipRect.y);
      overlap.width = Mathf.Min(r.x + r.width, clipRect.x + clipRect.width) - overlap.x;
      overlap.height = Mathf.Min(r.y + r.height, clipRect.y + clipRect.height) - overlap.y;
      if ((double) overlap.width > 0.0)
        return (double) overlap.height > 0.0;
      return false;
    }

    public float VerticalFader(Rect r, float value, int direction, float dragScale, bool drawScaleValues, bool drawMarkerValue, string tooltip, float maxValue, GUIStyle style)
    {
      Event current = Event.current;
      int fixedHeight = (int) style.fixedHeight;
      int num1 = (int) r.height - fixedHeight;
      float screenMapping1 = AudioMixerController.VolumeToScreenMapping(Mathf.Clamp(value, AudioMixerController.kMinVolume, maxValue), (float) num1, true);
      Rect rect = new Rect(r.x, r.y + (float) (int) screenMapping1, r.width, (float) fixedHeight);
      int controlId = GUIUtility.GetControlID(AudioMixerChannelStripView.kVerticalFaderHash, FocusType.Passive);
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (r.Contains(current.mousePosition) && GUIUtility.hotControl == 0)
          {
            this.m_MouseDragStartY = current.mousePosition.y;
            this.m_MouseDragStartValue = screenMapping1;
            GUIUtility.hotControl = controlId;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            value = Mathf.Clamp(AudioMixerController.VolumeToScreenMapping(Mathf.Clamp(this.m_MouseDragStartValue + dragScale * (current.mousePosition.y - this.m_MouseDragStartY), 0.0f, (float) num1), (float) num1, false), AudioMixerController.kMinVolume, maxValue);
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (drawScaleValues)
          {
            float num2 = r.y + (float) fixedHeight / 2f;
            float f = maxValue;
            EditorGUI.BeginDisabledGroup(true);
            while ((double) f >= (double) AudioMixerController.kMinVolume)
            {
              float screenMapping2 = AudioMixerController.VolumeToScreenMapping(f, (float) num1, true);
              if ((double) f / 10.0 % 2.0 == 0.0)
                GUI.Label(new Rect(r.x, (float) ((double) num2 + (double) screenMapping2 - 7.0), r.width, 14f), GUIContent.Temp(Mathf.RoundToInt(f).ToString()), this.styles.vuValue);
              EditorGUI.DrawRect(new Rect(r.x, (float) ((double) num2 + (double) screenMapping2 - 1.0), 5f, 1f), new Color(0.0f, 0.0f, 0.0f, 0.5f));
              f -= 10f;
            }
            EditorGUI.EndDisabledGroup();
          }
          if (drawMarkerValue)
            style.Draw(rect, GUIContent.Temp(Mathf.RoundToInt(value).ToString()), false, false, false, false);
          else
            style.Draw(rect, false, false, false, false);
          AudioMixerDrawUtils.AddTooltipOverlay(rect, tooltip);
          break;
      }
      return value;
    }

    public float HorizontalFader(Rect r, float value, float minValue, float maxValue, int direction, float dragScale)
    {
      ++this.m_IndexCounter;
      Rect r1 = new Rect(r);
      float num1 = r.width * 0.2f;
      float num2 = r1.width - num1;
      AudioMixerDrawUtils.DrawGradientRect(r1, AudioMixerChannelStripView.hfaderCol1, AudioMixerChannelStripView.hfaderCol2);
      Event current = Event.current;
      if (current.type == EventType.MouseDown && r1.Contains(current.mousePosition))
      {
        this.m_MouseDragStartX = current.mousePosition.x;
        this.m_MouseDragStartValue = value;
        this.SetFocus();
      }
      if (this.HasFocus())
      {
        if (current.type == EventType.MouseDrag)
        {
          value = this.m_MouseDragStartValue + (float) ((double) dragScale * ((double) maxValue - (double) minValue) * ((double) current.mousePosition.x - (double) this.m_MouseDragStartX)) / num2;
          Event.current.Use();
        }
        else if (current.type == EventType.MouseUp)
        {
          this.ClearFocus();
          Event.current.Use();
        }
      }
      value = Mathf.Clamp(value, minValue, maxValue);
      r1.x = r.x;
      r1.width = r.width;
      r1.x = r.x + num2 * (float) (((double) value - (double) minValue) / ((double) maxValue - (double) minValue));
      r1.width = num1;
      AudioMixerDrawUtils.DrawGradientRect(r1, AudioMixerChannelStripView.hfaderCol2, AudioMixerChannelStripView.hfaderCol1);
      return value;
    }

    public GUIStyle GetEffectBarStyle(AudioMixerEffectController effect)
    {
      if (effect.IsSend() || effect.IsReceive() || effect.IsDuckVolume())
        return this.styles.sendReturnBar;
      if (effect.IsAttenuation())
        return this.styles.attenuationBar;
      return this.styles.effectBar;
    }

    private void EffectSlot(Rect effectRect, AudioMixerSnapshotController snapshot, AudioMixerEffectController effect, int effectIndex, ref int highlightEffectIndex, AudioMixerChannelStripView.ChannelStripParams p, ref Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot> patchslots)
    {
      if ((UnityEngine.Object) effect == (UnityEngine.Object) null)
        return;
      Rect rect = effectRect;
      Event current = Event.current;
      if (current.type == EventType.Repaint && patchslots != null && (effect.IsSend() || MixerEffectDefinitions.EffectCanBeSidechainTarget(effect)))
        patchslots[effect] = new AudioMixerChannelStripView.PatchSlot()
        {
          group = p.group,
          x = rect.xMax - (float) (((double) rect.yMax - (double) rect.yMin) * 0.5),
          y = (float) (((double) rect.yMin + (double) rect.yMax) * 0.5)
        };
      bool flag1 = !effect.DisallowsBypass();
      Rect position = rect;
      position.width = 10f;
      rect.xMin += 10f;
      if (flag1 && GUI.Button(position, this.bypassButtonContent, GUIStyle.none))
      {
        effect.bypass = !effect.bypass;
        this.m_Controller.UpdateBypass();
        InspectorWindow.RepaintAllInspectors();
      }
      ++this.m_IndexCounter;
      float level = !((UnityEngine.Object) effect != (UnityEngine.Object) null) ? AudioMixerController.kMinVolume : Mathf.Clamp(effect.GetValueForMixLevel(this.m_Controller, snapshot), AudioMixerController.kMinVolume, AudioMixerController.kMaxEffect);
      bool flag2 = (UnityEngine.Object) effect != (UnityEngine.Object) null && (effect.IsSend() && (UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) null || effect.enableWetMix);
      if (current.type == EventType.Repaint)
      {
        GUIStyle effectBarStyle = this.GetEffectBarStyle(effect);
        float num1 = !flag2 ? 1f : (float) (((double) level - (double) AudioMixerController.kMinVolume) / ((double) AudioMixerController.kMaxEffect - (double) AudioMixerController.kMinVolume));
        bool flag3 = !p.group.bypassEffects && ((UnityEngine.Object) effect == (UnityEngine.Object) null || !effect.bypass) || (UnityEngine.Object) effect != (UnityEngine.Object) null && effect.DisallowsBypass();
        Color color1 = !((UnityEngine.Object) effect != (UnityEngine.Object) null) ? new Color(0.0f, 0.0f, 0.0f, 0.5f) : AudioMixerDrawUtils.GetEffectColor(effect);
        if (!flag3)
          color1 = new Color(color1.r * 0.5f, color1.g * 0.5f, color1.b * 0.5f);
        if (flag3)
        {
          if ((double) num1 < 1.0)
          {
            float num2 = rect.width * num1;
            if ((double) num2 < 4.0)
            {
              num2 = Mathf.Max(num2, 2f);
              float x = (float) (1.0 - (double) num2 / 4.0);
              Color color2 = GUI.color;
              if (!GUI.enabled)
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
              GUI.DrawTextureWithTexCoords(new Rect(rect.x, rect.y, num2, rect.height), (Texture) effectBarStyle.focused.background, new Rect(x, 0.0f, 1f - x, 1f));
              GUI.color = color2;
            }
            else
              effectBarStyle.Draw(new Rect(rect.x, rect.y, num2, rect.height), false, false, false, true);
            GUI.DrawTexture(new Rect(rect.x + num2, rect.y, rect.width - num2, rect.height), (Texture) effectBarStyle.onFocused.background, ScaleMode.StretchToFill);
          }
          else
            effectBarStyle.Draw(rect, !flag2, false, false, flag2);
        }
        else
          effectBarStyle.Draw(rect, false, false, false, false);
        if (flag1)
          this.styles.circularToggle.Draw(new Rect(position.x + 2f, position.y + 5f, position.width - 2f, position.width - 2f), false, false, !effect.bypass, false);
        if (effect.IsSend() && (UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) null)
        {
          --position.y;
          GUI.Label(position, this.styles.sendString, EditorStyles.miniLabel);
        }
        EditorGUI.BeginDisabledGroup(!flag3);
        string effectSlotName = this.GetEffectSlotName(effect, flag2, snapshot, p);
        string effectSlotTooltip = this.GetEffectSlotTooltip(effect, rect, p);
        GUI.Label(new Rect(rect.x, rect.y, rect.width - 10f, rect.height), GUIContent.Temp(effectSlotName, effectSlotTooltip), this.styles.effectName);
        EditorGUI.EndDisabledGroup();
      }
      else
        this.EffectSlotDragging(effectRect, snapshot, effect, flag2, level, effectIndex, ref highlightEffectIndex, p);
    }

    private string GetEffectSlotName(AudioMixerEffectController effect, bool showLevel, AudioMixerSnapshotController snapshot, AudioMixerChannelStripView.ChannelStripParams p)
    {
      if (this.m_ChangingWetMixIndex == this.m_IndexCounter && showLevel)
        return string.Format("{0:F1} dB", (object) effect.GetValueForMixLevel(this.m_Controller, snapshot));
      if (effect.IsSend() && (UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) null)
        return effect.GetSendTargetDisplayString(p.effectMap);
      return effect.effectName;
    }

    private string GetEffectSlotTooltip(AudioMixerEffectController effect, Rect effectRect, AudioMixerChannelStripView.ChannelStripParams p)
    {
      if (!effectRect.Contains(Event.current.mousePosition))
        return string.Empty;
      if (effect.IsSend())
      {
        if ((UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) null)
          return "Send to: " + effect.GetSendTargetDisplayString(p.effectMap);
        return this.styles.emptySendSlotGUIContent.tooltip;
      }
      if (effect.IsReceive())
        return this.styles.returnSlotGUIContent.tooltip;
      if (effect.IsDuckVolume())
        return this.styles.duckVolumeSlotGUIContent.tooltip;
      if (effect.IsAttenuation())
        return this.styles.attenuationSlotGUIContent.tooltip;
      return this.styles.effectSlotGUIContent.tooltip;
    }

    private void EffectSlotDragging(Rect r, AudioMixerSnapshotController snapshot, AudioMixerEffectController effect, bool showLevel, float level, int effectIndex, ref int highlightEffectIndex, AudioMixerChannelStripView.ChannelStripParams p)
    {
      Event current1 = Event.current;
      switch (current1.GetTypeForControl(this.m_EffectInteractionControlID))
      {
        case EventType.MouseDown:
          if (!r.Contains(current1.mousePosition) || current1.button != 0 || GUIUtility.hotControl != 0)
            break;
          GUIUtility.hotControl = this.m_EffectInteractionControlID;
          this.m_MouseDragStartX = current1.mousePosition.x;
          this.m_MouseDragStartValue = level;
          highlightEffectIndex = effectIndex;
          this.m_MovingEffectSrcIndex = -1;
          this.m_MovingEffectDstIndex = -1;
          this.m_WaitingForDragEvent = true;
          this.m_MovingSrcRect = r;
          this.m_MovingDstRect = r;
          this.m_MovingSrcGroup = p.group;
          this.m_MovingDstGroup = p.group;
          this.m_MovingEffectAllowed = true;
          this.SetFocus();
          Event.current.Use();
          EditorGUIUtility.SetWantsMouseJumping(1);
          InspectorWindow.RepaintAllInspectors();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != this.m_EffectInteractionControlID || current1.button != 0 || !p.stripRect.Contains(current1.mousePosition))
            break;
          if (this.m_MovingEffectDstIndex != -1 && this.m_MovingEffectAllowed)
          {
            if (this.IsDuplicateKeyPressed() && this.CanDuplicateDraggedEffect())
            {
              AudioMixerEffectController effect1 = this.m_MovingSrcGroup.controller.CopyEffect(this.m_MovingSrcGroup.effects[this.m_MovingEffectSrcIndex]);
              List<AudioMixerEffectController> list = ((IEnumerable<AudioMixerEffectController>) this.m_MovingDstGroup.effects).ToList<AudioMixerEffectController>();
              if (AudioMixerController.InsertEffect(effect1, ref list, this.m_MovingEffectDstIndex))
                this.m_MovingDstGroup.effects = list.ToArray();
            }
            else if ((UnityEngine.Object) this.m_MovingSrcGroup == (UnityEngine.Object) this.m_MovingDstGroup)
            {
              List<AudioMixerEffectController> list = ((IEnumerable<AudioMixerEffectController>) this.m_MovingSrcGroup.effects).ToList<AudioMixerEffectController>();
              if (AudioMixerController.MoveEffect(ref list, this.m_MovingEffectSrcIndex, ref list, this.m_MovingEffectDstIndex))
                this.m_MovingSrcGroup.effects = list.ToArray();
            }
            else if (!this.m_MovingSrcGroup.effects[this.m_MovingEffectSrcIndex].IsAttenuation())
            {
              List<AudioMixerEffectController> list1 = ((IEnumerable<AudioMixerEffectController>) this.m_MovingSrcGroup.effects).ToList<AudioMixerEffectController>();
              List<AudioMixerEffectController> list2 = ((IEnumerable<AudioMixerEffectController>) this.m_MovingDstGroup.effects).ToList<AudioMixerEffectController>();
              if (AudioMixerController.MoveEffect(ref list1, this.m_MovingEffectSrcIndex, ref list2, this.m_MovingEffectDstIndex))
              {
                this.m_MovingSrcGroup.effects = list1.ToArray();
                this.m_MovingDstGroup.effects = list2.ToArray();
              }
            }
          }
          this.ClearEffectDragging(ref highlightEffectIndex);
          current1.Use();
          EditorGUIUtility.SetWantsMouseJumping(0);
          GUIUtility.ExitGUI();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != this.m_EffectInteractionControlID)
            break;
          if (this.HasFocus() && this.m_WaitingForDragEvent)
          {
            this.m_ChangingWetMixIndex = -1;
            if (effectIndex < p.group.effects.Length)
            {
              if ((double) Mathf.Abs(current1.delta.y) > (double) Mathf.Abs(current1.delta.x))
              {
                this.m_MovingEffectSrcIndex = effectIndex;
                this.ClearFocus();
              }
              else
                this.m_ChangingWetMixIndex = this.m_IndexCounter;
            }
            this.m_WaitingForDragEvent = false;
          }
          if (this.IsMovingEffect() && p.stripRect.Contains(current1.mousePosition))
          {
            float num1 = r.height * 0.5f;
            float num2 = effectIndex != 0 ? 0.0f : -num1;
            float num3 = effectIndex != p.group.effects.Length - 1 ? r.height : r.height + num1;
            float num4 = current1.mousePosition.y - r.y;
            if ((double) num4 >= (double) num2 && (double) num4 <= (double) num3 && effectIndex < p.group.effects.Length)
            {
              int targetIndex = (double) num4 >= (double) num1 ? effectIndex + 1 : effectIndex;
              if (targetIndex != this.m_MovingEffectDstIndex || (UnityEngine.Object) this.m_MovingDstGroup != (UnityEngine.Object) p.group)
              {
                this.m_MovingDstRect.x = r.x;
                this.m_MovingDstRect.width = r.width;
                this.m_MovingDstRect.y = (float) (((double) num4 >= (double) num1 ? (double) r.y + (double) r.height : (double) r.y) - 1.0);
                this.m_MovingEffectDstIndex = targetIndex;
                this.m_MovingDstGroup = p.group;
                this.m_MovingEffectAllowed = (!this.m_MovingSrcGroup.effects[this.m_MovingEffectSrcIndex].IsAttenuation() || !((UnityEngine.Object) this.m_MovingSrcGroup != (UnityEngine.Object) this.m_MovingDstGroup)) && !AudioMixerController.WillMovingEffectCauseFeedback(p.allGroups, this.m_MovingSrcGroup, this.m_MovingEffectSrcIndex, this.m_MovingDstGroup, targetIndex, (List<AudioMixerController.ConnectionNode>) null) && (!this.IsDuplicateKeyPressed() || this.CanDuplicateDraggedEffect());
              }
              current1.Use();
            }
          }
          if (!this.IsAdjustingWetMix() || !this.HasFocus() || !showLevel)
            break;
          this.m_WaitingForDragEvent = false;
          float num = Mathf.Clamp(AudioMixerChannelStripView.kEffectScaleMouseDrag * HandleUtility.niceMouseDelta + level, AudioMixerController.kMinVolume, AudioMixerController.kMaxEffect) - level;
          if ((double) num != 0.0)
          {
            Undo.RecordObject((UnityEngine.Object) this.m_Controller.TargetSnapshot, "Change effect level");
            if (effect.IsSend() && this.m_Controller.CachedSelection.Count > 1 && this.m_Controller.CachedSelection.Contains(p.group))
            {
              List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>();
              using (List<AudioMixerGroupController>.Enumerator enumerator = this.m_Controller.CachedSelection.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  foreach (AudioMixerEffectController effect1 in enumerator.Current.effects)
                  {
                    if (effect1.effectName == effect.effectName && (UnityEngine.Object) effect1.sendTarget == (UnityEngine.Object) effect.sendTarget)
                      effectControllerList.Add(effect1);
                  }
                }
              }
              using (List<AudioMixerEffectController>.Enumerator enumerator = effectControllerList.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  AudioMixerEffectController current2 = enumerator.Current;
                  if (!current2.IsSend() || (UnityEngine.Object) current2.sendTarget != (UnityEngine.Object) null)
                    current2.SetValueForMixLevel(this.m_Controller, snapshot, Mathf.Clamp(current2.GetValueForMixLevel(this.m_Controller, snapshot) + num, AudioMixerController.kMinVolume, AudioMixerController.kMaxEffect));
                }
              }
            }
            else if (!effect.IsSend() || (UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) null)
              effect.SetValueForMixLevel(this.m_Controller, snapshot, Mathf.Clamp(level + num, AudioMixerController.kMinVolume, AudioMixerController.kMaxEffect));
            InspectorWindow.RepaintAllInspectors();
          }
          current1.Use();
          break;
      }
    }

    private void ClearEffectDragging(ref int highlightEffectIndex)
    {
      if (GUIUtility.hotControl == this.m_EffectInteractionControlID)
        GUIUtility.hotControl = 0;
      this.m_MovingEffectSrcIndex = -1;
      this.m_MovingEffectDstIndex = -1;
      this.m_MovingSrcRect = new Rect(-1f, -1f, 0.0f, 0.0f);
      this.m_MovingDstRect = new Rect(-1f, -1f, 0.0f, 0.0f);
      this.m_MovingSrcGroup = (AudioMixerGroupController) null;
      this.m_MovingDstGroup = (AudioMixerGroupController) null;
      this.m_ChangingWetMixIndex = -1;
      highlightEffectIndex = -1;
      this.ClearFocus();
      InspectorWindow.RepaintAllInspectors();
    }

    private void UnhandledEffectDraggingEvents(ref int highlightEffectIndex)
    {
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(this.m_EffectInteractionControlID);
      switch (typeForControl)
      {
        case EventType.MouseUp:
          if (GUIUtility.hotControl != this.m_EffectInteractionControlID || current.button != 0)
            break;
          this.ClearEffectDragging(ref highlightEffectIndex);
          current.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != this.m_EffectInteractionControlID)
            break;
          this.m_MovingEffectDstIndex = -1;
          this.m_MovingDstRect = new Rect(-1f, -1f, 0.0f, 0.0f);
          this.m_MovingDstGroup = (AudioMixerGroupController) null;
          current.Use();
          break;
        default:
          if (typeForControl != EventType.Repaint)
            break;
          if (this.IsMovingEffect() && current.type == EventType.Repaint)
          {
            EditorGUI.DrawRect(this.m_MovingSrcRect, AudioMixerChannelStripView.kMoveColorHighlight);
            MouseCursor mouse = !this.IsDuplicateKeyPressed() || !this.m_MovingEffectAllowed ? MouseCursor.ResizeVertical : MouseCursor.ArrowPlus;
            EditorGUIUtility.AddCursorRect(new Rect(current.mousePosition.x - 10f, current.mousePosition.y - 10f, 20f, 20f), mouse, this.m_EffectInteractionControlID);
          }
          if (this.m_MovingEffectDstIndex == -1 || (double) this.m_MovingDstRect.y < 0.0)
            break;
          float height = 2f;
          Color color1 = !this.m_MovingEffectAllowed ? AudioMixerChannelStripView.kMoveSlotColLoDisallowed : AudioMixerChannelStripView.kMoveSlotColLoAllowed;
          Color color2 = !this.m_MovingEffectAllowed ? AudioMixerChannelStripView.kMoveSlotColHiDisallowed : AudioMixerChannelStripView.kMoveSlotColHiAllowed;
          Color color3 = !this.m_MovingEffectAllowed ? AudioMixerChannelStripView.kMoveSlotColBorderDisallowed : AudioMixerChannelStripView.kMoveSlotColBorderAllowed;
          AudioMixerDrawUtils.DrawGradientRect(new Rect(this.m_MovingDstRect.x, this.m_MovingDstRect.y - height, this.m_MovingDstRect.width, height), color1, color2);
          AudioMixerDrawUtils.DrawGradientRect(new Rect(this.m_MovingDstRect.x, this.m_MovingDstRect.y, this.m_MovingDstRect.width, height), color2, color1);
          AudioMixerDrawUtils.DrawGradientRect(new Rect(this.m_MovingDstRect.x, this.m_MovingDstRect.y - 1f, this.m_MovingDstRect.width, 1f), color3, color3);
          break;
      }
    }

    private bool IsDuplicateKeyPressed()
    {
      return Event.current.alt;
    }

    private bool CanDuplicateDraggedEffect()
    {
      if (this.IsMovingEffect() && (UnityEngine.Object) this.m_MovingSrcGroup != (UnityEngine.Object) null)
        return !this.m_MovingSrcGroup.effects[this.m_MovingEffectSrcIndex].IsAttenuation();
      return false;
    }

    private bool DoSoloButton(Rect r, AudioMixerGroupController group, List<AudioMixerGroupController> allGroups, List<AudioMixerGroupController> selection)
    {
      Event current = Event.current;
      if (current.type == EventType.MouseUp && current.button == 1 && r.Contains(current.mousePosition) && allGroups.Any<AudioMixerGroupController>((Func<AudioMixerGroupController, bool>) (g => g.solo)))
      {
        Undo.RecordObject((UnityEngine.Object) group, "Change solo state");
        using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.solo = false;
        }
        current.Use();
        return true;
      }
      if (GUI.Toggle(r, group.solo, this.styles.soloGUIContent, AudioMixerDrawUtils.styles.soloToggle) == group.solo)
        return false;
      Undo.RecordObject((UnityEngine.Object) group, "Change solo state");
      group.solo = !group.solo;
      using (List<AudioMixerGroupController>.Enumerator enumerator = selection.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.solo = group.solo;
      }
      return true;
    }

    private bool DoMuteButton(Rect r, AudioMixerGroupController group, List<AudioMixerGroupController> allGroups, bool anySoloActive, List<AudioMixerGroupController> selection)
    {
      Event current = Event.current;
      if (current.type == EventType.MouseUp && current.button == 1 && r.Contains(current.mousePosition) && allGroups.Any<AudioMixerGroupController>((Func<AudioMixerGroupController, bool>) (g => g.mute)))
      {
        Undo.RecordObject((UnityEngine.Object) group, "Change mute state");
        if (allGroups.Any<AudioMixerGroupController>((Func<AudioMixerGroupController, bool>) (g => g.solo)))
          return false;
        using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.mute = false;
        }
        current.Use();
        return true;
      }
      Color color = GUI.color;
      bool flag1 = anySoloActive && group.mute;
      if (flag1)
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.5f);
      bool flag2 = GUI.Toggle(r, group.mute, this.styles.muteGUIContent, AudioMixerDrawUtils.styles.muteToggle);
      if (flag1)
        GUI.color = color;
      if (flag2 == group.mute)
        return false;
      Undo.RecordObject((UnityEngine.Object) group, "Change mute state");
      group.mute = !group.mute;
      using (List<AudioMixerGroupController>.Enumerator enumerator = selection.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.mute = group.mute;
      }
      return true;
    }

    private bool DoBypassEffectsButton(Rect r, AudioMixerGroupController group, List<AudioMixerGroupController> allGroups, List<AudioMixerGroupController> selection)
    {
      Event current = Event.current;
      if (current.type == EventType.MouseUp && current.button == 1 && r.Contains(current.mousePosition) && allGroups.Any<AudioMixerGroupController>((Func<AudioMixerGroupController, bool>) (g => g.bypassEffects)))
      {
        Undo.RecordObject((UnityEngine.Object) group, "Change bypass effects state");
        using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.bypassEffects = false;
        }
        current.Use();
        return true;
      }
      if (GUI.Toggle(r, group.bypassEffects, this.styles.bypassGUIContent, AudioMixerDrawUtils.styles.bypassToggle) == group.bypassEffects)
        return false;
      Undo.RecordObject((UnityEngine.Object) group, "Change bypass effects state");
      group.bypassEffects = !group.bypassEffects;
      using (List<AudioMixerGroupController>.Enumerator enumerator = selection.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.bypassEffects = group.bypassEffects;
      }
      return true;
    }

    private static bool RectOverlaps(Rect r1, Rect r2)
    {
      Rect rect = new Rect();
      rect.x = Mathf.Max(r1.x, r2.x);
      rect.y = Mathf.Max(r1.y, r2.y);
      rect.width = Mathf.Min(r1.x + r1.width, r2.x + r2.width) - rect.x;
      rect.height = Mathf.Min(r1.y + r1.height, r2.y + r2.height) - rect.y;
      if ((double) rect.width > 0.0)
        return (double) rect.height > 0.0;
      return false;
    }

    private bool IsRectSelectionActive()
    {
      return GUIUtility.hotControl == this.m_RectSelectionControlID;
    }

    private void GroupClicked(AudioMixerGroupController clickedGroup, AudioMixerChannelStripView.ChannelStripParams p, bool clickedControlInGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerChannelStripView.\u003CGroupClicked\u003Ec__AnonStorey60 clickedCAnonStorey60 = new AudioMixerChannelStripView.\u003CGroupClicked\u003Ec__AnonStorey60();
      List<int> allInstanceIDs = new List<int>();
      using (List<AudioMixerGroupController>.Enumerator enumerator = p.shownGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          allInstanceIDs.Add(current.GetInstanceID());
        }
      }
      List<int> selectedInstanceIDs = new List<int>();
      using (List<AudioMixerGroupController>.Enumerator enumerator = this.m_Controller.CachedSelection.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          selectedInstanceIDs.Add(current.GetInstanceID());
        }
      }
      int clickedInstanceId = this.m_State.m_LastClickedInstanceID;
      bool allowMultiSelection = true;
      bool keepMultiSelection = Event.current.shift || clickedControlInGroup;
      bool useShiftAsActionKey = false;
      // ISSUE: reference to a compiler-generated field
      clickedCAnonStorey60.newSelection = InternalEditorUtility.GetNewSelection(clickedGroup.GetInstanceID(), allInstanceIDs, selectedInstanceIDs, clickedInstanceId, keepMultiSelection, useShiftAsActionKey, allowMultiSelection);
      // ISSUE: reference to a compiler-generated method
      Selection.objects = (UnityEngine.Object[]) p.allGroups.Where<AudioMixerGroupController>(new Func<AudioMixerGroupController, bool>(clickedCAnonStorey60.\u003C\u003Em__A4)).ToList<AudioMixerGroupController>().ToArray();
      this.m_Controller.OnUnitySelectionChanged();
      InspectorWindow.RepaintAllInspectors();
    }

    private void DoAttenuationFader(Rect r, AudioMixerGroupController group, List<AudioMixerGroupController> selection, GUIStyle style)
    {
      float num1 = Mathf.Clamp(group.GetValueForVolume(this.m_Controller, this.m_Controller.TargetSnapshot), AudioMixerController.kMinVolume, AudioMixerController.GetMaxVolume());
      float num2 = this.VerticalFader(r, num1, 1, AudioMixerChannelStripView.kVolumeScaleMouseDrag, true, true, this.styles.attenuationFader.tooltip, AudioMixerController.GetMaxVolume(), style);
      if ((double) num1 == (double) num2)
        return;
      float num3 = num2 - num1;
      Undo.RecordObject((UnityEngine.Object) this.m_Controller.TargetSnapshot, "Change volume fader");
      using (List<AudioMixerGroupController>.Enumerator enumerator = selection.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          float num4 = Mathf.Clamp(current.GetValueForVolume(this.m_Controller, this.m_Controller.TargetSnapshot), AudioMixerController.kMinVolume, AudioMixerController.GetMaxVolume());
          current.SetValueForVolume(this.m_Controller, this.m_Controller.TargetSnapshot, Mathf.Clamp(num4 + num3, AudioMixerController.kMinVolume, AudioMixerController.GetMaxVolume()));
        }
      }
      InspectorWindow.RepaintAllInspectors();
    }

    internal static void AddEffectItemsToMenu(AudioMixerController controller, AudioMixerGroupController[] groups, int insertIndex, string prefix, GenericMenu pm)
    {
      string[] effectList = MixerEffectDefinitions.GetEffectList();
      for (int index = 0; index < effectList.Length; ++index)
      {
        if (effectList[index] != "Attenuation")
          pm.AddItem(new GUIContent(prefix + AudioMixerController.FixNameForPopupMenu(effectList[index])), false, new GenericMenu.MenuFunction2(AudioMixerChannelStripView.InsertEffectPopupCallback), (object) new AudioMixerChannelStripView.EffectContext(controller, groups, insertIndex, effectList[index]));
      }
    }

    private void DoEffectSlotInsertEffectPopup(Rect buttonRect, AudioMixerGroupController group, List<AudioMixerGroupController> allGroups, int effectSlotIndex, ref Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      GenericMenu pm = new GenericMenu();
      AudioMixerGroupController[] groups = new AudioMixerGroupController[1]{ group };
      if (effectSlotIndex < group.effects.Length)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AudioMixerChannelStripView.\u003CDoEffectSlotInsertEffectPopup\u003Ec__AnonStorey61 popupCAnonStorey61 = new AudioMixerChannelStripView.\u003CDoEffectSlotInsertEffectPopup\u003Ec__AnonStorey61();
        // ISSUE: reference to a compiler-generated field
        popupCAnonStorey61.\u003C\u003Ef__this = this;
        // ISSUE: reference to a compiler-generated field
        popupCAnonStorey61.effect = group.effects[effectSlotIndex];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!popupCAnonStorey61.effect.IsAttenuation() && !popupCAnonStorey61.effect.IsSend() && (!popupCAnonStorey61.effect.IsReceive() && !popupCAnonStorey61.effect.IsDuckVolume()))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          pm.AddItem(new GUIContent("Allow Wet Mixing (causes higher memory usage)"), popupCAnonStorey61.effect.enableWetMix, new GenericMenu.MenuFunction(popupCAnonStorey61.\u003C\u003Em__A5));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          pm.AddItem(new GUIContent("Bypass"), popupCAnonStorey61.effect.bypass, new GenericMenu.MenuFunction(popupCAnonStorey61.\u003C\u003Em__A6));
          pm.AddSeparator(string.Empty);
        }
        AudioMixerChannelStripView.AddEffectItemsToMenu(group.controller, groups, effectSlotIndex, "Add effect before/", pm);
        AudioMixerChannelStripView.AddEffectItemsToMenu(group.controller, groups, effectSlotIndex + 1, "Add effect after/", pm);
      }
      else
        AudioMixerChannelStripView.AddEffectItemsToMenu(group.controller, groups, effectSlotIndex, string.Empty, pm);
      if (effectSlotIndex < group.effects.Length)
      {
        AudioMixerEffectController effect = group.effects[effectSlotIndex];
        if (!effect.IsAttenuation())
        {
          pm.AddSeparator(string.Empty);
          pm.AddItem(new GUIContent("Remove"), false, new GenericMenu.MenuFunction2(this.RemoveEffectPopupCallback), (object) new AudioMixerChannelStripView.EffectContext(this.m_Controller, groups, effectSlotIndex, string.Empty));
          bool flag = false;
          if (effect.IsSend())
          {
            if ((UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) null)
            {
              if (!flag)
              {
                flag = true;
                pm.AddSeparator(string.Empty);
              }
              pm.AddItem(new GUIContent("Disconnect from '" + effect.GetSendTargetDisplayString(effectMap) + "'"), false, new GenericMenu.MenuFunction2(AudioMixerChannelStripView.ConnectSendPopupCallback), (object) new AudioMixerChannelStripView.ConnectSendContext(effect, (AudioMixerEffectController) null));
            }
            if (!flag)
              this.AddSeperatorIfAnyReturns(pm, allGroups);
            AudioMixerChannelStripView.AddMenuItemsForReturns(pm, "Connect to ", effectSlotIndex, group, allGroups, effectMap, effect, false);
          }
        }
      }
      pm.DropDown(buttonRect);
      Event.current.Use();
    }

    private void AddSeperatorIfAnyReturns(GenericMenu pm, List<AudioMixerGroupController> allGroups)
    {
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          foreach (AudioMixerEffectController effect in enumerator.Current.effects)
          {
            if (effect.IsReceive() || effect.IsDuckVolume())
            {
              pm.AddSeparator(string.Empty);
              return;
            }
          }
        }
      }
    }

    public static void AddMenuItemsForReturns(GenericMenu pm, string prefix, int effectIndex, AudioMixerGroupController group, List<AudioMixerGroupController> allGroups, Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap, AudioMixerEffectController effect, bool showCurrent)
    {
      using (List<AudioMixerGroupController>.Enumerator enumerator1 = allGroups.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          foreach (AudioMixerEffectController effect1 in enumerator1.Current.effects)
          {
            if (MixerEffectDefinitions.EffectCanBeSidechainTarget(effect1))
            {
              List<AudioMixerController.ConnectionNode> identifiedLoop = new List<AudioMixerController.ConnectionNode>();
              if (!AudioMixerController.WillChangeOfEffectTargetCauseFeedback(allGroups, group, effectIndex, effect1, identifiedLoop))
              {
                if (showCurrent || (UnityEngine.Object) effect.sendTarget != (UnityEngine.Object) effect1)
                  pm.AddItem(new GUIContent(prefix + "'" + effect1.GetDisplayString(effectMap) + "'"), (UnityEngine.Object) effect.sendTarget == (UnityEngine.Object) effect1, new GenericMenu.MenuFunction2(AudioMixerChannelStripView.ConnectSendPopupCallback), (object) new AudioMixerChannelStripView.ConnectSendContext(effect, effect1));
              }
              else
              {
                string str = "A connection to '" + AudioMixerController.FixNameForPopupMenu(effect1.GetDisplayString(effectMap)) + "' would result in a feedback loop/";
                pm.AddDisabledItem(new GUIContent(str + "Loop: "));
                int num = 1;
                using (List<AudioMixerController.ConnectionNode>.Enumerator enumerator2 = identifiedLoop.GetEnumerator())
                {
                  while (enumerator2.MoveNext())
                  {
                    AudioMixerController.ConnectionNode current = enumerator2.Current;
                    pm.AddDisabledItem(new GUIContent(str + (object) num + ": " + current.GetDisplayString() + "->"));
                    ++num;
                  }
                }
                pm.AddDisabledItem(new GUIContent(str + (object) num + ": ..."));
              }
            }
          }
        }
      }
    }

    public void VUMeter(AudioMixerGroupController group, Rect r, float level, float peak)
    {
      EditorGUI.VUMeter.VerticalMeter(r, level, peak, EditorGUI.VUMeter.verticalVUTexture, Color.grey);
    }

    private void DrawBackgrounds(AudioMixerChannelStripView.ChannelStripParams p, bool selected)
    {
      if (Event.current.type == EventType.Repaint)
      {
        this.styles.channelStripBg.Draw(p.stripRect, false, false, selected, false);
        float num1 = 0.4666667f;
        float num2 = 0.227451f;
        Color color = !EditorGUIUtility.isProSkin ? new Color(num1, num1, num1) : new Color(num2, num2, num2);
        Rect bgRect = p.bgRects[p.kEffectStartIndex];
        --bgRect.y;
        bgRect.height = 1f;
        EditorGUI.DrawRect(bgRect, color);
      }
      Rect bgRect1 = p.bgRects[p.kVUMeterFaderIndex];
      bgRect1.height = !EditorGUIUtility.isProSkin ? 2f : 1f;
      bgRect1.y -= bgRect1.height;
      int userColorIndex = p.group.userColorIndex;
      if (userColorIndex == 0)
        return;
      EditorGUI.DrawRect(bgRect1, AudioMixerColorCodes.GetColor(userColorIndex));
    }

    private void OpenGroupContextMenu(AudioMixerGroupController[] groups)
    {
      GenericMenu genericMenu = new GenericMenu();
      AudioMixerChannelStripView.AddEffectItemsToMenu(groups[0].controller, groups, 0, "Add effect at top/", genericMenu);
      AudioMixerChannelStripView.AddEffectItemsToMenu(groups[0].controller, groups, -1, "Add effect at bottom/", genericMenu);
      genericMenu.AddSeparator(string.Empty);
      AudioMixerColorCodes.AddColorItemsToGenericMenu(genericMenu, groups);
      genericMenu.AddSeparator(string.Empty);
      genericMenu.ShowAsContext();
    }

    private void DrawChannelStrip(AudioMixerChannelStripView.ChannelStripParams p, ref int highlightEffectIndex, ref Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot> patchslots, bool showBusConnectionsOfSelection)
    {
      Event current = Event.current;
      bool flag = current.type == EventType.MouseDown && p.stripRect.Contains(current.mousePosition);
      bool selected = this.m_Controller.CachedSelection.Contains(p.group);
      if (this.IsRectSelectionActive() && AudioMixerChannelStripView.RectOverlaps(p.stripRect, this.m_RectSelectionRect))
      {
        p.rectSelectionGroups.Add(p.group);
        selected = true;
      }
      this.DrawBackgrounds(p, selected);
      GUIContent headerGuiContent = this.headerGUIContent;
      string displayString = p.group.GetDisplayString();
      this.headerGUIContent.tooltip = displayString;
      string str = displayString;
      headerGuiContent.text = str;
      GUI.Label(p.bgRects[p.kHeaderIndex], this.headerGUIContent, AudioMixerDrawUtils.styles.channelStripHeaderStyle);
      Rect rect1 = new RectOffset(-6, 0, 0, -4).Add(p.bgRects[p.kVUMeterFaderIndex]);
      float num1 = 1f;
      float width1 = 54f;
      float width2 = rect1.width - width1 - num1;
      Rect vuRect = new Rect(rect1.x, rect1.y, width1, rect1.height);
      Rect rect2 = new Rect(vuRect.xMax + num1, rect1.y, width2, rect1.height);
      float width3 = 29f;
      Rect r = new Rect(rect2.x, rect2.y, width3, rect2.height);
      Rect bgRect = p.bgRects[p.kSoloMuteBypassIndex];
      GUIStyle attenuationMarkerSquare = AudioMixerDrawUtils.styles.channelStripAttenuationMarkerSquare;
      EditorGUI.BeginDisabledGroup(!AudioMixerController.EditingTargetSnapshot());
      double num2 = (double) this.DoVUMeters(vuRect, attenuationMarkerSquare.fixedHeight, p);
      this.DoAttenuationFader(r, p.group, this.m_Controller.CachedSelection, attenuationMarkerSquare);
      this.DoTotaldB(p);
      this.DoEffectList(p, selected, ref highlightEffectIndex, ref patchslots, showBusConnectionsOfSelection);
      EditorGUI.EndDisabledGroup();
      this.DoSoloMuteBypassButtons(bgRect, p.group, p.allGroups, this.m_Controller.CachedSelection, p.anySoloActive);
      if (flag && current.button == 0)
        this.GroupClicked(p.group, p, current.type == EventType.Used);
      if (current.type != EventType.ContextClick || !p.stripRect.Contains(current.mousePosition))
        return;
      current.Use();
      if (selected)
        this.OpenGroupContextMenu(this.m_Controller.CachedSelection.ToArray());
      else
        this.OpenGroupContextMenu(new AudioMixerGroupController[1]
        {
          p.group
        });
    }

    private void DoTotaldB(AudioMixerChannelStripView.ChannelStripParams p)
    {
      float num1 = 50f;
      this.styles.totalVULevel.padding.right = (int) (((double) p.stripRect.width - (double) num1) * 0.5);
      float num2 = Mathf.Max(p.vuinfo_level[8], -80f);
      GUI.Label(p.bgRects[p.kTotalVULevelIndex], string.Format("{0:F1} dB", (object) num2), this.styles.totalVULevel);
    }

    private void DoEffectList(AudioMixerChannelStripView.ChannelStripParams p, bool selected, ref int highlightEffectIndex, ref Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot> patchslots, bool showBusConnectionsOfSelection)
    {
      Event current = Event.current;
      for (int index = 0; index < p.maxEffects; ++index)
      {
        Rect bgRect = p.bgRects[p.kEffectStartIndex + index];
        if (index < p.group.effects.Length)
        {
          AudioMixerEffectController effect = p.group.effects[index];
          if (p.visible)
          {
            if (current.type == EventType.ContextClick && bgRect.Contains(Event.current.mousePosition))
            {
              this.ClearFocus();
              this.DoEffectSlotInsertEffectPopup(bgRect, p.group, p.allGroups, index, ref p.effectMap);
              current.Use();
            }
            this.EffectSlot(bgRect, this.m_Controller.TargetSnapshot, effect, index, ref highlightEffectIndex, p, ref patchslots);
          }
        }
      }
      if (!p.visible)
        return;
      Rect bgRect1 = p.bgRects[p.bgRects.Count - 1];
      if (current.type == EventType.Repaint)
      {
        GUI.DrawTextureWithTexCoords(new Rect(bgRect1.x, bgRect1.y, bgRect1.width, bgRect1.height - 1f), (Texture) this.styles.effectBar.hover.background, new Rect(0.0f, 0.5f, 0.1f, 0.1f));
        GUI.Label(bgRect1, this.addText, this.styles.effectName);
      }
      if (current.type != EventType.MouseDown || !bgRect1.Contains(Event.current.mousePosition))
        return;
      this.ClearFocus();
      int length = p.group.effects.Length;
      this.DoEffectSlotInsertEffectPopup(bgRect1, p.group, p.allGroups, length, ref p.effectMap);
      current.Use();
    }

    private float DoVUMeters(Rect vuRect, float attenuationMarkerHeight, AudioMixerChannelStripView.ChannelStripParams p)
    {
      float num1 = 1f;
      int num2 = p.numChannels;
      if (num2 == 0)
      {
        if (p.index >= 0 && p.index < this.m_LastNumChannels.Count)
          num2 = this.m_LastNumChannels[p.index];
      }
      else
      {
        while (p.index >= this.m_LastNumChannels.Count)
          this.m_LastNumChannels.Add(0);
        this.m_LastNumChannels[p.index] = num2;
      }
      if (num2 <= 2)
      {
        vuRect.x = vuRect.xMax - 25f;
        vuRect.width = 25f;
      }
      if (num2 == 0)
        return vuRect.x;
      float num3 = Mathf.Floor(attenuationMarkerHeight / 2f);
      vuRect.y += num3;
      vuRect.height -= 2f * num3;
      float width = Mathf.Round((vuRect.width - (float) num2 * num1) / (float) num2);
      Rect r = new Rect(vuRect.xMax - width, vuRect.y, width, vuRect.height);
      for (int index = num2 - 1; index >= 0; --index)
      {
        if (index != num2 - 1)
          r.x -= r.width + num1;
        float level = 1f - AudioMixerController.VolumeToScreenMapping(Mathf.Clamp(p.vuinfo_level[index], AudioMixerController.kMinVolume, AudioMixerController.GetMaxVolume()), 1f, true);
        float peak = 1f - AudioMixerController.VolumeToScreenMapping(Mathf.Clamp(p.vuinfo_peak[index], AudioMixerController.kMinVolume, AudioMixerController.GetMaxVolume()), 1f, true);
        this.VUMeter(p.group, r, level, peak);
      }
      AudioMixerDrawUtils.AddTooltipOverlay(vuRect, this.styles.vuMeterGUIContent.tooltip);
      return r.x;
    }

    private void DoSoloMuteBypassButtons(Rect rect, AudioMixerGroupController group, List<AudioMixerGroupController> allGroups, List<AudioMixerGroupController> selection, bool anySoloActive)
    {
      float num1 = 21f;
      float num2 = 2f;
      Rect r = new Rect(rect.x + (float) (((double) rect.width - ((double) num1 * 3.0 + (double) num2 * 2.0)) * 0.5), rect.y, num1, num1);
      bool flag = false | this.DoSoloButton(r, group, allGroups, selection);
      r.x += num1 + num2;
      if (flag | this.DoMuteButton(r, group, allGroups, anySoloActive, selection))
        this.m_Controller.UpdateMuteSolo();
      r.x += num1 + num2;
      if (!this.DoBypassEffectsButton(r, group, allGroups, selection))
        return;
      this.m_Controller.UpdateBypass();
    }

    public void OnMixerControllerChanged(AudioMixerController controller)
    {
      this.m_Controller = controller;
    }

    public void ShowDeveloperOverlays(Rect rect, Event evt, bool show)
    {
      if (!show || !Unsupported.IsDeveloperBuild() || evt.type != EventType.Repaint)
        return;
      AudioMixerDrawUtils.ReadOnlyLabel(new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, 20f), "Current snapshot: " + this.m_Controller.TargetSnapshot.name, this.developerInfoStyle);
      AudioMixerDrawUtils.ReadOnlyLabel(new Rect(rect.x + 5f, rect.y + 25f, rect.width - 10f, 20f), "Frame count: " + (object) this.FrameCounter++, this.developerInfoStyle);
    }

    public static float Lerp(float x1, float x2, float t)
    {
      return x1 + (x2 - x1) * t;
    }

    public static void GetCableVertex(float x1, float y1, float x2, float y2, float x3, float y3, float t, out float x, out float y)
    {
      x = AudioMixerChannelStripView.Lerp(AudioMixerChannelStripView.Lerp(x1, x2, t), AudioMixerChannelStripView.Lerp(x2, x3, t), t);
      y = AudioMixerChannelStripView.Lerp(AudioMixerChannelStripView.Lerp(y1, y2, t), AudioMixerChannelStripView.Lerp(y2, y3, t), t);
    }

    public void OnGUI(Rect rect, bool showReferencedBuses, bool showBusConnections, bool showBusConnectionsOfSelection, List<AudioMixerGroupController> allGroups, Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap, bool sortGroupsAlphabetically, bool showDeveloperOverlays, AudioMixerGroupController scrollToItem)
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
      {
        this.DrawAreaBackground(rect);
      }
      else
      {
        if (Event.current.type == EventType.Layout)
          return;
        this.m_RectSelectionControlID = GUIUtility.GetControlID(AudioMixerChannelStripView.kRectSelectionHashCode, FocusType.Passive);
        this.m_EffectInteractionControlID = GUIUtility.GetControlID(AudioMixerChannelStripView.kEffectDraggingHashCode, FocusType.Passive);
        this.m_IndexCounter = 0;
        Event current1 = Event.current;
        List<AudioMixerGroupController> list1 = ((IEnumerable<AudioMixerGroupController>) this.m_Controller.GetCurrentViewGroupList()).ToList<AudioMixerGroupController>();
        if (sortGroupsAlphabetically)
          list1.Sort((IComparer<AudioMixerGroupController>) this.m_GroupComparer);
        Rect rect1 = new Rect(this.channelStripsOffset.x, this.channelStripsOffset.y, 90f, 300f);
        if ((UnityEngine.Object) scrollToItem != (UnityEngine.Object) null)
        {
          int num1 = list1.IndexOf(scrollToItem);
          if (num1 >= 0)
          {
            float num2 = (rect1.width + 4f) * (float) num1 - this.m_State.m_ScrollPos.x;
            if ((double) num2 < -20.0 || (double) num2 > (double) rect.width)
              this.m_State.m_ScrollPos.x += num2;
          }
        }
        List<AudioMixerGroupController> source = new List<AudioMixerGroupController>();
        using (List<AudioMixerGroupController>.Enumerator enumerator = list1.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            foreach (AudioMixerEffectController effect1 in enumerator.Current.effects)
            {
              if ((UnityEngine.Object) effect1.sendTarget != (UnityEngine.Object) null)
              {
                AudioMixerGroupController effect2 = effectMap[effect1.sendTarget];
                if (!source.Contains(effect2) && !list1.Contains(effect2))
                  source.Add(effect2);
              }
            }
          }
        }
        List<AudioMixerGroupController> list2 = source.ToList<AudioMixerGroupController>();
        list2.Sort((IComparer<AudioMixerGroupController>) this.m_GroupComparer);
        int count = list1.Count;
        if (showReferencedBuses && list2.Count > 0)
          list1.AddRange((IEnumerable<AudioMixerGroupController>) list2);
        int num3 = 1;
        using (List<AudioMixerGroupController>.Enumerator enumerator = list1.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AudioMixerGroupController current2 = enumerator.Current;
            num3 = Mathf.Max(num3, current2.effects.Length);
          }
        }
        bool isShowingReferencedGroups = list1.Count != count;
        Rect contentRect = this.GetContentRect(list1, isShowingReferencedGroups, num3);
        this.m_State.m_ScrollPos = GUI.BeginScrollView(rect, this.m_State.m_ScrollPos, contentRect);
        this.DrawAreaBackground(new Rect(0.0f, 0.0f, 10000f, 10000f));
        AudioMixerChannelStripView.ChannelStripParams channelStripParams = new AudioMixerChannelStripView.ChannelStripParams() { effectMap = effectMap, allGroups = allGroups, shownGroups = list1, anySoloActive = allGroups.Any<AudioMixerGroupController>((Func<AudioMixerGroupController, bool>) (g => g.solo)), visibleRect = new Rect(this.m_State.m_ScrollPos.x, this.m_State.m_ScrollPos.y, rect.width, rect.height) };
        Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot> patchslots = !showBusConnections ? (Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot>) null : new Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot>();
        for (int index = 0; index < list1.Count; ++index)
        {
          AudioMixerGroupController mixerGroupController = list1[index];
          channelStripParams.index = index;
          channelStripParams.group = mixerGroupController;
          channelStripParams.drawingBuses = false;
          channelStripParams.visible = AudioMixerChannelStripView.RectOverlaps(channelStripParams.visibleRect, rect1);
          channelStripParams.Init(this.m_Controller, rect1, num3);
          this.DrawChannelStrip(channelStripParams, ref this.m_Controller.m_HighlightEffectIndex, ref patchslots, showBusConnectionsOfSelection);
          if (current1.type == EventType.MouseDown && current1.button == 0 && channelStripParams.stripRect.Contains(current1.mousePosition))
            current1.Use();
          if (this.IsMovingEffect() && current1.type == EventType.MouseDrag && (channelStripParams.stripRect.Contains(current1.mousePosition) && GUIUtility.hotControl == this.m_EffectInteractionControlID))
          {
            this.m_MovingEffectDstIndex = -1;
            current1.Use();
          }
          rect1.x += channelStripParams.stripRect.width + 4f;
          if (showReferencedBuses && index == count - 1 && list1.Count > count)
          {
            rect1.x += 50f;
            EditorGUI.BeginDisabledGroup(true);
            GUI.Label(new Rect(rect1.x, channelStripParams.stripRect.yMax, 130f, 22f), this.styles.referencedGroups, this.styles.channelStripHeaderStyle);
            EditorGUI.EndDisabledGroup();
          }
        }
        this.UnhandledEffectDraggingEvents(ref this.m_Controller.m_HighlightEffectIndex);
        if (current1.type == EventType.Repaint && patchslots != null)
        {
          using (Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot>.Enumerator enumerator = patchslots.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AudioMixerChannelStripView.PatchSlot patchSlot = enumerator.Current.Value;
              bool on = !showBusConnectionsOfSelection || this.m_Controller.CachedSelection.Contains(patchSlot.group);
              if (on)
                this.styles.circularToggle.Draw(new Rect(patchSlot.x - 3f, patchSlot.y - 3f, 6f, 6f), false, false, on, false);
            }
          }
          float num1 = Mathf.Exp(-0.03f * Time.time * Time.time) * 0.1f;
          Color color1 = new Color(0.0f, 0.0f, 0.0f, !AudioMixerController.EditingTargetSnapshot() ? 0.05f : 0.1f);
          Color color2 = new Color(0.0f, 0.0f, 0.0f, !AudioMixerController.EditingTargetSnapshot() ? 0.5f : 1f);
          using (Dictionary<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot>.Enumerator enumerator = patchslots.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<AudioMixerEffectController, AudioMixerChannelStripView.PatchSlot> current2 = enumerator.Current;
              AudioMixerEffectController key = current2.Key;
              AudioMixerEffectController sendTarget = key.sendTarget;
              if (!((UnityEngine.Object) sendTarget == (UnityEngine.Object) null))
              {
                AudioMixerChannelStripView.PatchSlot patchSlot1 = current2.Value;
                if (patchslots.ContainsKey(sendTarget))
                {
                  AudioMixerChannelStripView.PatchSlot patchSlot2 = patchslots[sendTarget];
                  int num2 = key.GetInstanceID() ^ sendTarget.GetInstanceID();
                  float num4 = (float) (num2 & 63) * 0.1f;
                  float x2 = (float) ((double) Mathf.Abs(patchSlot2.x - patchSlot1.x) * (double) Mathf.Sin(Time.time * 5f + num4) * (double) num1 + ((double) patchSlot1.x + (double) patchSlot2.x) * 0.5);
                  float y2 = Mathf.Abs(patchSlot2.y - patchSlot1.y) * Mathf.Cos(Time.time * 5f + num4) * num1 + Math.Max(patchSlot1.y, patchSlot2.y) + Mathf.Max(Mathf.Min(0.5f * Math.Abs(patchSlot2.y - patchSlot1.y), 50f), 50f);
                  for (int index = 0; index < this.cablepoints.Length; ++index)
                    AudioMixerChannelStripView.GetCableVertex(patchSlot1.x, patchSlot1.y, x2, y2, patchSlot2.x, patchSlot2.y, (float) index / (float) (this.cablepoints.Length - 1), out this.cablepoints[index].x, out this.cablepoints[index].y);
                  bool flag = showBusConnectionsOfSelection && !this.m_Controller.CachedSelection.Contains(patchSlot1.group) && !this.m_Controller.CachedSelection.Contains(patchSlot2.group);
                  Handles.color = !flag ? color2 : color1;
                  Handles.DrawAAPolyLine(7f, this.cablepoints.Length, this.cablepoints);
                  if (!flag)
                  {
                    int num5 = num2 ^ (num2 >> 6 ^ num2 >> 12 ^ num2 >> 18);
                    Handles.color = new Color((float) ((double) (num5 & 3) * 0.150000005960464 + 0.550000011920929), (float) ((double) (num5 >> 2 & 3) * 0.150000005960464 + 0.550000011920929), (float) ((double) (num5 >> 4 & 3) * 0.150000005960464 + 0.550000011920929), !AudioMixerController.EditingTargetSnapshot() ? 0.5f : 1f);
                    Handles.DrawAAPolyLine(4f, this.cablepoints.Length, this.cablepoints);
                    Handles.color = new Color(1f, 1f, 1f, !AudioMixerController.EditingTargetSnapshot() ? 0.25f : 0.5f);
                    Handles.DrawAAPolyLine(3f, this.cablepoints.Length, this.cablepoints);
                  }
                }
              }
            }
          }
        }
        this.RectSelection(channelStripParams);
        GUI.EndScrollView(true);
        AudioMixerDrawUtils.DrawScrollDropShadow(rect, this.m_State.m_ScrollPos.y, contentRect.height);
        this.WarningOverlay(allGroups, rect, contentRect);
        this.ShowDeveloperOverlays(rect, current1, showDeveloperOverlays);
        if (EditorApplication.isPlaying || this.m_Controller.isSuspended)
          return;
        this.m_RequiresRepaint = true;
      }
    }

    private bool IsMovingEffect()
    {
      return this.m_MovingEffectSrcIndex != -1;
    }

    private bool IsAdjustingWetMix()
    {
      return this.m_ChangingWetMixIndex != -1;
    }

    private void RectSelection(AudioMixerChannelStripView.ChannelStripParams channelStripParams)
    {
      Event current1 = Event.current;
      if (current1.type == EventType.MouseDown && current1.button == 0 && GUIUtility.hotControl == 0)
      {
        if (!current1.shift)
        {
          Selection.objects = new UnityEngine.Object[0];
          this.m_Controller.OnUnitySelectionChanged();
        }
        GUIUtility.hotControl = this.m_RectSelectionControlID;
        this.m_RectSelectionStartPos = current1.mousePosition;
        this.m_RectSelectionRect = new Rect(this.m_RectSelectionStartPos.x, this.m_RectSelectionStartPos.y, 0.0f, 0.0f);
        Event.current.Use();
        InspectorWindow.RepaintAllInspectors();
      }
      if (current1.type == EventType.MouseDrag)
      {
        if (this.IsRectSelectionActive())
        {
          this.m_RectSelectionRect.x = Mathf.Min(this.m_RectSelectionStartPos.x, current1.mousePosition.x);
          this.m_RectSelectionRect.y = Mathf.Min(this.m_RectSelectionStartPos.y, current1.mousePosition.y);
          this.m_RectSelectionRect.width = Mathf.Max(this.m_RectSelectionStartPos.x, current1.mousePosition.x) - this.m_RectSelectionRect.x;
          this.m_RectSelectionRect.height = Mathf.Max(this.m_RectSelectionStartPos.y, current1.mousePosition.y) - this.m_RectSelectionRect.y;
          Event.current.Use();
        }
        if ((double) this.m_MovingSrcRect.y >= 0.0)
          Event.current.Use();
      }
      if (this.IsRectSelectionActive() && current1.GetTypeForControl(this.m_RectSelectionControlID) == EventType.MouseUp)
      {
        List<AudioMixerGroupController> mixerGroupControllerList = !current1.shift ? new List<AudioMixerGroupController>() : this.m_Controller.CachedSelection;
        using (List<AudioMixerGroupController>.Enumerator enumerator = channelStripParams.rectSelectionGroups.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AudioMixerGroupController current2 = enumerator.Current;
            if (!mixerGroupControllerList.Contains(current2))
              mixerGroupControllerList.Add(current2);
          }
        }
        Selection.objects = (UnityEngine.Object[]) mixerGroupControllerList.ToArray();
        this.m_Controller.OnUnitySelectionChanged();
        GUIUtility.hotControl = 0;
        Event.current.Use();
      }
      if (current1.type != EventType.Repaint || !this.IsRectSelectionActive())
        return;
      Color color = new Color(1f, 1f, 1f, 0.3f);
      AudioMixerDrawUtils.DrawGradientRectHorizontal(this.m_RectSelectionRect, color, color);
    }

    private void WarningOverlay(List<AudioMixerGroupController> allGroups, Rect rect, Rect contentRect)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          if (current.solo)
            ++num1;
          if (current.mute)
            ++num2;
          if (current.bypassEffects)
            num3 += current.effects.Length - 1;
          else
            num3 += ((IEnumerable<AudioMixerEffectController>) current.effects).Count<AudioMixerEffectController>((Func<AudioMixerEffectController, bool>) (e => e.bypass));
        }
      }
      if ((Event.current.type != EventType.Repaint || num1 <= 0) && (num2 <= 0 && num3 <= 0))
        return;
      string t = "Warning: " + (num1 <= 0 ? (num2 <= 0 ? num3.ToString() + (num3 <= 1 ? (object) " effect" : (object) " effects") + " currently bypassed" : num2.ToString() + (num2 <= 1 ? (object) " group" : (object) " groups") + " currently muted") : num1.ToString() + (num1 <= 1 ? (object) " group" : (object) " groups") + " currently soloed");
      bool flag = (double) contentRect.width > (double) rect.width;
      float x = this.styles.warningOverlay.CalcSize(GUIContent.Temp(t)).x;
      GUI.Label(new Rect(rect.x + 5f + Mathf.Max((float) (((double) rect.width - 10.0 - (double) x) * 0.5), 0.0f), (float) ((double) rect.yMax - (double) this.styles.warningOverlay.fixedHeight - 5.0 - (!flag ? 0.0 : 17.0)), x, this.styles.warningOverlay.fixedHeight), GUIContent.Temp(t), this.styles.warningOverlay);
    }

    private Rect GetContentRect(List<AudioMixerGroupController> sortedGroups, bool isShowingReferencedGroups, int maxEffects)
    {
      float height = (float) ((double) this.channelStripsOffset.y + 239.0 + (double) maxEffects * 16.0 + 10.0 + 16.0 + 10.0);
      return new Rect(0.0f, 0.0f, (float) ((double) this.channelStripsOffset.x * 2.0 + 94.0 * (double) sortedGroups.Count + (!isShowingReferencedGroups ? 0.0 : 50.0)), height);
    }

    [Serializable]
    public class State
    {
      public Vector2 m_ScrollPos = new Vector2(0.0f, 0.0f);
      public int m_LastClickedInstanceID;
    }

    public class EffectContext
    {
      public AudioMixerController controller;
      public AudioMixerGroupController[] groups;
      public int index;
      public string name;

      public EffectContext(AudioMixerController controller, AudioMixerGroupController[] groups, int index, string name)
      {
        this.controller = controller;
        this.groups = groups;
        this.index = index;
        this.name = name;
      }
    }

    public class ConnectSendContext
    {
      public AudioMixerEffectController sendEffect;
      public AudioMixerEffectController targetEffect;

      public ConnectSendContext(AudioMixerEffectController sendEffect, AudioMixerEffectController targetEffect)
      {
        this.sendEffect = sendEffect;
        this.targetEffect = targetEffect;
      }
    }

    private class PatchSlot
    {
      public AudioMixerGroupController group;
      public float x;
      public float y;
    }

    private class BusConnection
    {
      public AudioMixerEffectController targetEffect;
      public float srcX;
      public float srcY;
      public float mixLevel;
      public Color color;
      public bool isSend;
      public bool isSelected;

      public BusConnection(float srcX, float srcY, AudioMixerEffectController targetEffect, float mixLevel, Color col, bool isSend, bool isSelected)
      {
        this.srcX = srcX;
        this.srcY = srcY;
        this.targetEffect = targetEffect;
        this.mixLevel = mixLevel;
        this.color = col;
        this.isSend = isSend;
        this.isSelected = isSelected;
      }
    }

    private class ChannelStripParams
    {
      public List<AudioMixerChannelStripView.BusConnection> busConnections = new List<AudioMixerChannelStripView.BusConnection>();
      public List<AudioMixerGroupController> rectSelectionGroups = new List<AudioMixerGroupController>();
      public float[] vuinfo_level = new float[9];
      public float[] vuinfo_peak = new float[9];
      public readonly int kVUMeterFaderIndex = 1;
      public readonly int kTotalVULevelIndex = 2;
      public readonly int kSoloMuteBypassIndex = 3;
      public readonly int kEffectStartIndex = 4;
      private const float kAddEffectButtonHeight = 16f;
      public int index;
      public Rect stripRect;
      public Rect visibleRect;
      public bool visible;
      public AudioMixerGroupController group;
      public int maxEffects;
      public bool drawingBuses;
      public bool anySoloActive;
      public List<AudioMixerGroupController> allGroups;
      public List<AudioMixerGroupController> shownGroups;
      public int numChannels;
      public Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap;
      public List<Rect> bgRects;
      public readonly int kHeaderIndex;

      public void Init(AudioMixerController controller, Rect channelStripRect, int maxNumEffects)
      {
        this.numChannels = controller.GetGroupVUInfo(this.group.groupID, false, ref this.vuinfo_level, ref this.vuinfo_peak);
        this.maxEffects = maxNumEffects;
        this.bgRects = this.GetBackgroundRects(channelStripRect, this.group, this.maxEffects);
        this.stripRect = channelStripRect;
        this.stripRect.yMax = this.bgRects[this.bgRects.Count - 1].yMax;
      }

      private List<Rect> GetBackgroundRects(Rect r, AudioMixerGroupController group, int maxNumGroups)
      {
        List<float> floatList = new List<float>();
        floatList.AddRange(Enumerable.Repeat<float>(0.0f, this.kEffectStartIndex));
        floatList[this.kHeaderIndex] = 22f;
        floatList[this.kVUMeterFaderIndex] = 170f;
        floatList[this.kTotalVULevelIndex] = 17f;
        floatList[this.kSoloMuteBypassIndex] = 30f;
        int num = maxNumGroups;
        for (int index = 0; index < num; ++index)
          floatList.Add(16f);
        floatList.Add(10f);
        List<Rect> rectList = new List<Rect>();
        float y1 = r.y;
        using (List<float>.Enumerator enumerator = floatList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = (int) enumerator.Current;
            if (rectList.Count > 0)
              y1 = y1;
            rectList.Add(new Rect(r.x, y1, r.width, (float) current));
            y1 += (float) current;
          }
        }
        float y2 = y1 + 10f;
        rectList.Add(new Rect(r.x, y2, r.width, 16f));
        return rectList;
      }
    }
  }
}
