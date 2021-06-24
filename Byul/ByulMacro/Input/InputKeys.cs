using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum WM
{
    WM_KEYDOWN = 0x0100,
    WM_KEYUP = 0x0101,
    WM_CHAR = 0x0102
}

public enum VK : ushort
{
    LeftButton = 0x01,
    RightButton = 0x02,
    Cancel = 0x03,
    MiddleButton = 0x04,
    ExtraButton1 = 0x05,
    ExtraButton2 = 0x06,
    Back = 0x08,
    Tab = 0x09,
    Clear = 0x0C,
    Return = 0x0D,
    Shift = 0x10,
    Control = 0x11,
    /// <summary></summary>
    Menu = 0x12,
    /// <summary></summary>
    Pause = 0x13,
    /// <summary></summary>
    CapsLock = 0x14,
    /// <summary></summary>
    Kana = 0x15,
    /// <summary></summary>
    Hangeul = 0x15,
    /// <summary></summary>
    Hangul = 0x15,
    /// <summary></summary>
    Junja = 0x17,
    /// <summary></summary>
    Final = 0x18,
    /// <summary></summary>
    Hanja = 0x19,
    /// <summary></summary>
    Kanji = 0x19,
    /// <summary></summary>
    Escape = 0x1B,
    /// <summary></summary>
    Convert = 0x1C,
    /// <summary></summary>
    NonConvert = 0x1D,
    /// <summary></summary>
    Accept = 0x1E,
    /// <summary></summary>
    ModeChange = 0x1F,
    /// <summary></summary>
    Space = 0x20,
    /// <summary></summary>
    Prior = 0x21,
    /// <summary></summary>
    Next = 0x22,
    /// <summary></summary>
    End = 0x23,
    /// <summary></summary>
    Home = 0x24,
    /// <summary></summary>
    Left = 0x25,
    /// <summary></summary>
    Up = 0x26,
    /// <summary></summary>
    Right = 0x27,
    /// <summary></summary>
    Down = 0x28,
    /// <summary></summary>
    Select = 0x29,
    /// <summary></summary>
    Print = 0x2A,
    /// <summary></summary>
    Execute = 0x2B,
    /// <summary></summary>
    Snapshot = 0x2C,
    /// <summary></summary>
    Insert = 0x2D,
    /// <summary></summary>
    Delete = 0x2E,
    /// <summary></summary>
    Help = 0x2F,
    /// <summary></summary>
    N0 = 0x30,
    /// <summary></summary>
    N1 = 0x31,
    /// <summary></summary>
    N2 = 0x32,
    /// <summary></summary>
    N3 = 0x33,
    /// <summary></summary>
    N4 = 0x34,
    /// <summary></summary>
    N5 = 0x35,
    /// <summary></summary>
    N6 = 0x36,
    /// <summary></summary>
    N7 = 0x37,
    /// <summary></summary>
    N8 = 0x38,
    /// <summary></summary>
    N9 = 0x39,
    /// <summary></summary>
    A = 0x41,
    /// <summary></summary>
    B = 0x42,
    /// <summary></summary>
    C = 0x43,
    /// <summary></summary>
    D = 0x44,
    /// <summary></summary>
    E = 0x45,
    /// <summary></summary>
    F = 0x46,
    /// <summary></summary>
    G = 0x47,
    /// <summary></summary>
    H = 0x48,
    /// <summary></summary>
    I = 0x49,
    /// <summary></summary>
    J = 0x4A,
    /// <summary></summary>
    K = 0x4B,
    /// <summary></summary>
    L = 0x4C,
    /// <summary></summary>
    M = 0x4D,
    /// <summary></summary>
    N = 0x4E,
    /// <summary></summary>
    O = 0x4F,
    /// <summary></summary>
    P = 0x50,
    /// <summary></summary>
    Q = 0x51,
    /// <summary></summary>
    R = 0x52,
    /// <summary></summary>
    S = 0x53,
    /// <summary></summary>
    T = 0x54,
    /// <summary></summary>
    U = 0x55,
    /// <summary></summary>
    V = 0x56,
    /// <summary></summary>
    W = 0x57,
    /// <summary></summary>
    X = 0x58,
    /// <summary></summary>
    Y = 0x59,
    /// <summary></summary>
    Z = 0x5A,
    /// <summary></summary>
    LeftWindows = 0x5B,
    /// <summary></summary>
    RightWindows = 0x5C,
    /// <summary></summary>
    Application = 0x5D,
    /// <summary></summary>
    Sleep = 0x5F,
    /// <summary></summary>
    Numpad0 = 0x60,
    /// <summary></summary>
    Numpad1 = 0x61,
    /// <summary></summary>
    Numpad2 = 0x62,
    /// <summary></summary>
    Numpad3 = 0x63,
    /// <summary></summary>
    Numpad4 = 0x64,
    /// <summary></summary>
    Numpad5 = 0x65,
    /// <summary></summary>
    Numpad6 = 0x66,
    /// <summary></summary>
    Numpad7 = 0x67,
    /// <summary></summary>
    Numpad8 = 0x68,
    /// <summary></summary>
    Numpad9 = 0x69,
    /// <summary></summary>
    Multiply = 0x6A,
    /// <summary></summary>
    Add = 0x6B,
    /// <summary></summary>
    Separator = 0x6C,
    /// <summary></summary>
    Subtract = 0x6D,
    /// <summary></summary>
    Decimal = 0x6E,
    /// <summary></summary>
    Divide = 0x6F,
    /// <summary></summary>
    F1 = 0x70,
    /// <summary></summary>
    F2 = 0x71,
    /// <summary></summary>
    F3 = 0x72,
    /// <summary></summary>
    F4 = 0x73,
    /// <summary></summary>
    F5 = 0x74,
    /// <summary></summary>
    F6 = 0x75,
    /// <summary></summary>
    F7 = 0x76,
    /// <summary></summary>
    F8 = 0x77,
    /// <summary></summary>
    F9 = 0x78,
    /// <summary></summary>
    F10 = 0x79,
    /// <summary></summary>
    F11 = 0x7A,
    /// <summary></summary>
    F12 = 0x7B,
    /// <summary></summary>
    F13 = 0x7C,
    /// <summary></summary>
    F14 = 0x7D,
    /// <summary></summary>
    F15 = 0x7E,
    /// <summary></summary>
    F16 = 0x7F,
    /// <summary></summary>
    F17 = 0x80,
    /// <summary></summary>
    F18 = 0x81,
    /// <summary></summary>
    F19 = 0x82,
    /// <summary></summary>
    F20 = 0x83,
    /// <summary></summary>
    F21 = 0x84,
    /// <summary></summary>
    F22 = 0x85,
    /// <summary></summary>
    F23 = 0x86,
    /// <summary></summary>
    F24 = 0x87,
    /// <summary></summary>
    NumLock = 0x90,
    /// <summary></summary>
    ScrollLock = 0x91,
    /// <summary></summary>
    NEC_Equal = 0x92,
    /// <summary></summary>
    Fujitsu_Jisho = 0x92,
    /// <summary></summary>
    Fujitsu_Masshou = 0x93,
    /// <summary></summary>
    Fujitsu_Touroku = 0x94,
    /// <summary></summary>
    Fujitsu_Loya = 0x95,
    /// <summary></summary>
    Fujitsu_Roya = 0x96,
    /// <summary></summary>
    LeftShift = 0xA0,
    /// <summary></summary>
    RightShift = 0xA1,
    /// <summary></summary>
    LeftControl = 0xA2,
    /// <summary></summary>
    RightControl = 0xA3,
    /// <summary></summary>
    LeftMenu = 0xA4,
    /// <summary></summary>
    RightMenu = 0xA5,
    /// <summary></summary>
    BrowserBack = 0xA6,
    /// <summary></summary>
    BrowserForward = 0xA7,
    /// <summary></summary>
    BrowserRefresh = 0xA8,
    /// <summary></summary>
    BrowserStop = 0xA9,
    /// <summary></summary>
    BrowserSearch = 0xAA,
    /// <summary></summary>
    BrowserFavorites = 0xAB,
    /// <summary></summary>
    BrowserHome = 0xAC,
    /// <summary></summary>
    VolumeMute = 0xAD,
    /// <summary></summary>
    VolumeDown = 0xAE,
    /// <summary></summary>
    VolumeUp = 0xAF,
    /// <summary></summary>
    MediaNextTrack = 0xB0,
    /// <summary></summary>
    MediaPrevTrack = 0xB1,
    /// <summary></summary>
    MediaStop = 0xB2,
    /// <summary></summary>
    MediaPlayPause = 0xB3,
    /// <summary></summary>
    LaunchMail = 0xB4,
    /// <summary></summary>
    LaunchMediaSelect = 0xB5,
    /// <summary></summary>
    LaunchApplication1 = 0xB6,
    /// <summary></summary>
    LaunchApplication2 = 0xB7,
    /// <summary></summary>
    OEM1 = 0xBA,
    /// <summary></summary>
    OEMPlus = 0xBB,
    /// <summary></summary>
    OEMComma = 0xBC,
    /// <summary></summary>
    OEMMinus = 0xBD,
    /// <summary></summary>
    OEMPeriod = 0xBE,
    /// <summary></summary>
    OEM2 = 0xBF,
    /// <summary></summary>
    OEM3 = 0xC0,
    /// <summary></summary>
    OEM4 = 0xDB,
    /// <summary></summary>
    OEM5 = 0xDC,
    /// <summary></summary>
    OEM6 = 0xDD,
    /// <summary></summary>
    OEM7 = 0xDE,
    /// <summary></summary>
    OEM8 = 0xDF,
    /// <summary></summary>
    OEMAX = 0xE1,
    /// <summary></summary>
    OEM102 = 0xE2,
    /// <summary></summary>
    ICOHelp = 0xE3,
    /// <summary></summary>
    ICO00 = 0xE4,
    /// <summary></summary>
    ProcessKey = 0xE5,
    /// <summary></summary>
    ICOClear = 0xE6,
    /// <summary></summary>
    Packet = 0xE7,
    /// <summary></summary>
    OEMReset = 0xE9,
    /// <summary></summary>
    OEMJump = 0xEA,
    /// <summary></summary>
    OEMPA1 = 0xEB,
    /// <summary></summary>
    OEMPA2 = 0xEC,
    /// <summary></summary>
    OEMPA3 = 0xED,
    /// <summary></summary>
    OEMWSCtrl = 0xEE,
    /// <summary></summary>
    OEMCUSel = 0xEF,
    /// <summary></summary>
    OEMATTN = 0xF0,
    /// <summary></summary>
    OEMFinish = 0xF1,
    /// <summary></summary>
    OEMCopy = 0xF2,
    /// <summary></summary>
    OEMAuto = 0xF3,
    /// <summary></summary>
    OEMENLW = 0xF4,
    /// <summary></summary>
    OEMBackTab = 0xF5,
    /// <summary></summary>
    ATTN = 0xF6,
    /// <summary></summary>
    CRSel = 0xF7,
    /// <summary></summary>
    EXSel = 0xF8,
    /// <summary></summary>
    EREOF = 0xF9,
    /// <summary></summary>
    Play = 0xFA,
    /// <summary></summary>
    Zoom = 0xFB,
    /// <summary></summary>
    Noname = 0xFC,
    /// <summary></summary>
    PA1 = 0xFD,
    /// <summary></summary>
    OEMClear = 0xFE
}
public enum InputKeys
{
    DIK_ESCAPE = 0x01,
    DIK_1 = 0x02,
    DIK_2 = 0x03,
    DIK_3 = 0x04,
    DIK_4 = 0x05,
    DIK_5 = 0x06,
    DIK_6 = 0x07,
    DIK_7 = 0x08,
    DIK_8 = 0x09,
    DIK_9 = 0x0A,
    DIK_0 = 0x0B,
    DIK_MINUS = 0x0C,    /* - on main keyboard */
    DIK_EQUALS = 0x0D,
    DIK_BACK = 0x0E,    /* backspace */
    DIK_TAB = 0x0F,
    DIK_Q = 0x10,
    DIK_W = 0x11,
    DIK_E = 0x12,
    DIK_R = 0x13,
    DIK_T = 0x14,
    DIK_Y = 0x15,
    DIK_U = 0x16,
    DIK_I = 0x17,
    DIK_O = 0x18,
    DIK_P = 0x19,
    DIK_LBRACKET = 0x1A,
    DIK_RBRACKET = 0x1B,
    DIK_RETURN = 0x1C,    /* Enter on main keyboard */
    DIK_LCONTROL = 0x1D,
    DIK_A = 0x1E,
    DIK_S = 0x1F,
    DIK_D = 0x20,
    DIK_F = 0x21,
    DIK_G = 0x22,
    DIK_H = 0x23,
    DIK_J = 0x24,
    DIK_K = 0x25,
    DIK_L = 0x26,
    DIK_SEMICOLON = 0x27,
    DIK_APOSTROPHE = 0x28,
    DIK_GRAVE = 0x29,    /* accent grave */
    DIK_LSHIFT = 0x2A,
    DIK_BACKSLASH = 0x2B,
    DIK_Z = 0x2C,
    DIK_X = 0x2D,
    DIK_C = 0x2E,
    DIK_V = 0x2F,
    DIK_B = 0x30,
    DIK_N = 0x31,
    DIK_M = 0x32,
    DIK_COMMA = 0x33,
    DIK_PERIOD = 0x34,    /* . on main keyboard */
    DIK_SLASH = 0x35,    /* / on main keyboard */
    DIK_RSHIFT = 0x36,
    DIK_MULTIPLY = 0x37,    /* * on numeric keypad */
    DIK_LMENU = 0x38,    /* left Alt */
    DIK_SPACE = 0x39,
    DIK_CAPITAL = 0x3A,
    DIK_F1 = 0x3B,
    DIK_F2 = 0x3C,
    DIK_F3 = 0x3D,
    DIK_F4 = 0x3E,
    DIK_F5 = 0x3F,
    DIK_F6 = 0x40,
    DIK_F7 = 0x41,
    DIK_F8 = 0x42,
    DIK_F9 = 0x43,
    DIK_F10 = 0x44,
    DIK_NUMLOCK = 0x45,
    DIK_SCROLL = 0x46,    /* Scroll Lock */
    DIK_NUMPAD7 = 0x47,
    DIK_NUMPAD8 = 0x48,
    DIK_NUMPAD9 = 0x49,
    DIK_SUBTRACT = 0x4A,    /* - on numeric keypad */
    DIK_NUMPAD4 = 0x4B,
    DIK_NUMPAD5 = 0x4C,
    DIK_NUMPAD6 = 0x4D,
    DIK_ADD = 0x4E,    /* + on numeric keypad */
    DIK_NUMPAD1 = 0x4F,
    DIK_NUMPAD2 = 0x50,
    DIK_NUMPAD3 = 0x51,
    DIK_NUMPAD0 = 0x52,
    DIK_DECIMAL = 0x53,    /* . on numeric keypad */
    DIK_OEM_102 = 0x56,    /* <> or \| on RT 102-key keyboard (Non-U.S.) */
    DIK_F11 = 0x57,
    DIK_F12 = 0x58,
    DIK_F13 = 0x64,    /*                     (NEC PC98) */
    DIK_F14 = 0x65,    /*                     (NEC PC98) */
    DIK_F15 = 0x66,    /*                     (NEC PC98) */
    DIK_KANA = 0x70,    /* (Japanese keyboard)            */
    DIK_ABNT_C1 = 0x73,    /* /? on Brazilian keyboard */
    DIK_CONVERT = 0x79,    /* (Japanese keyboard)            */
    DIK_NOCONVERT = 0x7B,    /* (Japanese keyboard)            */
    DIK_YEN = 0x7D,    /* (Japanese keyboard)            */
    DIK_ABNT_C2 = 0x7E,    /* Numpad . on Brazilian keyboard */
    DIK_NUMPADEQUALS = 0x8D,    /* = on numeric keypad (NEC PC98) */
    DIK_PREVTRACK = 0x90,    /* Previous Track (DIK_CIRCUMFLEX on Japanese keyboard) */
    DIK_AT = 0x91,    /*                     (NEC PC98) */
    DIK_COLON = 0x92,    /*                     (NEC PC98) */
    DIK_UNDERLINE = 0x93,    /*                     (NEC PC98) */
    DIK_KANJI = 0x94,    /* (Japanese keyboard)            */
    DIK_STOP = 0x95,    /*                     (NEC PC98) */
    DIK_AX = 0x96,    /*                     (Japan AX) */
    DIK_UNLABELED = 0x97,    /*                        (J3100) */
    DIK_NEXTTRACK = 0x99,    /* Next Track */
    DIK_NUMPADENTER = 0x9C,    /* Enter on numeric keypad */
    DIK_RCONTROL = 0x9D,
    DIK_MUTE = 0xA0,    /* Mute */
    DIK_CALCULATOR = 0xA1,    /* Calculator */
    DIK_PLAYPAUSE = 0xA2,    /* Play / Pause */
    DIK_MEDIASTOP = 0xA4,    /* Media Stop */
    DIK_VOLUMEDOWN = 0xAE,    /* Volume - */
    DIK_VOLUMEUP = 0xB0,    /* Volume + */
    DIK_WEBHOME = 0xB2,    /* Web home */
    DIK_NUMPADCOMMA = 0xB3,    /* , on numeric keypad (NEC PC98) */
    DIK_DIVIDE = 0xB5,    /* / on numeric keypad */
    DIK_SYSRQ = 0xB7,
    DIK_RMENU = 0xB8,    /* right Alt */
    DIK_PAUSE = 0xC5,    /* Pause */
    DIK_HOME = 0xC7,    /* Home on arrow keypad */
    DIK_UP = 0xC8,    /* UpArrow on arrow keypad */
    DIK_PRIOR = 0xC9,    /* PgUp on arrow keypad */
    DIK_LEFT = 0xCB,    /* LeftArrow on arrow keypad */
    DIK_RIGHT = 0xCD,    /* RightArrow on arrow keypad */
    DIK_END = 0xCF,    /* End on arrow keypad */
    DIK_DOWN = 0xD0,    /* DownArrow on arrow keypad */
    DIK_NEXT = 0xD1,    /* PgDn on arrow keypad */
    DIK_INSERT = 0xD2,    /* Insert on arrow keypad */
    DIK_DELETE = 0xD3,    /* Delete on arrow keypad */
    DIK_LWIN = 0xDB,    /* Left Windows key */
    DIK_RWIN = 0xDC,    /* Right Windows key */
    DIK_APPS = 0xDD,    /* AppMenu key */
    DIK_POWER = 0xDE,    /* System Power */
    DIK_SLEEP = 0xDF,    /* System Sleep */
    DIK_WAKE = 0xE3,    /* System Wake */
    DIK_WEBSEARCH = 0xE5,    /* Web Search */
    DIK_WEBFAVORITES = 0xE6,    /* Web Favorites */
    DIK_WEBREFRESH = 0xE7,    /* Web Refresh */
    DIK_WEBSTOP = 0xE8,    /* Web Stop */
    DIK_WEBFORWARD = 0xE9,    /* Web Forward */
    DIK_WEBBACK = 0xEA,    /* Web Back */
    DIK_MYCOMPUTER = 0xEB,    /* My Computer */
    DIK_MAIL = 0xEC,    /* Mail */
    DIK_MEDIASELECT = 0xED,  /* Media Select */

}