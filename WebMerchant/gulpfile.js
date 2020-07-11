/// <binding AfterBuild='js' />
var gulp = require("gulp"),
    fs = require("fs"),
    //less = require("gulp-less"),
    sass = require("gulp-sass"),
    cssnano = require('gulp-cssnano'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    concat = require('gulp-concat');


var jsDest = 'wwwroot/build/js';
var cssDest = 'wwwroot/build/css';

// other content removed
gulp.task('js', function () {
    return gulp.src(['wwwroot/js/app.js', 'wwwroot/js/common/*.js', 'wwwroot/js/modules/*.js', 'wwwroot/js/pages/*.js','wwwroot/js/site.js',])
        .pipe(concat('app.js'))
        .pipe(gulp.dest(jsDest))
        .pipe(rename('app.min.js'))
        .pipe(uglify().on('error', console.error))
        .pipe(gulp.dest(jsDest));
});

gulp.task("scss", function () {
    return gulp.src('wwwroot/scss/app.scss')
        .pipe(sass())
        .pipe(gulp.dest(cssDest))
        .pipe(rename('app.min.css'))
        .pipe(cssnano())
        .pipe(gulp.dest(cssDest));
});

gulp.task('watch', function () {
    gulp.watch('wwwroot/scss/*.scss', gulp.series('scss'));
    gulp.watch('wwwroot/js/*.js', gulp.series('js'));
    gulp.watch('wwwroot/js/*/*.js', gulp.series('js'));
});