"use strict";

/*******************
 * Courtney St. George 2009917250
 * This file contains functions for initalization, input handlers, sending vertex data, normals and texture data,
 * Light source data, and method for switching Projection matrix view
*******************/

var index = 0;
//info to send to shaders
var positionsArray = [];
var normalsArray = [];
var colorsArray = [];
var texCoordsArray = [];
var difficulty = 'pers';
var orthoFlag = true;
var changeLights = true;
//light initalization for colors, position, and attenuation
function initLights()
{
    var leftcamlight = new Light(vec4(0.2, 0.2, 0.2, 0.2), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(0.0, 0.0, 0.0, 1.0), 1.0);
    lights.cameraLeftLight = leftcamlight;
    var rightcamlight = new Light(vec4(0.2, 0.2, 0.2, 0.2), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(0.0, 0.0, 1.0, 1.0), 1.0);
    lights.cameraRightLight = rightcamlight;
    var spotlight = new Light(vec4(0.1, 0.1, 0.1, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(0.1, 0.1, 0.1, 1.0), vec4(0.0, 0.0, 3.0, 1.0), 1.0);
    lights.spotlight = spotlight;
}
//initalize models
function initModels()
{
    var cube = new Model('cube', cubeVertices, cubeIndices, vec4(1.0, 1.0, 1.0, 1.0), 
                        vec4(255.0/255.0, 116.0/255.0, 161.0/255.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.cube = cube;
    var pac = new Model('pac', msPacVertices, msPacIndices, vec4(1.0, 1.0, 1.0, 1.0),
                        vec4(1.0, 1.0, 0.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.mspacman = pac;
    var dot = new Model('dot', cubeVertices, cubeIndices, vec4(1.0, 1.0, 1.0, 1.0),
                        vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.dot = dot;
    var blinky = new Model('blinky', ghostVertices, ghostIndices, vec4(1.0, 1.0, 1.0, 1.0),
                        vec4(191.0/255.0, 51.0/255.0, 51.0/255.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.blinky = blinky;
    var pinky = new Model('pinky', ghostVertices, ghostIndices, vec4(1.0, 1.0, 1.0, 1.0),
    vec4(245.0/255.0, 146.0/255.0, 238.0/255.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.pinky = pinky;
    var inky = new Model('inky', ghostVertices, ghostIndices, vec4(1.0, 1.0, 1.0, 1.0),
    vec4(15.0/255.0, 167.0/255.0, 179.0/255.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.inky = inky;
    var sue = new Model('sue', ghostVertices, ghostIndices, vec4(1.0, 1.0, 1.0, 1.0),
    vec4(255.0/255.0, 130.0/255.0, 67.0/255.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    models.sue = sue;
    quad(models.cube);
    quad(models.dot);
    tri(models.mspacman);
    tri(models.blinky);
    
}
//models created with quads
function quad(model) {
    var ind = model.indices;
    var v = model.vertices;
    for(var i = 0; i < ind.length; i+=4)
    {
        quads(v[ind[i]], v[ind[i+1]], v[ind[i+2]], v[ind[i+3]]);
    }   
}
//handles four vertices
function quads(a, b, c, d)
{
    //normals based on game mode
    if(difficulty == 'orth' || difficulty == 'pers')
    {
        normalsArray.push(vec4(a[0], a[1], a[2], 0.0));
        normalsArray.push(vec4(b[0], b[1], b[2], 0.0));
        normalsArray.push(vec4(c[0], c[1], c[2], 0.0));
        normalsArray.push(vec4(a[0], a[1], a[2], 0.0));
        normalsArray.push(vec4(c[0], c[1], c[2], 0.0));
        normalsArray.push(vec4(d[0], d[1], d[2], 0.0));
    }
    else if(difficulty == 'spot')
    {
        var t1 = subtract(b, a);
        var t2 = subtract(c, b);
        var normal = normalize(cross(t1, t2));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
    }

    positionsArray.push(a);
    texCoordsArray.push(texquad[0]);
    positionsArray.push(b);
    texCoordsArray.push(texquad[1]);
    positionsArray.push(c);
    texCoordsArray.push(texquad[2]);
    positionsArray.push(a);
    texCoordsArray.push(texquad[0]);
    positionsArray.push(c);
    texCoordsArray.push(texquad[2]);
    positionsArray.push(d);
    texCoordsArray.push(texquad[3]);

}
//handles three vertices
function tri(model) {
    var ind = model.indices;
    var v = model.vertices;
    for(var i = 0; i < ind.length; i+=3)
    {
        triangles(v[ind[i]], v[ind[i+1]], v[ind[i+2]]);
    }   
}

function triangles(a, b, c)
{
    //normals based on game mode
    if(difficulty == 'orth' || difficulty == 'pers')
    {
        normalsArray.push(vec4(a[0], a[1], a[2], 0.0));
        normalsArray.push(vec4(b[0], b[1], b[2], 0.0));
        normalsArray.push(vec4(c[0], c[1], c[2], 0.0));
    }
    else if(difficulty == 'spot')
    {
        var t1 = subtract(b, a);
        var t2 = subtract(c, a);
        var normal = normalize(cross(t1, t2));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
    }
    

    positionsArray.push(a);
    texCoordsArray.push(textri[0]);
    positionsArray.push(b);
    texCoordsArray.push(textri[1]);
    positionsArray.push(c);
    texCoordsArray.push(textri[2]);
}

function initGame()
{
    //create levels
    createLevels();
    //create game grid
    createGrid(levels.level1.grid, levels.level1.width, levels.level1.height);
    //initalize models
    initModels();
    //initalize lights
    initLights();
}
//send light and material data to shader
function setLights(am, dif, spec, sh)
{

    var ambientProductLeft = mult(lights.cameraLeftLight.ambient, am);
    var diffuseProductLeft = mult(lights.cameraLeftLight.diffuse, dif);
    var specularProductLeft = mult(lights.cameraLeftLight.specular, spec);

    var ambientProductRight = mult(lights.cameraRightLight.ambient, am);
    var diffuseProductRight = mult(lights.cameraRightLight.diffuse, dif);
    var specularProductRight = mult(lights.cameraRightLight.specular, spec);

    var ambientProductSpot = mult(lights.spotlight.ambient, am);
    var diffuseProductSpot = mult(lights.spotlight.diffuse, dif);
    var specularProductSpot = mult(lights.spotlight.specular, spec);

    var at = (vec3(actors.msPac.x * levels.currentLevel.pacSpeed, actors.msPac.y * levels.currentLevel.pacSpeed,
                    actors.msPac.z* levels.currentLevel.pacSpeed));
    var lightDirection = normalize(subtract(at, 
        vec3(lights.spotlight.position[0], lights.spotlight.position[1], lights.spotlight.position[2])));
    var limit = Math.cos(4 * Math.PI/180);


    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientLeft"),ambientProductLeft);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseLeft"),diffuseProductLeft);
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularLeft"),specularProductLeft);
    gl.uniform4fv(gl.getUniformLocation(program, "uLeftPosition"),lights.cameraLeftLight.position);
    gl.uniform1f(gl.getUniformLocation(program, "uAttenuationLeft"),lights.cameraLeftLight.attenuation);

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientRight"), ambientProductRight);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseRight"),diffuseProductRight);
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularRight"),specularProductRight);
    gl.uniform4fv(gl.getUniformLocation(program, "uRightPosition"),lights.cameraRightLight.position);
    gl.uniform1f(gl.getUniformLocation(program, "uAttenuationRight"),lights.cameraRightLight.attenuation);

    gl.uniform1f(gl.getUniformLocation(program,"uLimit"), limit);
    gl.uniform3fv(gl.getUniformLocation(program, "uLightDirection"),lightDirection);
    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientSpot"), ambientProductSpot);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseSpot"), diffuseProductSpot);
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularSpot"), specularProductSpot);
    gl.uniform4fv(gl.getUniformLocation(program, "uSpotPosition"),lights.spotlight.position);
    gl.uniform1f(gl.getUniformLocation(program,"uShininessSpot"), sh);

}
//sets the values for adjusting attenuation in game
function setValues(leftout, rightout)
{
    lights.cameraLeftLight.attenuation = leftout.innerHTML;
    lights.cameraRightLight.attenuation = rightout.innerHTML;
}

function InputHandler()
{

    document.addEventListener('keydown', logDown);
    document.addEventListener('keyup', logUp)
    var wflag = true;
    var sflag = true;
    var aflag = true;
    var dflag = true;
    var leftflag = false;
    var rightflag = false;
    var paused = false;
    function logDown(key)
    {
        switch(key.key)
        {
            case 'w':
                if (wflag & gamestart)
                {actors.msPac.nextDirection = "up"; wflag = false;}
                break;
            case 's':
                if(sflag & gamestart)
                {actors.msPac.nextDirection = "down"; sflag = false;}
                break;
            case 'd':
                if(dflag & gamestart)
                {actors.msPac.nextDirection = "right"; dflag = false;}
                break;
            case 'a':
                if(aflag & gamestart)
                {actors.msPac.nextDirection = "left"; aflag = false;}
                break;
            case 'p':
                gamestart = !gamestart;
                paused = !paused;
            //activates left light to change color
            case '1':
                if(changeLights){leftflag = true;}
                break;
            //activates right light to change color
            case '2':
                if(changeLights){rightflag = true;}
                break;
            //check if flags are triggered to change light color to red
            case 'r':
                if(leftflag){lights.cameraLeftLight.ambient = vec4(0.7, 0.0, 0.0, 1.0); leftflag=false;}
                if(rightflag){lights.cameraRightLight.ambient = vec4(0.7, 0.0, 0.0, 1.0); rightflag=false;}
                break;
            //check if flags are triggered to change light color to green
            case 'g':
                if(leftflag){lights.cameraLeftLight.ambient = vec4(0.0, 0.7, 0.0, 1.0); leftflag=false;}
                if(rightflag){lights.cameraRightLight.ambient = vec4(0.0, 0.7, 0.0, 1.0); rightflag=false;}
                break;
            //check if flags are triggered to change light color to blue
            case 'b':
                if(leftflag){lights.cameraLeftLight.ambient = vec4(0.0, 0.0, 0.7, 1.0); leftflag=false;}
                if(rightflag){lights.cameraRightLight.ambient = vec4(0.0, 0.0, 0.7, 1.0); rightflag=false;}
                break;
            //check if flags are triggered to change light color back to normal
            case 'x':
                if(leftflag){lights.cameraLeftLight.ambient = vec4(0.2, 0.2, 0.2, 1.0); leftflag=false;}
                if(rightflag){lights.cameraRightLight.ambient = vec4(0.2, 0.2, 0.2, 1.0); rightflag=false;}
                break;
            case 'j':
                theta -= 0.05
                break;
            case 'l':
                theta += 0.05;
                break;
            case 'i':
                lights.cameraRightLight.position[2] += 0.05;
                break;
            case 'k':
                lights.cameraRightLight.position[2] -= 0.05;
                break;
        }
    }
    function logUp(key)
    {
        switch(key.key)
        {
            case 'w': wflag = true; break;
            case 's': sflag = true; break;
            case 'a': aflag = true; break;
            case 'd': dflag = true; break;
        }
    }
}
//main menu listeners
function menuClickHandler(start, orth, pers, spot, menuContainer)
{
    start.addEventListener('click', event => {
        setProjectionMatrix();
        updateLights();
        intro.play();
        menuContainer.style.display = "none";
        update = true;
        gameover = false;
        
    });
    orth.addEventListener('click', event => {
        difficulty = 'orth';
        changeLights = true;
        orth.style.border = "5px solid #ffffff";
        pers.style.border = "5px solid #fdbcb4";
        spot.style.border = "5px solid #fdbcb4";
    });
    pers.addEventListener('click', event => {
        difficulty = 'pers';
        changeLights = true;
        pers.style.border = "5px solid #ffffff";
        orth.style.border = "5px solid #fdbcb4";
        spot.style.border = "5px solid #fdbcb4";
    });
    spot.addEventListener('click', event => {
        difficulty = 'spot';
        changeLights = false;
        spot.style.border = "5px solid #ffffff";
        pers.style.border = "5px solid #fdbcb4";
        orth.style.border = "5px solid #fdbcb4";
    });
      
}
//sets projection based on game input
function setProjectionMatrix()
{
    var projection;
	if(difficulty == 'orth'){projection = ortho(oleft, oright, obottom, otop, near, far); orthoFlag = true;}
	if(difficulty == 'pers' || difficulty == 'spot'){projection = perspective(fovy, aspect, near, far); orthoFlag = false;}
	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uProjectionMatrix"), false, flatten(projection));
	return projection;
}
//updates lights based on changing colors
function updateLights()
{
    var left = lights.cameraLeftLight;
    var right = lights.cameraRightLight;
    var spot = lights.spotlight;
    var zeros = vec4(0.0, 0.0, 0.0, 1.0);
    if(difficulty == 'spot')
    {
        left.ambient = zeros; left.diffuse = zeros; left.specular = zeros;
        right.ambient = zeros; right.diffuse = zeros; right.specular = zeros;
        spot.ambient = spot.fixedAm; spot.diffuse = spot.fixedDif; spot.specular = spot.fixedSpec;
    }
    else
    {
        left.ambient = left.fixedAm; left.diffuse = left.fixedDif; left.specular = left.fixedSpec;
        right.ambient = right.fixedAm; right.diffuse = right.fixedDif; right.specular = right.fixedSpec;
        spot.ambient = zeros; spot.diffuse = zeros; spot.specular = zeros;
    }
}
//creates inital beginning level
function createLevels()
{
    //level 1 holds start level information for gameover
    var level1 = new Level(pink, L1_GRID_WIDTH, L1_GRID_HEIGHT, 0.004, 0.003, 0.002, 153,
        'levelStart', 'scatter', 1000, 1000, 700, 2000, 2000, 18, 36, 500, 1500, 0.0075, 0.004, 0.003);
    levels.level1 = level1;
    levels.currentLevel = level1;
}
//handles light movement
function moveLights()
{
    //this function moves the two active light sources in hierarachy with each other
    var radius = 1.0;
    var right = lights.cameraRightLight.position;
    var left = lights.cameraLeftLight.position;
    var instanceMatrix = scale(0.8, 0.8, 0.8);
    drawMiddle(instanceMatrix);
    left[0] = right[0] + radius*(Math.sin(theta)*Math.cos(phi));
    left[1] = right[1] + radius*(Math.cos(theta));
    left[2] = right[2] + radius*(Math.sin(theta)*Math.sin(phi));
    instanceMatrix = mult(instanceMatrix, translate(left[0], left[1], left[2]));
    drawRotating(instanceMatrix);
}
function drawMiddle(instance)
{
    //draws middle light
    var right = lights.cameraRightLight.position;
    instance = mult(instance, translate(right[0], right[1], right[2]));
    var t = mult(modelViewMatrix, instance);
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
    gl.drawArrays(gl.LINE_LOOP, 0, 36);
}

function drawRotating(instance)
{
    //draws light that rotates around
    var t = mult(modelViewMatrix, instance);
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
    gl.drawArrays(gl.LINE_LOOP, 0, 36);
}