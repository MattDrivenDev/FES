[<AutoOpen>]
module Util

/// Returns true if a bit in a byte is set at given position
let bit pos (b:byte) = 
  not ((b &&& (1uy <<< pos)) = 0uy)

/// Returns true if bit 7 (last bit) in a byte is set
let bit7 = bit 7

/// Reutrns the hex string of an unsigned-int16
let hexus (i:uint16) = "0x" + i.ToString("X")

/// Returns the hex string of a byte
let hex (b:byte) = b |> uint16 |> hexus

/// Takes a function and executes it... just adds some syntactic flavour
let exec f = f()