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
let size = 0xFFFFus

/// TODO: make private
let RAM = Array.create (int size) 0uy

/// Maps 16-bit addresses through to the correct locations of memory - which is not
/// necessarily in the same "physical" space. It can be in the RAM or one of the ROM
/// PGR DATA banks etc.
/// TODO: It only maps through to an oversized RAM at the moment - refactor the things.
type private MemoryMap() = 
  member this.Item
    with get(index) = 
      match index with 
      | _ when index < 0us -> failwith (sprintf "%i is less that lower-bound of memory" index)
      | _ when index >= size -> failwith (sprintf "%i is greater than upper-bound of memory" index)
      | _ -> RAM.[int index]
    and set index value = 
      match index with
      | _ when index < 0us -> failwith (sprintf "%i is less that lower-bound of memory" index)
      | _ when index >= size -> failwith (sprintf "%i is greater than upper-bound of memory" index)
      | _ -> RAM.[int index] <- value

/// Keep an instance of the memory map alive, but don't expose it.
let private memory = MemoryMap()

/// Read's byte of data from specified memory location
let read addr = 
  printfn "reading memory: %s" (hexus addr)
  memory.[addr]