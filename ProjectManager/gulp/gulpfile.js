'use strict';

var gulp = require('gulp'), 	
    requireDir = require('require-dir');

// Require all tasks in gulp/tasks, including subfolders
requireDir('./tasks', { recurse: true });
