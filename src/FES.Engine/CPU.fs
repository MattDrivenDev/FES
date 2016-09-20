/// Central Processing Unit
/// Ricoh produced an NMOS processor based on the 6502, the 2A03. The chip differed from a
/// standard 6502 in that it had the ability to handle sound, serving as pAPU (pseudo-Audio
/// Processing Unit) as well as CPU, and that it lacked a Binary Coded Decimal (BCD) mode
/// which allowed representing each digit using 4 bits. For the purposes of programming, the
/// 2A03 uses the same instruction set as the standard 6502 which is shown in figure 2-1. The
/// 6502 is a little endian processor which means that addresses are stored in memory least
/// significant byte first, for example the address $1234 would be stored in memory as $34 at
/// memory location x and $12 at memory location (x + 1). 
module CPU

open Memory