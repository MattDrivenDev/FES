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

/// Addressing Modes
/// The 6502 has several different addressing modes, providing different ways to access
/// memory locations. There are also addressing modes which operate on the contents of
/// registers, rather than memory. In total there are 13 different addressing modes on the 6502.
/// Some instructions can use more than one different addressing mode. 
/// NOTE: 13? Only find 11 in reference materials.
type AddressingMode = 

  /// Zero page addressing uses a single operand which serves as a pointer to an address in zero
  /// page ($0000-$00FF) where the data to be operated on can be found. By using zero page
  /// addressing, only one byte is needed for the operand, so the instruction is shorter and,
  /// therefore, faster to execute than with addressing modes which take two operands
  | ZeroPage

  /// Indexed zero page addressing takes a single operand and adds the value of a register to it to
  /// give an address in zero page ($0000-$00FF) where the data can be found.
  | IndexedZeroPage

  /// In absolute addressing, the address of the data to operate on is specified by the two
  /// operands supplied, least significant byte first. 
  | Absolute

  /// Indexed absolute addressing takes two operands, forming a 16-bit address, least significant
  /// byte first, and adds the value of a register to it to give the address where the data can be
  /// found. For example, if the operands are bb and cc, the address of the data will be ccbb + X. 
  | IndexedAbsolute

  /// Indirect addressing takes two operands, forming a 16-bit address, which identifies the least
  /// significant byte of another address which is where the data can be found. For example if the
  /// operands are bb and cc, and ccbb contains xx and ccbb + 1 contains yy, then the real target
  /// address is yyxx. On the 6502, only JMP (Jump) uses this addressing mode and an example
  /// is JMP ($1234).
  | Indirect

  /// Many instructions do not require access to operands stored in memory. Examples of implied
  /// instructions are CLD (Clear Decimal Mode) and NOP (No Operation). 
  | Implied

  /// Some instructions operate directly on the contents of the accumulator. The only instructions
  /// to use this addressing mode are the shift instructions, ASL (Arithmetic Shift Left), LSR
  /// (Logical Shift Right), ROL (Rotate Left) and ROR (Rotate Right). 
  | Accumulator

  /// Instructions which use immediate addressing operate directly on a constant supplied as an
  /// operand to the instruction. Immediate instructions are indicated by prefacing the operand
  /// with #, for example AND #$12.
  | Immediate

  /// Relative addressing is used in branch instructions. This addressing mode causes the value
  /// of the program counter to change if a certain condition is met. The condition is dependant on
  /// the instruction. The program counter increments by two regardless of the outcome of the
  /// condition but if the condition is true the single operand is added to the program counter to
  /// give the new value. For this purpose, the operand is interpreted as a signed byte, that is in
  /// the range -128 to 127 to allow forward and backward branching. An example of this
  /// addressing mode is BCS *+5 where * represents the current value of the program counter.
  | Relative

  /// Indexed indirect (also known as pre-indexed) addressing takes a single byte as an operand
  /// and adds the value of the X register to it (with wraparound) to give the address of the least
  /// significant byte of the target address. For example, if the operand is bb, 00bb is xx and 00bb
  /// + 1 is yy then the data can be found at yyxx. An example of this addressing mode is AND ($12,X). 
  | IndexedIndirect

  /// Indirect indexed (also known as post-indexed) addressing takes a single operand which
  /// gives the zero page address of the least significant byte of a 16-bit address which is then
  /// added to the Y register to give the target address. For example, if the operand is bb, 00bb is
  /// xx and 00bb + 1 is yy, then the data can be found at yyxx. An example of this addressing
  /// mode is AND ($12),Y. 
  | IndirectIndexed