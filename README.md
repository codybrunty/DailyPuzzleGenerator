# DailyPuzzleGenerator

The DailyPuzzleGenerator script is used in Unity to set up a Daily Puzzle system.

Every day the DailyPuzzleGenerator will select random Puzzles from a list of pre
made puzzles. Using the date as the randomizing seed number all users will 
receive the same randomly selected daily puzzles accross all devices. 

For example, I have a list of 168 easy puzzles. The date is December 29, 2019. The 
DailyPuzzleGenerator selects 5 of these 168 puzzles at random using the seed number
12292019 giving us this List<int> { 67, 53, 128, 117, 0 } 

All users no matter the device will randomize these 5 easy puzzles for the date
December 29, 2019 giving us an elegant solution to offering the user daily content without
having to connect to the internet. 

thanks and have fun
Cody Brunty
