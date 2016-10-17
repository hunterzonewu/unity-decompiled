// Decompiled with JetBrains decompiler
// Type: UnityEngine.Event
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A UnityGUI event.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class Event
  {
    [NonSerialized]
    internal IntPtr m_Ptr;
    private static Event s_Current;
    private static Event s_MasterEvent;

    /// <summary>
    ///   <para>The mouse position.</para>
    /// </summary>
    public Vector2 mousePosition
    {
      get
      {
        Vector2 vector2;
        this.Internal_GetMousePosition(out vector2);
        return vector2;
      }
      set
      {
        this.Internal_SetMousePosition(value);
      }
    }

    /// <summary>
    ///   <para>The relative movement of the mouse compared to last event.</para>
    /// </summary>
    public Vector2 delta
    {
      get
      {
        Vector2 vector2;
        this.Internal_GetMouseDelta(out vector2);
        return vector2;
      }
      set
      {
        this.Internal_SetMouseDelta(value);
      }
    }

    [Obsolete("Use HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);", true)]
    public Ray mouseRay
    {
      get
      {
        return new Ray(Vector3.up, Vector3.up);
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Is Shift held down? (Read Only)</para>
    /// </summary>
    public bool shift
    {
      get
      {
        return (this.modifiers & EventModifiers.Shift) != EventModifiers.None;
      }
      set
      {
        if (!value)
          this.modifiers &= ~EventModifiers.Shift;
        else
          this.modifiers |= EventModifiers.Shift;
      }
    }

    /// <summary>
    ///   <para>Is Control key held down? (Read Only)</para>
    /// </summary>
    public bool control
    {
      get
      {
        return (this.modifiers & EventModifiers.Control) != EventModifiers.None;
      }
      set
      {
        if (!value)
          this.modifiers &= ~EventModifiers.Control;
        else
          this.modifiers |= EventModifiers.Control;
      }
    }

    /// <summary>
    ///   <para>Is Alt/Option key held down? (Read Only)</para>
    /// </summary>
    public bool alt
    {
      get
      {
        return (this.modifiers & EventModifiers.Alt) != EventModifiers.None;
      }
      set
      {
        if (!value)
          this.modifiers &= ~EventModifiers.Alt;
        else
          this.modifiers |= EventModifiers.Alt;
      }
    }

    /// <summary>
    ///   <para>Is Command/Windows key held down? (Read Only)</para>
    /// </summary>
    public bool command
    {
      get
      {
        return (this.modifiers & EventModifiers.Command) != EventModifiers.None;
      }
      set
      {
        if (!value)
          this.modifiers &= ~EventModifiers.Command;
        else
          this.modifiers |= EventModifiers.Command;
      }
    }

    /// <summary>
    ///   <para>Is Caps Lock on? (Read Only)</para>
    /// </summary>
    public bool capsLock
    {
      get
      {
        return (this.modifiers & EventModifiers.CapsLock) != EventModifiers.None;
      }
      set
      {
        if (!value)
          this.modifiers &= ~EventModifiers.CapsLock;
        else
          this.modifiers |= EventModifiers.CapsLock;
      }
    }

    /// <summary>
    ///   <para>Is the current keypress on the numeric keyboard? (Read Only)</para>
    /// </summary>
    public bool numeric
    {
      get
      {
        return (this.modifiers & EventModifiers.Numeric) != EventModifiers.None;
      }
      set
      {
        if (!value)
          this.modifiers &= ~EventModifiers.Shift;
        else
          this.modifiers |= EventModifiers.Shift;
      }
    }

    /// <summary>
    ///   <para>Is the current keypress a function key? (Read Only)</para>
    /// </summary>
    public bool functionKey
    {
      get
      {
        return (this.modifiers & EventModifiers.FunctionKey) != EventModifiers.None;
      }
    }

    /// <summary>
    ///   <para>The current event that's being processed right now.</para>
    /// </summary>
    public static Event current
    {
      get
      {
        if (GUIUtility.Internal_GetGUIDepth() > 0)
          return Event.s_Current;
        return (Event) null;
      }
      set
      {
        Event.s_Current = value == null ? Event.s_MasterEvent : value;
        Event.Internal_SetNativeEvent(Event.s_Current.m_Ptr);
      }
    }

    /// <summary>
    ///   <para>Is this event a keyboard event? (Read Only)</para>
    /// </summary>
    public bool isKey
    {
      get
      {
        EventType type = this.type;
        if (type != EventType.KeyDown)
          return type == EventType.KeyUp;
        return true;
      }
    }

    /// <summary>
    ///   <para>Is this event a mouse event? (Read Only)</para>
    /// </summary>
    public bool isMouse
    {
      get
      {
        EventType type = this.type;
        switch (type)
        {
          case EventType.MouseMove:
          case EventType.MouseDown:
          case EventType.MouseUp:
            return true;
          default:
            return type == EventType.MouseDrag;
        }
      }
    }

    public EventType rawType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The type of event.</para>
    /// </summary>
    public EventType type { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which mouse button was pressed.</para>
    /// </summary>
    public int button { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which modifier keys are held down.</para>
    /// </summary>
    public EventModifiers modifiers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public float pressure { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How many consecutive mouse clicks have we received.</para>
    /// </summary>
    public int clickCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The character typed.</para>
    /// </summary>
    public char character { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The name of an ExecuteCommand or ValidateCommand Event.</para>
    /// </summary>
    public string commandName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The raw key code for keyboard events.</para>
    /// </summary>
    public KeyCode keyCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Index of display that the event belongs to.</para>
    /// </summary>
    public int displayIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Event()
    {
      this.Init(0);
    }

    public Event(int displayIndex)
    {
      this.Init(displayIndex);
    }

    public Event(Event other)
    {
      if (other == null)
        throw new ArgumentException("Event to copy from is null.");
      this.InitCopy(other);
    }

    private Event(IntPtr ptr)
    {
      this.InitPtr(ptr);
    }

    ~Event()
    {
      this.Cleanup();
    }

    [RequiredByNativeCode]
    private static void Internal_MakeMasterEventCurrent(int displayIndex)
    {
      if (Event.s_MasterEvent == null)
        Event.s_MasterEvent = new Event(displayIndex);
      Event.s_MasterEvent.displayIndex = displayIndex;
      Event.s_Current = Event.s_MasterEvent;
      Event.Internal_SetNativeEvent(Event.s_MasterEvent.m_Ptr);
    }

    /// <summary>
    ///   <para>Create a keyboard event.</para>
    /// </summary>
    /// <param name="key"></param>
    public static Event KeyboardEvent(string key)
    {
      Event @event = new Event(0);
      @event.type = EventType.KeyDown;
      if (string.IsNullOrEmpty(key))
        return @event;
      int startIndex = 0;
      bool flag1 = false;
      bool flag2;
      do
      {
        flag2 = true;
        if (startIndex >= key.Length)
        {
          flag1 = false;
          break;
        }
        char ch = key[startIndex];
        switch (ch)
        {
          case '#':
            @event.modifiers |= EventModifiers.Shift;
            ++startIndex;
            break;
          case '%':
            @event.modifiers |= EventModifiers.Command;
            ++startIndex;
            break;
          case '&':
            @event.modifiers |= EventModifiers.Alt;
            ++startIndex;
            break;
          default:
            if ((int) ch == 94)
            {
              @event.modifiers |= EventModifiers.Control;
              ++startIndex;
              break;
            }
            flag2 = false;
            break;
        }
      }
      while (flag2);
      string lower = key.Substring(startIndex, key.Length - startIndex).ToLower();
      string key1 = lower;
      if (key1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Event.\u003C\u003Ef__switch\u0024map0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Event.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(49)
          {
            {
              "[0]",
              0
            },
            {
              "[1]",
              1
            },
            {
              "[2]",
              2
            },
            {
              "[3]",
              3
            },
            {
              "[4]",
              4
            },
            {
              "[5]",
              5
            },
            {
              "[6]",
              6
            },
            {
              "[7]",
              7
            },
            {
              "[8]",
              8
            },
            {
              "[9]",
              9
            },
            {
              "[.]",
              10
            },
            {
              "[/]",
              11
            },
            {
              "[-]",
              12
            },
            {
              "[+]",
              13
            },
            {
              "[=]",
              14
            },
            {
              "[equals]",
              15
            },
            {
              "[enter]",
              16
            },
            {
              "up",
              17
            },
            {
              "down",
              18
            },
            {
              "left",
              19
            },
            {
              "right",
              20
            },
            {
              "insert",
              21
            },
            {
              "home",
              22
            },
            {
              "end",
              23
            },
            {
              "pgup",
              24
            },
            {
              "page up",
              25
            },
            {
              "pgdown",
              26
            },
            {
              "page down",
              27
            },
            {
              "backspace",
              28
            },
            {
              "delete",
              29
            },
            {
              "tab",
              30
            },
            {
              "f1",
              31
            },
            {
              "f2",
              32
            },
            {
              "f3",
              33
            },
            {
              "f4",
              34
            },
            {
              "f5",
              35
            },
            {
              "f6",
              36
            },
            {
              "f7",
              37
            },
            {
              "f8",
              38
            },
            {
              "f9",
              39
            },
            {
              "f10",
              40
            },
            {
              "f11",
              41
            },
            {
              "f12",
              42
            },
            {
              "f13",
              43
            },
            {
              "f14",
              44
            },
            {
              "f15",
              45
            },
            {
              "[esc]",
              46
            },
            {
              "return",
              47
            },
            {
              "space",
              48
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (Event.\u003C\u003Ef__switch\u0024map0.TryGetValue(key1, out num))
        {
          switch (num)
          {
            case 0:
              @event.character = '0';
              @event.keyCode = KeyCode.Keypad0;
              goto label_72;
            case 1:
              @event.character = '1';
              @event.keyCode = KeyCode.Keypad1;
              goto label_72;
            case 2:
              @event.character = '2';
              @event.keyCode = KeyCode.Keypad2;
              goto label_72;
            case 3:
              @event.character = '3';
              @event.keyCode = KeyCode.Keypad3;
              goto label_72;
            case 4:
              @event.character = '4';
              @event.keyCode = KeyCode.Keypad4;
              goto label_72;
            case 5:
              @event.character = '5';
              @event.keyCode = KeyCode.Keypad5;
              goto label_72;
            case 6:
              @event.character = '6';
              @event.keyCode = KeyCode.Keypad6;
              goto label_72;
            case 7:
              @event.character = '7';
              @event.keyCode = KeyCode.Keypad7;
              goto label_72;
            case 8:
              @event.character = '8';
              @event.keyCode = KeyCode.Keypad8;
              goto label_72;
            case 9:
              @event.character = '9';
              @event.keyCode = KeyCode.Keypad9;
              goto label_72;
            case 10:
              @event.character = '.';
              @event.keyCode = KeyCode.KeypadPeriod;
              goto label_72;
            case 11:
              @event.character = '/';
              @event.keyCode = KeyCode.KeypadDivide;
              goto label_72;
            case 12:
              @event.character = '-';
              @event.keyCode = KeyCode.KeypadMinus;
              goto label_72;
            case 13:
              @event.character = '+';
              @event.keyCode = KeyCode.KeypadPlus;
              goto label_72;
            case 14:
              @event.character = '=';
              @event.keyCode = KeyCode.KeypadEquals;
              goto label_72;
            case 15:
              @event.character = '=';
              @event.keyCode = KeyCode.KeypadEquals;
              goto label_72;
            case 16:
              @event.character = '\n';
              @event.keyCode = KeyCode.KeypadEnter;
              goto label_72;
            case 17:
              @event.keyCode = KeyCode.UpArrow;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 18:
              @event.keyCode = KeyCode.DownArrow;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 19:
              @event.keyCode = KeyCode.LeftArrow;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 20:
              @event.keyCode = KeyCode.RightArrow;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 21:
              @event.keyCode = KeyCode.Insert;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 22:
              @event.keyCode = KeyCode.Home;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 23:
              @event.keyCode = KeyCode.End;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 24:
              @event.keyCode = KeyCode.PageDown;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 25:
              @event.keyCode = KeyCode.PageUp;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 26:
              @event.keyCode = KeyCode.PageUp;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 27:
              @event.keyCode = KeyCode.PageDown;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 28:
              @event.keyCode = KeyCode.Backspace;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 29:
              @event.keyCode = KeyCode.Delete;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 30:
              @event.keyCode = KeyCode.Tab;
              goto label_72;
            case 31:
              @event.keyCode = KeyCode.F1;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 32:
              @event.keyCode = KeyCode.F2;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 33:
              @event.keyCode = KeyCode.F3;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 34:
              @event.keyCode = KeyCode.F4;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 35:
              @event.keyCode = KeyCode.F5;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 36:
              @event.keyCode = KeyCode.F6;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 37:
              @event.keyCode = KeyCode.F7;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 38:
              @event.keyCode = KeyCode.F8;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 39:
              @event.keyCode = KeyCode.F9;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 40:
              @event.keyCode = KeyCode.F10;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 41:
              @event.keyCode = KeyCode.F11;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 42:
              @event.keyCode = KeyCode.F12;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 43:
              @event.keyCode = KeyCode.F13;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 44:
              @event.keyCode = KeyCode.F14;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 45:
              @event.keyCode = KeyCode.F15;
              @event.modifiers |= EventModifiers.FunctionKey;
              goto label_72;
            case 46:
              @event.keyCode = KeyCode.Escape;
              goto label_72;
            case 47:
              @event.character = '\n';
              @event.keyCode = KeyCode.Return;
              @event.modifiers &= ~EventModifiers.FunctionKey;
              goto label_72;
            case 48:
              @event.keyCode = KeyCode.Space;
              @event.character = ' ';
              @event.modifiers &= ~EventModifiers.FunctionKey;
              goto label_72;
          }
        }
      }
      if (lower.Length != 1)
      {
        try
        {
          @event.keyCode = (KeyCode) Enum.Parse(typeof (KeyCode), lower, true);
        }
        catch (ArgumentException ex)
        {
          Debug.LogError((object) UnityString.Format("Unable to find key name that matches '{0}'", (object) lower));
        }
      }
      else
      {
        @event.character = lower.ToLower()[0];
        @event.keyCode = (KeyCode) @event.character;
        if (@event.modifiers != EventModifiers.None)
          @event.character = char.MinValue;
      }
label_72:
      return @event;
    }

    public override int GetHashCode()
    {
      int num = 1;
      if (this.isKey)
        num = (int) (ushort) this.keyCode;
      if (this.isMouse)
        num = this.mousePosition.GetHashCode();
      return (int) ((EventModifiers) (num * 37) | this.modifiers);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      if (obj.GetType() != this.GetType())
        return false;
      Event @event = (Event) obj;
      if (this.type != @event.type || (this.modifiers & ~EventModifiers.CapsLock) != (@event.modifiers & ~EventModifiers.CapsLock))
        return false;
      if (this.isKey)
        return this.keyCode == @event.keyCode;
      if (this.isMouse)
        return this.mousePosition == @event.mousePosition;
      return false;
    }

    public override string ToString()
    {
      if (this.isKey)
      {
        if ((int) this.character == 0)
          return UnityString.Format("Event:{0}   Character:\\0   Modifiers:{1}   KeyCode:{2}", (object) this.type, (object) this.modifiers, (object) this.keyCode);
        return "Event:" + (object) this.type + "   Character:" + (object) (int) this.character + "   Modifiers:" + (object) this.modifiers + "   KeyCode:" + (object) this.keyCode;
      }
      if (this.isMouse)
        return UnityString.Format("Event: {0}   Position: {1} Modifiers: {2}", (object) this.type, (object) this.mousePosition, (object) this.modifiers);
      if (this.type != EventType.ExecuteCommand && this.type != EventType.ValidateCommand)
        return string.Empty + (object) this.type;
      return UnityString.Format("Event: {0}  \"{1}\"", (object) this.type, (object) this.commandName);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init(int displayIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Cleanup();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InitCopy(Event other);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InitPtr(IntPtr ptr);

    /// <summary>
    ///   <para>Get a filtered event type for a given control ID.</para>
    /// </summary>
    /// <param name="controlID">The ID of the control you are querying from.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public EventType GetTypeForControl(int controlID);

    private void Internal_SetMousePosition(Vector2 value)
    {
      Event.INTERNAL_CALL_Internal_SetMousePosition(this, ref value);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetMousePosition(Event self, ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_GetMousePosition(out Vector2 value);

    private void Internal_SetMouseDelta(Vector2 value)
    {
      Event.INTERNAL_CALL_Internal_SetMouseDelta(this, ref value);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetMouseDelta(Event self, ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_GetMouseDelta(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetNativeEvent(IntPtr ptr);

    /// <summary>
    ///   <para>Use this event.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Use();

    /// <summary>
    ///   <para>Get the next queued [Event] from the event system.</para>
    /// </summary>
    /// <param name="outEvent">Next Event.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool PopEvent(Event outEvent);

    /// <summary>
    ///   <para>Returns the current number of events that are stored in the event queue.</para>
    /// </summary>
    /// <returns>
    ///   <para>Current number of events currently in the event queue.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetEventCount();
  }
}
