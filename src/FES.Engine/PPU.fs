/// Picture Processing Unit
/// Ricoh also supplied the 2C02 to serve as PPU. The PPU’s registers are mostly located in the
/// I/O registers section of CPU memory at $2000-$2007 and $4014 as described in Appendix
/// B. In addition, there are some special registers used for screen scrolling. 
module PPU

open Memory