"use strict";

var stage, tree, tent, fence, rock, log;
var orthoFlag = false;
var orthoScale = 1.0;
var gouraud = false;
var flat = false;
var phong = true;
var vertList = [stageVertices, treeVertices, tentVertices, fenceVertices, rockVertices, logVertices, lightVertices];
var lightSources = [];
var shapesArray = [];
var positionsArray = [];
var normalsArray = [];
var index = 0;
var ambientValue;
var fragFlag, fragFlagLoc;

var minLightYPos = -2.0;
var maxLightYPos = 2.0;

var canMoveUp = true;

var fixed1, fixed2, moving1, headlamp1, left1, right1, top1;
var fixed1b, fixed2b, movingb, headlampb, leftb, rightb, topb;

function createObjects()
{
    //shapes with attached indices and materials
    stage = new Shape(stageindices, vec4(0.2, 0.2, 0.2, 1.0), vec4(34.0/255, 145.0/255, 80.0/255, 1.0),   vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    shapesArray.push(stage);
    tree = new Shape(treeindices,   vec4(0.2, 0.2, 0.2, 1.0), vec4(18.0/255, 42.0/255, 11.0/255, 1.0),    vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    shapesArray.push(tree);
    tent = new Shape(tentindices,   vec4(0.2, 0.2, 0.2, 1.0), vec4(214.0/255, 109.0/255, 141.0/255, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    shapesArray.push(tent);
    fence = new Shape(fenceindices, vec4(0.2, 0.2, 0.2, 1.0), vec4(202.0/255, 159.0/255, 125.0/255, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    shapesArray.push(fence);
    rock = new Shape(rockindices,   vec4(0.2, 0.2, 0.2, 1.0), vec4(139.0/255, 139.0/255, 139.0/255, 1.0), vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    shapesArray.push(rock);
    log = new Shape(logindices,     vec4(0.2, 0.2, 0.2, 1.0), vec4(119.0/255, 85.0/255, 58.0/255, 1.0),   vec4(1.0, 1.0, 1.0, 1.0), 20.0);
    shapesArray.push(log);
}

function createLighting()
{
    //creates all the light sources
    var fixedsource1 = new LightSource(vec4(1.0, 1.0, 1.0, 0.0), vec4(0.2, 0.2, 0.2, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), true);
    lightSources.push(fixedsource1);
    UpdateSources(lightSources[0], 0);
    var fixedsource2 = new LightSource(vec4(-1.0, 1.0, 1.0, 0.0), vec4(0.2, 0.2, 0.2, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), true);
    lightSources.push(fixedsource2);
    UpdateSources(lightSources[1], 1);
    var movingsource = new LightSource(vec4(0.0, 0.0, -1.0, 1.0), vec4(0.5, 0.5, 0.5, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), true);
    lightSources.push(movingsource);
    UpdateSources(lightSources[2], 2);
    var spotlight= new LightSource(vec4(0.0, 0.0, 4.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(1.0, 1.0, 1.0, 1.0), false);
    lightSources.push(spotlight);
    UpdateSources(lightSources[3], 3);
    var camLeftsource = new LightSource(vec4(-1.0, 0.0, -1.0, 0.0), vec4(1.0, 0.0, 0.0, 1.0), vec4(1.0, 0.0, 0.0, 1.0), vec4(1.0, 0.0, 0.0, 1.0), false);
    lightSources.push(camLeftsource);
    UpdateSources(lightSources[4], 4);
    var camRightsource = new LightSource(vec4(1.0, 0.0, -1.0, 0.0), vec4(0.0, 1.0, 0.0, 1.0), vec4(0.0, 1.0, 0.0, 1.0), vec4(0.0, 1.0, 0.0, 1.0), false);
    lightSources.push(camRightsource);
    UpdateSources(lightSources[5], 5);
    var camTopsource = new LightSource(vec4(0.0, 1.0, -1.0, 0.0), vec4(0.0, 0.0, 1.0, 1.0), vec4(0.0, 0.0, 1.0, 1.0), vec4(0.0, 0.0, 1.0, 1.0), false);
    lightSources.push(camTopsource);
    UpdateSources(lightSources[6], 6);
    
}

//push vertices and normals when using quad
function quad(a, b, c, d) {
    var vertices = vertList[index];
    if(gouraud | phong)
    {
        //true vertex normals
        normalsArray.push(vec4(vertices[a][0],vertices[a][1], vertices[a][2], 0.0));
        normalsArray.push(vec4(vertices[b][0],vertices[b][1], vertices[b][2], 0.0));
        normalsArray.push(vec4(vertices[c][0],vertices[c][1], vertices[c][2], 0.0));
        normalsArray.push(vec4(vertices[a][0],vertices[a][1], vertices[a][2], 0.0));
        normalsArray.push(vec4(vertices[c][0],vertices[c][1], vertices[c][2], 0.0));
        normalsArray.push(vec4(vertices[d][0],vertices[d][1], vertices[d][2], 0.0));
    }
    
    if(flat)
    {
        //normals computed from three vertices
        var t1 = subtract(vertices[b], vertices[a]);
        var t2 = subtract(vertices[c], vertices[b]);
        var normal = normalize(cross(t1, t2));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
    }
    
    positionsArray.push(vertices[a]);
    positionsArray.push(vertices[b]);
    positionsArray.push(vertices[c]);
    positionsArray.push(vertices[a]);
    positionsArray.push(vertices[c]);
    positionsArray.push(vertices[d]);
}
//push verices and normals when using triangles
function triangle(a, b, c)
{
    var vertices = vertList[index];
    if(gouraud | phong)
    {
        //true vertex normals
        normalsArray.push(vec4(vertices[a][0],vertices[a][1], vertices[a][2], 0.0));
        normalsArray.push(vec4(vertices[b][0],vertices[b][1], vertices[b][2], 0.0));
        normalsArray.push(vec4(vertices[c][0],vertices[c][1], vertices[c][2], 0.0));
    }
    if(flat)
    {
        var t1 = subtract(vertices[b], vertices[a]);
        var t2 = subtract(vertices[c], vertices[a]);
        var normal = normalize(cross(t1, t2));
        //normals computed from three vertices
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
        normalsArray.push(vec4(normal[0], normal[1], normal[2], 0.0));
    }

    positionsArray.push(vertices[a]);
    positionsArray.push(vertices[b]);
    positionsArray.push(vertices[c]);
}

//send quad indices to push
function quadIndices(qind)
{
    for(var i = 0; i < qind.length; i+=4)
    {
        quad(qind[i], qind[i+1], qind[i+2], qind[i+3]);
    }
    index += 1;
}
//send triangle indices to push
function triangleIndices(tind)
{
    for(var i = 0; i < tind.length; i+=3)
    {
        triangle(tind[i], tind[i+1], tind[i+2]); 
    }
    index += 1;
}
//set projection matrix
function setProjectionMatrix(){
	var projection;
	if(projectionflag){projection = perspective(fovy, aspect, near, far);}
	else {projection = ortho(lefto, righto, bottomo, topo, near, far);}
	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uProjectionMatrix"), false, flatten(projection));
	return projection;
}
//sets materials for each light source
function setMaterial(am, dif, spec, shn){
    fixedSource1(am, dif, spec, shn);
    fixedSource2(am, dif, spec, shn);
    movingLightSource(am, dif, spec, shn);
    spotlightSource(am, dif, spec, shn);
    camLeftsource(am, dif, spec, shn);
    camRightsource(am, dif, spec, shn);
    camTopsource(am, dif, spec, shn);
    
}
//fixed ambient lightsource 1 (with geometry)
function fixedSource1(am, dif, spec, shn)
{
    var ambientProduct = mult(lightSources[0].am, am);
    var diffuseProduct = mult(lightSources[0].dif, dif);
    var specularProduct = mult(lightSources[0].spec, spec);


    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct1"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct1"),diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct1"),specularProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition1"),lightSources[0].pos );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess1"), shn);
}
//fixed ambient lightsource 2 (with geometry)
function fixedSource2(am, dif, spec, shn)
{
    var ambientProduct = mult(lightSources[1].am, am);
    var diffuseProduct = mult(lightSources[1].dif, dif);
    var specularProduct = mult(lightSources[1].spec, spec);

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct2"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct2"),diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct2"),specularProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition2"),lightSources[1].pos );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess2"), shn);
}
//fixed light source moving on y axis (with geometry)
function movingLightSource(am, dif, spec, shn)
{
    var ambientProduct = mult(lightSources[2].am, am);
    var diffuseProduct = mult(lightSources[2].dif, dif);
    var specularProduct = mult(lightSources[2].spec, spec);

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct3"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct3"),diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct3"),specularProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition3"),lightSources[2].pos );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess3"), shn);

}
//spotlight
function spotlightSource(am, dif, spec, shn)
{
    var Yoffset = 0;
    var Xoffset = 0;
    var ambientProduct = mult(lightSources[3].am, am);
    var diffuseProduct = mult(lightSources[3].dif, dif);
    var specularProduct = mult(lightSources[3].spec, spec);

    var lightDirection = normalize((subtract(at, vec3(lightSources[3].pos[0], lightSources[3].pos[1], lightSources[3].pos[2]))));
    var limit = Math.cos(2 * Math.PI/180);
    
    gl.uniform3fv(gl.getUniformLocation(program, "uLightDirection"),lightDirection);
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition4"),lightSources[3].pos);
    gl.uniform1f(gl.getUniformLocation(program,"uLimit"), limit);

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct4"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct4"), diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct4"),specularProduct );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess4"), shn);

    canvas.addEventListener("click", function(event){
        lightSources[3].pos[0] = -(2*event.clientX/canvas.width-1) + Xoffset;
        lightSources[3].pos[1] = -(2*(canvas.height-event.clientY)/canvas.height-1) + Yoffset;
    });
        
    
}

