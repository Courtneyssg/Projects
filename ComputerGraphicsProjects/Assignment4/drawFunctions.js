"use strict";

function DrawStage()
{

    setMaterial(stage.am, stage.dif, stage.spec, stage.sh);
	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(0, -0.9, 0)); // translate on y axis
	var t = mult(modelViewMatrix, instanceMatrix);
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
    gl.drawArrays(gl.TRIANGLES, 0, 36);
	
}

function DrawTree()
{
    setMaterial(tree.am, tree.dif, tree.spec, tree.sh);
	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(treeXPos, -0.2, -0.8));
	instanceMatrix = mult(instanceMatrix, scale(0.5, 0.75, 0.5));
	var t = mult(modelViewMatrix, instanceMatrix);
    gl.uniformMatrix4fv( gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t) );
    gl.drawArrays(gl.TRIANGLES, 36, 54);

	
}

function DrawTent() {

    setMaterial(tent.am, tent.dif, tent.spec, tent.sh);
	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(0.3, -0.6, -0.3));
	instanceMatrix = mult(instanceMatrix, scale(0.6, 0.6, 0.6));
	instanceMatrix = mult(instanceMatrix, rotate(the[yAxis], vec3(0, 1, 0)));
	var t = mult(modelViewMatrix, instanceMatrix);
	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t));
	gl.drawArrays(gl.TRIANGLES, 90, 60);
}

function DrawFence() {
    setMaterial(fence.am, fence.dif, fence.spec, fence.sh);
	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(0, -0.61, 0.95));
	//uniform scale
	instanceMatrix = mult(instanceMatrix, scale(fenceScale, fenceScale, fenceScale));
	var t = mult(modelViewMatrix, instanceMatrix);

	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t));
	gl.drawArrays(gl.TRIANGLES, 150, 144);
}

function DrawRock() {;
    setMaterial(rock.am, rock.dif, rock.spec, rock.sh);

	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(rockXPos, rockYPos, rockZPos));
	instanceMatrix = mult(instanceMatrix, scale(0.05, 0.05, 0.05));
	instanceMatrix = mult(instanceMatrix, rotateX(270));
	var t = mult(modelViewMatrix, instanceMatrix);

	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t));
	gl.drawArrays(gl.TRIANGLES, 294, 84);
}

function DrawLog() {
    setMaterial(log.am, log.dif, log.spec, log.sh);
    
	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(-0.3, -0.72, 0));
	instanceMatrix = mult(instanceMatrix, rotateY(45));
	instanceMatrix = mult(instanceMatrix, scale(0.1, 0.1, logScale));
	var t = mult(modelViewMatrix, instanceMatrix);
	
	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t));
	gl.drawArrays(gl.TRIANGLES, 378, 288);
}

function DrawLightGeometry(start, count, source)
{
	var instanceMatrix = mat4();
	instanceMatrix = mult(instanceMatrix, translate(source[0], source[1], source[2]));
	instanceMatrix = mult(instanceMatrix, scale(0.25, 0.25, 0.25));
	var t = mult(modelViewMatrix, instanceMatrix);
	gl.uniformMatrix4fv(gl.getUniformLocation(program, "uModelViewMatrix"), false, flatten(t));
	gl.drawArrays(gl.LINE_LOOP, start, count);
}