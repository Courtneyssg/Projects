"use strict";
var program, program1, program2;

function LightSource(pos, am, dif, spec, flag)
{
    this.pos = pos;
    this.am = am;
    this.dif = dif;
    this.spec = spec;
    this.flag = flag;
}

function Shape(indices, am, dif, spec, sh)
{
    this.indices = indices;
    this.am = am;
    this.dif = dif;
    this.spec = spec;
    this.sh = sh;
}