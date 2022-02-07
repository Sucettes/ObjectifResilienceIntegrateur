/* ————————————————————————————————————————————————————————————————————————————
	"gulp" –– watch, process sass/js
	"gulp sync" –– stream sass, reload js/markup
	"gulp build" –– process sass/js
	"gulp prod" –– process sass/js and optimize, wo/ sass silent error
—————————————————————————————————————————————————————————————————————————————*/
const gulp = require('gulp');
const sass = require('gulp-sass');
const cssnano = require('cssnano');
const uglify = require('gulp-uglify');
const eslint = require('gulp-eslint');
const rename = require('gulp-rename');
const postcss = require('gulp-postcss');
const plumber = require('gulp-plumber');
const include = require('gulp-include');
const babel = require('rollup-plugin-babel');
const rollup = require('gulp-better-rollup');
const autoprefixer = require('autoprefixer');
const mqpacker = require('css-mqpacker');
const sourcemaps = require('gulp-sourcemaps');
const browserSync = require('browser-sync').create();
const cjsResolve = require('rollup-plugin-commonjs');
const nodeResolve = require('rollup-plugin-node-resolve');
const svgSprite = require('gulp-svg-sprite');

const utils = require('./gulp.config');
const config = utils.config;

const svgConfig = {
	mode: {
		stack: {
			dest: '',
			sprite: 'sprite.svg'
		}
	},
	svg: {
		namespaceClassnames: false
	}
}

/* html —————————————————————————————————————————————————————————————————————*/
gulp.task('html', () => {
	return gulp.src(config.views.src)
		.pipe(include())
		.pipe(gulp.dest(config.views.dest));
});

/* sass —————————————————————————————————————————————————————————————————————*/
gulp.task('sass:dev',
    () => {
        return gulp.src(config.sass.src)
            .pipe(plumber(utils.sassReporter))
            .pipe(sourcemaps.init())
            .pipe(sass(config.sass.options))
            .pipe(postcss([
                mqpacker,
                autoprefixer(config.sass.autoprefixer)
            ]))
            .pipe(sourcemaps.write())
            .pipe(rename({ dirname: '' }))
            .pipe(gulp.dest(config.sass.dest))
            .pipe(browserSync.stream({ match: '**/*.css' }));
    });

gulp.task('sass:prod',
    () => {
        return gulp.src(config.sass.src)
            .pipe(sass(config.sass.options))
            .pipe(postcss([
                mqpacker,
                autoprefixer(config.sass.autoprefixer),
                cssnano
            ]))
            .pipe(rename({ dirname: '' }))
            .pipe(gulp.dest(config.sass.dest));
    });

/* js ———————————————————————————————————————————————————————————————————————*/
gulp.task('js:dev',
    () => {
        return gulp.src(config.js.src, { allowEmpty: true })
            .pipe(plumber(utils.jsReporter))
            .pipe(eslint())
            .pipe(eslint.format())
            .pipe(eslint({ configFile: '.eslintrc' }))
            .pipe(rollup({
                    plugins: [
                        cjsResolve(),
                        nodeResolve(),
                        babel(config.js.babel)
                    ]
                },
                'iife'))
            .pipe(rename({ dirname: '' }))
            .pipe(gulp.dest(config.js.dest));
    });

gulp.task('js:prod',
    () => {
        return gulp.src(config.js.src, { allowEmpty: true })
            .pipe(rollup({
                    plugins: [
                        cjsResolve(),
                        nodeResolve(),
                        babel(config.js.babel)
                    ]
                },
                'iife'))
            .pipe(uglify())
            .pipe(gulp.dest(config.js.dest));
    });

gulp.task('svg',
    () => {
        return gulp.src(config.svg.src)
            .pipe(plumber())
            .pipe(svgSprite(svgConfig))
            //.on('error',
            //    (e) => {
            //        //let title = 'SVG Sprite Error -> \n There is an error in your SVG source files (useless error message will follow)'
            //        // console.log( title, '\n', error)
            //    })
            .pipe(gulp.dest(config.svg.dest));
    });


/* builds ———————————————————————————————————————————————————————————————————*/
gulp.task('dev', gulp.series('sass:dev', 'js:dev'));
gulp.task('prod', gulp.series('sass:prod', 'js:prod'));
/* dev ——————————————————————————————————————————————————————————————————————*/
gulp.task('default',
    gulp.series('dev',
        (done) => {
            gulp.watch(config.watch.views, gulp.series('html'));
            gulp.watch(config.watch.sass, gulp.series('sass:dev'));
            gulp.watch(config.watch.js, gulp.series('js:dev'));

            done();
}));

/* browser sync —————————————————————————————————————————————————————————————*/
gulp.task('sync',
    gulp.series('default',
        () => {
            browserSync.init(utils.bsConfig);
            gulp.watch(`${config.js.dest}/*.js`).on('change', browserSync.reload);
            gulp.watch(config.watch.views).on('change', browserSync.reload);
        }));
