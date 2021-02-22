# Collision Structures

This is a reference for the collision structures used in the `COLS`/`COLI` section of `MAPINFO.BIN`. 

Most collision data is 2D (X, Z) coordinates with some fixed height (possibly specified in `HGHT` sections, but more research is required). It seems that most collision data extends basically infinitely and is two sided (once Ryo is out of bounds you're unable to re-enter bounds without activating a cutscene or entering a door).

All collision objects are terminated with `0xFFFFFFFF`

## Header
This header is present for all variations of collision type.

### Structure
| Position | Length | Type | Description |
|----------|--------|------|-------------|
| `0x00`   | `0x04` | uint | Collision Layer? (e.g. `0x00`, `0x09`, `0x64`) |
| `0x04`   | `0x04` | uint | Collision Shape (see section below) |

## Shape 0x01
Shape `0x01` is a line. Objects of type `0x01` contain two floating point [x, z] coordinates. It is suitable to draw a line (or quad) between them to visually represent them.

### Structure
| Position | Length | Type | Description |
|----------|--------|------|-------------|
| `0x08`     | `0x08`   | float[2] | (X, Z) coordinate 0
| `0x10`     | `0x08`   | float[2] | (X, Z) coordinate 1
| `0x18`     | `0x04`   | ----- | Terminator

### Example
```
00 00 00 00
01 00 00 00
A2 08 2C 42
D6 75 77 C2
BD 6F 5D 42
50 6B 46 C2
FF FF FF FF
```

## Shape 0x02
Shape `0x02` is a series of lines but not necessarily a closed polygon (in most cases not). The number of coordinates in the object is provided at offset `0x08` and then the remainder of the data is that many floating point (X, Z) coordinates. It is suitable to draw a line (or quad) between them to visually represent them.

### Structure
| Position | Length | Type | Description |
|----------|--------|------|-------------|
| `0x08` | `0x04` | uint  | Count of coordinates in the shape
| `0x0C` | `0x08` | float[2] | (X, Z) coordinate 0
| ...    | `0x08` | float[2] | (X, Z) coordinates,up to value in offset `0x08`
| (count * 2) + `0x08` | `0x04` | ----- | Terminator

### Example
```
00 00 00 00
02 00 00 00
04 00 00 00
C2 42 F1 C2
73 FB C0 40
C2 42 F1 C2
08 9E 99 40
57 64 EF C2
08 9E 99 40
68 3C EE C2
A1 37 83 40
FF FF FF FF
```

## Shape 0x03
Shape `0x03` contains some sort of pointer or reference maybe and then an (X, Z) coordinate to place it at.

### Structure
| Position | Length | Type | Description |
|----------|--------|------|-------------|
| `0x08`   | `0x04` | ???? | ????
| `0x10`   | `0x08` | float[2] | (X, Z) coordinate
| `0x14`   | `0x04` | ----- | Terminator

### Example
```
00 00 00 00
03 00 00 00
DE 5B F8 3F
F6 D9 A6 C2
A5 85 34 C2
FF FF FF FF
```

## Shape 0x05
Shape `0x05` contains an (X, Z) coordinate and then some other data. Unsure what this data is.

### Structure
| Position | Length | Type  | Description |
|----------|--------|-------|-------------|
| `0x08`   | `0x08` | float[2] | (X, Z) coordinate
| `0x10`   | `0x04` | ???? | ????
| `0x14`   | `0x04` | ???? | ????
| `0x18`   | `0x04` | ???? | ????
| `0x1C`   | `0x04` | ???? | ????
| `0x20`   | `0x04` | ----- | Terminator

### Example
```
00 00 00 00
05 00 00 00
CA 55 41 42
7F 54 62 C2
10 0C B7 C0
38 30 B7 40
90 BC A3 C0
48 A9 A3 C0
FF FF FF FF
```