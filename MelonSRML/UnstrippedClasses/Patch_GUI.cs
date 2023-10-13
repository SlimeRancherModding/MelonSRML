using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Il2CppInterop.Runtime;

namespace MelonSRML.UnstrippedClasses;

[HarmonyPatch(typeof(GUI))]
internal static class Patch_GUI
{
    private static int s_ScrollControlId;
    private static readonly int s_SliderHash            = "Slider".GetHashCode();
    private const float s_ScrollStepSize = 10f;
    private static readonly int s_ScrollviewHash        = "scrollView".GetHashCode();
    private static readonly int s_RepeatButtonHash      = "repeatButton".GetHashCode();
    
    internal static System.Func<DateTime> GetNextScrollStepTime = AccessTools.MethodDelegate<System.Func<DateTime>>(AccessTools.PropertyGetter(typeof(GUI), "nextScrollStepTime"));
    internal static System.Action<Il2CppSystem.DateTime> SetNextScrollStepTime = AccessTools.MethodDelegate<System.Action<Il2CppSystem.DateTime>>(AccessTools.PropertySetter(typeof(GUI), "nextScrollStepTime"));
    

    private static int scrollTroughSideFixed;
    private static Il2CppSystem.DateTime nextScrollStepTimeFixed;

    [HarmonyPatch("scrollTroughSide", MethodType.Getter), HarmonyPrefix]
    public static bool get_scrollTroughSidePatch(ref int __result)
    {
        __result = scrollTroughSideFixed;
        return false;
    }
    [HarmonyPatch("scrollTroughSide", MethodType.Setter), HarmonyPrefix]
    public static bool set_scrollTroughSidePatch(int value)
    {
        scrollTroughSideFixed = value;
        return false;

    }
    [HarmonyPatch(("nextScrollStepTime"), MethodType.Getter), HarmonyPrefix]
    public static bool get_nextScrollStepTimePatch(ref DateTime __result)
    {
        ref Il2CppSystem.DateTime nextScrollStepTimeFixedRef = ref nextScrollStepTimeFixed;
        DateTime dateTime = Unsafe.As<Il2CppSystem.DateTime, DateTime>(ref nextScrollStepTimeFixedRef);
        __result = dateTime;
        return false;
    }

    public static ConstructorInfo privateConstructor = AccessTools.Constructor(typeof(DateTime), new[]
    {
        typeof(ulong)
    });

    [HarmonyPatch("nextScrollStepTime", MethodType.Setter), HarmonyPrefix]
    public static bool set_nextScrollStepTimePatch(Il2CppSystem.DateTime value)
    {
        nextScrollStepTimeFixed = value;
        return false;

    }

    [HarmonyPatch("Slider", typeof(Rect), typeof(float), typeof(float), typeof(float), typeof(float), typeof(GUIStyle), typeof(GUIStyle), typeof(bool), typeof(int), typeof(GUIStyle)), HarmonyPrefix]
    public static bool SliderPatch(Rect position,
        float value,
        float size,
        float start,
        float end,
        GUIStyle slider,
        GUIStyle thumb,
        bool horiz,
        int id,
        GUIStyle thumbExtent, ref float __result)
    {
        GUIUtility.CheckOnGUI();
        if (id == 0)
        {
            id = GUIUtility.GetControlID(GUI.s_SliderHash, FocusType.Passive, position);
        }
        __result =  new SliderHandler(position, value, size, start, end, slider, thumb, horiz, id, thumbExtent).Handle();
        return false;
    }

    [HarmonyPatch("ScrollerRepeatButton", typeof(int), typeof(Rect), typeof(GUIStyle)), HarmonyPrefix]
    public static bool ScrollerRepeatButtonPatch(int scrollerID, Rect rect, GUIStyle style, ref bool __result)
    {
        bool hasChanged = false;
        if (GUI.DoRepeatButton(rect, GUIContent.none, style, FocusType.Passive))
        {
            bool firstClick = s_ScrollControlId != scrollerID;
            s_ScrollControlId = scrollerID;

            if (firstClick)
            {
                hasChanged = true;
                SetNextScrollStepTime(Il2CppSystem.DateTime.Now.AddMilliseconds(250)); }
            else
            {
                if (DateTime.Now >= GetNextScrollStepTime())
                {
                    hasChanged = true;
                    SetNextScrollStepTime(Il2CppSystem.DateTime.Now.AddMilliseconds(250)); 
                }
            }

            if (Event.current.type == EventType.Repaint)
                GUI.InternalRepaintEditorWindow();
        }
        __result =  hasChanged;
        return false;
    }

