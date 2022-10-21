/*******************
 * Courtney St. George 2009917250
 * This is the main game file. Handles most of the game logic.
*******************/

"use strict";
//init setup
var canvas, gl, aspect, program;
var modelViewMatrix, projectionMatrix;
//eye info
var eye;
const up = vec3(0.0, 1.0, 0.0);
//light position rotation
var lightPos, theta = 4.715, phi = 0;
var  fovy = 45.0;
var near = 0.3;
var far = 20.0;
//orthographic info
var oleft = -1;
var oright = 1;
var obottom = -1
var otop = 1;
//grid data
var cells;
var sval = 0.05;
//html elements for main menu & display
var attenLeftSlider, attenLeftOutput, attenRightSlider, attenRightOutput;
var scoreOutput, livesOutput, levelOutput;
//start menu and change level type
var start, orth, pers, spot, menuContainer;
//game state variables
var gamestart = false, gameover = false, gamewin=false, inkySet = false, sueSet = false, update = true;
//current level and life data
var currentLevel = 1, nextLevel = 2, totalLives = 3, currentLives = 0;
//timers and placeholders
var currentModeIndex = 0, levelTime = 0, score = 0;
var elapsedInkyTime = 0, elapsedSueTime = 0, deathtimer = 0, winTimer = 0;
var collisionOccurred = false;
var ghostPoints = 100, ghostCounter = 0, frightTimer = 0;
var pacAngles = {'left': 270, 'right': 90, 'up': 180, 'down': 360};
//textures
var wallTexture, msPacTexture, dotTexture;
//audio
var intro = new Audio('assets/intro.wav');
var eatdot1 = new Audio('assets/m1.wav');
var sdot = new Audio('assets/sdot.wav');
var death1 = new Audio('assets/d1.wav');
var death2 = new Audio('assets/d2.wav');
var gdeath = new Audio('assets/ghost.wav');
window.onload = function init()
{
    //get canvas from html
    canvas = document.getElementById("gl-canvas");
    gl = canvas.getContext('webgl2');
    if (!gl) alert("WebGL 2.0 isn't available" );
	//set viwport and color
    
    gl.viewport(0, 0, canvas.width, canvas.height);
    gl.clearColor(0.0, 0.0, 0.0, 1.0);
    gl.enable(gl.DEPTH_TEST);
    //onscreen text output
    attenLeftSlider = document.getElementById("atten-left-slider");
    attenLeftOutput = document.getElementById("atten-left-output");
    attenRightSlider = document.getElementById("atten-right-slider");
    attenRightOutput = document.getElementById("atten-right-output");
    scoreOutput = document.getElementById("score-output");
    livesOutput = document.getElementById("lives-output");
    levelOutput = document.getElementById("level-output");

    start = document.getElementById("start");
    orth = document.getElementById('orth');
    pers = document.getElementById('pers');
    spot = document.getElementById('spot');
    menuContainer = document.getElementById('menu-container');
    
	//set aspect ratio for perspective viewing
	aspect =  canvas.width/canvas.height;
    program = initShaders(gl, "vertex-shader", "fragment-shader");
	gl.useProgram(program);
    //initialize levels, grid, models and lights
    initGame();
    InputHandler();
    //vertex buffer
    var vBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, vBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, flatten(positionsArray), gl.STATIC_DRAW);

    var positionLoc = gl.getAttribLocation(program, "aPosition");
    gl.vertexAttribPointer(positionLoc, 4, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(positionLoc);
	//create normals buffer
	var nBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, nBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, flatten(normalsArray), gl.STATIC_DRAW);

    var normalLoc = gl.getAttribLocation(program, "aNormal");
    gl.vertexAttribPointer(normalLoc, 4, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(normalLoc);
    //create texture buffer
    var texBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, texBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, flatten(texCoordsArray), gl.STATIC_DRAW);

    var texCoordLoc = gl.getAttribLocation(program, "aTexCoord");
    gl.vertexAttribPointer(texCoordLoc, 2, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(texCoordLoc);
    //handles input from main menu
    menuClickHandler(start, orth, pers, spot, menuContainer);
    projectionMatrix = setProjectionMatrix();
    modelViewMatrix = mat4();
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"),
       false, flatten(modelViewMatrix));
    //sets initial round
    levelTime = levels.currentLevel.targetTimers[currentModeIndex][1];
    render();
}
var render = function(){
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
    //handles interactive light movement
    moveLights();
    //used to switch game modes from main menu adjusts eye position
    if(orthoFlag)
    {
        eye = vec3(0, 0, 2);
        modelViewMatrix = lookAt(eye, vec3(0, 0, 0), up);
    }
    else
    {
        eye = vec3(actors.msPac.x + 0,actors.msPac.y - 1, actors.msPac.z + 1.5); //eye 
        modelViewMatrix = lookAt(eye, vec3(actors.msPac.x, actors.msPac.y, actors.msPac.z), up);
    }
    //set screen output
    levelOutput.innerHTML = currentLevel;
    scoreOutput.innerHTML = score;
    livesOutput.innerHTML = levels.currentLevel.lifeCounter;
    setValues(attenLeftOutput, attenRightOutput);
    
    setGridandDots();
    
    //waits for intro music to end to start game
    intro.addEventListener('ended', (event) => {
        gamestart = true;
    });

    //contols main came look
    if(gamestart & !gameover & !gamewin)
    {
        //mode manager handles different ghost stages
        if(currentLevel < 20){modeManager();}
        else{levels.currentLevel.mode = 'chase';}
        //round stages handle levelstart or pacman death modes
        manageRoundStages(levels.currentLevel.roundState);
        //pacman movement
        moveMsPacman();
        //ghost target system and ghost movement
        ghostTargetSystem();
    }

    //if collision occurs and ghosts aren't blue, pacman loses a life
    if(collisionOccurred)
    {gamestart = false;lostALife();}
    //eat all dots to win game
    if(levels.currentLevel.dotCount <= 0){gamewin = true;}

    //generates a new level
    if(gamewin)
    {
        winTimer += 1;
        gamestart = false;
        update = false;
        gameover = false;
        currentLives = levels.currentLevel.lifeCounter;
        setupNextLevel(currentLives);
    }
    //resets game and brings up main menu
    if(gameover)
    {
        gamestart = false;
        update = false;
        gamewin = false;
        resetGame();
    }
    requestAnimationFrame(render);

}

