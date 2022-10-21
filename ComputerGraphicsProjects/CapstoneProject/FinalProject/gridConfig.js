"use strict";

/*******************
 * Courtney St. George 2009917250
 * This file reads data from the gridInput file and configured the level and cell data
*******************/
var cells = [];
var posZ = 0.025;
var negZ = -0.025;
var total = 0;

function createGrid(level, GRID_WIDTH, GRID_HEIGHT)
{
    //need to create bounding boxes of each cell to know left x right x, bottom y and top y
    var xstartpos = -((CELL_WIDTH/2) + (CELL_WIDTH * (GRID_WIDTH/2)));
    var ystartpos = 0 + (CELL_HEIGHT * (GRID_HEIGHT/2));
    var xpos = xstartpos;
    var ypos = ystartpos;
    for(var i = 0; i < level.length; i++)
    {
        ypos -= CELL_HEIGHT;
        var row = [];
        for(var j = 0; j < level[i].length; j++)
        {
            xpos += CELL_WIDTH;
            var left = xpos - (CELL_WIDTH/2);
            var right = xpos + (CELL_WIDTH/2);
            var top = ypos + (CELL_HEIGHT/2);
            var bottom = ypos - (CELL_HEIGHT/2);
            var cell = new Cell(xpos, ypos, 0, left, right, top, bottom, level[i][j]);
            //check if the cell is a path for pacman and ghosts by checking list of legal cells by contents in grid
            if(LEGAL.includes(level[i][j]))
            {
                cell.legal = true;
            }
            else{cell.legal = false;}
            row.push(cell);
        }
        xpos = xstartpos;
        cells.push(row);
    }
    //sets the valid directions for each cell
    setValidInput();
    //builds level based on grid input
    buildLevel();
}

//sets the vertex locations for each piece of the 
//inner walls based on tag value indicated in gridinput
function setValidInput()
{
    for(var i = 0; i < cells.length; i++)
    {
        for(var j = 0; j < cells[i].length; j++)
        {
            var current = cells[i][j];
            if(current.legal)
            {
                    var top = cells[i-1][j].legal;
                    var right = cells[i][j+1].legal;
                    var bottom = cells[i+1][j].legal;
                    var left = cells[i][j-1].legal;

                var inputs = [];
                if(top){inputs.push('up');}
                if(left){inputs.push('left');}
                if(bottom){inputs.push('down');}
                if(right){inputs.push('right');}
                current.inputs = inputs;
            }
        }
    }
}
function buildLevel()
{
    for(var i = 0; i < cells.length; i++)
    {
        for(var j = 0; j < cells[i].length; j++)
        {
            switch(cells[i][j].tag)
            {
                case 5:
                    //contains regular dot
                    cells[i][j].containsDot = true;
                    total += 1;
                    break;
                case 6:
                    //contains big dot
                    cells[i][j].containsSDot = true;
                    total += 1;
                case 8:
                    //sue's scatter target
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var suecorner = new Target();
                    suecorner.x = x;
                    suecorner.y = y;
                    suecorner.z = z;
                    scatterTargets.sueScatter = suecorner;
                    break;
                case 9:
                    //inkys scatter target
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var inkycorner = new Target();
                    inkycorner.x = x;
                    inkycorner.y = y;
                    inkycorner.z = z;
                    scatterTargets.inkyScatter = inkycorner;
                    break;
                case 10:
                    //blinky's scatter target
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var blinkycorner = new Target();
                    blinkycorner.x = x;
                    blinkycorner.y = y;
                    blinkycorner.z = z;
                    scatterTargets.blinkyScatter = blinkycorner;
                    break;
                case 11: 
                    //pinky's scatter target
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var pinkycorner = new Target();
                    pinkycorner.x = x;
                    pinkycorner.y = y;
                    pinkycorner.z = z;
                    scatterTargets.pinkyScatter = pinkycorner;
                    break;
                case 25:
                    //pacman's starting location
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var mspacman = new Actor('pac', x, y, z, j, i, levels.currentLevel.mspacSpeed, 'left');
                    actors.msPac = mspacman;
                    break;
                case 26:
                    //blinky starting location
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var blinky = new Actor('blinky', x, y, z, j, i, levels.currentLevel.blinkySpeed, 'left');
                    actors.Blinky = blinky;
                    break;
                case 27:
                    //inky starting location
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var inky = new Actor('inky', x, y, z, j, i, levels.currentLevel.inkySpeed, 'right');
                    actors.Inky = inky;
                    //ghost house cell
                    var ghostCell = new GhostCell(i, j);
                    ghostCells.leftGhostCell = ghostCell;
                    break;
                case 28:
                    //pinky starting location
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var pinky = new Actor('pinky', x, y, z, j, i, levels.currentLevel.pinkySpeed, 'up');
                    actors.Pinky = pinky;
                    //ghost house cell
                    var ghostCell = new GhostCell(i, j);
                    ghostCells.middleGhostCell = ghostCell;
                    break;
                case 29:
                    //sue's starting location
                    var x = cells[i][j].x;
                    var y = cells[i][j].y;
                    var z = cells[i][j].z;
                    var sue = new Actor('sue', x, y, z, j, i, levels.currentLevel.sueSpeed, 'left');
                    actors.Sue = sue;
                    //ghost house cell
                    var ghostCell = new GhostCell(i, j);
                    ghostCells.rightGhostCell = ghostCell;
                    break;
                    //following are cells that teleport actors to opposite side
                case 30:
                    cells[i][j].containsTeleport = true;
                    cells[i][j].connectedTeleportj = j + 19;
                    break;
                case 31:
                    cells[i][j].containsTeleport = true;
                    cells[i][j].connectedTeleportj = j - 19;
                    break;
                case 32:
                    cells[i][j].containsTeleport = true;
                    cells[i][j].connectedTeleportj = j + 19;
                    break;
                case 33:
                    cells[i][j].containsTeleport = true;
                    cells[i][j].connectedTeleportj = j - 19;
                    break;
            }
        }
    }
}