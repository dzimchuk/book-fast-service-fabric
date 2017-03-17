/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var webroot = "./wwwroot/";

var paths = {
    js: webroot + "js/**/*.js",
    minJs: webroot + "js/**/*.min.js",
    css: webroot + "css/**/*.css",
    minCss: webroot + "css/**/*.min.css",
    concatJsDest: webroot + "js/site.min.js",
    concatCssDest: webroot + "css/site.min.css"
};

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

var lib = webroot + "lib/";

gulp.task("copy:bootstrap", function () {
    return gulp.src("./node_modules/bootstrap/**/*")
        .pipe(gulp.dest(lib + "bootstrap"));
});

gulp.task("copy:jquery", function () {
    return gulp.src("./node_modules/jquery/**/*")
        .pipe(gulp.dest(lib + "jquery"));
});

gulp.task("copy:jquery:validation", function () {
    return gulp.src("./node_modules/jquery-validation/**/*")
        .pipe(gulp.dest(lib + "jquery-validation"));
});

gulp.task("copy:jquery:validation:unobtrusive", function () {
    return gulp.src("./node_modules/jquery-validation-unobtrusive/**/*")
        .pipe(gulp.dest(lib + "jquery-validation-unobtrusive"));
});

gulp.task("copy:node:modules", ["copy:bootstrap", "copy:jquery", "copy:jquery:validation", "copy:jquery:validation:unobtrusive"]);