function modeManager()
{
    //run timer to update scatter and chase mode
    if(update){levelTime -=1;}
    //update scatter and chase mode
    if(levelTime <= 0)
    {
        currentModeIndex += 1;
        if(currentModeIndex < levels.currentLevel.targetTimers.length)
        {
            levels.currentLevel.mode = levels.currentLevel.targetTimers[currentModeIndex][0];
            levelTime = levels.currentLevel.targetTimers[currentModeIndex][1];
        }else{update=false; levels.currentLevel.mode = 'chase';}
    }
    //update frightened mode
    if(frightTimer >= levels.currentLevel.frightTimer)
    {
        models.blinky.diffuse = models.blinky.normalDif;
        models.pinky.diffuse = models.pinky.normalDif;
        models.inky.diffuse = models.inky.normalDif;
        models.sue.diffuse = models.sue.normalDif;
        if(currentModeIndex < levels.currentLevel.targetTimers.length)
        {
            update = true;
            levelTime = levels.currentLevel.targetTimers[currentModeIndex][1];
            levels.currentLevel.mode = levels.currentLevel.targetTimers[currentModeIndex][0];
        }else{levels.currentLevel.mode = 'chase';}
        frightTimer = 0;
    }
}

function ghostTargetSystem()
{
    //handles all the ghosts targeting systems
    var blinkTarget = new Target();
    var pinkTarget = new Target();
    var inkTarget = new Target();
    var sueTarget = new Target();

    if(levels.currentLevel.mode == 'chase')
    {
        blinkTarget.x = actors.msPac.x;
        blinkTarget.y = actors.msPac.y;
        blinkTarget.z = actors.msPac.z;

        sueTarget.x = scatterTargets.sueScatter.x;
        sueTarget.y = scatterTargets.sueScatter.y;
        sueTarget.z = scatterTargets.sueScatter.z;
        //pinky targets three cells in front of pacman
        var ppacNegX = actors.msPac.x - (CELL_WIDTH * 3);
        var ppacPosX = actors.msPac.x + (CELL_WIDTH * 3);
        var ppacNegY = actors.msPac.y - (CELL_HEIGHT * 3);
        var ppacPosY = actors.msPac.y + (CELL_HEIGHT * 3);
        //inky targets three cells behind pacman
        var ipacNegX = actors.msPac.x - (CELL_WIDTH * 3);
        var ipacPosX = actors.msPac.x + (CELL_WIDTH * 3);
        var ipacNegY = actors.msPac.y - (CELL_HEIGHT * 3);
        var ipacPosY = actors.msPac.y + (CELL_HEIGHT * 3);
        pinkTarget.z = actors.msPac.z;
        inkTarget.z = actors.msPac.z;
        // if Sue is within 7 grid spaces of Pacman she targets pacman directly otherwise she resumes her scatter AI
        if(Math.abs(actors.msPac.cellX - actors.Sue.cellX) <= 7 && Math.abs(actors.msPac.cellY - actors.Sue.cellY) <= 7)
        {
            sueTarget.x = actors.msPac.x;
            sueTarget.y = actors.msPac.y;
            sueTarget.z = actors.msPac.z;
        }

        if(actors.msPac.currentDirection == 'left')
        {
            pinkTarget.x = ppacNegX; 
            pinkTarget.y = actors.msPac.y;
            inkTarget.x = ipacPosX;
            inkTarget.y = actors.msPac.y;
        }
        if(actors.msPac.currentDirection == 'right')
        {
            pinkTarget.x = ppacPosX; 
            pinkTarget.y = actors.msPac.y;
            inkTarget.x = ipacNegX;
            inkTarget.y = actors.msPac.y;
        }
        if(actors.msPac.currentDirection == 'up')
        {
            pinkTarget.x = actors.msPac.x; 
            pinkTarget.y = ppacPosY;
            inkTarget.x = actors.msPac.x;
            inkTarget.y = ipacNegY;
        }
        if(actors.msPac.currentDirection == 'down')
        {
            pinkTarget.x = actors.msPac.x; 
            pinkTarget.y = ppacNegY;
            inkTarget.x = actors.msPac.x;
            inkTarget.y = ipacPosY;
        }
    }
    else if(levels.currentLevel.mode == 'scatter')
    {
        sueTarget.x = scatterTargets.sueScatter.x;
        sueTarget.y = scatterTargets.sueScatter.y;
        inkTarget = scatterTargets.inkyScatter;
        blinkTarget = scatterTargets.blinkyScatter;
        pinkTarget = scatterTargets.pinkyScatter;
    }
    
    moveGhosts(actors.Blinky, levels.currentLevel.blinkySpeed, blinkTarget, models.blinky, levels.currentLevel.bOffset);
    moveGhosts(actors.Pinky, levels.currentLevel.pinkySpeed, pinkTarget, models.pinky, levels.currentLevel.gOffset);
    moveGhosts(actors.Inky, levels.currentLevel.inkySpeed, inkTarget, models.inky, levels.currentLevel.gOffset);
    moveGhosts(actors.Sue, levels.currentLevel.sueSpeed, sueTarget, models.sue, levels.currentLevel.gOffset);
}