    [HarmonyPatch("Scroller", typeof(Rect), typeof(float), typeof(float), typeof(float), typeof(float),
        typeof(GUIStyle), typeof(GUIStyle), typeof(GUIStyle), typeof(GUIStyle), typeof(bool)), HarmonyPrefix]
    public static bool ScrollerPatch(ref float __result, float value, Rect position, float size, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, GUIStyle leftButton, GUIStyle rightButton, bool horiz)
    {
         GUIUtility.CheckOnGUI();
         int id = GUIUtility.GetControlID(s_SliderHash, FocusType.Passive, position);
         Rect sliderRect, minRect, maxRect;
         if (horiz)
         {
             sliderRect = new Rect(
                 position.x + leftButton.fixedWidth, position.y,
                 position.width - leftButton.fixedWidth - rightButton.fixedWidth, position.height
             );
             minRect = new Rect(position.x, position.y, leftButton.fixedWidth, position.height);
             maxRect = new Rect(position.xMax - rightButton.fixedWidth, position.y, rightButton.fixedWidth, position.height);
         }
         else
         {
             sliderRect = new Rect(
                 position.x, position.y + leftButton.fixedHeight,
                 position.width, position.height - leftButton.fixedHeight - rightButton.fixedHeight
             );
             minRect = new Rect(position.x, position.y, position.width, leftButton.fixedHeight);
             maxRect = new Rect(position.x, position.yMax - rightButton.fixedHeight, position.width, rightButton.fixedHeight);
         }

         value = GUI.Slider(sliderRect, value, size, leftValue, rightValue, slider, thumb, horiz, id);

         bool wasMouseUpEvent = Event.current.type == EventType.MouseUp;

         if (GUI.ScrollerRepeatButton(id, minRect, leftButton))
             value -= s_ScrollStepSize * (leftValue < rightValue ? 1f : -1f);

         if (GUI.ScrollerRepeatButton(id, maxRect, rightButton))
             value += s_ScrollStepSize * (leftValue < rightValue ? 1f : -1f);

         if (wasMouseUpEvent && Event.current.type == EventType.Used) // repeat buttons ate mouse up event - release scrolling
             s_ScrollControlId = 0;

         if (leftValue < rightValue)
             value = Mathf.Clamp(value, leftValue, rightValue - size);
         else
             value = Mathf.Clamp(value, rightValue, leftValue - size);
         __result = value;
         return false;
    }

