| Type | SubType? | Count? | Entry Count | Frequency | Structure | Shape | Done | Info |
|------|----------|--------|-------------|-----------|-----------|------|------|------|
| 00   | --       | 01     | 4           | 150       | single[4] | Vector 2d | ✔️    | These are simple panels |
| 00   | --       | 03     | 3           | 61        | long?? model??<br />single[2] point| Point w/ Model? | ✔️    | These seem like models, have them plotting but there's more data there. These appear to be vertical poles throughout town in Dobuita. See 0003ColLocs for more information |
| 00   | --       | 05     | 6           | 43        | single[2] coord<br />long[4]????  | Point w/ Model?   | ✔️    | This isn't done, have points plotting in 2d, but there's more data in these. |
| 00   | 02       | 03     | 6           | 58        | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 04     | 8           | 36        | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 05     | 10          | 21        | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 06     | 12          | 20        | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 07     | 14          | 15        | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 08     | 16          | 4         | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 09     | 18          | 4         | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 0A     | 20          | 2         | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 0B     | 22          | 2         | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 0D     | 26          | 2         | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 00   | 02       | 10     | 32          | 1         | single[count*2] coords| Vector 2d | ✔️    | These are simple panels like sides of buildings or fences |
| 07   | --       | 01     | 4           | 13        | single[4] coords| Vector 2d | ✔️    | These seem like the garage-style doors and other door-like objects that load in at night to prevent you from going into stores. They appear in Dobuita in front of many of the stores but are not visible during the day. |
| 07   | 02       | 03     | 6           | 1         | single[count*2] coords| Vector 2d | ✔️    | These look like the inner collision for the inside of the stores that have garage doors in 07 01 |
| 07   | 02       | 04     | 8           | 1         | single[count*2] coords| Vector 2d | ✔️    | These look like the inner collision for the inside of the stores that have garage doors in 07 01 |
| 08   | --       | 01     | 4           | 6         | single[4] coords| Vector 2d | ✔️    | In Dobuita, these are in pairs. Almost looks like door reinforcement |
| 09   | --       | 01     | 4           | 13        | single[4] coords| Vector 2d | ✔️    | These are invisible walls. In Dobuita the best example is the invisible walls that block you from entering the street or continuing down the sidewalk. This also blocks you from reaching the walls of the parking lot where you can train (can't reach or go past the parking spot bumpers) | 
| 09   | --       | 03     | 3           | 6         | long?? model??<br />single[2] point | Point w/ Model?   | ✔️    | These are 2d points with something associated as the first word. Idk what these are, will have to look more into it |
| 09   | --       | 05     | 6           | 9         | single[2] coord<br />long[4]???? | Point w/ Model?   | ✔️    | First two words are a coordinate, plotting in 2d, need to figure the rest of this type. |
| 09   | 02       | 03     | 6           | 6         | single[count*2] coords | Vector 2d | ✔️    | These seem like barriers around objects. Don't have all the objects plotting yet but it certainly looks like it in Dobuita |
| 09   | 02       | 04     | 8           | 8         | single[count*2] coords | Vector 2d | ✔️    | These seem like barriers arou nd objects. Don't have all the objects plotting yet but it certainly looks like it in Dobuita |
| 09   | 02       | 05     | 10          | 4         | single[count*2] coords | Vector 2d | ✔️    | These seem like barriers around objects. Don't have all the objects plotting yet but it certainly looks like it in Dobuita |
| 09   | 02       | 06     | 12          | 4         | single[count*2] coords | Vector 2d | ✔️    | These seem like barriers around objects. Don't have all the objects plotting yet but it certainly looks like it in Dobuita |
| 0A   | --       | 01     | 4           | 18        | single[4] coords | Vector 2d | ✔️    | In Sakuragaoka there are stairs between pairs of these, same with Dobuita. These are stairs beginning and ends. |
| 64   | --       | 01     | 4           | 57        | single[4] coords | Vector 2d | ✔️    | These appear to be collision on doors. This includes doors that Ryo can't interact with (but NPCs can). |
| 65   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 66   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 67   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 68   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 69   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 6A   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 6B   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 6C   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 6D   | --       | 01     | 4           | 2         | single[4] coords | Vector 2d | ✔️    | These are the sliding doors of You Arcade in Dobuita | 
| 6E   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 6F   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 70   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 71   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 72   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 73   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 74   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 75   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| 76   | --       | 01     | 4           | 1         | single[4] coords | Vector 2d | ✔️    | These are the sliding door of Tomato Convenience Store in Dobuita |
| C8   | 02       | 04     | 8           | 1         | single[count*2] coords| Vector 2d | ✔️    | This is be the collision around Tom's Hot Dog cart in Dobuita | 