function moveMsPacman()
{
    //get current cell pacman is in
    var cellX = actors.msPac.cellX;
    var cellY = actors.msPac.cellY;
    //retrieves valid input for cells
    var validInputs = cells[cellY][cellX].inputs;
    getCellBounds(actors.msPac);
    
    if(cells[cellY][cellX].tag == 26)
    {
        validInputs = ['left', 'right'];
    }
     //check that the move is valid for the gridspace that ms pacman is in
    if(validInputs.includes(actors.msPac.nextDirection)
    & actors.msPac.nextDirection != actors.msPac.currentDirection)
    {
        //only updates the current direction when pacman is centered in the cell when changing axis
        if( ((actors.msPac.nextDirection == 'right' || actors.msPac.nextDirection == 'left') && 
        (actors.msPac.currentDirection == 'up' || actors.msPac.currentDirection == 'down')) ||
        ((actors.msPac.nextDirection == 'up' || actors.msPac.nextDirection == 'down') && 
        (actors.msPac.currentDirection == 'left' || actors.msPac.currentDirection == 'right')))
        {
            if((Math.abs(actors.msPac.x - cells[cellY][cellX].x) <= levels.currentLevel.pOffset && 
            Math.abs(actors.msPac.y - cells[cellY][cellX].y) <= levels.currentLevel.pOffset))
            {
                actors.msPac.x = cells[cellY][cellX].x;
                actors.msPac.y = cells[cellY][cellX].y;
                actors.msPac.currentDirection = actors.msPac.nextDirection;
                actors.msPac.nextDirection = null;
            }
        }
        else
        {
            //pacman is on the same axis and we don't want to center him
            actors.msPac.currentDirection = actors.msPac.nextDirection;
            actors.msPac.nextDirection = null;
        }
    }
    else{actors.msPac.nextDirection = null;}
    //check if the current direction of ms pacman is a valid direction for the cell
    getMovement(actors.msPac, levels.currentLevel.pacSpeed);
    //eat dots if pacman is in cell with dot
    if(cells[cellY][cellX].containsDot)
    {
        cells[cellY][cellX].containsDot = false;
        levels.currentLevel.dotCount -=1;
        score += 10;
        eatdot1.play();
        
    }
    //big dots to kill ghosts
    if(cells[cellY][cellX].containsSDot)
    {
        levels.currentLevel.dotCount -=1;
        score += 50;
        if(currentLevel < 20){levels.currentLevel.mode = 'frightened';}
        update = false;
        cells[cellY][cellX].containsSDot = false;    
    }

    //handles teleporting
    if(cells[cellY][cellX].containsTeleport)
    {
        TeleportActor(actors.msPac, cells[cellY][cellX].connectedTeleportj, cellY);
    }

    //sets material of pacman under texture to differ him from ghosts
    setLights(models.mspacman.ambient, models.mspacman.diffuse, models.mspacman.specular, models.mspacman.shininess);
    var instanceMatrix = mat4();
    var pacAngle = pacAngles[actors.msPac.currentDirection];
    instanceMatrix = mult(instanceMatrix, translate(actors.msPac.x, actors.msPac.y, actors.msPac.z));
    instanceMatrix = mult(instanceMatrix, rotateX(-45));
    instanceMatrix = mult(instanceMatrix, rotateZ(pacAngle));
    instanceMatrix = mult(instanceMatrix, scale(sval, sval, sval));
    
    //gl.bindTexture(gl.TEXTURE_2D, null);
    var t = mult(modelViewMatrix, instanceMatrix);
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
    //pacman texture
    var image = document.getElementById("pacimage");
    configureTexture(image);
    gl.drawArrays(gl.TRIANGLES, 72, 558);

}

