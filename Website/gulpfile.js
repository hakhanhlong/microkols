/// <binding BeforeBuild='sass' Clean='sass' />
var gulp = require("gulp"),
    fs = require("fs"),
    //less = require("gulp-less"),
    sass = require("gulp-sass");

// other content removed

gulp.task("scss", function () {
    return gulp.src('wwwroot/scss/app.scss')
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('watch',function () {
    gulp.watch('wwwroot/scss/*.scss', gulp.series('scss'));
});