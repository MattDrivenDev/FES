/// NES games came on cartridges known as a Game Pak. The game itself was stored on ROM
/// chips inside the cartridge. Some cartridges also featured RAM, powered by a battery, in
/// order to allow games to be saved. 
module ROM

type INES = {
  header : byte[]
  trainer : byte[] 
  prgRom : byte[]
  chrRom : byte[] }

let load (data:byte[]) =
  // TODO 1: FLAGS wat!?
  // TODO 2: determine trainer because it can affect PRG ROM location
  // TODO 3: CHR ROM
  let prgSize = 16384 * int data.[4]
  let prgRom = Array.create prgSize 0x0uy
  Array.blit data 0x10 prgRom 0 prgSize
  let rom = {
    header = Array.take 16 data
    trainer = [||] 
    prgRom = prgRom
    chrRom = [||] }

  // TODO 4: This is NOT correct - but it should just let me interpret instructions
  Array.blit rom.prgRom 0 Memory.RAM 0 prgSize