function moveGhosts(actor, speed, target, model, offset)
{
    //identify cell ghost is in
    var cellX = actor.cellX;
    var cellY = actor.cellY;
    var curCell = cells[cellY][cellX];
    var valid = curCell.inputs;
    var oppositeDirection = getOpposite(actor.currentDirection);
    var input = valid.indexOf(oppositeDirection);
    var input1 = valid.slice(0, input);
    var input2 = valid.slice(input+1);
    var validInputs = input1.concat(input2);

    //get the bound of the cell the ghost is in and check for collision
    if(actor.cellX == actors.msPac.cellX && actor.cellY == actors.msPac.cellY)
    {
        if(levels.currentLevel.mode == 'chase' || levels.currentLevel.mode == 'scatter')
        {
            collisionOccurred = true; 
            death1.play();
            death1.addEventListener('ended', (event) => {
                death2.play();
            });
        }   
        else if(levels.currentLevel.mode == 'frightened')
        {
            actorKilled(actor);
            gdeath.play();
        }  
    }
    getCellBounds(actor);
    //handle movement for ghost house
    if(curCell.tag == 26)
    {cells[cellY+1][cellX].legal = false; validInputs = ['left', 'right'];}
    if(curCell.tag == 28){validInputs = ['up'];}
    if(curCell.tag == 27){validInputs = ['right'];}
    if(curCell.tag == 29){validInputs = ['left'];}
    //allows ghosts to use teleport
    if(cells[cellY][cellX].containsTeleport)
    {
        if(curCell.tag == 30 || curCell.tag == 32){validInputs = ['left'];}
        if(curCell.tag == 31 || curCell.tag == 33){validInputs = ['right'];}
        TeleportActor(actor, cells[cellY][cellX].connectedTeleportj, cellY);
    }
    //waits for scatter and chase mode
    if(levels.currentLevel.mode == 'scatter' || levels.currentLevel.mode == 'chase')
    {
        ghostCounter = 0;
        ghostPoints = 100;
        scatterAndChaseMechanics(actor, speed, cellX, cellY, curCell, validInputs, target, offset);
    }
    //waits for frightened mode when pacman eats ghost
    if(levels.currentLevel.mode == 'frightened')
    {
        //update material to blue, start timer & use frightened mechanics
        offset = offset/2;
        model.diffuse = vec4(0.0, 0.0, 1.0, 1.0);
        frightTimer += 1;
        update = false;
        frightenedMechanics(actor, speed/2, curCell, validInputs, offset);
        sdot.play();
    }
    //ghost materials under texture
    setLights(model.ambient, model.diffuse, model.specular, model.shininess);
    var instancematrix = mat4();
    instancematrix = mult(instancematrix, translate(actor.x, actor.y, actor.z));
    instancematrix = mult(instancematrix, scale(0.85, 0.85, 0.85));
    var t = mult(modelViewMatrix, instancematrix);
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
    gl.drawArrays(gl.TRIANGLES, 630, 426);
}
//gets the cell bounds for each cell for collision detection with pacman and ghosts
function getCellBounds(actor)
{
    var cellX = actor.cellX;
    var cellY = actor.cellY;

    if(actor.x < cells[cellY][cellX].left)
    {
        actor.cellX = (cellX - 1);
    }
    //update the cell to the rightmost cell if ms pacman moving right outside right bound
    if(actor.x > cells[cellY][cellX].right)
    {
        actor.cellX = cellX + 1;
    }
    if(actor.y > cells[cellY][cellX].top)
    {
        actor.cellY = cellY - 1;
    }
    if(actor.y < cells[cellY][cellX].bottom)
    {
        actor.cellY = cellY + 1;
    }
}

