/// <binding BeforeBuild='sass' Clean='sass' />
var gulp = require("gulp"),
    fs = require("fs"),
    //less = require("gulp-less"),
    sass = require("gulp-sass"),
    concat = require('gulp-concat');


// other content removed
gulp.task('pack-js', function () {
    return gulp.src(['wwwroot/js/app.js', 'wwwroot/js/app.*.js'])
        .pipe(concat('app.js'))
        .pipe(gulp.dest('wwwroot/build/js'));
});

gulp.task("scss", function () {
    return gulp.src('wwwroot/scss/app.scss')
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('watch', function () {
    gulp.watch('wwwroot/scss/*.scss', gulp.series('scss'));
});