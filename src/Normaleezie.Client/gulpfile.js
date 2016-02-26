/// <binding ProjectOpened='watch' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp"),
    typescript = require('gulp-typescript'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename');

gulp.task("typescript", function () {

    var tsProject = typescript.createProject('./scripts/tsconfig.json');

    var tsResult = tsProject.src(["./scripts/**/*.ts"])
        .pipe(typescript(tsProject));
    return tsResult
        .pipe(gulp.dest("./scripts"));
});

gulp.task("copytoDist", ['typescript'], function () {
    return gulp.src("./scripts/normaleezie.js")
        .pipe(gulp.dest("./dist/"));
});

gulp.task("uglify", ['copytoDist'], function () {
    return gulp.src("./dist/normaleezie.js")
        .pipe(uglify())
        .pipe(rename('normaleezie.min.js'))
        .pipe(gulp.dest("./dist"));
});

gulp.task("watch", function () {
    gulp.watch("./scripts/**/*.ts", ['uglify']);
});