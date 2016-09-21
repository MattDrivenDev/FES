/// The 6502 used memory mapped I/O (input/output). This means that the same instructions
/// and bus are used to communicate with I/O devices as with memory, that writing to a specific
/// memory location writes to the appropriate device. In the NES, the I/O ports for input devices
/// were $4016 and $4017 (see Appendix B). 
module Input

/// The original NES used a rectangular control pad as shown in figure 5-1. The pad featured
/// four buttons, A, B, Start and Select as well as a four-directional cross used to control
/// movement. 
module ControlPad = ()

/// When the NES first launched in America, Nintendo included a light-gun known as the
/// Zapper. Figure 5-2 shows the original version of the Zapper, although the colour was later
/// changed to orange. By aiming using the sight, the gamer could produce quite accurate
/// results. Several games featured Zapper support including Duck Hunt, Gumshoe and Wild
/// Gunman [44]. 
module Zapper = ()