    [HarmonyPatch("BeginScrollView", typeof(Rect), typeof(Vector2), typeof(Rect), typeof(bool), typeof(bool),
         typeof(GUIStyle), typeof(GUIStyle), typeof(GUIStyle)), HarmonyPrefix]
    public static bool BeginScrollViewPatch(ref Vector2 __result, Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background)
    { 
        GUIUtility.CheckOnGUI();
        if (Event.current.type == EventType.DragUpdated && position.Contains(Event.current.mousePosition))
        {
            if (Mathf.Abs(Event.current.mousePosition.y - position.y) < 8)
            {
                scrollPosition.y -= 16;
                GUI.InternalRepaintEditorWindow();
            }
            else if (Mathf.Abs(Event.current.mousePosition.y - position.yMax) < 8)
            {
                scrollPosition.y += 16;
                GUI.InternalRepaintEditorWindow();
            }
        }

        int id = GUIUtility.GetControlID(s_ScrollviewHash, FocusType.Passive);
        ScrollViewStateModified state = GUIUtility.GetStateObject(Il2CppType.Of<ScrollViewStateModified>(), id).Cast<ScrollViewStateModified>();

        if (state.apply)
        {
            scrollPosition = state.scrollPosition;
            state.apply = false;
        }
        state.position = position;
        state.scrollPosition = scrollPosition;
        state.visibleRect = state.viewRect = viewRect;
        state.visibleRect.width = position.width;
        state.visibleRect.height = position.height; 
        GUI.scrollViewStates.Push(state.Cast<Il2CppSystem.Object>());

        Rect clipRect = new Rect(position.x, position.y, position.width, position.height);
        switch (Event.current.type)
        {
            case EventType.Layout:
                GUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                GUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                break;
            case EventType.Used:
                break;
            default:
                bool needsVertical = alwaysShowVertical, needsHorizontal = alwaysShowHorizontal;

                // Check if we need a horizontal scrollbar
                if (needsHorizontal || viewRect.width > clipRect.width)
                {
                    state.visibleRect.height = position.height - horizontalScrollbar.fixedHeight + horizontalScrollbar.margin.top;
                    clipRect.height -= horizontalScrollbar.fixedHeight + horizontalScrollbar.margin.top;
                    needsHorizontal = true;
                }
                if (needsVertical || viewRect.height > clipRect.height)
                {
                    state.visibleRect.width = position.width - verticalScrollbar.fixedWidth + verticalScrollbar.margin.left;
                    clipRect.width -= verticalScrollbar.fixedWidth + verticalScrollbar.margin.left;
                    needsVertical = true;
                    if (!needsHorizontal && viewRect.width > clipRect.width)
                    {
                        state.visibleRect.height = position.height - horizontalScrollbar.fixedHeight + horizontalScrollbar.margin.top;
                        clipRect.height -= horizontalScrollbar.fixedHeight + horizontalScrollbar.margin.top;
                        needsHorizontal = true;
                    }
                }

                if (Event.current.type == EventType.Repaint && background != GUIStyle.none)
                {
                    background.Draw(position, position.Contains(Event.current.mousePosition), false, needsHorizontal && needsVertical, false);
                }
                if (needsHorizontal && horizontalScrollbar != GUIStyle.none)
                {
                    scrollPosition.x = GUI.HorizontalScrollbar(new Rect(position.x, position.yMax - horizontalScrollbar.fixedHeight, clipRect.width, horizontalScrollbar.fixedHeight),
                        scrollPosition.x, Mathf.Min(clipRect.width, viewRect.width), 0, viewRect.width,
                        horizontalScrollbar);
                }
                else
                {
                    GUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                    GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    scrollPosition.x = horizontalScrollbar != GUIStyle.none ? 0 : Mathf.Clamp(scrollPosition.x, 0, Mathf.Max(viewRect.width - position.width, 0));
                }

                if (needsVertical && verticalScrollbar != GUIStyle.none)
                {
                    scrollPosition.y = GUI.VerticalScrollbar(new Rect(clipRect.xMax + verticalScrollbar.margin.left, clipRect.y, verticalScrollbar.fixedWidth, clipRect.height),
                        scrollPosition.y, Mathf.Min(clipRect.height, viewRect.height), 0, viewRect.height,
                        verticalScrollbar);
                }
                else
                {
                    GUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                    GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    GUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    scrollPosition.y = verticalScrollbar != GUIStyle.none ? 0 : Mathf.Clamp(scrollPosition.y, 0, Mathf.Max(viewRect.height - position.height, 0));
                }
                break;
        }
        GUIClip.Push(clipRect, new Vector2(Mathf.Round(-scrollPosition.x - viewRect.x), Mathf.Round(-scrollPosition.y - viewRect.y)), Vector2.zero, false);
        __result =  scrollPosition;
        return false;
    }