function manageRoundStages(mode)
{
    //uses different modes based on if it's the start of the level or if pacman died
    if(mode == 'levelStart')
    {
        elapsedInkyTime += 1;
        //release inky and sue after elapsed time or dot count is eaten
        if(elapsedInkyTime >= levels.currentLevel.timer || 
          ((levelData.dotCount - levels.currentLevel.dotCount) >= levels.currentLevel.iDotCount))
        {
            levels.currentLevel.inkySpeed = levels.currentLevel.enemySpeed;
            inkySet = true;
        }
        elapsedSueTime += 0.5;
        if(elapsedSueTime >= levels.currentLevel.timer || 
          ((levelData.dotCount- levels.currentLevel.dotCount) >= levels.currentLevel.sDotCount))
        {
            levels.currentLevel.sueSpeed = levels.currentLevel.enemySpeed;
            sueSet = true;
        }

        if(inkySet && sueSet)
        {
            elapsedInkyTime = 0;
            elapsedSueTime = 0;
            levels.currentLevel.roundState = "";
        }
    }
    else if(mode == 'msPacDied')
    {
        elapsedInkyTime += 2;
        //release inky and after elapsed time or dot count is eaten
        if(elapsedInkyTime >= levels.currentLevel.timer)
        {
            levels.currentLevel.inkySpeed = levels.currentLevel.enemySpeed;
            inkySet = true;
        }
        elapsedSueTime += 1;
        if(elapsedSueTime >= levels.currentLevel.timer)
        {
            levels.currentLevel.sueSpeed = levels.currentLevel.enemySpeed;
            sueSet = true;
        }

        if(inkySet && sueSet)
        {
            elapsedInkyTime = 0;
            elapsedSueTime = 0;
            levels.currentLevel.roundState = "";
        }
    }
}

