/// The memory is divided into three parts, ROM inside the cartridges, the CPU’s RAM and the I/O registers. The address
/// bus is used to set the address of the required location. The control bus is used to inform the
/// components whether the request is a read or a write. The data bus is used to read or write
/// the byte to the selected address. Note that ROM is read-only and is accessed via a MMC, to
/// allow bank switching to occur. The I/O registers are used to communicate with the other
/// components of the system, the PPU and the control devices. 
module Memory

/// RAM
/// The 2A03 had a 16-bit address bus and as such could support 64 KB of memory with
/// addresses from $0000-$FFFF.
let size = 0x10000
let RAM = Array.create size 0uy

/// Program Counter
/// The program counter is a 16-bit register which holds the address of the next instruction to be
/// executed. As instructions are executed, the value of the program counter is updated, usually
/// moving on to the next instruction in the sequence. The value can be affected by branch and
/// jump instructions, procedure calls and interrupts. 
let PC = ref 0us

/// Stack Pointer
/// The stack is located at memory locations $0100-$01FF. The stack pointer is an 8-bit register
/// which serves as an offset from $0100. The stack works top-down, so when a byte is pushed
/// on to the stack, the stack pointer is decremented and when a byte is pulled from the stack,
/// the stack pointer is incremented. There is no detection of stack overflow and the stack
/// pointer will just wrap around from $00 to $FF.
let SP = ref 0uy

/// Accumulator
/// The accumulator is an 8-bit register which stores the results of arithmetic and logic
/// operations. The accumulator can also be set to a value retrieved from memory. 
let A = ref 0uy

/// Index Register X
/// The X register is an 8-bit register typically used as a counter or an offset for certain
/// addressing modes. The X register can be set to a value retrieved from memory and can be
/// used to get or set the value of the stack pointer. 
let X = ref 0uy

/// Index Register Y
/// The Y register is an 8-bit register which is used in the same way as the X register, as a
/// counter or to store an offset. Unlike the X register, the Y register cannot affect the stack
/// pointer.
let Y = ref 0uy

/// Processor Status
/// The status register contains a number of single bit flags which are set or cleared when
/// instructions are executed. 
module P = 

  /// Carry Flag 
  /// The carry flag is set if the last instruction resulted in an overflow from bit
  /// 7 or an underflow from bit 0. For example performing 255 + 1 would result in an answer
  /// of 0 with the carry bit set. This allows the system to perform calculations on numbers
  /// longer than 8-bits by performing the calculation on the first byte, storing the carry and
  /// then using that carry when performing the calculation on the second byte. The carry flag
  /// can be set by the SEC (Set Carry Flag) instruction and cleared by the CLC (Clear Carry
  /// Flag) instruction. 
  let C = ref false

  /// Zero Flag
  /// The zero flag is set if the result of the last instruction was zero. So for
  /// example 128 - 127 does not set the zero flag, whereas 128 - 128 does.
  let Z = ref false

  /// Interupt Disable
  /// The interrupt disable flag can be used to prevent the system
  /// responding to IRQs. It is set by the SEI (Set Interrupt Disable) instruction and IRQs will
  /// then be ignored until execution of a CLI (Clear Interrupt Disable) instruction. 
  let I = ref false

  /// Decimal Mode
  /// The decimal mode flag is used to switch the 6502 into BCD mode.
  /// However the 2A03 does not support BCD mode so although the flag can be set, its value
  /// will be ignored. This flag can be set SED (Set Decimal Flag) instruction and cleared by
  /// CLD (Clear Decimal Flag). 
  let D = ref false

  /// Break Command
  /// The break command flag is used to indicate that a BRK (Break)
  /// instruction has been executed, causing an IRQ. 
  let B = ref false

  /// Overflow
  /// The overflow flag is set if an invalid two’s complement result was
  /// obtained by the previous instruction. This means that a negative result has been obtained
  /// when a positive one was expected or vice versa. For example, adding two positive
  /// numbers should give a positive answer. However 64 + 64 gives the result -128 due to the
  /// sign bit. Therefore the overflow flag would be set. The overflow flag is determined by
  /// taking the exclusive-or of the carry from between bits 6 and 7 and between bit 7 and the
  /// carry flag [29]. An explanation of two’s complement can be found in Appendix A.
  let V = ref false

  /// Negative
  /// Bit 7 of a byte represents the sign of that byte, with 0 being positive
  /// and 1 being negative. The negative flag (also known as the sign flag) is set if this sign bit
  /// is 1.
  let N = ref false

/// Read's byte of data from specified memory location
let readn (n:uint16) = RAM.[int n]

/// Read's the byte of data as specified by the program counter (PC)
let read() = !PC |> readn

/// Increments the program counter (PC) by 1
let incrementPC() = PC := !PC + 1us