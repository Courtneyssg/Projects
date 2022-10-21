"use strict";

/*******************
 * Courtney St. George 2009917250
 * This file contains all of the classes created
*******************/

class Cell {
    constructor(xpos, ypos, zpos, left, right, top, bottom, tag) 
    {
        this.x = xpos;
        this.y = ypos;
        this.z = zpos;
        this.posZ = 0.025;
        this.negZ = 0.025;
        this.tag = tag;
        this.legal;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        this.left = left;
        this.inputs;
        this.containsDot = false;
        this.containsSDot = false;
        this.containsTeleport = false;
        this.connectedTeleportj;
        this.containsGhost = false;
    }
}

class GhostCell{
    constructor(i, j)
    {
        this.cellX = j;
        this.cellY = i;
    }
}

class Actor{
    constructor(tag, xpos, ypos, zpos, cellx, celly, speed, currentDirection)
    {
        this.tag = tag;
        this.x = xpos;
        this.y = ypos;
        this.z = zpos;
        this.startX = xpos;
        this.startY = ypos;
        this.startZ = zpos;
        this.startCellX = cellx;
        this.startCellY = celly;
        this.cellX = cellx;
        this.cellY = celly;
        this.speed = speed;
        this.lastCell;
        this.currentDirection = currentDirection;
        this.nextDirection;
        this.position = vec3(xpos, ypos, zpos);
    }
}

class Light
{
    constructor(am, dif, spec, pos, att)
    {
        this.fixedAm = am;
        this.ambient = am;
        this.fixedDif = dif;
        this.diffuse = dif;
        this.fixedSpec = spec;
        this.specular = spec;
        this.originalPosition = pos;
        this.position = pos;
        this.attenuation = att;
    }
}

class Model{
    constructor(name, vertices, indices, am, dif, spec, sh)
    {
        this.name = name;
        this.vertices = vertices;
        this.indices = indices;
        this.normalDif = dif;
        this.ambient = am;
        this.diffuse = dif;
        this.specular = spec;
        this.shininess = sh;
    }
}

class Dot{
    constructor(posX, posY, posZ, active, i, j)
    {
        this.x = posX;
        this.y = posY;
        this.z = posZ;
        this.active = active;
        this.cellX = j;
        this.cellY = i;
    }
}

class Level{
    constructor(grid, width, height, pacSpeed, bSpeed, enemySpeed, dotCount, 
                roundState, mode, s1, s2, s3, c1, c2, iDotCount, sDotCount, timer, frightTimer, pOffset, bOffset, gOffset)
    {
        this.grid = grid;
        this.width = width;
        this.height = height;
        this.pacSpeed = pacSpeed;
        this.fixedBlinkySpeed = bSpeed;
        this.enemySpeed = enemySpeed;
        this.blinkySpeed = bSpeed;
        this.pinkySpeed = enemySpeed;
        this.inkySpeed = 0;
        this.sueSpeed = 0;
        this.dotCount = dotCount;
        this.s1 = s1;
        this.s2 = s2;
        this.s3 = s3;
        this.mode = mode;
        this.targetTimers = [['scatter', s1], ['chase', c1], ['scatter', s2], ['chase', c2], ['scatter', s3]];
        this.roundState = roundState;
        this.iDotCount = iDotCount;
        this.sDotCount = sDotCount;
        this.timer = timer;
        this.lifeCounter = 3;
        this.score = 0;
        this.frightTimer = frightTimer;
        this.pOffset = pOffset;
        this.bOffset = bOffset;
        this.gOffset = gOffset;
    }
}

class Target{
    constructor()
    {
        this.x;
        this.y;
        this.z;
    }
}