    [HarmonyPatch("EndScrollView", typeof(bool)), HarmonyPrefix]
    public static bool EndScrollViewPatch(bool handleScrollWheel)
    {
          GUIUtility.CheckOnGUI();
          if (GUI.scrollViewStates.Count == 0)
                return false;
          ScrollViewStateModified state = GUI.scrollViewStates.Peek().Cast<ScrollViewStateModified>();

          GUIClip.Pop();

          GUI.scrollViewStates.Pop();

          bool needApply = false;

          float deltaTime = Time.realtimeSinceStartup - state.previousTimeSinceStartup;
          state.previousTimeSinceStartup = Time.realtimeSinceStartup;
          // If touch scroll, then handle inertia
          if (Event.current.type == EventType.Repaint && state.velocity != Vector2.zero)
          {
              for (int axis = 0; axis < 2; axis++)
              {
                  state.velocity[axis] *= Mathf.Pow(0.1f, deltaTime); // Decrease in a timely fashion (~/10 per second)
                  float velocityToSubstract = 0.1f / deltaTime;
                  if (Mathf.Abs(state.velocity[axis]) < velocityToSubstract)
                      state.velocity[axis] = 0;
                  else
                  {
                      state.velocity[axis] += state.velocity[axis] < 0 ? velocityToSubstract : -velocityToSubstract; // Substract directly to stop it faster on low velocity
                      state.scrollPosition[axis] += state.velocity[axis] * deltaTime;

                      needApply = true;
                        // Reset the scrolling start info so that dragging works fine after inertia
                      state.touchScrollStartMousePosition = Event.current.mousePosition;
                      state.touchScrollStartPosition = state.scrollPosition;
                  }
              }

              if (state.velocity != Vector2.zero)
                  GUI.InternalRepaintEditorWindow(); // Repaint to smooth the scroll
          }

          // This is the mac way of handling things: if the mouse is over a scrollview, the scrollview gets the event.
          if (handleScrollWheel &&
              (Event.current.type == EventType.ScrollWheel
               || Event.current.type == EventType.TouchDown
               || Event.current.type == EventType.TouchUp
               || Event.current.type == EventType.TouchMove)
              // avoid eating scroll events if a scroll view is not necessary
              && (state.viewRect.width > state.visibleRect.width || state.viewRect.height > state.visibleRect.height)
             )
          {
              // Using scrollwheel
              if (Event.current.type == EventType.ScrollWheel
                  // avoid eating scroll events if a scroll view is not necessary
                  && ((state.viewRect.width > state.visibleRect.width && !Mathf.Approximately(0f, Event.current.delta.x))
                      || (state.viewRect.height > state.visibleRect.height && !Mathf.Approximately(0f, Event.current.delta.y)))
                  && state.position.Contains(Event.current.mousePosition)
                 )
              {
                  state.scrollPosition.x = Mathf.Clamp(state.scrollPosition.x + (Event.current.delta.x * 20f), 0f, state.viewRect.width - state.visibleRect.width);
                  state.scrollPosition.y = Mathf.Clamp(state.scrollPosition.y + (Event.current.delta.y * 20f), 0f, state.viewRect.height - state.visibleRect.height);
                  Event.current.Use();

                  needApply = true;
              }
              // Using touch
              else if (Event.current.type == EventType.TouchDown && (Event.current.modifiers & EventModifiers.Alt) == EventModifiers.Alt && state.position.Contains(Event.current.mousePosition))
              {
                  state.isDuringTouchScroll = true;
                  state.touchScrollStartMousePosition = Event.current.mousePosition;
                  state.touchScrollStartPosition = state.scrollPosition;

                  GUIUtility.hotControl = GUIUtility.GetControlID(s_ScrollviewHash, FocusType.Passive, state.position);;
                  Event.current.Use();
              }
              else if (state.isDuringTouchScroll && Event.current.type == EventType.TouchUp)
                  state.isDuringTouchScroll = false;
              else if (state.isDuringTouchScroll && Event.current.type == EventType.TouchMove)
              {
                  Vector2 previousPosition = state.scrollPosition;

                  state.scrollPosition.x = Mathf.Clamp(state.touchScrollStartPosition.x - (Event.current.mousePosition.x - state.touchScrollStartMousePosition.x), 0f, state.viewRect.width - state.visibleRect.width);
                  state.scrollPosition.y = Mathf.Clamp(state.touchScrollStartPosition.y - (Event.current.mousePosition.y - state.touchScrollStartMousePosition.y), 0f, state.viewRect.height - state.visibleRect.height);
                  Event.current.Use();

                  // Sets the new volicity
                  Vector2 newVelocity = (state.scrollPosition - previousPosition) / deltaTime;
                  state.velocity = Vector2.Lerp(state.velocity, newVelocity, deltaTime * 10);

                  needApply = true;
              }
          }
          if (needApply)
          {
              // If one of the visible rect dimensions is larger than the view rect dimensions
              if (state.scrollPosition.x < 0f)
                  state.scrollPosition.x = 0f;
              if (state.scrollPosition.y < 0f)
                  state.scrollPosition.y = 0f;
              state.apply = true;
          }
          
          return false;
    }
}