function camLeftsource(am, dif, spec, shn)
{
    var ambientProduct = mult(lightSources[4].am, am);
    var diffuseProduct = mult(lightSources[4].dif, dif);
    var specularProduct = mult(lightSources[4].spec, spec);
    lightSources[4].pos[0] = (eye[0] - 0.5);

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct5"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct5"),diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct5"),specularProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition5"),lightSources[4].pos );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess5"), shn);
}

function camRightsource(am, dif, spec, shn)
{
    var ambientProduct = mult(lightSources[5].am, am);
    var diffuseProduct = mult(lightSources[5].dif, dif);
    var specularProduct = mult(lightSources[5].spec, spec);
    lightSources[5].pos[0] = (eye[0] + 0.5);

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct6"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct6"),diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct6"),specularProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition6"),lightSources[5].pos );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess6"), shn);
}

function camTopsource(am, dif, spec, shn)
{
    var ambientProduct = mult(lightSources[6].am, am);
    var diffuseProduct = mult(lightSources[6].dif, dif);
    var specularProduct = mult(lightSources[6].spec, spec);
    lightSources[6].pos[1] = (eye[1] + 0.5);
    

    gl.uniform4fv(gl.getUniformLocation(program, "uAmbientProduct7"), ambientProduct);
    gl.uniform4fv(gl.getUniformLocation(program, "uDiffuseProduct7"),diffuseProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uSpecularProduct7"),specularProduct );
    gl.uniform4fv(gl.getUniformLocation(program, "uLightPosition7"),lightSources[6].pos );
    gl.uniform1f(gl.getUniformLocation(program,"uShininess7"), shn);
}

function setNormals()
{
    //call quadindices or triangleindices methods to push vertices and normals to positionsArray and normalsArray
    quadIndices(stage.indices);
	triangleIndices(tree.indices);
    quadIndices(tent.indices);
	quadIndices(fence.indices);
	triangleIndices(rock.indices);
	triangleIndices(log.indices);
    quadIndices(lightindices);
}

function resetNormals()
{
    positionsArray = [];
    normalsArray = [];
    index = 0;
    quadIndices(stage.indices);
	triangleIndices(tree.indices);
    quadIndices(tent.indices);
	quadIndices(fence.indices);
	triangleIndices(rock.indices);
	triangleIndices(log.indices);

    gl.bufferData(gl.ARRAY_BUFFER, flatten(normalsArray), gl.STATIC_DRAW);
	//get location of normals in shader and specify parameters
    var normalLoc = gl.getAttribLocation(program, "aNormal");
    gl.vertexAttribPointer(normalLoc, 4, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(normalLoc);
}


function UpdateSources(light, index)
{
    if(light.flag)
    {
        if(index < 4)
        {
            lightSources[index].am = vec4(0.2, 0.2, 0.2, 1.0);
            lightSources[index].dif = vec4(1.0, 1.0, 1.0, 1.0);
            lightSources[index].spec = vec4(1.0, 1.0, 1.0, 1.0);
            if(index == 0)
            {
                fixed1b.innerHTML = "On";
            }
            else if(index == 1)
            {
                fixed2b.innerHTML = "On";
            }
            else if(index == 2)
            {
                movingb.innerHTML = "On";
            }
            else if(index == 3)
            {
                headlampb.innerHTML = "On";
            }
        }

        if(index == 4)
        {
            lightSources[index].am = vec4(0.7, 0.0, 0.0, 1.0);
            lightSources[index].dif = vec4(1.0, 0.0, 0.0, 1.0);
            lightSources[index].spec = vec4(1.0, 0.0, 0.0, 1.0);
            leftb.innerHTML = "On";
        }
        if(index == 5)
        {
            lightSources[index].am = vec4(0.0, 0.7, 0.0, 1.0);
            lightSources[index].dif = vec4(0.0, 1.0, 0.0, 1.0);
            lightSources[index].spec = vec4(0.0, 1.0, 0.0, 1.0);
            rightb.innerHTML = "On";
            
        }
        if(index == 6)
        {
            lightSources[index].am = vec4(0.0, 0.0, 0.7, 1.0);
            lightSources[index].dif = vec4(0.0, 0.0, 1.0, 1.0);
            lightSources[index].spec = vec4(0.0, 0.0, 1.0, 1.0);
            topb.innerHTML = "On";
        }
    }
    else
    {
        lightSources[index].am = vec4(0.0, 0.0, 0.0, 0.0);
        lightSources[index].dif = vec4(0.0, 0.0, 0.0, 0.0);
        lightSources[index].spec = vec4(0.0, 0.0, 0.0, 0.0);
        if(index == 0)
        {
            fixed1b.innerHTML = "Off";
        }
        else  if(index == 1)
        {
            fixed2b.innerHTML = "Off";
        }
        else if(index == 2)
        {
            movingb.innerHTML = "Off";
        }
        else if(index == 3)
        {
            headlampb.innerHTML = "Off";
        }
        else if(index == 4)
        {
            leftb.innerHTML = "Off";
        }
        else if(index ==5)
        {
            rightb.innerHTML = "Off";
        }
        else if(index==6)
        {
            topb.innerHTML = "Off";
        }
    }
}

function checkFixedFlag()
{
    //check if two ambient fixed light sources are active
    //if they are update the ambient component with slider
    if(lightSources[0].flag)
	{
		lightSources[0].am[0] = amOutput.innerHTML;
		lightSources[0].am[1] = amOutput.innerHTML;
		lightSources[0].am[2] = amOutput.innerHTML;
	}

    if(lightSources[1].flag)
    {
        lightSources[1].am[0] = amOutput.innerHTML;
		lightSources[1].am[1] = amOutput.innerHTML;
		lightSources[1].am[2] = amOutput.innerHTML;
    }
}

function MovePointLight()
{ 
    //update y movement
    if(lightSources[2].pos[1] >= maxLightYPos){canMoveUp = false;}
    if(lightSources[2].pos[1] <= minLightYPos ){canMoveUp = true;}

    if(canMoveUp)
    {
        lightSources[2].pos[1] += 0.02;
        lightSources[2].pos[2] += 0.02;
    }
	else
    {
        lightSources[2].pos[1] -= 0.02;
        lightSources[2].pos[2] -= 0.02;
    }
}
function ToggleLight(index)
{
    lightSources[index].flag = !lightSources[index].flag;
    UpdateSources(lightSources[index], index);
}

function phongShader()
{
    fragFlag = 1.0;
    gl.uniform1f( gl.getUniformLocation(program, "flagFlag"), fragFlag);
}

function alternateShader()
{
    fragFlag = 0.0;
    gl.uniform1f(gl.getUniformLocation(program,"fragFlag"), fragFlag);
}

function InputHandler()
{
    
    document.addEventListener('keydown', logKey);

    function logKey(key)
    {
        switch(key.key)
        {
            case 'w':
                //move forward (radius between origin and camera is reduced)

                if(radius > topo)
                {
                    radius -= 0.1;
                    orthoScale += 0.005;
                }   
                break;
            case 's':
                //move backward (radius between origin and camera is increased)
                if(radius < far - 1)
                {
                    radius += 0.1;
                    orthoScale -= 0.005;
                }
                break;
            case 'a':
                theta -= 0.1;
			    if (theta < -360) {theta += 360;}
                break;
            case 'd':
                theta += 0.1;

				if (theta > 360) {theta -= 360;}
                break;
            case 'e':
                //gaze up
                at = vec3(0, y+=0.1, 0);
                break;
            case 'q':
                //gaze down
                at = vec3(0, y-=0.1, 0);
                break;
            case '1':
                //control rotation
                rotationFlag = !rotationFlag;
                break;
            case '2':
                //control translation
                translateFlag = !translateFlag;
                break;
            case '3':
                //control uniform scaling
                uniformScaleFlag = !uniformScaleFlag;
                break;
            case '4':
                //control non-uniform scaling
                nonUniformScaleFlag = !nonUniformScaleFlag;
                break;
            case '5':
                backAndForthFlag = !backAndForthFlag;
                break;
            case 'o':
                projectionflag = false;
                setProjectionMatrix();
                orthoFlag = true;
                break;
            case 'p':
                projectionflag = true;
                setProjectionMatrix();
                orthoFlag = false;
                break;
            case 'f':
                //flat shading
                flat = true;
                gouraud = false;
                phong = false;
                alternateShader();
                resetNormals();
                break;
            case 'g':
                flat = false;
                gouraud = true;
                phong = false;
                alternateShader();
                resetNormals();
                break;
            case 'h':
                flat = false;
                gouraud = false;
                phong = true;
                phongShader();
                resetNormals();
                break;
            case 'z':
                //first fixed ambient light source
                lightSources[0].flag = !lightSources[0].flag;
                UpdateSources(lightSources[0], 0);
                break;
            case 'x':
                //second fixed ambient light source
                lightSources[1].flag = !lightSources[1].flag;
                UpdateSources(lightSources[1], 1);
                break;
            case 'v':
                //spotlight
                lightSources[3].flag = !lightSources[3].flag;
                UpdateSources(lightSources[3], 3);
                break;
            case'l':
                //left
                lightSources[4].flag = !lightSources[4].flag;
                UpdateSources(lightSources[4], 4);
                break;
            case'r':
                lightSources[5].flag = !lightSources[5].flag;
                UpdateSources(lightSources[5], 5);
                break;
            case't':
                lightSources[6].flag = !lightSources[6].flag;
                UpdateSources(lightSources[6], 6);
                break;
            case 'c':
                //moving point light
                lightSources[2].flag = !lightSources[2].flag;
                UpdateSources(lightSources[2], 2);
                break;
        }
    }
}

