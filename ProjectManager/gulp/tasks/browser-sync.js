var gulp        = require('gulp'),
    browserSync = require('browser-sync'),
    config      = require('./../gulpfile.config');

/**
 * Run the build task and start a server with BrowserSync
 */
gulp.task('browser-sync', function() {
  return browsersync();
});

var browsersync = function() {
	  browserSync.init([config.source + 'index.html'], {
    browser: 'chrome',
    port: 9090,
    server: {
      baseDir: "./"
    }
  });
}

module.exports = browsersync;