function actorKilled(actor)
{
    //handles ghost house movement if ghost is killed
    var leftCell = ghostCells.leftGhostCell;
    var left = cells[leftCell.cellY][leftCell.cellX];
    var middleCell = ghostCells.middleGhostCell;
    var middle = cells[middleCell.cellY][middleCell.cellX];
    var rightCell = ghostCells.rightGhostCell;
    var right = cells[rightCell.cellY][rightCell.cellX];

    if(!left.containsGhost)
    {
        actor.x = left.x;
        actor.y = left.y;
        actor.z = left.z;
        actor.currentDirection = "right"
        actor.cellX = leftCell.cellX;
        actor.cellY = leftCell.cellY;
        left.containsGhost = true;
    }
    else if(!middle.containsGhost)
    {
        actor.x = middle.x;
        actor.y = middle.y;
        actor.z = middle.z;
        actor.currentDirection = "up"
        actor.cellX = middleCell.cellX;
        actor.cellY = middleCell.cellY;
        middle.containsGhost = true;
    }

    else if(!right.containsGhost)
    {
        actor.x = right.x;
        actor.y = right.y;
        actor.z = right.z;
        actor.currentDirection = "left"
        actor.cellX = rightCell.cellX;
        actor.cellY = rightCell.cellY;
        middle.containsGhost = true;
    }

    else if(left.containsGhost && middle.containsGhost && right.containsGhost)
    {
        actor.x = middle.x;
        actor.y = middle.y;
        actor.z = middle.z;
        actor.currentDirection = "up"
        actor.cellX = middleCell.cellX;
        actor.cellY = middleCell.cellY;
        middle.containsGhost = true;
    }
    actor.nextDirection = null;

    ghostCounter +=1;
    ghostPoints = ghostPoints * 2;
    score += ghostPoints;
}

