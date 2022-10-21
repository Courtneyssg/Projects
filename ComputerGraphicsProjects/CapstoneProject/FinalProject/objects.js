"use strict";

/*******************
 * Courtney St. George 2009917250
 * This file contains all the objects created for reference
*******************/

//holds data for each model created
var models = {
    cube: null,
    mspacman: null,
    dot: null,
    blinky: null,
    pinky: null,
    inky: null,
    sue: null,
    paclife: null,
}
//hold references to lights
var lights = {
    cameraLeftLight: null,
    cameraRightLight: null,
    spotlight: null
}
//characters
var actors = {
    msPac: null,
    Blinky: null,
    Pinky: null,
    Inky: null,
    Sue: null
}
//holds current level and default level1
var levels = {
    currentLevel: null,
    level1: null,
}
//holds fixed location for scatter targets
var scatterTargets = {
    blinkyScatter: null,
    pinkyScatter: null,
    inkyScatter: null,
    sueScatter: null
}
// holds data for ghost house cells
var ghostCells = {
    leftGhostCell: null,
    middleGhostCell: null,
    rightGhostCell: null
}
//data for level generation
var levelData = {
    pacSpeed: 0.004, 
    blinkySpeed: 0.003, 
    ghostSpeed: 0.002, 
    dotCount: 153, 
    scatter1: 1000, 
    scatter2: 1000, 
    scatter3: 750, 
    timer: 500, 
    frightenedTime: 1500, 
    pacOffset: 0.008, 
    blinkyOffset: 0.004, 
    ghostOffset: 0.003, 
    speedOffsetInc: 0.00075,
    frightTimeDec: 375,
    scatterTimeDec: 250,
    timerdec: 110
}

