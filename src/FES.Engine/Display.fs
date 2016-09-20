﻿module Display

open Microsoft.Xna.Framework

/// The NES has a colour palette containing 52 colours although there is actually room for 64.
/// However, not all of these can be displayed at a given time. The NES uses two palettes, each
/// with 16 entries, the image palette ($3F00-$3F0F) and the sprite palette ($3F10-$3F1F). The
/// image palette shows the colours currently available for background tiles. The sprite palette
/// shows the colours currently available for sprites. These palettes do not store the actual
/// colour values but rather the index of the colour in the system palette. Since only 64 unique
/// values are needed, bits 6 and 7 can be ignored. 
let ColorPalette = 
  [| Color(0x75, 0x75, 0x75)
     Color(0x27, 0x1B, 0x8F)
     Color(0x00, 0x00, 0xAB)
     Color(0x47, 0x00, 0x9F)
     Color(0x8F, 0x00, 0x77)
     Color(0xAB, 0x00, 0x13)
     Color(0xA7, 0x00, 0x00)
     Color(0x7F, 0x0B, 0x00)
     Color(0x43, 0x2F, 0x00)
     Color(0x00, 0x47, 0x00)
     Color(0x00, 0x51, 0x00)
     Color(0x00, 0x3F, 0x17)
     Color(0x1B, 0x3F, 0x5F)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00)
     Color(0xBC, 0xBC, 0xBC)
     Color(0x00, 0x73, 0xEF)
     Color(0x23, 0x3B, 0xEF)
     Color(0x83, 0x00, 0xF3)
     Color(0xBF, 0x00, 0xBF)
     Color(0xE7, 0x00, 0x5B)
     Color(0xDB, 0x2B, 0x00)
     Color(0xCB, 0x4F, 0x0F)
     Color(0x8B, 0x73, 0x00)
     Color(0x00, 0x97, 0x00)
     Color(0x00, 0xAB, 0x00)
     Color(0x00, 0x93, 0x3B)
     Color(0x00, 0x83, 0x8B)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00) 
     Color(0xFF, 0xFF, 0xFF)
     Color(0x3F, 0xBF, 0xFF)
     Color(0x5F, 0x97, 0xFF)
     Color(0xA7, 0x8B, 0xFD)
     Color(0xF7, 0x7B, 0xFF)
     Color(0xFF, 0x77, 0xB7)
     Color(0xFF, 0x77, 0x63)
     Color(0xFF, 0x9B, 0x3B)
     Color(0xF3, 0xBF, 0x3F)
     Color(0x83, 0xD3, 0x13)
     Color(0x4F, 0xDF, 0x4B)
     Color(0x58, 0xF8, 0x98)
     Color(0x00, 0xEB, 0xDB)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00)
     Color(0xFF, 0xFF, 0xFF)
     Color(0xAB, 0xE7, 0xFF)
     Color(0xC7, 0xD7, 0xFF)
     Color(0xD7, 0xCB, 0xFF)
     Color(0xFF, 0xC7, 0xFF)
     Color(0xFF, 0xC7, 0xDB)
     Color(0xFF, 0xBF, 0xB3)
     Color(0xFF, 0xDB, 0xAB)
     Color(0xFF, 0xE7, 0xA3)
     Color(0xE3, 0xFF, 0xA3)
     Color(0xAB, 0xF3, 0xBF)
     Color(0xB3, 0xFF, 0xCF)
     Color(0x9F, 0xFF, 0xF3)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00)
     Color(0x00, 0x00, 0x00) |]