function lostALife()
{
    //if pacman dies, reset all positions
    //can't rebuild level because dotcount will get reset
    deathtimer += 1;
    actors.msPac.x = actors.msPac.startX;
    actors.msPac.y = actors.msPac.startY;
    actors.msPac.z = actors.msPac.startZ;
    actors.msPac.currentDirection = 'left';
    actors.msPac.nextDirection = null;
    actors.msPac.cellX = actors.msPac.startCellX;
    actors.msPac.cellY = actors.msPac.startCellY;

    actors.Blinky.x = actors.Blinky.startX;
    actors.Blinky.y = actors.Blinky.startY;
    actors.Blinky.z = actors.Blinky.startZ;
    actors.Blinky.currentDirection = 'left';
    actors.Blinky.nextDirection = null;
    actors.Blinky.cellX = actors.Blinky.startCellX;
    actors.Blinky.cellY = actors.Blinky.startCellY;

    actors.Pinky.x = actors.Pinky.startX;
    actors.Pinky.y = actors.Pinky.startY;
    actors.Pinky.z = actors.Pinky.startZ;
    actors.Pinky.currentDirection = 'up';
    actors.Pinky.nextDirection = null;
    actors.Pinky.cellX = actors.Pinky.startCellX;
    actors.Pinky.cellY = actors.Pinky.startCellY;

    actors.Inky.x = actors.Inky.startX;
    actors.Inky.y = actors.Inky.startY;
    actors.Inky.z = actors.Inky.startZ;
    actors.Inky.currentDirection = 'right';
    actors.Inky.nextDirection = null;
    actors.Inky.cellX = actors.Inky.startCellX;
    actors.Inky.cellY = actors.Inky.startCellY;
    levels.currentLevel.inkySpeed = 0;

    actors.Sue.x = actors.Sue.startX;
    actors.Sue.y = actors.Sue.startY;
    actors.Sue.z = actors.Sue.startZ;
    actors.Sue.currentDirection = 'left';
    actors.Sue.nextDirection = null;
    actors.Sue.cellX = actors.Sue.startCellX;
    actors.Sue.cellY = actors.Sue.startCellY;
    levels.currentLevel.sueSpeed = 0;
    levels.currentLevel.roundState = "msPacDied";
    if(deathtimer >= 200)
    {
        levels.currentLevel.lifeCounter -=1;
        if(levels.currentLevel.lifeCounter < 1)
        {
            gameover = true;
        }
        inkySet = false;
        sueSet = false;
        elapsedInkyTime = 0;
        elapsedSueTime = 0;
        if(currentModeIndex < levels.currentLevel.targetTimers.length)
        {
            update = true;
            levelTime = levels.currentLevel.targetTimers[currentModeIndex][1];
        }
        collisionOccurred = false;
        gamestart = true;
        deathtimer = 0;
    }
}

function resetGame()
{
    //resets all variables back to normal if gameover occurs
    var d = levelData;
    var l1 = levels.level1;
    score = 0;
    nextLevel = 2;
    currentLevel = 1;
    levels.currentLevel = l1;
    //reset generated leveldata back to original 
    d.pacSpeed = l1.pacSpeed;
    d.blinkySpeed = l1.blinkySpeed;
    d.ghostSpeed = l1.enemySpeed;
    d.scatter1 = l1.s1;
    d.scatter2= l1.s2;
    d.scatter3 = l1.s3;
    d.timer = l1.timer;
    d.frightenedTime = l1.frightTimer;
    d.pacOffset = l1.pOffset;
    d.blinkyOffset = l1.bOffset;
    d.ghostOffset = l1.gOffset;
    console.log(levelData);
    resetVariables();
    levels.currentLevel.dotCount = levelData.dotCount; 
    levels.currentLevel.lifeCounter = totalLives;
    buildLevel();
    levels.currentLevel.roundState = 'levelStart';
    levels.currentLevel.mode = 'scatter';
    menuContainer.style.display = 'block';
    menuClickHandler(start, orth, pers, spot, menuContainer);
}

function setupNextLevel(cLives)
{   
    intro.play();
    //generates a new level
    if(winTimer >= 250)
    {
        createNewLevel();
        resetVariables();
        levels.currentLevel.lifeCounter = cLives;
        buildLevel();
        levels.currentLevel.roundState = 'levelStart';
        currentLevel +=1;
        nextLevel +=1;
        gamestart = true;
        update = true;
        gamewin = false;
        winTimer = 0;  
    }
}

