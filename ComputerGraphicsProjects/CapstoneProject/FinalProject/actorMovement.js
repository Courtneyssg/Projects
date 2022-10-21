"use strict";

/*******************
 * Courtney St. George 2009917250
 * This file contains the ghost movement logic and the main movement method that pacman uses as well
*******************/

var pacAngle = 90;
//handles movement for ghost scatter mode
function scatterAndChaseMechanics(actor, speed, cellX, cellY, curCell, validInputs, target, offset)
{

    var cellNegX = cells[cellY][cellX - 1];
    var cellPosX = cells[cellY][cellX + 1];
    var cellNegY = cells[cellY + 1][cellX];
    var cellPosY = cells[cellY - 1][cellX];

    var distdown = {direction: null, distance: null}
    var distup = {direction: null, distance: null}
    var distright = {direction: null, distance: null}
    var distleft = {direction: null, distance: null}

    var arr = [];
    //checks if valid inputs includes a direction
    //calculates the cell that is the shortest distance away from pacman
    if(validInputs.includes('down'))
    {
        distdown.distance = Math.sqrt(Math.pow(target.x - cellNegY.x, 2) + 
                            Math.pow(target.y - cellNegY.y, 2) + Math.pow(target.z - cellNegY.z, 2));
        distdown.direction = 'down';
        arr.push(distdown);
    }
    if(validInputs.includes('up'))
    {
        distup.distance = Math.sqrt(Math.pow(target.x - cellPosY.x, 2) + 
                            Math.pow(target.y - cellPosY.y, 2) + Math.pow(target.z - cellPosY.z, 2));
        distup.direction = 'up';
        arr.push(distup);
    }
    if(validInputs.includes('right'))
    {
        distright.distance = Math.sqrt(Math.pow(target.x - cellPosX.x, 2) + 
                             Math.pow(target.y - cellPosX.y, 2) + Math.pow(target.z - cellPosX.z, 2));
        distright.direction = 'right';
        arr.push(distright);
    }
    if(validInputs.includes('left'))
    {
        distleft.distance = Math.sqrt(Math.pow(target.x - cellNegX.x, 2) + 
                            Math.pow(target.y - cellNegX.y, 2) + Math.pow(target.z - cellNegX.z, 2));
        distleft.direction = 'left';
        arr.push(distleft);
    }
    
    arr.sort(function(a, b){
        return a.distance-b.distance;
    });
    //checks that the next direction is set to null and actor is in new cell before updating direction
    if(actor.nextDirection == null && curCell != actor.lastCell)
    {
        actor.nextDirection = arr[0].direction;
        actor.lastCell = curCell;
    }
    //if next direction is valid we accept it or set it to null
    if(validInputs.includes(actor.nextDirection))
    {
        if(Math.abs(actor.x - curCell.x) <= offset && Math.abs(actor.y - curCell.y) <= offset)
        {
            actor.x = curCell.x;
            actor.y = curCell.y;
            actor.currentDirection = actor.nextDirection;
            actor.nextDirection = null;
        }
    }else{actor.nextDirection = null;}

    getMovement(actor, speed);
}

function frightenedMechanics(actor, speed, curCell, validInputs, offset)
{

    //if in frightened mode they pick a random direction instead of pathfinding
    var randomDirection = Math.floor(Math.random() * validInputs.length);

    if(actor.nextDirection == null && curCell !=actor.lastCell)
    {
        actor.nextDirection = validInputs[randomDirection];
        actor.lastCell = curCell;
    }
    if(validInputs.includes(actor.nextDirection))
    {
        if(Math.abs(actor.x - curCell.x) <= offset && Math.abs(actor.y - curCell.y) <= offset)
        {
            actor.x = curCell.x;
            actor.y = curCell.y;
            actor.currentDirection = actor.nextDirection;
            actor.nextDirection = null;
            update = false;
        }
    }else{actor.nextDirection = null;}
    getMovement(actor, speed);
}

function TeleportActor(actor, destinationCellX, cellY)
{
    actor.x = cells[cellY][destinationCellX].x;
    actor.cellX = destinationCellX;
}

function getMovement(actor, levelSpeed)
{
    if(cells[actor.cellY][actor.cellX].inputs.includes(actor.currentDirection))
    {
        //if valid and direction is up
        if(actor.currentDirection == "up")
        {
            //move in positive Y direction
            actor.y += levelSpeed;
        }
        else if(actor.currentDirection == "right")
        {
            //move in positive X direction
            actor.x += levelSpeed;
        }
        else if(actor.currentDirection == "left")
        {
            //move in negative X direction
            actor.x -= levelSpeed;
        }
        else if(actor.currentDirection == "down")
        {
            //move in negative Y direction
            actor.y -= levelSpeed;
        }
    }
    else 
    {
        //if we have a direction already in getDirection, it means that it was valid and we entered an invalid cell
        if(actor.currentDirection == "left" & actor.x > cells[actor.cellY][actor.cellX].x)
        {
            //move in negative X direction
            actor.x -= levelSpeed;
        }
        if(actor.currentDirection == "right" & actor.x < cells[actor.cellY][actor.cellX].x)
        {
            //move in positive X direction
            actor.x += levelSpeed;
        }
        if(actor.currentDirection == "up" & actor.y < cells[actor.cellY][actor.cellX].y)
        {
            //move in positive Y direction
            actor.y += levelSpeed;
        }
        if(actor.currentDirection == "down" & actor.y > cells[actor.cellY][actor.cellX].y)
        {
            //move in negaitve Y direction
            actor.y -= levelSpeed;
        }
    }
}
//calculates opposite direction to ghost because they can't turn around
function getOpposite(direction)
{
    if(direction !=null)
    {
        if(direction  == 'right'){return 'left';}
        if(direction == 'left'){return 'right';}
        if(direction == 'down'){return 'up';}
        if(direction == 'up'){return 'down';}
    }

}