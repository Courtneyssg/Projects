"use strict";

var canvas;
var gl;
var aspect;
var onStart = true;

var modelViewMatrix, projectionMatrix;

//flags for transformations
var rotationFlag = true;
var uniformScaleFlag = true;
var nonUniformScaleFlag = true;
var backAndForthFlag = true;
var translateFlag = true;
//transformation params for rock
var rockXPos, rockYPos, rockZPos;
var rPos = [-0.8, -0.8, 0, 0.8, -0.8, 0, 0, -0.8, 0.8];
var rindex = 0;
var currentTime = 5;
var TotalTime = 5;
//transformation params for fence
var fenceScaleDown = false;
var fenceScale = 0;
var fenceMaxScale = 1.0;
var fenceMinScale = 0.1;
//transformation params for tent
var xAxis = 0;
var yAxis = 1;
var zAxis = 2;
var axis = 0;
var the = vec3(0, 0, 0);
//transformation params for log
var logScaleDown = false;
var logScale = 0;
var maxLogScale = 1.0;
var minLogScale = 0.1;
//transformation params for tree
var canMoveNeg = false;
var maxTreePos = 0.8;
var minTreePos = -0.8;
var treeXPos = -0.8;
//orthographic & perspective params
var lefto = -1;
var righto = 1;
var topo = 1;
var bottomo = -1;
var near = 0.3;
var far = 20.0;
var radius = 4.0;
var theta = 0.0;
var phi = 0.0;
var  fovy = 45.0; 
//eye coordinates & lookat params
var eye;
var at = vec3(0.0, 0.0, 0.0);
const up = vec3(0.0, 1.0, 0.0);
var lAt = vec3(0.0, 0.0, 0.0);
//sets default view to perspective
var projectionflag = true;
//variables for ambient slider
var amSlider, amOutput;
//rotate
var y = 0;

window.onload = function init() {

	//get canvas from html
    canvas = document.getElementById("gl-canvas");
    gl = canvas.getContext('webgl2');
    if (!gl) alert("WebGL 2.0 isn't available" );
	//set viwport and color

    gl.viewport(0, 0, canvas.width, canvas.height);
    gl.clearColor(135.0/255, 206.0/255, 235.0/255, 1.0);
    gl.enable(gl.DEPTH_TEST);
	//set aspect ratio for perspective viewing
	aspect =  canvas.width/canvas.height;
	//ambient slider
	amSlider = document.getElementById("am-select");
    amOutput = document.getElementById("am-value");

	fixed1 = document.getElementById("fixed-1");
    fixed1b = document.getElementById("fixed-1-output");

	fixed2 = document.getElementById("fixed-2");
    fixed2b = document.getElementById("fixed-2-output");

	moving1 = document.getElementById("moving");
    movingb = document.getElementById("moving-output");

	headlamp1 = document.getElementById("headlamp");
    headlampb = document.getElementById("headlamp-output");

	top1 = document.getElementById("top");
    topb = document.getElementById("top-output");

	left1 = document.getElementById("left");
	leftb = document.getElementById("left-output");

	right1 = document.getElementById("right");
    rightb = document.getElementById("right-output");
	
	//create program and initalize shaders
	program = initShaders(gl, "vertex-shader", "fragment-shader");
	gl.useProgram(program);
	
	//create objects with material properties
	createObjects();
	//set vertices and normals
	setNormals();
	//create lightSources
	createLighting();
	//create vertex buffer
    var vBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, vBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, flatten(positionsArray), gl.STATIC_DRAW);
	//get location of vertices in shader and specify parameters
    var positionLoc = gl.getAttribLocation(program, "aPosition");
    gl.vertexAttribPointer(positionLoc, 4, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(positionLoc);
	//create normals buffer
	var nBuffer = gl.createBuffer();
    gl.bindBuffer(gl.ARRAY_BUFFER, nBuffer);
    gl.bufferData(gl.ARRAY_BUFFER, flatten(normalsArray), gl.STATIC_DRAW);
	//get location of normals in shader and specify parameters
    var normalLoc = gl.getAttribLocation(program, "aNormal");
    gl.vertexAttribPointer(normalLoc, 4, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(normalLoc);
	//flag for switching to phong shading so I didn't have to use different shaders
	fragFlagLoc = gl.getUniformLocation(program, "fragFlag");
	//default is set to phong
	fragFlag = 1.0;
	gl.uniform1f(gl.getUniformLocation(program,"fragFlag"), fragFlag);
	InputHandler();
	//set projection matrix
	projectionMatrix = setProjectionMatrix();
    render();
}

var render = function(){
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
	//check if light sources are on
	checkFixedFlag();
	//moving point light
	MovePointLight();
	eye = vec3(radius*Math.sin(theta)*Math.cos(phi),(radius*Math.sin(theta)*Math.sin(phi)), radius*Math.cos(theta)); //eye
	modelViewMatrix = lookAt(eye, at, up);
	//check if orthographic view is enabled and scale the scene
	if(orthoFlag)
	{
		modelViewMatrix = mult(modelViewMatrix, scale(orthoScale, orthoScale, orthoScale));
	}
	//draw function for stage
	DrawStage();
	// animation for tree
	if(backAndForthFlag)
	{
		if(treeXPos >= maxTreePos){canMoveNeg = true;}
		if(treeXPos <= minTreePos) {canMoveNeg = false;}

		if(canMoveNeg){treeXPos -= 0.01;}
		else{treeXPos += 0.01;}
	}
	//draw function for tree
	DrawTree();
	//animation for tent
		axis = yAxis;
		if(rotationFlag) the[axis] += 2.0;
	//draw function for tent
	DrawTent();
	//animation for fence
	if(uniformScaleFlag)
	{
		if( fenceScale >= fenceMaxScale){fenceScaleDown = true;}
		if(fenceScale <= fenceMinScale){fenceScaleDown = false;}
		if(fenceScaleDown){fenceScale -= 0.005;}
		else {fenceScale += 0.005;}
	}
	//draw function for fence
	DrawFence();
	//animation for rock
	if(translateFlag)
	{
		//moves rock by translating to different position
		//endabled by default, press 2 key to start or stop animation
		if(rindex >= rPos.length){rindex = 0;}
		currentTime += 0.1;
        var dt = TotalTime - currentTime;
        if(dt <= 0)
        {
			rockXPos = rPos[rindex];
			rockYPos = rPos[rindex+1];
			rockZPos = rPos[rindex+2];
			rindex += 3;
            currentTime = 0;
        }
	}
	//draw function for rock
	DrawRock();
	//animation for log
	if(nonUniformScaleFlag)
	{
		if(logScale >= maxLogScale){logScaleDown = true;}
		if(logScale <= minLogScale){logScaleDown = false;}
		
		if(logScaleDown){logScale -= 0.005;}
		else {logScale += 0.005;}

	}
	//draw function for log
	DrawLog();

	DrawLightGeometry(666, 36, lightSources[0].pos);
	DrawLightGeometry(666, 36, lightSources[1].pos);
	DrawLightGeometry(666, 36, lightSources[2].pos);
    requestAnimationFrame(render);
}