function createNewLevel()
{
    //code for creating a new level
    var d = levelData;
    //if we're going into level 5, 10, 15, or 20 there are speed increases and decreased
    //scatter time and ghosts turn blue for less time
    if(currentLevel == 4 || currentLevel == 9 || currentLevel == 14 || currentLevel == 19)
    {
        d.pacSpeed += d.speedOffsetInc;
        d.blinkySpeed += d.speedOffsetInc;
        d.ghostSpeed += d.speedOffsetInc;
        d.scatter1 -= d.scatterTimeDec;
        d.scatter2 -= d.scatterTimeDec;
        d.scatter3 -= d.scatterTimeDec;
        d.timer -= d.timerdec;
        d.frightenedTime -= d.frightTimeDec;
        d.pacOffset += d.speedOffsetInc;
        d.blinkyOffset += d.speedOffsetInc;
        d.ghostOffset += d.speedOffsetInc;

        var tempLevel = new Level(pink, L1_GRID_WIDTH, L1_GRID_HEIGHT, d.pacSpeed, d.blinkySpeed,
            d.ghostSpeed, d.dotCount, "LevelStart", "scatter", d.scatter1, d.scatter2, d.scatter3, 
            2000, 2000, 18, 36, d.timer, d.frightenedTime, d.pacOffset, d.blinkyOffset, d.ghostOffset);
        levels.currentLevel = tempLevel;
    }
    else
    {
        //if we're not on level 5, 10, or 15 our level variables aren't updated
        var tempLevel = new Level(pink, L1_GRID_WIDTH, L1_GRID_HEIGHT, d.pacSpeed, d.blinkySpeed, 
            d.ghostSpeed, d.dotCount, "LevelStart", "scatter", d.scatter1, d.scatter2, d.scatter3,
            2000, 2000, 18, 36, d.timer, d.frightenedTime, d.pacOffset, d.blinkyOffset, d.ghostOffset);
        levels.currentLevel = tempLevel;
    }
}

function resetVariables(){
    //reset variables that reset every level/gameover
    inkySet=false; sueSet=false; collisionOccurred = false;
    currentModeIndex = 0; ghostPoints = 100; ghostCounter = 0;
    elapsedInkyTime = 0; elapsedSueTime = 0; deathtimer = 0; frightTimer = 0;
    levelTime = levels.currentLevel.targetTimers[currentModeIndex][1];
}

function configureTexture(image, texture) {
    //configures textures
    texture = gl.createTexture();
    gl.bindTexture(gl.TEXTURE_2D, texture);
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGB,
         gl.RGB, gl.UNSIGNED_BYTE, image);
    gl.generateMipmap(gl.TEXTURE_2D);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER,
                      gl.NEAREST_MIPMAP_LINEAR);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);

    gl.uniform1i(gl.getUniformLocation(program, "uSampler"), 0);
}

function setGridandDots()
{
    //wall and dot texture
    var image = document.getElementById("wallimage");
    configureTexture(image, wallTexture);
    setLights(models.dot.ambient, models.dot.diffuse, models.dot.specular, models.dot.shininess);
    //iterate through the cell grid and read cell tag to indicate if it's a wall a dot or big dot
    //cell contents initalized in gridconfig
    for(var i =0; i < cells.length; i++)
    {
        for(var j = 0; j < cells[i].length; j++)
        {
            var pos = cells[i][j];
            
            if(cells[i][j].tag == 1 || (cells[i][j].tag >= 8 & cells[i][j].tag <= 11) | cells[i][j].tag ==2)
            {
                var instancematrix = mat4();
                instancematrix = mult(instancematrix, translate(pos.x, pos.y, pos.z));
                var t = mult(modelViewMatrix, instancematrix);
                gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
                gl.drawArrays(gl.TRIANGLES, 0, 36);
                
            }
            if(cells[i][j].containsDot)
            {
                
                var instancematrix = mat4();
                instancematrix = mult(instancematrix, translate(pos.x, pos.y, pos.z));
                instancematrix = mult(instancematrix, scale(0.2, 0.2, 0.2));
                var t = mult(modelViewMatrix, instancematrix);
                gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
                gl.drawArrays(gl.TRIANGLES, 36, 36);
            }
            if(cells[i][j].containsSDot)
            {
                var instancematrix = mat4();
                instancematrix = mult(instancematrix, translate(pos.x, pos.y, pos.z));
                instancematrix = mult(instancematrix, scale(0.4, 0.4, 0.4));
                var t = mult(modelViewMatrix, instancematrix);
                gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
                gl.drawArrays(gl.TRIANGLES, 36, 36);
            }
            
        }
    }
}